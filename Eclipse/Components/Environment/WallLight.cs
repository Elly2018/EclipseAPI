using UnityEditor;
using UnityEngine;
using Eclipse.Base;
using Eclipse.Managers;
using Eclipse.Base.Struct;

namespace Eclipse.Components.Environment
{
    [AddComponentMenu("Eclipse/Components/Environment/Wall Light")]
    public class WallLight : ComponentBase
    {
        public enum TriggerType
        {
            Events = 0, Sun_Detection = 1, Always_On = 2, Always_Off = 3
        }
        [SerializeField] private TriggerType TT = TriggerType.Always_On;
        [SerializeField] private GameObject LightProb;
        [SerializeField] private bool Begining;
        [SerializeField] private float CompareDegree;
        [SerializeField] private Material OnMat;
        [SerializeField] private Material OffMat;

        private void Start()
        {
            if(TT == TriggerType.Events)
            {
                if (LightProb) LightProb.SetActive(Begining);
            }else if(TT == TriggerType.Always_On)
            {
                if (LightProb) LightProb.SetActive(true);
                if (OnMat) GetComponent<Renderer>().material = OnMat;
            }
            else if (TT == TriggerType.Always_Off)
            {
                if (LightProb) LightProb.SetActive(false);
                if (OffMat) GetComponent<Renderer>().material = OnMat;
            }

        }

        private void Update()
        {
            if (TT != TriggerType.Sun_Detection) return;
            if(MapManager.SkyControl.GetCurrentSunDegree() > CompareDegree && LightProb && LightProb.activeInHierarchy == false)
            {
                LightProb.SetActive(true);
                if (OnMat) GetComponent<Renderer>().material = OnMat;
            }
            if(MapManager.SkyControl.GetCurrentSunDegree() < CompareDegree && LightProb && LightProb.activeInHierarchy == true)
            {
                LightProb.SetActive(false);
                if (OnMat) GetComponent<Renderer>().material = OffMat;
            }
        }

        public void LightTrigger(bool active)
        {
            if (TT != TriggerType.Events) return;
            if (LightProb) LightProb.SetActive(active);

            if (active) { if (OnMat) GetComponent<Renderer>().material = OnMat; }
            else { if (OnMat) GetComponent<Renderer>().material = OffMat; }
        }

    }

    [CustomEditor(typeof(WallLight))]
    [CanEditMultipleObjects]
    public class WallLightEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorHelper.EditorOption.BeginEclipseEditor(new EngineGUIString("牆燈", "Wall Light").ToString(), serializedObject);
            /* Wall light type */
            #region Trigger Type
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("TT"), new GUIContent(new EngineGUIString("牆燈觸發方式", "Wall Light Trigger Type").ToString()));
            EditorGUILayout.EndVertical();
            #endregion
            /* Event */
            #region Event
            if (serializedObject.FindProperty("TT").enumValueIndex == 0)
            {
                EditorGUILayout.BeginVertical("GroupBox");
                EditorGUILayout.PropertyField(serializedObject.FindProperty("LightProb"), new GUIContent(new EngineGUIString("燈實體", "Light GameObject").ToString()));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("OnMat"), new GUIContent(new EngineGUIString("開啟材質", "Material On").ToString()));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("OffMat"), new GUIContent(new EngineGUIString("關閉材質", "Material Off").ToString()));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Begining"), new GUIContent(new EngineGUIString("起始狀態", "Begining State").ToString()));

                EditorGUILayout.EndVertical();
            }
            #endregion
            /* Sun detection */
            #region Sun Detection
            if (serializedObject.FindProperty("TT").enumValueIndex == 1)
            {
                EditorGUILayout.BeginVertical("GroupBox");
                EditorGUILayout.PropertyField(serializedObject.FindProperty("LightProb"), new GUIContent(new EngineGUIString("燈實體", "Light GameObject").ToString()));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("OnMat"), new GUIContent(new EngineGUIString("開啟材質", "Material On").ToString()));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("OffMat"), new GUIContent(new EngineGUIString("關閉材質", "Material Off").ToString()));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("CompareDegree"), new GUIContent(new EngineGUIString("太陽角度", "Compare Sun Degree").ToString()));
                EditorGUILayout.EndVertical();
            }
            #endregion
            /* Always */
            #region Always
            if (serializedObject.FindProperty("TT").enumValueIndex == 2)
            {
                EditorGUILayout.BeginVertical("GroupBox");
                EditorGUILayout.PropertyField(serializedObject.FindProperty("LightProb"), new GUIContent(new EngineGUIString("燈實體", "Light GameObject").ToString()));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("OnMat"), new GUIContent(new EngineGUIString("開啟材質", "Material On").ToString()));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("OffMat"), new GUIContent(new EngineGUIString("關閉材質", "Material Off").ToString()));
                EditorGUILayout.EndVertical();
            }
            if (serializedObject.FindProperty("TT").enumValueIndex == 3)
            {
                EditorGUILayout.BeginVertical("GroupBox");
                EditorGUILayout.PropertyField(serializedObject.FindProperty("LightProb"), new GUIContent(new EngineGUIString("燈實體", "Light GameObject").ToString()));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("OnMat"), new GUIContent(new EngineGUIString("開啟材質", "Material On").ToString()));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("OffMat"), new GUIContent(new EngineGUIString("關閉材質", "Material Off").ToString()));
                EditorGUILayout.EndVertical();
            }
            #endregion
            EditorHelper.EditorOption.EndEclipseEditor(serializedObject);
        }
    }
}
