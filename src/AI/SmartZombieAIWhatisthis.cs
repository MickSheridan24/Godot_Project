using System;
using System.Collections.Generic;
using Godot;

public class SmartZombieAI : IAI
{
	public BaseActorNode node { get; set; }
	public IMove move { get; }
	public BaseActorState state;
	private ITarget finalTarget;
	private float range;
	private ITarget currentTarget;

	public SmartZombieAI(BaseActorNode node, ITarget target, float range)
	{
		this.node = node;
		this.state = node.state;
		this.finalTarget = target;
		this.move = node as IMove;
		this.range = range;
	}

	public IOrder Request(EnemyState enemyState, float d)
	{
		if (finalTarget.IsFreed() || finalTarget == null)
		{
			return new StandByOrder();
		}
		else
		{
			if (currentTarget == null || currentTarget.IsFreed())
			{
				//Try to find new Distraction 
				currentTarget = findNewDistraction();
				if (currentTarget != null)
				{
					//Success, set destination
					state.taskQueue.Add(currentTarget.GetHostileTask(node), "ATTACK");
					return new SetDestinationOrder(move, currentTarget.GetTargetPosition());
				}
				else if (move.destination != finalTarget.GetTargetPosition())
				{
					//Failure set final target
					state.taskQueue.Add(new DoNothingTask(), "ATTACK");
					return new SetDestinationOrder(move, finalTarget.GetTargetPosition());
				}
				else if (!move.Position.WithinRange(finalTarget.GetTargetPosition(), state.range.current.ToVector()))
				{
					//Failure, continue towards final target
					return new MoveOrder(move, d);
				}
			}
			else if (!move.Position.WithinRange(currentTarget.GetTargetPosition(), state.range.current.ToVector()))
			{
				//Move towards current distraction
				return new MoveOrder(move, d);
			}

		}
		return new StandByOrder();
	}

	private ITarget findNewDistraction()
	{
		var targets = state?.runtime?.entityFinder?.FindMinions(move.Position, range.ToVector()) ?? new List<NPC>();

		if (targets.Count > 0)
		{
			return targets[0];
		}
		return null;
	}
}
