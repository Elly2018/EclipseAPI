using System.Collections.Generic;
using UnityEngine;

namespace Eclipse.Base.Struct
{
    [System.Serializable]
    public class ControlKeycodeBase
    {
        public MovementControl movementControl = new MovementControl();
        public ActionControl actionControl = new ActionControl();
        public AdvenceControl advenceControl = new AdvenceControl();
        public PluginControl[] pluginControls = new PluginControl[0];

        [System.Serializable]
        public class EditorControlKeycodeStruct
        {
            public string keyCodeID;
            public string GUIKeyCodeString;
            public KeyCode keyCode;

            public EditorControlKeycodeStruct(string keyCodeID, string gUIKeyCodeString, KeyCode keyCode)
            {
                this.keyCodeID = keyCodeID;
                GUIKeyCodeString = gUIKeyCodeString;
                this.keyCode = keyCode;
            }
        }

        [System.Serializable]
        public class ControlBase {

        }

        [System.Serializable]
        public class MovementControl : ControlBase
        {
            public EditorControlKeycodeStruct[] movementKeyList = new EditorControlKeycodeStruct[9];

            public MovementControl()
            {
                movementKeyList[0] = new EditorControlKeycodeStruct("Forward", "前進", KeyCode.W);
                movementKeyList[1] = new EditorControlKeycodeStruct("Backward", "後退", KeyCode.S);
                movementKeyList[2] = new EditorControlKeycodeStruct("Leftward", "左行", KeyCode.A);
                movementKeyList[3] = new EditorControlKeycodeStruct("Rightward", "右行", KeyCode.D);
                movementKeyList[4] = new EditorControlKeycodeStruct("LookLeftBack", "往左後方看", KeyCode.Q);
                movementKeyList[5] = new EditorControlKeycodeStruct("LookRightBack", "往右後方看", KeyCode.E);
                movementKeyList[6] = new EditorControlKeycodeStruct("Jump", "跳躍", KeyCode.Space);
                movementKeyList[7] = new EditorControlKeycodeStruct("Running", "跑步", KeyCode.LeftShift);
                movementKeyList[8] = new EditorControlKeycodeStruct("Squart", "蹲下", KeyCode.LeftControl);
            }
        }

        [System.Serializable]
        public class ActionControl : ControlBase
        {
            public EditorControlKeycodeStruct[] actionKeyList = new EditorControlKeycodeStruct[1];

            public ActionControl()
            {
                actionKeyList[0] = new EditorControlKeycodeStruct("Use", "使用", KeyCode.F);
            }
        }

        [System.Serializable]
        public class AdvenceControl : ControlBase
        {
            public EditorControlKeycodeStruct[] advenceKeyList = new EditorControlKeycodeStruct[1];

            public AdvenceControl()
            {
                advenceKeyList[0] = new EditorControlKeycodeStruct("Console", "控制台", KeyCode.F1);
            }
        }

        [System.Serializable]
        public class PluginControl : ControlBase
        {
            public string PluginControlName = "";
            public List<EditorControlKeycodeStruct> keycodeStructs = new List<EditorControlKeycodeStruct>();
        }

        /* Get the in-build engine control object by id */
        public EditorControlKeycodeStruct GetEditorControlKeycodeStructByID<T>(string ID) where T : ControlBase
        {
            if (typeof(T) == typeof(ActionControl))
            {
                for (int i = 0; i < actionControl.actionKeyList.Length; i++)
                {
                    if (actionControl.actionKeyList[i].keyCodeID == ID) return actionControl.actionKeyList[i];
                }
            }
            else if (typeof(T) == typeof(AdvenceControl))
            {
                for (int i = 0; i < advenceControl.advenceKeyList.Length; i++)
                {
                    if (advenceControl.advenceKeyList[i].keyCodeID == ID) return advenceControl.advenceKeyList[i];
                }
            }
            else if (typeof(T) == typeof(MovementControl))
            {
                for (int i = 0; i < movementControl.movementKeyList.Length; i++)
                {
                    if (movementControl.movementKeyList[i].keyCodeID == ID) return movementControl.movementKeyList[i];
                }
            }
            return null;
        }

        /* Get the plugin control object by id */
        public EditorControlKeycodeStruct GetPluginEditorControlKeycodeStructByID(string PluginID, string ID)
        {
            for(int i = 0; i < pluginControls.Length; i++)
            {
                if(pluginControls[i].PluginControlName == PluginID)
                {
                    for(int j = 0; j < pluginControls[i].keycodeStructs.Count; j++)
                    {
                        if (pluginControls[i].keycodeStructs[j].keyCodeID == ID) return pluginControls[i].keycodeStructs[j];
                    }
                }
            }
            return null;
        }
    }
}
