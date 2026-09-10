using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using wowfishbot.Models;

namespace wowfishbot.Services;

public sealed class BobberLocator
{
    private readonly WowWindowService windowService;

    public BobberLocator(WowWindowService windowService)
    {
        this.windowService = windowService;
    }

    public bool TryCaptureCalibrationImage(
        IntPtr windowHandle,
        Point screenPoint,
        out Bitmap screenshot,
        out Point screenshotOrigin,
        out string reason)
    {
        screenshot = null!;
        screenshotOrigin = Point.Empty;
        reason = string.Empty;

        if (!windowService.TryGetClientBounds(windowHandle, out var clientBounds))
        {
            reason = "The WoW client bounds are unavailable.";
            return false;
        }

        if (!clientBounds.Contains(screenPoint))
        {
            reason = "The calibration point is outside the selected WoW client.";
            return false;
        }

        var width = Math.Min(120, clientBounds.Width);
        var height = Math.Min(120, clientBounds.Height);
        var originX = Math.Clamp(screenPoint.X - width / 2, clientBounds.Left, clientBounds.Right - width);
        var originY = Math.Clamp(screenPoint.Y - height / 2, clientBounds.Top, clientBounds.Bottom - height);
        screenshotOrigin = new Point(originX, originY);

        var restoreCursor = false;
        var cursorPosition = Point.Empty;
        try
        {
            cursorPosition = Cursor.Position;
            restoreCursor = clientBounds.Contains(cursorPosition) && cursorPosition == screenPoint;
            if (restoreCursor)
            {
                Cursor.Position = new Point(clientBounds.Left - 2, clientBounds.Top - 2);
                Thread.Sleep(150);
            }

            screenshot = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            using var graphics = Graphics.FromImage(screenshot);
            graphics.CopyFromScreen(screenshotOrigin, Point.Empty, screenshot.Size, CopyPixelOperation.SourceCopy);
            return true;
        }
        catch (Exception exception) when (exception is ExternalException or ArgumentException)
        {
            screenshot.Dispose();
            screenshot = null!;
            reason = $"The bobber screenshot could not be captured: {exception.Message}";
            return false;
        }
        finally
        {
            if (restoreCursor)
            {
                Cursor.Position = cursorPosition;
            }
        }
    }

    public bool TryFindBobber(
        IntPtr windowHandle,
        IReadOnlyList<BobberPixelSample> pixels,
        Point clickOffset,
        bool strict,
        int colorTolerance,
        int neighborhoodRadius,
        int minimumMatchScorePercent,
        BobberSearchArea? searchArea,
        out Point bobberPoint,
        out double score,
        out string reason)
    {
        bobberPoint = Point.Empty;
        score = 0;
        reason = string.Empty;

        if (!TryCaptureClientImage(windowHandle, out var bitmap, out var clientBounds, out reason))
        {
            return false;
        }

        using (bitmap)
        {
            var requestedRegion = ToClientRectangle(searchArea);
            var candidates = FindCandidates(bitmap, clientBounds, pixels, clickOffset, requestedRegion, strict, colorTolerance, neighborhoodRadius, minimumMatchScorePercent);
            var best = candidates.OrderByDescending(candidate => candidate.Score).FirstOrDefault();
            if (best is null)
            {
                reason = $"The saved bobber pixel pattern was not found in the WoW client; {candidates.Count} candidate(s) passed the anchor check.";
                return false;
            }

            bobberPoint = best.ScreenPoint;
            score = best.Score;
            reason = $"Found {candidates.Count} candidate(s); best pattern score was {best.Score:0.00}.";
            return true;
        }
    }

    public bool TryValidateAt(
        IntPtr windowHandle,
        Point expectedPoint,
        IReadOnlyList<BobberPixelSample> pixels,
        Point clickOffset,
        bool strict,
        int radius,
        int colorTolerance,
        int neighborhoodRadius,
        int minimumMatchScorePercent,
        BobberSearchArea? searchArea,
        out Point bobberPoint,
        out string reason)
    {
        bobberPoint = expectedPoint;
        reason = string.Empty;

        if (!TryCaptureClientImage(windowHandle, out var bitmap, out var clientBounds, out reason))
        {
            return false;
        }

        using (bitmap)
        {
            var localPoint = new Point(expectedPoint.X - clientBounds.Left, expectedPoint.Y - clientBounds.Top);
            var patternRadius = radius + pixels.Max(pixel => Math.Max(Math.Abs(pixel.OffsetX), Math.Abs(pixel.OffsetY)));
            var anchorPoint = new Point(localPoint.X - clickOffset.X, localPoint.Y - clickOffset.Y);
            var searchRegion = new Rectangle(anchorPoint.X - patternRadius, anchorPoint.Y - patternRadius, patternRadius * 2 + 1, patternRadius * 2 + 1);
            var requestedRegion = ToClientRectangle(searchArea);
            if (requestedRegion.HasValue)
            {
                searchRegion = Rectangle.Intersect(searchRegion, requestedRegion.Value);
            }

            var candidates = FindCandidates(bitmap, clientBounds, pixels, clickOffset, searchRegion, strict, colorTolerance, neighborhoodRadius, minimumMatchScorePercent);
            var nearest = candidates
                .OrderBy(candidate => DistanceSquared(candidate.ScreenPoint, expectedPoint))
                .FirstOrDefault();

            if (nearest is null || DistanceSquared(nearest.ScreenPoint, expectedPoint) > radius * radius)
            {
                reason = "The saved bobber pixel pattern was not visible near the last detection.";
                return false;
            }

            bobberPoint = nearest.ScreenPoint;
            return true;
        }
    }

