using System;
public class FarmOrder : IOrder
{
    private PlayerState player;

    private Farm farm;
    private Func<bool> condition;

    public FarmOrder(PlayerState player, Farm farm, Func<bool> condition)
    {
        this.player = player;
        this.farm = farm;
        this.condition = condition;
    }

    public bool Execute()
    {
        if (condition())
        {
            player.bank.food += 1;
            return true;
        }
        return false;

    }
}