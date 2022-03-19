using Godot;
using System;

public class MenuButton : Button, IHaveSize
{
	private bool mouseIn;
	public string Hotkey { get; set; }
	public Func<bool> Action { get; internal set; }

	public Runtime runtime => GetParent<IHaveRuntime>().runtime;

	public Vector2 size => size;

	public Vector2 Position => Position;
	public Vector2 GlobalPosition => GlobalPosition;

	public UIHighlight highlight => GetNode<UIHighlight>("Highlight");

	public int flashFrames;

	public override void _Ready()
	{
		mouseIn = false;

		flashFrames = 0;
		Connect("pressed", this, "_button_pressed");

	}

	public override void _Input(InputEvent @event)
	{
		if (!runtime.WizardIsSelected() && Visible && @event is InputEventKey && @event.AsText() == Hotkey && @event.IsPressed() && !@event.IsEcho())
		{
			Click();
		}
	}
	public override void _PhysicsProcess(float delta)
	{
		Text = Hotkey;
		flashFrames--;
		if (flashFrames <= 0)
		{
			flashFrames = 0;
			highlight.color = new Color("#000000");
			highlight.Visible = false;
		}
	}

	public void Click()
	{
		if (Action != null)
		{
			GD.Print("CLICK BUTTON");
			if (Action())
			{
				FlashColor(new UITheme().cGreen);
			}
			else
			{
				FlashColor(new UITheme().cRed);
			}

		}
		GetTree().SetInputAsHandled();

	}

	private void FlashColor(Color color)
	{
		flashFrames = 20;
		highlight.color = color;
		highlight.Visible = true;
	}

	private void _button_pressed()
	{
		GD.Print("CLICK");
		Click();
	}

}






