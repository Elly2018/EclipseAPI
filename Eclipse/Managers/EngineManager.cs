using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Eclipse.Base;
using Eclipse.Backend;
using Eclipse.Base.Struct;

namespace Eclipse.Managers
{
    [AddComponentMenu("Eclipse/Managers/Engine")]
    public class EngineManager : ManagerBase
    {
        public enum EngineGUIStringSelection
        {
            CH, EN
        }
        public static EngineGUIStringSelection stringSelection { set { LinkerHelper.ToManager.GetManagerByType<EngineManager>().SetEngineGUIString(value); }
            get { return LinkerHelper.ToManager.GetManagerByType<EngineManager>().GetEngineGUIString(); } }
        [SerializeField] private EngineGUIStringSelection _stringSelection = EngineGUIStringSelection.EN;
        public EngineGUIStringSelection GetEngineGUIString() { return _stringSelection; }
        public void SetEngineGUIString(EngineGUIStringSelection v) { _stringSelection = v; }

        /* Debug setting */
        [SerializeField] private bool DebugOutput;
        [SerializeField] private int DebugLevel = 1; // 1-5
        [SerializeField] private PermissionBase UserPermission = new PermissionBase();
        /* Language buffer */
        [SerializeField] private string LanguageSelectionBuffer;

        public class EngineControl
        {
            public static PermissionBase GetPermission()
            {
                return LinkerHelper.ToManager.GetManagerByType<EngineManager>().GetPermission();
            }
        }

        private void Start()
        {
            CommandBackend.CommandRecordEnter(new string[1] { "Initialized Eclipse API." });
        }

        private void Update()
        {
            EngineBackend.Running();
        }

        public bool GetDebugOutput()
        {
            return DebugOutput;
        }

        public int GetDebugLevel()
        {
            return DebugLevel;
        }

        public PermissionBase GetPermission()
        {
            return UserPermission;
        }
    }

    [CustomEditor(typeof(EngineManager))]
    [CanEditMultipleObjects]
    public class EngineManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUIStyle skinT = EditorHelper.TypeOption.GetCustomStyle(16, FontStyle.Normal, TextAnchor.MiddleCenter);
            /* Begining */
            EditorHelper.EditorOption.BeginEclipseEditor(new EngineGUIString("引擎主控管理腳本", "Eclipse Engine Manager"), serializedObject);
            /* Button link */
            #region Button Link
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("連接至其他管理腳本", "Link To Other Manager").ToString(), skinT);
            EditorGUILayout.Space();
            if (GUILayout.Button(new EngineGUIString("音源管理腳本", "Audio Manager").ToString(), GUILayout.Height(30))) /* To Audio */
            { EditorHelper.EditorOption.ChangeSelection(LinkerHelper.ToManager.GetManagerObjectByType<AudioManager>()); }
            if (GUILayout.Button(new EngineGUIString("國際語言管理腳本", "Language Manager").ToString(), GUILayout.Height(30))) /* To String */
            { EditorHelper.EditorOption.ChangeSelection(LinkerHelper.ToManager.GetManagerObjectByType<StringManager>()); }
            if (GUILayout.Button(new EngineGUIString("角色管理腳本", "Character Manager").ToString(), GUILayout.Height(30))) /* To State */
            { EditorHelper.EditorOption.ChangeSelection(LinkerHelper.ToManager.GetManagerObjectByType<CharacterManager>()); }
            if (GUILayout.Button(new EngineGUIString("地圖管理腳本", "Map Manager").ToString(), GUILayout.Height(30))) /* To State */
            { EditorHelper.EditorOption.ChangeSelection(LinkerHelper.ToManager.GetManagerObjectByType<MapManager>()); }
            if (GUILayout.Button(new EngineGUIString("介面管理腳本", "UI Manager").ToString(), GUILayout.Height(30))) /* To State */
            { EditorHelper.EditorOption.ChangeSelection(LinkerHelper.ToManager.GetManagerObjectByType<UIManager>()); }
            if (GUILayout.Button(new EngineGUIString("輸入控制管理腳本", "Control Manager").ToString(), GUILayout.Height(30))) /* To State */
            { EditorHelper.EditorOption.ChangeSelection(LinkerHelper.ToManager.GetManagerObjectByType<ControlManager>()); }
            if (GUILayout.Button(new EngineGUIString("實體物件管理腳本", "Entity Manager").ToString(), GUILayout.Height(30))) /* To State */
            { EditorHelper.EditorOption.ChangeSelection(LinkerHelper.ToManager.GetManagerObjectByType<EntityManager>()); }
            if (GUILayout.Button(new EngineGUIString("插件管理腳本", "Plugin Manager").ToString(), GUILayout.Height(30))) /* To State */
            { EditorHelper.EditorOption.ChangeSelection(LinkerHelper.ToManager.GetManagerObjectByType<PluginManager>()); }
            if (GUILayout.Button(new EngineGUIString("狀態管理腳本", "State Manager").ToString(), GUILayout.Height(30))) /* To State */
            { EditorHelper.EditorOption.ChangeSelection(LinkerHelper.ToManager.GetManagerObjectByType<StateManager>()); }
            EditorGUILayout.EndVertical();
            #endregion
            /* Engine gui string type setting */
            #region GUI String
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("引擎 GUI 語言", "GUI language").ToString(), skinT);
            EditorGUILayout.Space();
            EngineManager.stringSelection = (EngineManager.EngineGUIStringSelection)EditorGUILayout.EnumPopup(EngineManager.stringSelection);
            EditorGUILayout.EndVertical();
            #endregion
            /* Debug setting */
            #region Debug Setting
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("Debug 輸出事件設定", "Debug Output Event").ToString(), skinT);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("DebugOutput"), new GUIContent(new EngineGUIString("Debug 是否輸出", "Debug Output").ToString()));
            serializedObject.FindProperty("DebugLevel").intValue =
            EditorGUILayout.IntSlider(new GUIContent(new EngineGUIString("Debug 等級輸出", "Debug Level Output").ToString()), serializedObject.FindProperty("DebugLevel").intValue, 1, 5);
            EditorGUILayout.EndVertical();
            #endregion
            /* Ending */
            EditorHelper.EditorOption.EndEclipseEditor(serializedObject);
        }
    }
}
