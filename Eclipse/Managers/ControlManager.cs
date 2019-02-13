using UnityEditor;
using UnityEngine;
using Eclipse.Base;
using Eclipse.Base.Struct;
using Eclipse.Components.Controller;
using Eclipse.Backend;

namespace Eclipse.Managers
{
    [AddComponentMenu("Eclipse/Managers/Control")]
    public class ControlManager : ManagerBase
    {
        [SerializeField] private ControllerBase CurrentController;
        [SerializeField] private ControlKeycodeBase ConsoleKey;

        private void Awake()
        {
            ConsoleKey = new ControlKeycodeBase();
            ConsoleKey.pluginControls = PluginManager.GetAllPluginControl();
            CommandBackend.CommandRecordEnter(new string[1] { "Plugin control keyset loaded." });
        }
        /* Controller worker */
        public class ControllerAssign
        {
            public static bool CheckPairController(ControllerBase CB)
            {
                ControlManager CM = LinkerHelper.ToManager.GetManagerByType<ControlManager>();
                if (CM.GetControllerBase() == null || CB == null) return false;
                if (CM.GetControllerBase() == CB) return true;
                return false;
            }

            public static void RegisterPlayerController(ControllerBase CB)
            {
                if (!CB) return;
                ControlManager CM = LinkerHelper.ToManager.GetManagerByType<ControlManager>();
                CM.SetControllerBase(CB);
            }

            public static void CleanController()
            {
                ControlManager CM = LinkerHelper.ToManager.GetManagerByType<ControlManager>();
                CM.SetControllerBase(null);
            }

            public static ControllerBase GetCurrentController()
            {
                ControlManager CM = LinkerHelper.ToManager.GetManagerByType<ControlManager>();
                return CM.GetControllerBase();
            }
        }
        /* The worker handle the control keycode assign */
        public class ControlAssign {

            public static ControlKeycodeBase GetControlKeycode()
            {
                return LinkerHelper.ToManager.GetManagerByType<ControlManager>().GetControlKeycodeBase();
            }

            public static void SetControlKeycode(ControlKeycodeBase CKB)
            {
                LinkerHelper.ToManager.GetManagerByType<ControlManager>().SetControlKeycodeBase(CKB);
            }
        }
        /* Basic getter and setter */
        #region Setter And Getter
        public void SetControlKeycodeBase(ControlKeycodeBase CKB) { ConsoleKey = CKB; }
        public void SetControllerBase(ControllerBase CB) { CurrentController = CB; }
        public ControlKeycodeBase GetControlKeycodeBase() { return ConsoleKey; }
        public ControllerBase GetControllerBase() { return CurrentController; }
        #endregion
    }

    [CustomEditor(typeof(ControlManager))]
    [CanEditMultipleObjects]
    public class ControlManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GUIStyle skinT = EditorHelper.TypeOption.GetCustomStyle(16, FontStyle.Normal, TextAnchor.MiddleCenter);
            ControlKeycodeBase CKB = ControlManager.ControlAssign.GetControlKeycode();
            /* Begining */
            EditorHelper.EditorOption.BeginEclipseEditor(new EngineGUIString("輸入控制管理腳本", "Control Manager"), serializedObject);
            /* Return */
            EditorHelper.EditorOption.ReturnToEngineManager("");
            /* Player controller */
            #region Player Controller
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("玩家的輸入器", "Player Controller").ToString(), skinT);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("CurrentController"), new GUIContent(new EngineGUIString("控制器","Controller").ToString()));
            EditorGUILayout.EndVertical();
            #endregion
            /* Movement key list */
            #region Movement Key
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("方向輸入", "Movement").ToString(), skinT);
            EditorGUILayout.Space();
            for (int i = 0; i < CKB.movementControl.movementKeyList.Length; i++)
            {
                CKB.movementControl.movementKeyList[i].keyCode = (KeyCode)EditorGUILayout.EnumPopup(EngineManager.stringSelection == EngineManager.EngineGUIStringSelection.CH ?
                    CKB.movementControl.movementKeyList[i].GUIKeyCodeString : CKB.movementControl.movementKeyList[i].keyCodeID,
                    CKB.movementControl.movementKeyList[i].keyCode);
            }
            EditorGUILayout.EndVertical();
            #endregion
            /* Action key list */
            #region Action Key
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("動作輸入", "Action").ToString(), skinT);
            EditorGUILayout.Space();
            for(int i = 0; i < CKB.actionControl.actionKeyList.Length; i++)
            {
                CKB.actionControl.actionKeyList[i].keyCode = (KeyCode)EditorGUILayout.EnumPopup(EngineManager.stringSelection == EngineManager.EngineGUIStringSelection.CH ?
                    CKB.actionControl.actionKeyList[i].GUIKeyCodeString : CKB.actionControl.actionKeyList[i].keyCodeID,
                    CKB.actionControl.actionKeyList[i].keyCode);
            }
            EditorGUILayout.EndVertical();
            #endregion
            /* Advence key list */
            #region Advence Key
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("進階輸入", "Advence").ToString(), skinT);
            EditorGUILayout.Space();
            for (int i = 0; i < CKB.advenceControl.advenceKeyList.Length; i++)
            {
                CKB.advenceControl.advenceKeyList[i].keyCode = (KeyCode)EditorGUILayout.EnumPopup( EngineManager.stringSelection == EngineManager.EngineGUIStringSelection.CH ?
                    CKB.advenceControl.advenceKeyList[i].GUIKeyCodeString : CKB.advenceControl.advenceKeyList[i].keyCodeID,
                    CKB.advenceControl.advenceKeyList[i].keyCode);
            }
            EditorGUILayout.EndVertical();
            #endregion
            /* Plugin key list */
            #region Plugin Key
            for (int i = 0; i < CKB.pluginControls.Length; i++)
            {
                EditorGUILayout.BeginVertical("GroupBox");
                EditorGUILayout.LabelField(CKB.pluginControls[i].PluginControlName, skinT);
                EditorGUILayout.Space();
                for (int j = 0; j < CKB.pluginControls[i].keycodeStructs.Count; j++)
                {
                    CKB.pluginControls[i].keycodeStructs[j].keyCode = (KeyCode)EditorGUILayout.EnumPopup(EngineManager.stringSelection == EngineManager.EngineGUIStringSelection.CH ?
                        CKB.pluginControls[i].keycodeStructs[j].GUIKeyCodeString : CKB.pluginControls[i].keycodeStructs[j].keyCodeID,
                        CKB.pluginControls[i].keycodeStructs[j].keyCode);
                }
                EditorGUILayout.EndVertical();
            }

            #endregion
            /* Ending */
            ControlManager.ControlAssign.SetControlKeycode(CKB);
            EditorHelper.EditorOption.EndEclipseEditor(serializedObject);
        }
    }
}
