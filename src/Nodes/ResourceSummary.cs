using Godot;
using System;

public class ResourceSummary : Control
{
    public Runtime runtime => GetParent<IHaveRuntime>().runtime;
    public IResourceBank bank => runtime.playerState.bank;

    public ColorRect panel => GetNode<ColorRect>("Panel");
    public Label food => GetNode<Label>("Panel/Food");
    public Label wealth => GetNode<Label>("Panel/Wealth");
    public Label pop => GetNode<Label>("Panel/Population");
    public Label knowledge => GetNode<Label>("Panel/Knowledge");
    public Label insight => GetNode<Label>("Panel/Insight");


    public override void _Ready()
    {
        var theme = new UITheme();
        panel.Modulate = theme.cPrimary;
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
