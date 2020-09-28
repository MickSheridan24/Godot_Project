
public class ReEnableFallOrder : IOrder
{
    private IElevatable elevatable;

    public ReEnableFallOrder(IElevatable e)
    {
        this.elevatable = e;
    }

    public bool Execute()
    {
        elevatable.isFallDisabled = false;
        return true;
    }
}