    public bool TryCaptureClientImage(
        IntPtr windowHandle,
        out Bitmap bitmap,
        out Rectangle clientBounds,
        out string reason)
    {
        bitmap = null!;
        clientBounds = Rectangle.Empty;
        reason = string.Empty;

        if (!windowService.TryGetClientBounds(windowHandle, out clientBounds))
        {
            reason = "The WoW client bounds are unavailable.";
            return false;
        }

        try
        {
            bitmap = new Bitmap(clientBounds.Width, clientBounds.Height, PixelFormat.Format32bppArgb);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.CopyFromScreen(clientBounds.Location, Point.Empty, clientBounds.Size, CopyPixelOperation.SourceCopy);
            return true;
        }
        catch (Exception exception) when (exception is ExternalException or ArgumentException)
        {
            bitmap.Dispose();
            bitmap = null!;
            reason = $"The WoW client could not be captured: {exception.Message}";
            return false;
        }
    }

    private static List<BobberCandidate> FindCandidates(
        Bitmap bitmap,
        Rectangle clientBounds,
        IReadOnlyList<BobberPixelSample> pixels,
        Point clickOffset,
        Rectangle? requestedRegion,
        bool strict,
        int colorTolerance,
        int neighborhoodRadius,
        int minimumMatchScorePercent)
    {
        if (pixels.Count < 2)
        {
            return [];
        }

        var width = bitmap.Width;
        var height = bitmap.Height;
        var scanRegion = requestedRegion.HasValue
            ? Rectangle.Intersect(new Rectangle(0, 0, width, height), requestedRegion.Value)
            : new Rectangle(0, 0, width, height);
        if (scanRegion.IsEmpty)
        {
            return [];
        }

        var candidates = new List<BobberCandidate>();
        var bitmapData = bitmap.LockBits(
            new Rectangle(0, 0, width, height),
            ImageLockMode.ReadOnly,
            PixelFormat.Format32bppArgb);
        try
        {
            var stride = bitmapData.Stride;
            var absoluteStride = Math.Abs(stride);
            var bytes = new byte[checked(absoluteStride * height)];
            Marshal.Copy(bitmapData.Scan0, bytes, 0, bytes.Length);
            var anchor = pixels[0];
            var anchorTolerance = Math.Clamp(colorTolerance, 1, 442);

            for (var y = scanRegion.Top; y < scanRegion.Bottom; y++)
            {
                for (var x = scanRegion.Left; x < scanRegion.Right; x++)
                {
                    var anchorRed = ReadRed(bytes, absoluteStride, stride, x, y);
                    var anchorGreen = ReadGreen(bytes, absoluteStride, stride, x, y);
                    var anchorBlue = ReadBlue(bytes, absoluteStride, stride, x, y);
                    if (ColorDistance(anchor.Red, anchor.Green, anchor.Blue, anchorRed, anchorGreen, anchorBlue) > anchorTolerance)
                    {
                        continue;
                    }

                    var match = MatchPattern(bytes, width, height, absoluteStride, stride, x, y, pixels, strict, neighborhoodRadius, minimumMatchScorePercent, colorTolerance);
                    if (match is null)
                    {
                        continue;
                    }

                    var localClickPoint = new Point(
                        x + (int)Math.Round(clickOffset.X * match.Scale),
                        y + (int)Math.Round(clickOffset.Y * match.Scale));
                    var clickPoint = new Point(clientBounds.Left + localClickPoint.X, clientBounds.Top + localClickPoint.Y);
                    candidates.Add(new BobberCandidate(clickPoint, match.Score));
                }
            }
        }
        finally
        {
            bitmap.UnlockBits(bitmapData);
        }

        return candidates;
    }

