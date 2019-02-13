using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Eclipse.Base.Struct;
using Eclipse.Managers;

namespace Eclipse
{
    public class EditorHelper
    {
        /* The useful function for editor gui */
        public class EditorOption
        {
            /* Set the current scene to dirty */
            public static void MarkDirtyCheck()
            {
                if (GUI.changed)
                {
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                }
            }
            /* Moving the selection of unity hierarchy */
            public static void ChangeSelection(GameObject sceneObj)
            {
                if (sceneObj == null)
                {
                    EclipseDebug.Log(2, EclipseDebug.DebugState.Warning, "此管理腳本並不存在.");
                    return;
                }
                Selection.activeGameObject = sceneObj;
            }
            /* Return to main manager */
            public static void ReturnToEngineManager(EngineGUIString helpboxinfo)
            {
                EditorGUILayout.BeginVertical("GroupBox");
                if (GUILayout.Button(new EngineGUIString("返回主控管理腳本", "Return To Menu").ToString(), GUILayout.Height(30))) /* To State */
                { EditorHelper.EditorOption.ChangeSelection(LinkerHelper.ToManager.GetManagerObjectByType<EngineManager>()); }
                if(helpboxinfo != null)
                    EditorGUILayout.HelpBox(helpboxinfo.ToString(), MessageType.Info);
                EditorGUILayout.EndVertical();
            }
            public static void ReturnToEngineManager(string helpboxinfo)
            {
                EditorGUILayout.BeginVertical("GroupBox");
                if (GUILayout.Button(new EngineGUIString("返回主控管理腳本", "Return To Menu").ToString(), GUILayout.Height(30))) /* To State */
                { EditorHelper.EditorOption.ChangeSelection(LinkerHelper.ToManager.GetManagerObjectByType<EngineManager>()); }
                if (helpboxinfo != "")
                    EditorGUILayout.HelpBox(helpboxinfo, MessageType.Info);
                EditorGUILayout.EndVertical();
            }
            /* Return to plugin manager */
            public static void ReturnToPluginManager(EngineGUIString helpboxinfo)
            {
                EditorGUILayout.BeginVertical("GroupBox");
                if (GUILayout.Button(new EngineGUIString("返回插件管理腳本", "Return To Plugin Manager").ToString(), GUILayout.Height(30))) /* To State */
                { EditorHelper.EditorOption.ChangeSelection(LinkerHelper.ToManager.GetManagerObjectByType<PluginManager>()); }
                if (helpboxinfo != null)
                    EditorGUILayout.HelpBox(helpboxinfo.ToString(), MessageType.Info);
                EditorGUILayout.EndVertical();
            }
            public static void ReturnToPluginManager(string helpboxinfo)
            {
                EditorGUILayout.BeginVertical("GroupBox");
                if (GUILayout.Button(new EngineGUIString("返回插件管理腳本", "Return To Plugin Manager").ToString(), GUILayout.Height(30))) /* To State */
                { EditorHelper.EditorOption.ChangeSelection(LinkerHelper.ToManager.GetManagerObjectByType<PluginManager>()); }
                if (helpboxinfo != "")
                    EditorGUILayout.HelpBox(helpboxinfo, MessageType.Info);
                EditorGUILayout.EndVertical();
            }
            /* Begin Title */
            public static void BeginEclipseEditor(EngineGUIString Title, SerializedObject SO)
            {
                GUIStyle skinT = EditorHelper.TypeOption.GetCustomStyle(16, FontStyle.Normal, TextAnchor.MiddleCenter);
                SO.Update();
                EditorGUILayout.BeginVertical("HelpBox");
                EditorGUILayout.Space();
                EditorGUILayout.LabelField(Title.ToString(), skinT);
            }
            public static void BeginEclipseEditor(string Title, SerializedObject SO)
            {
                GUIStyle skinT = EditorHelper.TypeOption.GetCustomStyle(16, FontStyle.Normal, TextAnchor.MiddleCenter);
                SO.Update();
                EditorGUILayout.BeginVertical("HelpBox");
                EditorGUILayout.Space();
                EditorGUILayout.LabelField(Title, skinT);
            }
            public static void BeginEclipseEditor(EngineGUIString Title, EngineGUIString helpboxinfo, SerializedObject SO)
            {
                GUIStyle skinT = EditorHelper.TypeOption.GetCustomStyle(16, FontStyle.Normal, TextAnchor.MiddleCenter);
                SO.Update();
                EditorGUILayout.BeginVertical("HelpBox");
                EditorGUILayout.Space();
                EditorGUILayout.LabelField(Title.ToString(), skinT);
                EditorGUILayout.HelpBox(helpboxinfo.ToString(), MessageType.Info);
            }
            public static void BeginEclipseEditor(string Title, EngineGUIString helpboxinfo, SerializedObject SO)
            {
                GUIStyle skinT = EditorHelper.TypeOption.GetCustomStyle(16, FontStyle.Normal, TextAnchor.MiddleCenter);
                SO.Update();
                EditorGUILayout.BeginVertical("HelpBox");
                EditorGUILayout.Space();
                EditorGUILayout.LabelField(Title, skinT);
                EditorGUILayout.HelpBox(helpboxinfo.ToString(), MessageType.Info);
            }
            public static void BeginEclipseEditor(string Title, string helpboxinfo, SerializedObject SO)
            {
                GUIStyle skinT = EditorHelper.TypeOption.GetCustomStyle(16, FontStyle.Normal, TextAnchor.MiddleCenter);
                SO.Update();
                EditorGUILayout.BeginVertical("HelpBox");
                EditorGUILayout.Space();
                EditorGUILayout.LabelField(Title, skinT);
                EditorGUILayout.HelpBox(helpboxinfo, MessageType.Info);
            }
            /* End */
            public static void EndEclipseEditor(SerializedObject SO)
            {
                EditorGUILayout.EndVertical();
                SO.ApplyModifiedProperties();
                EditorHelper.EditorOption.MarkDirtyCheck();
            }
        }
        /* Useful function of gui content  */
        public class TypeOption
        {
            /* Get the custom use style */
            public static GUIStyle GetCustomStyle(int _fsize, FontStyle _style, TextAnchor _alignment)
            {
                GUIStyle skin = new GUIStyle();
                skin.fontStyle = _style;
                skin.fontSize = _fsize;
                skin.alignment = _alignment;
                return skin;
            }
        }
        /* Mark help */
        public class MarkOption
        {
            public static void MarksOrder(Transform transform)
            {
                GUIStyle skinT = EditorHelper.TypeOption.GetCustomStyle(16, FontStyle.Normal, TextAnchor.MiddleCenter);
                EditorGUILayout.BeginVertical("GroupBox");
                EditorGUILayout.LabelField(new EngineGUIString("呼叫順序: ", "Calling Order: ").ToString() + transform.GetSiblingIndex().ToString(), skinT);
                EditorGUILayout.EndVertical();
            }

            public static void MarksSceneGUI(string _name, Vector3 pos)
            {
                GUIStyle skinT = EditorHelper.TypeOption.GetCustomStyle(16, FontStyle.Bold, TextAnchor.MiddleCenter);
                skinT.richText = true;
                Handles.Label(pos + new Vector3(0, 1.5f, 0), "<color=#ff0000>" + _name + "</color>", skinT);
                Handles.DrawWireCube(pos, new Vector3(1, 1, 1));
            }
        }
    }
}