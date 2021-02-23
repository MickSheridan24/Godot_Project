using System;
using Godot;

public class InputHandler
{
    private Runtime runtime;

    private bool dragging;
    public InputHandler(Runtime runtime)
    {
        this.runtime = runtime;
        dragging = false;
    }
    public bool HandleInput(InputEvent @event, Vector2 mousePos, ISelectable effectedNode = null)
    {
        var inputHandled = true;



        if (!dragging)
        {
            if (@event.LeftClickJustPressed())
            {
                if (Input.IsKeyPressed((int)KeyList.Space))
                {
                    HandleSpaceLeft(@event, mousePos, effectedNode);
                }
                else
                {
                    ClearTargets();
                    HandleLeftClickEvent(@event, mousePos, effectedNode);
                }
            }
            else if (@event.RightClickJustPressed())
            {
                if (Input.IsKeyPressed((int)KeyList.Space))
                {
                    HandleSpaceRight(@event, mousePos, effectedNode);
                }
                else
                {
                    ClearTargets();
                    HandleRightClickEvent(@event, mousePos, effectedNode);
                }

            }
            else if (@event.GetKeyJustPressed() != 0)
            {
                HandleKeyEvent(@event.GetKeyJustPressed(), @event);
            }
        }
        if (!dragging && @event is InputEventMouseButton && @event.IsPressed())
        {
            runtime.World.startDrag();
            dragging = true;
        }
        else if (dragging && @event is InputEventMouseButton)
        {
            dragging = false;
            runtime.World.endDrag();
        }
        return inputHandled;
    }

    private void ClearTargets()
    {
        if (runtime.currentSelection is IHaveTarget && !runtime.WizardIsSelected())
        {
            (runtime.currentSelection as IHaveTarget).ClearTargets();
        }
    }

    private void HandleKeyEvent(int key, InputEvent @event)
    {
        if (key == (int)KeyList.W && !runtime.IsCasting)
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
        if (runtime.currentSelection is IHaveTarget)
        {
            var target = effectedNode is ITarget ? effectedNode as ITarget
                                                 : new VectorTarget(mousePos);

            var targeter = runtime.currentSelection as IHaveTarget;
            if (targeter.CanTarget(target))
            {
                targeter.SetLeftTarget(target);
            }
        }
        else HandleLeftClickEvent(@event, mousePos, effectedNode);
    }
    private void HandleSpaceRight(InputEvent @event, Vector2 mousePos, ISelectable effectedNode)
    {
        if (runtime.currentSelection is IHaveTarget)
        {
            var target = effectedNode is ITarget ? effectedNode as ITarget
                                                 : new VectorTarget(mousePos);

            var targeter = runtime.currentSelection as IHaveTarget;
            if (targeter.CanTarget(target))
            {
                targeter.SetRightTarget(target);
            }
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