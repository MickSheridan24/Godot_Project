
public class TargetingSystem
{
    public ICaster targeter { get; set; }

    public ITarget target { get; private set; }



    public void SetTarget(ITarget target)
    {
        this.target = target;
    }


    public void RemoveTarget()
    {
        target = null;
    }
}
