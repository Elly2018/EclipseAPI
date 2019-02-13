using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Eclipse.Base;
using Eclipse.Base.Struct;
using Eclipse.Components.Command;

namespace Eclipse.Managers
{
    [AddComponentMenu("Eclipse/Managers/Plugin")]
    public class PluginManager : ManagerBase
    {
        public static PluginManagerBase GetPluginObject(string PluginID)
        {
            PluginManager PM = LinkerHelper.ToManager.GetManagerByType<PluginManager>();
            PluginManagerBase[] MB = PM.GetPluginManagers();
            for(int i = 0; i < MB.Length; i++)
            {
                if (MB[i].GetManagerName() == PluginID) return MB[i];
            }
            return null;
        }

        public static CommandBase[] GetAllPluginCommand()
        {
            PluginManager PM = LinkerHelper.ToManager.GetManagerByType<PluginManager>();
            List<CommandBase> CB = new List<CommandBase>();
            PluginManagerBase[] MB = PM.GetPluginManagers();
            for (int i = 0; i < MB.Length; i++)
            {
                CommandBase[] CommandBuffer = MB[i].GetCommands();
                if (CommandBuffer != null)
                {
                    for(int j = 0; j < CommandBuffer.Length; j++)
                    {
                        CB.Add(CommandBuffer[j]);
                    }
                }
            }
            return CB.ToArray();
        }

        public static ControlKeycodeBase.PluginControl[] GetAllPluginControl()
        {
            PluginManager PM = LinkerHelper.ToManager.GetManagerByType<PluginManager>();
            List<ControlKeycodeBase.PluginControl> CB = new List<ControlKeycodeBase.PluginControl>();
            PluginManagerBase[] MB = PM.GetPluginManagers();
            for (int i = 0; i < MB.Length; i++)
            {
                ControlKeycodeBase.PluginControl[] ControlBuffer = MB[i].GetPluginControl();
                if (ControlBuffer != null)
                {
                    for (int j = 0; j < ControlBuffer.Length; j++)
                    {
                        CB.Add(ControlBuffer[j]);
                    }
                }
            }
            return CB.ToArray();
        }

        public PluginManagerBase[] GetPluginManagers()
        {
            return GetComponentsInChildren<PluginManagerBase>();
        }
    }

    [CustomEditor(typeof(PluginManager))]
    [CanEditMultipleObjects]
    public class PluginManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            PluginManager PM = (PluginManager)target;
            GUIStyle skinT = EditorHelper.TypeOption.GetCustomStyle(16, FontStyle.Normal, TextAnchor.MiddleCenter);
            /* Begining */
            EditorHelper.EditorOption.BeginEclipseEditor(new EngineGUIString("插件管理腳本", "Plugin Manager"), serializedObject);
            /* Return */
            EditorHelper.EditorOption.ReturnToEngineManager("");
            /* Plugin managers selection list */
            #region Plugin Managers list
            PluginManagerBase[] MB = PM.GetPluginManagers();
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("插件管理腳本列表", "Plugins List").ToString(), skinT);
            EditorGUILayout.Space();
            for (int i = 0; i < MB.Length; i++) {
                if (GUILayout.Button(MB[i].GetManagerName(), GUILayout.Height(30))) /* To State */
                { EditorHelper.EditorOption.ChangeSelection(MB[i].gameObject); }
            }
            EditorGUILayout.EndVertical();
            #endregion
            /* Ending */
            EditorHelper.EditorOption.EndEclipseEditor(serializedObject);
        }
    }
}
