using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using wowfishbot.Models;

namespace wowfishbot;

public sealed partial class BobberCalibrationForm : Form
{
    private const int Zoom = 6;

    private readonly Bitmap sourceBitmap;
    private readonly Point cursorLocalPoint;
    private readonly List<BobberPixelSample> selectedPixels = [];
    private Bitmap? displayBitmap;
    private Point anchorPoint;
    private Point clickOffset;

    public BobberCalibrationForm()
    {
        sourceBitmap = new Bitmap(1, 1);
        cursorLocalPoint = Point.Empty;
        InitializeComponent();
        RefreshDisplay();
    }

    private void undoButton_Click(object? sender, EventArgs e) => UndoSelection();

    public BobberCalibrationForm(Bitmap screenshot, Point screenshotOrigin, Point cursorScreenPoint)
    {
        sourceBitmap = screenshot ?? throw new ArgumentNullException(nameof(screenshot));
        cursorLocalPoint = new Point(cursorScreenPoint.X - screenshotOrigin.X, cursorScreenPoint.Y - screenshotOrigin.Y);
        InitializeComponent();
        RefreshDisplay();
    }

    private void clearButton_Click(object? sender, EventArgs e) => ClearSelection();

    private void confirmButton_Click(object? sender, EventArgs e)
    {
        if (selectedPixels.Count < 2)
        {
            selectionLabel.Text = "Select the red and blue feather pixels before confirming.";
            return;
        }

        DialogResult = DialogResult.OK;
    }

    public IReadOnlyList<BobberPixelSample> SelectedPixels => selectedPixels;

    public Point ClickOffset => clickOffset;

    private void ImageBox_MouseClick(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left || selectedPixels.Count >= 3)
        {
            return;
        }

        var sourcePoint = new Point(e.X / Zoom, e.Y / Zoom);
        if (sourcePoint.X < 0 || sourcePoint.Y < 0 || sourcePoint.X >= sourceBitmap.Width || sourcePoint.Y >= sourceBitmap.Height)
        {
            return;
        }

        if (sourcePoint == cursorLocalPoint)
        {
            selectionLabel.Text = "The original cursor pixel is excluded; select a feather pixel instead.";
            return;
        }

        var color = sourceBitmap.GetPixel(sourcePoint.X, sourcePoint.Y);
        if (selectedPixels.Count == 0 && !LooksRed(color))
        {
            selectionLabel.Text = "Select a red feather pixel first.";
            return;
        }

        if (selectedPixels.Count == 1 && !LooksBlue(color))
        {
            selectionLabel.Text = "Select a blue feather pixel second.";
            return;
        }

        if (selectedPixels.Any(pixel => pixel.OffsetX == sourcePoint.X - anchorPoint.X && pixel.OffsetY == sourcePoint.Y - anchorPoint.Y))
        {
            selectionLabel.Text = "That pixel is already selected.";
            return;
        }

        if (selectedPixels.Count == 0)
        {
            anchorPoint = sourcePoint;
            clickOffset = new Point(cursorLocalPoint.X - anchorPoint.X, cursorLocalPoint.Y - anchorPoint.Y);
        }

        selectedPixels.Add(new BobberPixelSample(
            sourcePoint.X - anchorPoint.X,
            sourcePoint.Y - anchorPoint.Y,
            color.R,
            color.G,
            color.B));
        RefreshDisplay();
    }

    private void UndoSelection()
    {
        if (selectedPixels.Count == 0)
        {
            return;
        }

        selectedPixels.RemoveAt(selectedPixels.Count - 1);
        if (selectedPixels.Count == 0)
        {
            anchorPoint = Point.Empty;
            clickOffset = Point.Empty;
        }

        RefreshDisplay();
    }

    private void ClearSelection()
    {
        selectedPixels.Clear();
        anchorPoint = Point.Empty;
        clickOffset = Point.Empty;
        RefreshDisplay();
    }

    private void RefreshDisplay()
    {
        displayBitmap?.Dispose();
        displayBitmap = new Bitmap(sourceBitmap.Width * Zoom, sourceBitmap.Height * Zoom, PixelFormat.Format32bppArgb);
        using (var graphics = Graphics.FromImage(displayBitmap))
        {
            graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            graphics.PixelOffsetMode = PixelOffsetMode.Half;
            graphics.DrawImage(sourceBitmap, new Rectangle(0, 0, displayBitmap.Width, displayBitmap.Height));

            using var gridPen = new Pen(Color.FromArgb(55, 255, 255, 255));
            for (var x = 0; x <= sourceBitmap.Width; x++)
            {
                graphics.DrawLine(gridPen, x * Zoom, 0, x * Zoom, displayBitmap.Height);
            }

            for (var y = 0; y <= sourceBitmap.Height; y++)
            {
                graphics.DrawLine(gridPen, 0, y * Zoom, displayBitmap.Width, y * Zoom);
            }

            using var cursorPen = new Pen(Color.Yellow, 2);
            DrawMarker(graphics, cursorPen, cursorLocalPoint, 4);
            for (var index = 0; index < selectedPixels.Count; index++)
            {
                var pixel = selectedPixels[index];
                var point = new Point(anchorPoint.X + pixel.OffsetX, anchorPoint.Y + pixel.OffsetY);
                using var selectedPen = new Pen(index == 0 ? Color.Red : index == 1 ? Color.DeepSkyBlue : Color.Lime, 2);
                DrawMarker(graphics, selectedPen, point, 3);
            }
        }

        imageBox.Image = displayBitmap;
        imageBox.Size = displayBitmap.Size;
        imagePanel.AutoScrollMinSize = new Size(displayBitmap.Width + imagePanel.Padding.Horizontal, displayBitmap.Height + imagePanel.Padding.Vertical);
        undoButton.Enabled = selectedPixels.Count > 0;
        clearButton.Enabled = selectedPixels.Count > 0;
        confirmButton.Enabled = selectedPixels.Count >= 2;
        selectionLabel.Text = selectedPixels.Count switch
        {
            0 => "0 of 3 pixels selected — select the red feather pixel",
            1 => "1 of 3 selected — now select the blue feather pixel",
            2 => "2 pixels selected — optionally select a third, then save",
            _ => "3 pixels selected — pattern ready to save"
        };
    }

    private static void DrawMarker(Graphics graphics, Pen pen, Point sourcePoint, int radius)
    {
        var center = new Point(sourcePoint.X * Zoom + Zoom / 2, sourcePoint.Y * Zoom + Zoom / 2);
        graphics.DrawEllipse(pen, center.X - radius * 2, center.Y - radius * 2, radius * 4, radius * 4);
        graphics.DrawLine(pen, center.X - radius * 3, center.Y, center.X + radius * 3, center.Y);
        graphics.DrawLine(pen, center.X, center.Y - radius * 3, center.X, center.Y + radius * 3);
    }

    private static bool LooksRed(Color color) => color.R >= color.G + 8 && color.R >= color.B + 8 && color.R >= 45;

    private static bool LooksBlue(Color color) => color.B >= color.R + 8 && color.B >= color.G - 8 && color.B >= 45;

}
