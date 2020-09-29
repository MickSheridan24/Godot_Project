using System.Collections.Generic;
using Godot;
using System.Linq;
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
    public static Vector2 GetDirectionToCoords(this Vector2 vector, Vector2 dest)
    {
        return (dest - vector).Normalized();
    }

    public static IEnumerable<Vector2> FillOutCoords(this IEnumerable<Vector2> vectors)
    {
        var toAdd = new List<Vector2>();

        foreach (var v in vectors)
        {
            toAdd.AddRange(v.GetAdjacent());
        }
        toAdd.AddRange(vectors);
        return toAdd.Distinct().ToList();
    }


    public static List<Vector2> GetAdjacent(this Vector2 v)
    {
        return new List<Vector2>{
            v + Vector2.Up,
            v + Vector2.Down,
            v + Vector2.Left,
            v + Vector2.Right,
            v + Vector2.Up + Vector2.Right,
            v + Vector2.Down + Vector2.Left,
            v + Vector2.Left + Vector2.Up,
            v + Vector2.Right + Vector2.Down
        };
    }
    public static Vector2 ProximityTo(this Vector2 vector, Vector2 dest)
    {
        return (dest - vector).Abs();
    }
    public static Vector2 Rounded(this Vector2 vector2)
    {
        var x = (int)vector2.x;
        var y = (int)vector2.y;

        return new Vector2(x, y);
    }

    public static Vector2 ClosestInRange(this Vector2 origin, Vector2 dest, Vector2 length)
    {
        if (origin.Abs() - dest.Abs() < length.Abs())
        {
            return origin;
        }
        else
        {
            return dest + dest.DirectionTo(origin) * length;
        }
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

    public static bool WithinAreaOf(this Vector2 vector, Vector2 dest, Vector2 size)
    {
        return vector.InBounds(dest - size / 2, dest + size / 2);
    }

    public static Vector2 ToVector(this int s)
    {
        return new Vector2(s, s);
    }


    public static bool InMap(this Vector2 vector, int mapSize)
    {
        return vector.InBounds(new Vector2(-1 * mapSize, -1 * mapSize), new Vector2(mapSize, mapSize));
    }

    public static bool RightClickJustPressed(this InputEvent e)
    {
        return (e as InputEventMouseButton)?.ButtonIndex == (int)ButtonList.Right && e.IsPressed() && !e.IsEcho();
    }
    public static bool LeftClickJustPressed(this InputEvent e)
    {
        return (e as InputEventMouseButton)?.ButtonIndex == (int)ButtonList.Left && e.IsPressed() && !e.IsEcho();
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