    private static PatternMatch? MatchPattern(
        byte[] bytes,
        int width,
        int height,
        int absoluteStride,
        int stride,
        int anchorX,
        int anchorY,
        IReadOnlyList<BobberPixelSample> pixels,
        bool strict,
        int neighborhoodRadius,
        int minimumMatchScorePercent,
        int colorTolerance)
    {
        var best = (PatternMatch?)null;
        var scales = new[] { 0.80, 0.90, 1.00, 1.10, 1.20 };
        var searchRadius = Math.Clamp(neighborhoodRadius, 0, 10);
        var minimumPixelScore = Math.Max(0.10, 1 - Math.Clamp(colorTolerance, 1, 442) / 442d);

        foreach (var scale in scales)
        {
            var totalScore = ColorMatchScore(
                bytes,
                absoluteStride,
                stride,
                anchorX,
                anchorY,
                pixels[0].Red,
                pixels[0].Green,
                pixels[0].Blue);
            var matched = true;

            for (var index = 1; index < pixels.Count; index++)
            {
                var pixel = pixels[index];
                var expectedX = anchorX + (int)Math.Round(pixel.OffsetX * scale);
                var expectedY = anchorY + (int)Math.Round(pixel.OffsetY * scale);
                var bestPixelScore = 0d;
                for (var offsetY = -searchRadius; offsetY <= searchRadius; offsetY++)
                {
                    for (var offsetX = -searchRadius; offsetX <= searchRadius; offsetX++)
                    {
                        var candidateX = expectedX + offsetX;
                        var candidateY = expectedY + offsetY;
                        if (candidateX < 0 || candidateY < 0 || candidateX >= width || candidateY >= height)
                        {
                            continue;
                        }

                        var candidateScore = ColorMatchScore(
                            bytes,
                            absoluteStride,
                            stride,
                            candidateX,
                            candidateY,
                            pixel.Red,
                            pixel.Green,
                            pixel.Blue) - (Math.Abs(offsetX) + Math.Abs(offsetY)) * 0.015;
                        bestPixelScore = Math.Max(bestPixelScore, candidateScore);
                    }
                }

                if (bestPixelScore < minimumPixelScore)
                {
                    matched = false;
                    break;
                }

                totalScore += bestPixelScore;
            }

            if (matched)
            {
                var score = totalScore / pixels.Count;
                if (best is null || score > best.Score)
                {
                    best = new PatternMatch(scale, score);
                }
            }
        }

        var minimumScore = Math.Clamp(minimumMatchScorePercent, 1, 100) / 100d;
        return best is not null && best.Score >= minimumScore ? best : null;
    }

    private static Rectangle? ToClientRectangle(BobberSearchArea? searchArea)
    {
        return searchArea is { IsValid: true } area
            ? new Rectangle(area.X, area.Y, area.Width, area.Height)
            : null;
    }

    private static double ColorMatchScore(
        byte[] bytes,
        int absoluteStride,
        int stride,
        int x,
        int y,
        int expectedRed,
        int expectedGreen,
        int expectedBlue)
    {
        var red = ReadRed(bytes, absoluteStride, stride, x, y);
        var green = ReadGreen(bytes, absoluteStride, stride, x, y);
        var blue = ReadBlue(bytes, absoluteStride, stride, x, y);
        return Math.Max(0, 1 - ColorDistance(expectedRed, expectedGreen, expectedBlue, red, green, blue) / 442d);
    }

    private static int ColorDistance(int expectedRed, int expectedGreen, int expectedBlue, int red, int green, int blue)
    {
        var redDifference = expectedRed - red;
        var greenDifference = expectedGreen - green;
        var blueDifference = expectedBlue - blue;
        return (int)Math.Sqrt(redDifference * redDifference + greenDifference * greenDifference + blueDifference * blueDifference);
    }

    private static int ReadRed(byte[] bytes, int absoluteStride, int stride, int x, int y)
    {
        return bytes[GetByteOffset(absoluteStride, stride, x, y) + 2];
    }

    private static int ReadGreen(byte[] bytes, int absoluteStride, int stride, int x, int y)
    {
        return bytes[GetByteOffset(absoluteStride, stride, x, y) + 1];
    }

    private static int ReadBlue(byte[] bytes, int absoluteStride, int stride, int x, int y)
    {
        return bytes[GetByteOffset(absoluteStride, stride, x, y)];
    }

    private static int GetByteOffset(int absoluteStride, int stride, int x, int y)
    {
        var row = stride < 0 ? absoluteStride * (y + 1) - absoluteStride : y * absoluteStride;
        return row + x * 4;
    }

    private static int DistanceSquared(Point left, Point right)
    {
        var x = left.X - right.X;
        var y = left.Y - right.Y;
        return x * x + y * y;
    }

    private sealed record BobberCandidate(Point ScreenPoint, double Score);

    private sealed record PatternMatch(double Scale, double Score);
}
