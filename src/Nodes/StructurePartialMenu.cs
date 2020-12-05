using Godot;
using System;
using System.Collections.Generic;

public class StructurePartialMenu : PartialMenu
{
    public List<MenuButton> Buttons { get; set; }

    public UITheme theme => new UITheme();




    public override void _Ready()
    {
        Buttons = new List<MenuButton>();
    }



    public void AddButton(MenuButton b)
    {
        Buttons.Add(b);
        AddChild(b);
        // b.RectPosition = new Vector2(-50 + Buttons.Count * 75, 25);
    }

}
