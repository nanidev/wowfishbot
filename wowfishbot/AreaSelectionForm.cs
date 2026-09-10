using wowfishbot.Models;

namespace wowfishbot;

public sealed partial class AreaSelectionForm : Form
{
    private readonly Bitmap sourceBitmap;
    private Rectangle selectedRectangle;
    private Point selectionStart;
    private bool selecting;

    public AreaSelectionForm()
        : this(new Bitmap(1, 1))
    {
    }

    public AreaSelectionForm(Bitmap screenshot)
    {
        sourceBitmap = screenshot ?? throw new ArgumentNullException(nameof(screenshot));
        InitializeComponent();
        imageBox.Image = sourceBitmap;
        imageBox.Size = sourceBitmap.Size;
        imagePanel.AutoScrollMinSize = new Size(sourceBitmap.Width + imagePanel.Padding.Horizontal, sourceBitmap.Height + imagePanel.Padding.Vertical);
        UpdateSelectionUi();
    }

    public BobberSearchArea? SelectedArea { get; private set; }

    private void imageBox_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left)
        {
            return;
        }

        selectionStart = ClampToImage(e.Location);
        selectedRectangle = new Rectangle(selectionStart, Size.Empty);
        selecting = true;
        imageBox.Invalidate();
    }

    private void imageBox_MouseMove(object? sender, MouseEventArgs e)
    {
        if (!selecting)
        {
            return;
        }

        selectedRectangle = CreateRectangle(selectionStart, ClampToImage(e.Location));
        UpdateSelectionUi();
        imageBox.Invalidate();
    }

    private void imageBox_MouseUp(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left || !selecting)
        {
            return;
        }

        selectedRectangle = CreateRectangle(selectionStart, ClampToImage(e.Location));
        selecting = false;
        UpdateSelectionUi();
        imageBox.Invalidate();
    }

    private void imageBox_Paint(object? sender, PaintEventArgs e)
    {
        if (selectedRectangle.IsEmpty)
        {
            return;
        }

        using var fill = new SolidBrush(Color.FromArgb(55, 37, 99, 235));
        using var border = new Pen(Color.FromArgb(37, 99, 235), 2);
        e.Graphics.FillRectangle(fill, selectedRectangle);
        e.Graphics.DrawRectangle(border, selectedRectangle);
    }

    private void clearButton_Click(object? sender, EventArgs e)
    {
        SelectedArea = null;
        DialogResult = DialogResult.OK;
    }

    private void confirmButton_Click(object? sender, EventArgs e)
    {
        if (selectedRectangle.Width < 2 || selectedRectangle.Height < 2)
        {
            selectionLabel.Text = "Drag across a game area before confirming.";
            return;
        }

        SelectedArea = new BobberSearchArea(
            selectedRectangle.X,
            selectedRectangle.Y,
            selectedRectangle.Width,
            selectedRectangle.Height);
        DialogResult = DialogResult.OK;
    }

    private Point ClampToImage(Point point) => new(
        Math.Clamp(point.X, 0, sourceBitmap.Width),
        Math.Clamp(point.Y, 0, sourceBitmap.Height));

    private static Rectangle CreateRectangle(Point first, Point second) => Rectangle.FromLTRB(
        Math.Min(first.X, second.X),
        Math.Min(first.Y, second.Y),
        Math.Max(first.X, second.X),
        Math.Max(first.Y, second.Y));

    private void UpdateSelectionUi()
    {
        confirmButton.Enabled = selectedRectangle.Width >= 2 && selectedRectangle.Height >= 2;
        selectionLabel.Text = selectedRectangle.IsEmpty
            ? "Drag over the part of the client where the bobber appears."
            : $"Selected: ({selectedRectangle.X}, {selectedRectangle.Y}) {selectedRectangle.Width} x {selectedRectangle.Height}";
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (!ReferenceEquals(imageBox.Image, sourceBitmap))
            {
                sourceBitmap.Dispose();
            }

            components?.Dispose();
        }

        base.Dispose(disposing);
    }
}
