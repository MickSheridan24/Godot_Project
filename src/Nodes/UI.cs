using Godot;
using System;

public class UI : Control, IHaveRuntime
{
    public Runtime runtime => GetParent().GetParent<IHaveRuntime>().runtime;
    public UIState state => runtime.UIState;
    public UITheme theme;
    public override void _Ready()
    {
        theme = new UITheme();

    }

}
