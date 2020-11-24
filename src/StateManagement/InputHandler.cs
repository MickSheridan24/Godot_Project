using System;
using Godot;

public class InputHandler
{
    private Runtime runtime;

    public InputHandler(Runtime runtime)
    {
        this.runtime = runtime;
    }
    public void HandleInput(InputEvent @event, Vector2 mousePos)
    {
        if (@event.LeftClickJustPressed())
        {
            HandleLeftClickEvent(@event, mousePos);
        }
        else if (@event.RightClickJustPressed())
        {
            if ((@event as InputEventMouseButton).Doubleclick)
            {
                HandleDoubleRight(@event, mousePos);
            }
            else HandleRightClickEvent(@event, mousePos);
        }
        else if (@event.GetKeyJustPressed() != 0)
        {
            HandleKeyEvent(@event.GetKeyJustPressed());
        }
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


    private void HandleDoubleRight(InputEvent @event, Vector2 mousePos)
    {
        if (runtime.WizardIsSelected())
        {
            runtime.currentSelection.RightClick(@event as InputEventMouseButton);
        }
        else HandleRightClickEvent(@event, mousePos);
    }
    private void HandleRightClickEvent(InputEvent @event, Vector2 mousePos)
    {
        if (runtime.WizardIsSelected())
        {
            runtime.SetRightTarget(new VectorTarget(mousePos));

        }
        else if (runtime.currentSelection != null)
        {
            runtime.currentSelection.RightClick(@event as InputEventMouseButton);
        }
    }

    private void HandleLeftClickEvent(InputEvent @event, Vector2 mousePos)
    {
        if (runtime.WizardIsSelected())
        {
            runtime.SetLeftTarget(new VectorTarget(mousePos));
        }

        //DID I CLICK ON SOMETHING SELECTABLE? 

    }
}