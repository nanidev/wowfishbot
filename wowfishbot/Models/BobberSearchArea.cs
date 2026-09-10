namespace wowfishbot.Models;

public sealed record BobberSearchArea(int X, int Y, int Width, int Height)
{
    public bool IsValid => Width > 0 && Height > 0;
}
