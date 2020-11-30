using System;
using Godot;

public class InputHandler
{
    private Runtime runtime;

    public InputHandler(Runtime runtime)
    {
        this.runtime = runtime;
    }
    public bool HandleInput(InputEvent @event, Vector2 mousePos, ISelectable effectedNode = null)
    {
        if (@event is InputEventMouseButton)
        {
            var y = 0;
        }

        var inputHandled = true;
        if (@event.LeftClickJustPressed())
        {
            GD.Print("LEFT CLICK: " + effectedNode);
            if (Input.IsKeyPressed((int)KeyList.Space))
            {
                HandleSpaceLeft(@event, mousePos, effectedNode);
            }
            HandleLeftClickEvent(@event, mousePos, effectedNode);
        }
        else if (@event.RightClickJustPressed())
        {
            GD.Print("RIGHT CLICK: " + effectedNode);
            if (Input.IsKeyPressed((int)KeyList.Space))
            {
                HandleSpaceRight(@event, mousePos, effectedNode);
            }
            else HandleRightClickEvent(@event, mousePos, effectedNode);
        }
        else if (@event.GetKeyJustPressed() != 0)
        {
            HandleKeyEvent(@event.GetKeyJustPressed());
        }
        return inputHandled;
    }


    private void HandleKeyEvent(int key)
    {
        if (key == (int)KeyList.Space && !runtime.IsCasting)
        {
            runtime.SelectWizard();
        }
        else if (key == (int)KeyList.Shift)
        {
            runtime.ToggleCasting();
        }
    }

    private void HandleSpaceLeft(InputEvent @event, Vector2 mousePos, ISelectable effectedNode)
    {
        if (runtime.WizardIsSelected())
        {
            runtime.SetLeftTarget(new VectorTarget(mousePos));
        }
        else HandleLeftClickEvent(@event, mousePos, effectedNode);
    }
    private void HandleSpaceRight(InputEvent @event, Vector2 mousePos, ISelectable effectedNode)
    {
        if (runtime.WizardIsSelected())
        {
            runtime.SetRightTarget(new VectorTarget(mousePos));
        }
        else HandleRightClickEvent(@event, mousePos, effectedNode);
    }
    private void HandleRightClickEvent(InputEvent @event, Vector2 mousePos, ISelectable effectedNode)
    {
        if (runtime.currentSelection != null)
        {
            runtime.currentSelection.RightClick(@event as InputEventMouseButton);
        }
    }

    private void HandleLeftClickEvent(InputEvent @event, Vector2 mousePos, ISelectable effectedNode)
    {
        if (effectedNode != null)
        {
            effectedNode.Select();
        }
        else if (!runtime.WizardIsSelected())
        {
            runtime.ClearSelection();

        }
    }
}