using System.Collections.Generic;
using System.Linq;
using Mavisithor_Beaconizath.src.Interfaces;

public class StartFarmingTask : ITask
{
    private ICanDoLabor actor;
    private Farm farm;

    public IEnumerable<IOrder> Orders { get; set; }

    private TickOrder cooldownTick;

    public StartFarmingTask(ICanDoLabor actor, Farm farm)
    {
        this.actor = actor;
        this.farm = farm;


        cooldownTick = new TickOrder()
        {
            order = new StandByOrder(() => true),
            ticks = 10,
            defaultTicks = 10,
            complete = true
        };

        Orders = new List<IOrder>(){
            new StandByOrder(() => !cooldownTick.complete),
            new FarmOrder(actor.state.player, farm, CanExecute),
            new SetDestinationOrder(actor as IMove, farm.Position(), () => actor is IMove)
        };
    }

    public bool Execute()
    {
        return Orders.Any(o => o.Execute());
    }

    public bool IsComplete()
    {
        return false;
    }

    private bool CanExecute()
    {
        return actor.GlobalPosition == farm.Position();
    }
}