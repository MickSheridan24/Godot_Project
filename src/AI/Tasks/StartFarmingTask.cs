using Mavisithor_Beaconizath.src.Interfaces;

public class StartFarmingTask : ITask
{
    private ICanDoLabor actor;
    private Farm farm;

    public StartFarmingTask(ICanDoLabor actor, Farm farm)
    {
        this.actor = actor;
        this.farm = farm;
    }
    public bool CanExecute()
    {
        return actor.Position == farm.Position();
    }

    public void Execute()
    {
        var farmOrder = new FarmOrder(actor.state.player, farm);

        var tickOrder = new TickOrder()
        {
            order = farmOrder,
            ticks = 1,
            defaultTicks = 1,
            complete = false
        };

        var continuousTask = new ContinuousTask(() => actor.Position == farm.Position(), tickOrder, actor.state.tickHandler);
        actor.state.tickHandler.AddOrder(tickOrder);
        actor.state.continuousActionHandler.Add(continuousTask);
    }
}