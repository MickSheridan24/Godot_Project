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
        if (subject.Position != subject.destination)
        {
            var delta = (subject.Position.DirectionTo(subject.destination)) * d * subject.speed;

            if ((subject.destination.Rounded() - (subject.Position + delta).Rounded()).Abs() <= new Vector2(1, 1))
            {
                subject.MoveAndCollide(subject.destination - subject.Position);
                if (subject.Position != subject.destination)
                {
                    subject.Position = subject.destination;
                }
            }
            else
            {
                subject.MoveAndCollide(delta);
            }
        }
    }

    public void TryMove(Vector2 dest)
    {
        subject.MoveAndCollide(dest);
    }
}