using UnityEditor;
using UnityEngine;
using Eclipse.Base;
using Eclipse.Base.Struct;

namespace Eclipse.Components.Environment
{
    [AddComponentMenu("Eclipse/Components/Environment/Sky")]
    public class Sky : EnvironmentBase
    {
        [SerializeField] private Material SkyMat;
        [SerializeField] private Light Sun;

        private void Awake()
        {
            environmentType = EnvironmentType.Lighting;
        }

        private void OnValidate()
        {
            environmentType = EnvironmentType.Lighting;
        }

        private void Start()
        {
            if(SkyMat)
                RenderSettings.skybox = SkyMat;
            if(Sun)
                RenderSettings.sun = Sun;
        }

        public float GetRotationVector()
        {
            if (Sun == null) return -1.0f;
            return 180.0f - Mathf.Abs(Vector3.Angle(Sun.transform.rotation * Vector3.forward, new Vector3(0, 1, 0)));
        }
    }

    [CustomEditor(typeof(Sky))]
    [CanEditMultipleObjects]
    public class SkyEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            /* Begining */
            EditorHelper.EditorOption.BeginEclipseEditor(new EngineGUIString("天空", "Sky"), serializedObject);
            /* Setting */
            #region Setting
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("SkyMat"), new GUIContent(new EngineGUIString("天空材質", "Sky Material").ToString()));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Sun"), new GUIContent(new EngineGUIString("陽光", "Sun").ToString()));
            EditorGUILayout.EndVertical();
            #endregion
            /* Ending */
            EditorHelper.EditorOption.EndEclipseEditor(serializedObject);
        }
    }
}
