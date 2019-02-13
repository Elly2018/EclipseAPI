using UnityEditor;
using UnityEngine;
using Eclipse.Base;
using Eclipse.Base.Struct;

namespace Eclipse.Managers
{
    [AddComponentMenu("Eclipse/Managers/State")]
    public class StateManager : ManagerBase
    {

    }

    [CustomEditor(typeof(StateManager))]
    [CanEditMultipleObjects]
    public class StateManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GUIStyle skinT = EditorHelper.TypeOption.GetCustomStyle(16, FontStyle.Normal, TextAnchor.MiddleCenter);
            float Mv = LinkerHelper.ToManager.GetManagerByType<AudioManager>().GetMusicVolume();
            float Sv = LinkerHelper.ToManager.GetManagerByType<AudioManager>().GetSFXVolume();
            string Ltag = LinkerHelper.ToManager.GetManagerByType<StringManager>().GetLanguageTag();
            /* Begining */
            EditorHelper.EditorOption.BeginEclipseEditor(new EngineGUIString("狀態管理腳本", "State Manager"), serializedObject);
            /* Return */
            EditorHelper.EditorOption.ReturnToEngineManager("");
            /* State viewer */
            #region Viewer
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField("狀態顯示", skinT);
            GUI.enabled = false;
            EditorGUILayout.Slider("音樂音量", Mv, 0, 1.0f);
            EditorGUILayout.Slider("音效音量", Sv, 0, 1.0f);
            EditorGUILayout.TextField("語言選擇", Ltag);
            GUI.enabled = true;
            EditorGUILayout.EndVertical();
            #endregion
            /* Ending */
            EditorHelper.EditorOption.EndEclipseEditor(serializedObject);
        }
    }
}
