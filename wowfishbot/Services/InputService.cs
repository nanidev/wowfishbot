using System.Runtime.InteropServices;
using System.Windows.Forms;
using wowfishbot.Models;

namespace wowfishbot.Services;

public sealed class InputService
{
    private const uint InputKeyboard = 1;
    private const uint InputMouse = 0;
    private const uint KeyEventKeyUp = 0x0002;
    private const uint MouseEventLeftDown = 0x0002;
    private const uint MouseEventLeftUp = 0x0004;
    private const uint MouseEventRightDown = 0x0008;
    private const uint MouseEventRightUp = 0x0010;
    private const uint MouseEventMiddleDown = 0x0020;
    private const uint MouseEventMiddleUp = 0x0040;

    public bool TryParseKey(string text, out Keys key)
    {
        key = Keys.None;
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        var trimmed = text.Trim();
        if (trimmed.Length == 1 && char.IsDigit(trimmed[0]))
        {
            key = (Keys)((int)Keys.D0 + (trimmed[0] - '0'));
            return true;
        }

        try
        {
            var converted = new KeysConverter().ConvertFromInvariantString(trimmed);
            if (converted is Keys parsed && parsed != Keys.None)
            {
                key = parsed;
                return true;
            }
        }
        catch (ArgumentException)
        {
            // Report invalid input through the return value.
        }

        return false;
    }

    public bool PressKey(Keys key)
    {
        if (key == Keys.None)
        {
            return false;
        }

        var virtualKey = (ushort)(key & Keys.KeyCode);
        var inputs = new[]
        {
            CreateKeyboardInput(virtualKey, 0),
            CreateKeyboardInput(virtualKey, KeyEventKeyUp)
        };
        return SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<NativeInput>()) == inputs.Length;
    }

    public bool TryParseKeyCombination(string text, out Keys keyCombination)
    {
        keyCombination = Keys.None;
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        var parts = text.Split('+', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
        {
            return false;
        }

        var modifiers = Keys.None;
        Keys key = Keys.None;
        foreach (var part in parts)
        {
            switch (part.ToLowerInvariant())
            {
                case "ctrl":
                case "control":
                    modifiers |= Keys.Control;
                    break;
                case "shift":
                    modifiers |= Keys.Shift;
                    break;
                case "alt":
                    modifiers |= Keys.Alt;
                    break;
                default:
                    if (key != Keys.None || !TryParseKey(part, out key))
                    {
                        return false;
                    }

                    break;
            }
        }

        if (key == Keys.None)
        {
            return false;
        }

        keyCombination = modifiers | key;
        return true;
    }

    public bool PressKeyCombination(Keys keyCombination)
    {
        var key = keyCombination & Keys.KeyCode;
        if (key == Keys.None)
        {
            return false;
        }

        var modifiers = new List<ushort>();
        if ((keyCombination & Keys.Control) != Keys.None)
        {
            modifiers.Add((ushort)Keys.ControlKey);
        }

        if ((keyCombination & Keys.Shift) != Keys.None)
        {
            modifiers.Add((ushort)Keys.ShiftKey);
        }

        if ((keyCombination & Keys.Alt) != Keys.None)
        {
            modifiers.Add((ushort)Keys.Menu);
        }

        var inputs = new List<NativeInput>(modifiers.Count * 2 + 2);
        foreach (var modifier in modifiers)
        {
            inputs.Add(CreateKeyboardInput(modifier, 0));
        }

        inputs.Add(CreateKeyboardInput((ushort)key, 0));
        inputs.Add(CreateKeyboardInput((ushort)key, KeyEventKeyUp));
        for (var index = modifiers.Count - 1; index >= 0; index--)
        {
            inputs.Add(CreateKeyboardInput(modifiers[index], KeyEventKeyUp));
        }

        return SendInput((uint)inputs.Count, inputs.ToArray(), Marshal.SizeOf<NativeInput>()) == inputs.Count;
    }

    public static string FormatKeyCombination(Keys keyCombination)
    {
        var parts = new List<string>();
        if ((keyCombination & Keys.Control) != Keys.None)
        {
            parts.Add("Ctrl");
        }

        if ((keyCombination & Keys.Alt) != Keys.None)
        {
            parts.Add("Alt");
        }

        if ((keyCombination & Keys.Shift) != Keys.None)
        {
            parts.Add("Shift");
        }

        var key = keyCombination & Keys.KeyCode;
        if (key != Keys.None)
        {
            parts.Add(new KeysConverter().ConvertToInvariantString(key) ?? key.ToString());
        }

        return string.Join('+', parts);
    }

    public void MoveCursor(Point screenPoint) => SetCursorPos(screenPoint.X, screenPoint.Y);

    public bool Click(MouseButton button)
    {
        var (down, up) = button switch
        {
            MouseButton.Left => (MouseEventLeftDown, MouseEventLeftUp),
            MouseButton.Middle => (MouseEventMiddleDown, MouseEventMiddleUp),
            _ => (MouseEventRightDown, MouseEventRightUp)
        };

        var inputs = new[]
        {
            CreateMouseInput(down),
            CreateMouseInput(up)
        };
        return SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<NativeInput>()) == inputs.Length;
    }

    private static NativeInput CreateKeyboardInput(ushort virtualKey, uint flags) => new()
    {
        Type = InputKeyboard,
        Data = new NativeInputUnion
        {
            Keyboard = new KeyboardInput
            {
                VirtualKey = virtualKey,
                ScanCode = 0,
                Flags = flags,
                Time = 0,
                ExtraInfo = IntPtr.Zero
            }
        }
    };

    private static NativeInput CreateMouseInput(uint flags) => new()
    {
        Type = InputMouse,
        Data = new NativeInputUnion
        {
            Mouse = new MouseInput
            {
                Flags = flags,
                MouseData = 0,
                Time = 0,
                ExtraInfo = IntPtr.Zero
            }
        }
    };

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint inputCount, NativeInput[] inputs, int inputSize);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetCursorPos(int x, int y);

    [StructLayout(LayoutKind.Sequential)]
    private struct NativeInput
    {
        public uint Type;
        public NativeInputUnion Data;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct NativeInputUnion
    {
        [FieldOffset(0)]
        public MouseInput Mouse;

        [FieldOffset(0)]
        public KeyboardInput Keyboard;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MouseInput
    {
        public int X;
        public int Y;
        public uint MouseData;
        public uint Flags;
        public uint Time;
        public IntPtr ExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct KeyboardInput
    {
        public ushort VirtualKey;
        public ushort ScanCode;
        public uint Flags;
        public uint Time;
        public IntPtr ExtraInfo;
    }
}
