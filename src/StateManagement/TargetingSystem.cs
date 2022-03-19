
using System;

public class TargetingSystem
{
    public ITarget rightTarget { get; set; }
    public ITarget leftTarget { get; set; }


    public void SetRightTarget(ITarget target)
    {
        if (rightTarget != null)
        {
            rightTarget.DeHighlightTarget();
        }
        rightTarget = target;
        rightTarget.HighlightTarget();
    }

    public void SetLeftTarget(ITarget target)
    {
        if (leftTarget != null)
        {
            leftTarget.DeHighlightTarget();
        }
        leftTarget = target;
        rightTarget.HighlightTarget();
    }


    public void RemoveLeftTarget()
    {
        if (leftTarget != null)
        {
            leftTarget.DeHighlightTarget();
        }
        leftTarget = null;
    }
    public void RemoveRightTarget()
    {
        if (rightTarget != null)
        {
            rightTarget.DeHighlightTarget();
        }
        rightTarget = null;
    }

    public void Clear()
    {
        RemoveLeftTarget();
        RemoveRightTarget();
    }
}
