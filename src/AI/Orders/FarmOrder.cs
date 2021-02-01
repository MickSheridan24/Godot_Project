public class FarmOrder : IOrder
{
    private PlayerState player;

    private Farm farm;

    public FarmOrder(PlayerState player, Farm farm)
    {
        this.player = player;
        this.farm = farm;
    }

    public bool Execute()
    {
        player.bank.food += 1;
        return true;
    }
}