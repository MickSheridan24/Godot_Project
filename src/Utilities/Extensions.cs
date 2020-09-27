using Godot;
public static class Extensions
{
    public static Vector2 SPACE_SIZE(this Vector2 v)
    {
        return new Vector2(32, 32);
    }

    public static Vector2 GetDirectionTo(this Vector2 vector, Vector2 dest)
    {
        return (dest - vector).Normalized();
    }

    public static Vector2 ProximityTo(this Vector2 vector, Vector2 dest)
    {
        return (dest - vector).Abs();
    }

    public static float GetScale(this Vector2 vector)
    {
        return vector.x + vector.y;
    }

    public static bool WithinRange(this Vector2 pos, Vector2 dest, Vector2 range)
    {
        var diff = (pos.Abs() - dest.Abs()).Abs();
        return diff.x < range.x && diff.y < range.y;
    }

    public static bool InBounds(this Vector2 vector, Vector2 low, Vector2 high)
    {
        return vector.x >= low.x && vector.y >= low.y && vector.x < high.x && vector.y < high.y;
    }


    public static bool InMap(this Vector2 vector, int mapSize)
    {
        return vector.InBounds(new Vector2(-1 * mapSize, -1 * mapSize), new Vector2(mapSize, mapSize));
    }


    public static int GetKeyJustPressed(this InputEvent e)
    {
        var keyEvent = (e as InputEventKey);
        if (keyEvent != null && keyEvent.Pressed && !keyEvent.IsEcho())
        {
            return keyEvent.Scancode;
        }
        return -1;
    }

    public static int GetAlphaOrSpaceJustPressed(this InputEvent e)
    {
        var key = e.GetKeyJustPressed();

        if ((key >= 65 && key <= 90) || key == 32)
        {
            GD.Print(key + "--" + e.AsText());
            return key;
        }
        else return -1;
    }
}