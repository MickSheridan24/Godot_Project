using System;
using Godot;
public class Moveable
{
    private IMove subject;
    public Moveable(IMove subject)
    {
        this.subject = subject;
    }

    public void HandleMove()
    {
        if (subject.Position != subject.destination && subject.CanMove())
        {
            if (subject.Position.WithinRange(subject.destination, subject.speed))
            {
                TryMove(subject.destination);
            }
            else
            {
                var dir = subject.Position.DirectionTo(subject.destination);
                TryMove(subject.Position + dir * subject.speed);
            }
        }
    }

    public void TryMove(Vector2 dest)
    {
        subject.Position = dest;
    }
}