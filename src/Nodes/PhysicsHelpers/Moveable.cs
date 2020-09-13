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
        if (subject.Position != subject.destination)
        {
            if (subject.Position.WithinRange(subject.destination, subject.speed))
            {
                subject.Position = subject.destination;
            }
            else
            {
                var dir = subject.Position.DirectionTo(subject.destination);
                subject.Position += dir * subject.speed;
            }
        }
    }
}