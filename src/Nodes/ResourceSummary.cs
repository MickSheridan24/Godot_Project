using Godot;
using System;

public class ResourceSummary : ColorRect
{
    public Runtime runtime => GetParent<IHaveRuntime>().runtime;
    public IResourceBank bank => runtime.playerState.bank;
    public Label food => GetNode<Label>("Food");
    public Label wealth => GetNode<Label>("Wealth");
    public Label pop => GetNode<Label>("Population");
    public Label knowledge => GetNode<Label>("Knowledge");
    public Label insight => GetNode<Label>("Insight");


    public override void _Ready()
    {
        var theme = new UITheme();
        Modulate = theme.cPrimary;
        food.Set("custom_colors/font_color", theme.cRed);
        wealth.Set("custom_colors/font_color", theme.cRed);
        pop.Set("custom_colors/font_color", theme.cRed);
        knowledge.Set("custom_colors/font_color", theme.cRed);
        insight.Set("custom_colors/font_color", theme.cRed);
    }
    public override void _Process(float d)
    {
        food.Text = $"{bank.foodDisplay}: {bank.food}";
        wealth.Text = $"Wealth: {bank.wealth}";
        pop.Text = $"{bank.popDisplay}: {bank.pop}/{bank.supply}";
        knowledge.Text = $"Knowledge: {bank.knowledge}";
        insight.Text = $"Insight: {bank.insight}";
    }


}
