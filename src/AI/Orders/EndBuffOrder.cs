
public class EndBuffOrder : IOrder
{
    private Stat stat;

    public EndBuffOrder(Stat stat)
    {
        this.stat = stat;
    }

    public bool Execute()
    {
        return true;
    }
}
