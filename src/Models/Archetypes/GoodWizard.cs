
public class GoodWizard : WizardState, IWizardState
{
    public GoodWizard(Wizard node) : base(node)
    {
        Name = "Bartholemew the Good Wizard";
        Description = "He's a nice guy";
    }

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
