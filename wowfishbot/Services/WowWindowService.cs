using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace wowfishbot.Services;

public sealed record WowWindowInfo(
    IntPtr Handle,
    int ProcessId,
    string ProcessName,
    string Title,
    Rectangle ClientBounds)
{
    public override string ToString() => $"{Title} ({ProcessName}, PID {ProcessId})";
}

public sealed class WowWindowService
{
    private const int SwRestore = 9;
    private const uint GaRoot = 2;

    public IReadOnlyList<WowWindowInfo> FindWindows(string processName, string title)
    {
        var results = new List<WowWindowInfo>();
        var normalizedProcessName = Path.GetFileNameWithoutExtension(processName).Trim();
        var normalizedTitle = title.Trim();

        EnumWindows((handle, _) =>
        {
            if (!IsWindowVisible(handle) || GetAncestor(handle, GaRoot) != handle)
            {
                return true;
            }

            GetWindowThreadProcessId(handle, out var processId);
            if (processId == 0)
            {
                return true;
            }

            try
            {
                using var process = Process.GetProcessById((int)processId);
                var processMatches = string.IsNullOrWhiteSpace(normalizedProcessName) ||
                    string.Equals(process.ProcessName, normalizedProcessName, StringComparison.OrdinalIgnoreCase);
                var windowTitle = GetWindowTitle(handle);
                var titleMatches = string.IsNullOrWhiteSpace(normalizedTitle) ||
                    windowTitle.Contains(normalizedTitle, StringComparison.OrdinalIgnoreCase);

                if (processMatches && titleMatches && TryGetClientBounds(handle, out var bounds))
                {
                    results.Add(new WowWindowInfo(handle, process.Id, process.ProcessName, windowTitle, bounds));
                }
            }
            catch (ArgumentException)
            {
                // The process can exit while the window list is being enumerated.
            }
            catch (InvalidOperationException)
            {
                // The process can exit while the window list is being enumerated.
            }

            return true;
        }, IntPtr.Zero);

        return results;
    }

    public bool TryGetClientBounds(IntPtr handle, out Rectangle bounds)
    {
        bounds = Rectangle.Empty;
        if (handle == IntPtr.Zero || !IsWindow(handle) || !GetClientRect(handle, out var clientRect))
        {
            return false;
        }

        var topLeft = new Point(clientRect.Left, clientRect.Top);
        var bottomRight = new Point(clientRect.Right, clientRect.Bottom);
        if (!ClientToScreen(handle, ref topLeft) || !ClientToScreen(handle, ref bottomRight))
        {
            return false;
        }

        bounds = Rectangle.FromLTRB(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
        return !bounds.IsEmpty;
    }

    public bool TryFocus(WowWindowInfo window)
    {
        if (!IsWindow(window.Handle))
        {
            return false;
        }

        ShowWindow(window.Handle, SwRestore);
        SetForegroundWindow(window.Handle);
        Thread.Sleep(80);
        return GetForegroundWindow() == window.Handle;
    }

    public static bool IsForeground(IntPtr handle) => handle != IntPtr.Zero && GetForegroundWindow() == handle;

    private static string GetWindowTitle(IntPtr handle)
    {
        var length = GetWindowTextLength(handle);
        if (length == 0)
        {
            return string.Empty;
        }

        var builder = new StringBuilder(length + 1);
        GetWindowText(handle, builder, builder.Capacity);
        return builder.ToString();
    }

    private delegate bool EnumWindowsProc(IntPtr handle, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern bool EnumWindows(EnumWindowsProc callback, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern bool IsWindowVisible(IntPtr handle);

    [DllImport("user32.dll")]
    private static extern bool IsWindow(IntPtr handle);

    [DllImport("user32.dll")]
    private static extern IntPtr GetAncestor(IntPtr handle, uint flags);

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr handle, out uint processId);

    [DllImport("user32.dll")]
    private static extern int GetWindowTextLength(IntPtr handle);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetWindowText(IntPtr handle, StringBuilder text, int maxCount);

    [DllImport("user32.dll")]
    private static extern bool GetClientRect(IntPtr handle, out NativeRect rect);

    [DllImport("user32.dll")]
    private static extern bool ClientToScreen(IntPtr handle, ref Point point);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr handle);

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr handle, int command);

    [StructLayout(LayoutKind.Sequential)]
    private struct NativeRect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
}
