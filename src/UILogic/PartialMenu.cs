using Godot;

public class PartialMenu : Container
{
    public IMenuState state { get; set; }
    public void Config(IMenuState state)
    {
        this.state = state;
        state.AddButtons(this);
    }
}