using Eclipse.Managers;
using Eclipse.Base.Struct;
using UnityEngine;
using Eclipse.Components.Controller;
using Eclipse.Base;

namespace Eclipse.Backend
{
    public class EngineBackend
    {
        /* The engine backend */
        public static void Running()
        {
            ConsoleEvent();
        }

        private static void ConsoleEvent()
        {
            ControlKeycodeBase.EditorControlKeycodeStruct consoleKey = ControlManager.ControlAssign.GetControlKeycode().
                GetEditorControlKeycodeStructByID<ControlKeycodeBase.AdvenceControl>("Console");
            if (consoleKey != null)
            {
                if (Input.GetKeyDown(consoleKey.keyCode))
                {
                    ControllerBase CB = ControlManager.ControllerAssign.GetCurrentController();
                    if (UIManager.UIManagerControl.SpawnCanvas("Console", true))
                    {
                        if (CB) { CB.MovementControl = true; }
                    }
                    else
                    {
                        if (CB) { CB.MovementControl = false; }
                    }
                }
            }
        }
    }
}
