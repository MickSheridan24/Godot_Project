using System;
using Godot;

public class Runtime
{
    public ISelectable currentSelection { get; set; }
    public WizardState WizardState { get; set; }
    public Wizard wizardNode;
    public UIState UIState { get; }
    public UI UINode;
    public Vector2 hoveredCell;
    public Vector2 MousePosition { get; set; }
    public CastManager castManager { get; set; }
    public ITarget currentTarget => targeting.rightTarget;
    public DebugInfo Debug { get; set; }
    public World World { get; private set; }
    public ITarget RightTarget => targeting.rightTarget;
    public ITarget LeftTarget => targeting.leftTarget;

    private TargetingSystem targeting;

    public bool IsCasting;

    public Runtime()
    {
        this.WizardState = new WizardState();
        this.UIState = new UIState();
        targeting = new TargetingSystem();
    }

    public void ClearRightTarget()
    {
        targeting.RemoveRightTarget();
    }

    public void ClearLeftTarget()
    {
        targeting.RemoveLeftTarget();
    }

    public EnemyState CreateEnemyState(Enemy enemy)
    {
        var AI = new PatrolAI(enemy, new Vector2(-1, 0), new Vector2(300, 300));
        return new EnemyState(AI, enemy);
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

    public void RegisterWizard(Wizard wizard)
    {
        wizardNode = wizard;
        targeting.targeter = wizardNode;
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
        wizardNode.destination = wizardNode.Position;
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
