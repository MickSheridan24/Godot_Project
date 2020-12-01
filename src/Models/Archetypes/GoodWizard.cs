
public class GoodWizard : WizardState, IWizardState
{
    public GoodWizard(Wizard node) : base(node)
    {

    }

    public string Name => "Bartholemew the Good Wizard";

    public string Description => "He's a nice guy";

    public IResourceBank InitResourceBank()
    {
        return new ResourceBank()
        {
            food = 0,
            wealth = 0,
            insight = 0,
            knowledge = 0,
            pop = 10,
            foodDisplay = "Food",
            popDisplay = "Population"
        };
    }
}
