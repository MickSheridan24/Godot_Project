
using System;

public class TargetingSystem
{
    public ICaster targeter { get; set; }

    public ITarget rightTarget { get; set; }
    public ITarget leftTarget { get; set; }


    public void SetRightTarget(ITarget target)
    {
        rightTarget = target;
    }

    public void SetLeftTarget(ITarget target)
    {
        leftTarget = target;
    }



    public void RemoveLeftTarget()
    {
        leftTarget = null;
    }
    public void RemoveRightTarget()
    {
        rightTarget = null;
    }

    public void Clear()
    {
        RemoveLeftTarget();
        RemoveRightTarget();
    }
}
