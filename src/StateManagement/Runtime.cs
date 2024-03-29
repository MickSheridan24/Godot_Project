using System;
using System.Collections.Generic;
using Godot;

public class Runtime
{
    public ISelectable currentSelection { get; set; }
    public IWizardState WizardState { get; set; }
    public Wizard wizardNode;
    public UIState UIState { get; }
    public UI UINode;
    public Vector2 hoveredCell;
    public Vector2 MousePosition { get; set; }
    public CastManager castManager { get; set; }
    public DebugInfo Debug { get; set; }
    public World World { get; private set; }

    public ITarget RightTarget => targeting?.rightTarget;
    public ITarget LeftTarget => targeting?.leftTarget;

    private List<IEnemySpawner> spawners = new List<IEnemySpawner>();

    private TargetingSystem targeting => currentSelection?.Targeting;
    public PlayerState playerState;
    public PlayerState enemyPlayerState;

    internal void RegisterTower(StructureNode cottage)
    {
        this.Tower = cottage;
    }

    public int MasterClock = 0;
    public EntityFinder entityFinder;
    public EntityRegistry entityRegistry;
    public UIEffectHandler uIEffectHandler { get; private set; }
    public StructureNode Tower { get; set; }

    public bool IsCasting;
    public InputHandler inputHandler;

    public Runtime()
    {

        this.UIState = new UIState();

        playerState = new PlayerState();


        enemyPlayerState = new PlayerState();
        uIEffectHandler = new UIEffectHandler();

        inputHandler = new InputHandler(this);
        entityRegistry = new EntityRegistry();
        entityFinder = new EntityFinder(World, entityRegistry);
    }

    internal bool IsSelected(ISelectable selectable)
    {
        if (currentSelection == selectable)
        {
            return true;
        }
        else if (currentSelection is GroupSelection)
        {
            var group = currentSelection as GroupSelection;
            return group.Has(selectable);
        }
        return false;
    }

    public void ClearRightTarget()
    {
        targeting.RemoveRightTarget();
    }

    public void ClearLeftTarget()
    {
        targeting.RemoveLeftTarget();
    }

    internal void DeSelect()
    {
        if (currentSelection != null)
        {
            currentSelection.DeSelect();
        }
    }

    public EnemyState CreateEnemyState(Enemy enemy)
    {
        var AI = new SmartZombieAI(enemy, Tower, 1250);
        var state = new EnemyState(AI, enemy);
        AI.state = state;
        return state;
    }

    internal void RemoveEntity(BaseActorNode node)
    {


        entityRegistry.Remove(node);
        if (currentSelection == node)
        {
            currentSelection = null;
        }
        if (LeftTarget == node)
        {
            ClearLeftTarget();
        }

        if (RightTarget == node)
        {
            ClearRightTarget();
        }
    }

    public void SetLeftTarget(ITarget t)
    {
        targeting.SetLeftTarget(t);
    }

    internal void SetRightTarget(ITarget t)
    {
        targeting.SetRightTarget(t);
    }


    public void SelectWizard()
    {
        currentSelection = wizardNode;
    }
    public bool WizardIsSelected()
    {
        return currentSelection == wizardNode;
    }

    public void RegisterWizard(Wizard wizard)
    {
        wizardNode = wizard;
        WizardState = new GoodWizard(wizardNode);
        playerState.wizardState = WizardState;
        playerState.bank = WizardState.InitResourceBank();
    }


    internal void RegisterNPC(NPC npc)
    {
        npc.state = new NPCState(npc);
        entityRegistry.Add(npc);
    }

    internal void RegisterEnemy(Enemy enemy)
    {
        enemy.state = CreateEnemyState(enemy);
        entityRegistry.Add(enemy);
    }

    internal void RegisterEnemySpawner(IEnemySpawner spawner)
    {
        this.spawners.Add(spawner);
        spawner.World = this.World;
        spawner.Registry = this.entityRegistry;
        spawner.Runtime = this;
    }



    public void TimerUp()
    {
        MasterClock++;
        Spawn();
    }

    internal void Spawn()
    {
        var toRemove = new List<IEnemySpawner>();
        foreach (var spawner in spawners)
        {
            if (spawner.IsExhausted(MasterClock))
            {
                toRemove.Add(spawner);
            }
            else if (spawner.CanSpawn(MasterClock))
            {
                spawner.Spawn(GetRandomEnemySpawn(2000, 1000));
            }
        }
        toRemove.ForEach(tr => spawners.Remove(tr));
    }

    private Vector2 GetRandomEnemySpawn(int minDistance, int spread)
    {
        var origin = Vector2.Zero;

        var rand = new Random();

        var magnitudeX = minDistance + rand.Next(spread);
        var magnitudeY = minDistance + rand.Next(spread);

        var dirX = rand.Next(2) == 0 ? -1 : 1;
        var dirY = rand.Next(2) == 0 ? -1 : 1;

        return new Vector2(magnitudeX * dirX, magnitudeY * dirY);
    }

    public void SetSelection(ISelectable selected)
    {
        DeSelect();

        if (WizardIsSelected() && selected != wizardNode)
        {
            targeting.Clear();
        }
        this.currentSelection = selected;
    }

    public void ClearSelection()
    {
        SetSelection(null);
    }

    public void RegisterUI(UI ui)
    {
        UINode = ui;
    }

    public Vector2? currentTargetPosition()
    {
        if (targeting.rightTarget != null)
        {
            return targeting.rightTarget.GetTargetPosition();
        }
        else
        {
            return null;
        }
    }

    public void InitCast()
    {
        castManager = new CastManager(WizardState);
        wizardNode.destination = wizardNode.GlobalPosition;
    }
    public void ToggleCasting()
    {
        ToggleCasting(!IsCasting);
    }
    public void ToggleCasting(bool value)
    {
        IsCasting = value;
        if (IsCasting)
        {
            InitCast();
        }
        if (!IsCasting)
        {
            castManager = null;
        }
    }

    public void RegisterWorld(World world)
    {
        this.World = world;
        uIEffectHandler.world = world;
    }

    public void SetWorldTarget(Vector2 position)
    {
        targeting.SetRightTarget(new VectorTarget(position));
    }

    public void SetTarget(ITarget t)
    {
        targeting.SetRightTarget(t);
    }

    public ISpell GetCurrentSpell()
    {
        return castManager?.currentSpell ?? null;
    }
    public SpellCastResult UpdateCast(string key)
    {
        var result = castManager.UpdateCast(key);
        if (result.complete)
        {
            CompleteCast(result);
            uIEffectHandler.CompleteCast();
        }
        else
        {
            uIEffectHandler.HintSpell(result.Spell, wizardNode);
        }
        return result;
    }

    private void CompleteCast(SpellCastResult result)
    {
        GD.Print(result.Spell.name + " Cast");
        ToggleCasting(false);
        result.Spell.Cast(wizardNode);
    }
}
