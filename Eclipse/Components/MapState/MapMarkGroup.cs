using UnityEditor;
using UnityEngine;
using Eclipse.Base;
using Eclipse.Base.Struct;

namespace Eclipse.Components.MapMark
{
    [AddComponentMenu("Eclipse/Components/Map State/Marks Group")]
    public class MapMarkGroup : ComponentBase
    {
        public void CallingGroup()
        {
            MapMarkBase[] MMB = GetComponentsInChildren<MapMarkBase>();
            for(int i = 0; i < MMB.Length; i++)
            {
                MMB[i].MarkCalling();
            }
        }
    }

    [CustomEditor(typeof(MapMarkGroup))]
    [CanEditMultipleObjects]
    public class MapMarkGroupEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GUIStyle skinT = EditorHelper.TypeOption.GetCustomStyle(12, FontStyle.Normal, TextAnchor.MiddleCenter);
            EditorGUILayout.BeginVertical("HelpBox");
            EditorGUILayout.TextArea(new EngineGUIString("指令集呼叫, 呼叫時批次呼叫子物件指令.",
                "Commands Group Event, Call All The Child Command Objects When Calling").ToString(), skinT);
            EditorGUILayout.TextArea(new EngineGUIString("呼叫函數: CallingGroup().",
                "The Function Name: CallingGroup()").ToString(), skinT);
            EditorGUILayout.EndVertical();
        }
    }
}
