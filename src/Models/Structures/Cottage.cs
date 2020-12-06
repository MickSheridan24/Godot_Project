using System;
using Godot;


public class Cottage : IStructure, IMenuState
{
    private StructureNode node;

    public string Name => "Cottage";
    public string Description => "Just a boring old cottage";
    private Texture text = GD.Load<Texture>("res://assets/Cottage.png");


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

    private bool TryCreateNPC()
    {
        GD.Print("CREATING NPC");
        return true;
    }

    public void ConfigureButtons(PartialMenu partialMenu)
    {
        var createNPC = partialMenu.button1;
        createNPC.Hotkey = "Q";
        createNPC.Action = () => TryCreateNPC();

        createNPC.Visible = true;
        partialMenu.button2.Visible = true;
        partialMenu.button3.Visible = true;
        partialMenu.button4.Visible = true;

    }
}
