using System;
using Godot;
public class Moveable
{
    private IMove subject;
    public bool debug { get; set; }
    public bool moving;
    public Moveable(IMove subject, bool debug = false)
    {
        this.subject = subject;
        this.debug = debug;
        moving = false;
    }

    public void HandleMove(float d)
    {
        if (this.debug)
        {
            GD.Print("Debugging Movement, " + d);
        }
        KinematicCollision2D collision;
        if (subject.Position != subject.destination)
        {
            var delta = (subject.Position.DirectionTo(subject.destination)) * d * subject.speed;

            if ((subject.destination.Rounded() - (subject.Position + delta).Rounded()).Abs() <= new Vector2(1, 1))
            {
                collision = subject.MoveAndCollide(subject.destination - subject.Position);
                if (subject.Position != subject.destination)
                {
                    subject.Position = subject.destination;
                }
                moving = false;
            }
            else
            {
                collision = subject.MoveAndCollide(delta);
                moving = true;
            }
            if (collision != null)
            {
                subject.HandleCollision(collision);
                moving = false;
            }
        }
    }

    public void TryMove(Vector2 dest)
    {
        subject.MoveAndCollide(dest);
    }
}