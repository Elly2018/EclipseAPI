using Eclipse.Base.Struct;
using UnityEditor;
using UnityEngine;

namespace Eclipse.Base
{
    public class PluginManagerEditorBase : Editor
    {
        public string Title;

        public virtual void RenderPluginContent() { }

        public override void OnInspectorGUI()
        {
            GUIStyle skinT = EditorHelper.TypeOption.GetCustomStyle(16, FontStyle.Normal, TextAnchor.MiddleCenter);
            /* Begining */
            EditorHelper.EditorOption.BeginEclipseEditor(Title, serializedObject);
            /* Return */
            EditorHelper.EditorOption.ReturnToPluginManager("");
            /* Content */
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("插件調整", "Pluging Setting").ToString(), skinT);
            EditorGUILayout.Space();
            RenderPluginContent();
            EditorGUILayout.EndVertical();
            /* Ending */
            EditorHelper.EditorOption.EndEclipseEditor(serializedObject);
        }
    }
}
