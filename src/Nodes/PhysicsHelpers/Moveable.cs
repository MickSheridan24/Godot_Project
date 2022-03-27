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

        var dest = subject.destination;

        if (subject.Position != dest)
        {
            var delta = (subject.Position.DirectionTo(dest)) * d * subject.speed;

            if ((dest.Rounded() - (subject.Position + delta).Rounded()).Abs() <= new Vector2(1, 1))
            {
                collision = subject.MoveAndCollide(dest - subject.Position);


                if (subject.Position != dest)
                {
                    subject.Position = dest;
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