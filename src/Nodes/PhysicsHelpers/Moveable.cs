using System;
using Godot;
public class Moveable
{
    private IMove subject;
    public Moveable(IMove subject)
    {
        this.subject = subject;
    }

    public void HandleMove(float d)
    {
        if (subject.Position != subject.destination && subject.CanMove())
        {
            if (subject.Position.WithinRange(subject.destination, subject.speed * d))
            {
                TryMove(subject.destination);
            }
            else
            {
                var dir = subject.Position.DirectionTo(subject.destination);
                TryMove(dir * subject.speed * d);
            }
        }
    }

    public void TryMove(Vector2 dest)
    {
        subject.MoveAndCollide(dest);
    }
}