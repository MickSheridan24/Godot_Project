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
        var inputHandled = true;
        if (@event.LeftClickJustPressed())
        {
            if (Input.IsKeyPressed((int)KeyList.Space))
            {
                HandleSpaceLeft(@event, mousePos, effectedNode);
            }
            else
            {
                HandleLeftClickEvent(@event, mousePos, effectedNode);
            }
        }
        else if (@event.RightClickJustPressed())
        {
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
            var target = effectedNode is ITarget ? effectedNode as ITarget
                                                 : new VectorTarget(mousePos);

            runtime.SetLeftTarget(target);
        }
        else HandleLeftClickEvent(@event, mousePos, effectedNode);
    }
    private void HandleSpaceRight(InputEvent @event, Vector2 mousePos, ISelectable effectedNode)
    {
        if (runtime.WizardIsSelected())
        {
            var target = effectedNode is ITarget ? effectedNode as ITarget
                                                 : new VectorTarget(mousePos);

            runtime.SetRightTarget(target);
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
        else
        {
            runtime.ClearSelection();
        }
    }
}