using System;
using Godot;


public class Cottage : IStructure, IMenuState
{
    private StructureNode node;

    public string Name => "Cottage";
    public string Description => "Just a boring old cottage";
    private Texture text = GD.Load<Texture>("res://assets/Cottage.png");


    private PackedScene snMenuButton => (PackedScene)ResourceLoader.Load("res://scenes/MenuButton.tscn");

    public void ConfigureNode(StructureNode structureNode)
    {
        this.node = structureNode;
        OverrideSpriteColor();
    }

    public void RightClick(Vector2 position)
    {
        return;
    }

    private void OverrideSpriteColor()
    {
        var theme = new SpriteTheme();
        var defaultColor = theme.cEnemy;
        node.sprite.Modulate = theme.cCottage;
    }

    public void AddButtons(PartialMenu structurePartialMenu)
    {
        var createNPC = snMenuButton.Instance() as MenuButton;
        createNPC.Hotkey = "Q";
        createNPC.Action = () => TryCreateNPC();
        (structurePartialMenu as StructurePartialMenu).AddButton(createNPC);
    }

    private bool TryCreateNPC()
    {
        GD.Print("CREATING NPC");
        return true;
    }
}
