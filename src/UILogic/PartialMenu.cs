using System;
using System.Collections.Generic;
using Godot;

public class PartialMenu : Container, IHaveRuntime
{
	public IMenuState state { get; set; }
	public Runtime runtime => GetParent<IHaveRuntime>().runtime;
	public List<MenuButton> Buttons { get; set; }
	public UITheme theme => new UITheme();

	public MenuButton button1 => GetNode<MenuButton>("Button1");
	public MenuButton button2 => GetNode<MenuButton>("Button2");
	public MenuButton button3 => GetNode<MenuButton>("Button3");
	public MenuButton button4 => GetNode<MenuButton>("Button4");

	public void Config(IMenuState state)
	{
		this.state = state;
	}

	public override void _Ready()
	{
		Buttons = new List<MenuButton>();
		button1.Hotkey = "Q";
		button2.Hotkey = "W";
		button3.Hotkey = "E";
		button4.Hotkey = "R";
	}

	public void ResetButtons()
	{
		if (state != null)
		{
			state?.ConfigureButtons(this);
		}
		else ClearButtons();
	}

	private void ClearButtons()
	{
		button1.Visible = false;
		button2.Visible = false;
		button3.Visible = false;
		button4.Visible = false;
	}
}
