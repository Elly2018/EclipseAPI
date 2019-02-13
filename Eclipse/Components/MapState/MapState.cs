using UnityEngine;
using UnityEngine.Events;
using Eclipse.Base;
using System.Collections.Generic;
using UnityEditor;
using Eclipse.Managers;
using Eclipse.Base.Struct;

namespace Eclipse.Components.MapState
{
    [AddComponentMenu("Eclipse/Components/Map State/Map State")]
    public class MapState : ComponentBase
    {
        [SerializeField] private bool AllowRespawn;
        [SerializeField] private Vector3 PlayerSpawnPoint = new Vector3(0, 0, 0);
        [SerializeField] private UnityEvent Begining;

        public void Start()
        {
            BeginingCall();
        }

        public void SetPlayerSpawnPoint(Vector3 pos)
        {
            PlayerSpawnPoint = pos;
        }

        /* First respawn will happen, no matter what allow respawn setting is. */
        private void BeginingCall()
        {
            Begining.Invoke();
        }

        public void PlayerRespawn()
        {
            if (!AllowRespawn)
            {
                EclipseDebug.Log(4, EclipseDebug.DebugState.Warning, new EngineGUIString("這個地圖不允許重生.", "This map is not allow respawn.").ToString());
                return;
            }
            CharacterManager.CharacterControl.SpawnPlayerCharacter(PlayerSpawnPoint);
        }
    }

    [CustomEditor(typeof(MapState))]
    [CanEditMultipleObjects]
    public class MapStateEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GUIStyle skinT = EditorHelper.TypeOption.GetCustomStyle(16, FontStyle.Normal, TextAnchor.MiddleCenter);
            /* Begining */
            EditorHelper.EditorOption.BeginEclipseEditor(new EngineGUIString("世界狀態", "World State").ToString(), serializedObject);
            /* Setting */
            #region Setting
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("AllowRespawn"),
                new GUIContent(new EngineGUIString("許可重生", "Allow Respawn").ToString()));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("PlayerSpawnPoint"),
                new GUIContent(new EngineGUIString("重生地點", "Respawn Position").ToString()));
            EditorGUILayout.EndVertical();
            #endregion
            /* Begining events */
            #region Begining
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Begining"),
                new GUIContent(new EngineGUIString("起始事件", "Begining Events").ToString()));
            EditorGUILayout.EndVertical();
            #endregion
            /* Ending */
            EditorHelper.EditorOption.EndEclipseEditor(serializedObject);
        }
    }
}
