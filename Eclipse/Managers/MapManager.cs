using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Eclipse.Base;
using Eclipse.Base.Struct;
using Eclipse.Components.MapState;
using Eclipse.Backend;
using Eclipse.Components.Environment;

namespace Eclipse.Managers
{
    [AddComponentMenu("Eclipse/Managers/Map")]
    public class MapManager : ManagerBase
    {
        [SerializeField] private Transform MapParentRegister;
        [SerializeField] private Transform EnvironmentParentRegister;
        [SerializeField] private string StartCallingMapID;
        [SerializeField] private List<GameObjectAsset> MapAssets = new List<GameObjectAsset>();
        [SerializeField] private List<GameObjectAsset> skyboxes = new List<GameObjectAsset>();

        private void Start()
        {
            CommandBackend.InputCommand = "world clear";
            CommandBackend.CommandRecordEnter(CommandBackend.CommandEnter());
            CommandBackend.InputCommand = "world spawn " + StartCallingMapID;
            CommandBackend.CommandRecordEnter(CommandBackend.CommandEnter());
            //MapManager.MapControl.SpawnMap(StartCallingMapID);
        }

        /* The worker that control the map manager */
        public class MapControl
        {
            public static bool SpawnMap(string MapID)
            {
                MapManager mm = LinkerHelper.ToManager.GetManagerByType<MapManager>();
                List<GameObjectAsset> assets = mm.GetAsset();
                if(assets == null)
                {
                    EclipseDebug.Log(4, EclipseDebug.DebugState.Warning, new EngineGUIString("地圖尚未有任何註冊", "Map list is empty.").ToString());
                    return false;
                }

                if (mm.GetMapParent() != null)
                {
                    for (int i = 0; i < assets.Count; i++)
                    {
                        if(assets[i].AssetID == MapID)
                        {
                            /* Clean all object in scene */
                            CleanMap();
                            CharacterManager.CharacterControl.KillAll();
                            EntityManager.EntityControl.CleanAllEntity();
                            MapManager.SkyControl.ClearSkyBox();
                            ControlManager.ControllerAssign.CleanController();

                            GameObject g = GameObject.Instantiate(assets[i].Asset, mm.GetMapParent());
                            g.name = "Map: " + assets[i].Asset.name;
                            EclipseDebug.Log(4, EclipseDebug.DebugState.Warning, new EngineGUIString("地圖生成成功: ", "Spawn map successfully: ").ToString() + MapID);
                            return true;
                        }
                    }
                    EclipseDebug.Log(4, EclipseDebug.DebugState.Warning, new EngineGUIString("該 ID 未在地圖管理腳本中註冊: ", "Didn't find the id in map list.").ToString() + MapID);
                    return false;
                }
                EclipseDebug.Log(2, EclipseDebug.DebugState.Warning, new EngineGUIString("地圖生成區尚未註冊.", "Map spawn root didn't register yet.").ToString());
                return false;
            }
            public static bool SetMapRespawnPoint(Vector3 pos)
            {
                if (!IfMapLoad()) {
                    EclipseDebug.Log(2, EclipseDebug.DebugState.Warning, 
                        new EngineGUIString("尚未載入地圖.", "Map is not loaded.").ToString());
                    return false;
                } 
                MapManager mm = LinkerHelper.ToManager.GetManagerByType<MapManager>();
                Transform t = mm.GetMapParent();
                MapState MS = t.transform.GetChild(0).GetComponent<MapState>();
                if (!MS) {
                    EclipseDebug.Log(2, EclipseDebug.DebugState.Warning, 
                        new EngineGUIString("地圖尚未掛載 [地圖狀態] 元件", "Map doesn't have [MapState] component.").ToString());
                    return false;
                } 
                MS.SetPlayerSpawnPoint(pos);
                return true;
            }
            public static void CleanMap()
            {
                Transform t = LinkerHelper.ToManager.GetManagerByType<MapManager>().GetMapParent();
                if(t != null)
                {
                    for(int i = 0; i < t.childCount; i++)
                    {
                        Destroy(t.GetChild(i).gameObject);
                    }
                    return;
                }
                EclipseDebug.Log(2, EclipseDebug.DebugState.Warning, new EngineGUIString("地圖生成區尚未註冊.", "Map spawn root didn't register yet.").ToString());
            }
            public static bool IfMapLoad()
            {
                MapManager MM = LinkerHelper.ToManager.GetManagerByType<MapManager>();
                Transform t = MM.GetMapParent();
                if (!t) return false;
                if (t.childCount == 0) return false;
                return true;
            }
        }
        public class SkyControl
        {
            public static List<GameObjectAsset> GetSkyboxAsset() {
                return LinkerHelper.ToManager.GetManagerByType<MapManager>().GetSkyboxAsset();
            }
            public static float GetCurrentSunDegree()
            {
                Sky sky = GetCurrentSky();
                if (sky == null) return -1.0f;

                float Degree = sky.GetRotationVector();
                return Degree;
            }
            public static bool SetSkyBox(string ID)
            {
                List<GameObjectAsset> skyboxe =
                LinkerHelper.ToManager.GetManagerByType<MapManager>().GetSkyboxAsset();

                for(int i = 0; i < skyboxe.Count; i++)
                {
                    if(skyboxe[i].AssetID == ID)
                    {
                        if(skyboxe[i].Asset != null)
                        {
                            GameObject g =
                            GameObject.Instantiate(skyboxe[i].Asset, LinkerHelper.ToManager.GetManagerByType<MapManager>().GetEnvironmentParent());
                            g.name = "Enviroment: " + skyboxe[i].Asset.name;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return false;
            }
            public static void ClearSkyBox()
            {
                Transform t = LinkerHelper.ToManager.GetManagerByType<MapManager>().GetEnvironmentParent();
                for(int i = 0; i < t.childCount; i++)
                {
                    EnvironmentBase EB = t.GetChild(i).GetComponent<EnvironmentBase>();
                    if (EB)
                    {
                        if (EB.GetEnviromentType() == EnvironmentBase.EnvironmentType.Lighting) Destroy(t.GetChild(i).gameObject);
                    }
                }
            }
            private static Sky GetCurrentSky()
            {
                Transform t = LinkerHelper.ToManager.GetManagerByType<MapManager>().GetEnvironmentParent();
                for (int i = 0; i < t.childCount; i++)
                {
                    Sky sky = t.GetChild(i).GetComponent<Sky>();
                    if (sky) return sky;
                }
                return null;
            }
        }
        public class PhysicsControl
        {
            public static void SetGravity(Vector3 gra)
            {
                Physics.gravity = gra;
            }
            public static void SetGravity(float gra)
            {
                Physics.gravity = new Vector3(0, gra, 0);
            }
        }
        /* Set and Get the variable in this worker */
        #region Setter And Getter
        public Transform GetMapParent() { return MapParentRegister; }
        public Transform GetEnvironmentParent() { return EnvironmentParentRegister; }
        public List<GameObjectAsset> GetAsset() { return MapAssets; }
        public List<GameObjectAsset> GetSkyboxAsset() { return skyboxes; }
        #endregion

        #region Editor Function
        public void AddNew()
        {
            MapAssets.Add(new GameObjectAsset("Empty", null));
        }

        public void DeleteLast()
        {
            if (MapAssets.Count > 0)
                MapAssets.RemoveAt(MapAssets.Count - 1);
        }

        public void AddNewSky()
        {
            skyboxes.Add(new GameObjectAsset("Empty", null));
        }

        public void DeleteLastSky()
        {
            if (skyboxes.Count > 0)
                skyboxes.RemoveAt(skyboxes.Count - 1);
        }
        #endregion
    }

    [CustomEditor(typeof(MapManager))]
    [CanEditMultipleObjects]
    public class MapManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            MapManager Target = (MapManager)target;
            GUIStyle skinT = EditorHelper.TypeOption.GetCustomStyle(16, FontStyle.Normal, TextAnchor.MiddleCenter);
            /* Begining */
            EditorHelper.EditorOption.BeginEclipseEditor(new EngineGUIString("地圖管理腳本", "Map Manager"), serializedObject);
            /* Return */
            EditorHelper.EditorOption.ReturnToEngineManager(new EngineGUIString("你能透過呼叫 ID 的方式去控制地圖", "You Can Control Map By Calling ID"));
            /* Setting and parent register */
            #region Setting
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("地圖設定", "Map Setting").ToString(), skinT);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("MapParentRegister"), new GUIContent(new EngineGUIString("地圖生成區", "Map Root Transform").ToString()));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("EnvironmentParentRegister"), new GUIContent(new EngineGUIString("環境生成區", "Environment Root Transform").ToString()));
            EditorGUILayout.EndVertical();
            #endregion
            /* Start id */
            #region Start ID
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("地圖起始呼叫", "UI Begining").ToString(), skinT);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("StartCallingMapID"), new GUIContent(new EngineGUIString("地圖起始呼叫", "Map Begining ID").ToString()));
            EditorGUILayout.EndVertical();
            #endregion
            /* Register list */
            #region Map Register
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("地圖列表註冊", "Map Register List").ToString(), skinT);
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new EngineGUIString("加入地圖", "Add Map").ToString(), GUILayout.Height(25))) { Target.AddNew(); }
            if (GUILayout.Button(new EngineGUIString("刪除地圖", "Delete Map").ToString(), GUILayout.Height(25))) { Target.DeleteLast(); }
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel++;
            RenderMapList();
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            #endregion
            /* Register list */
            #region Map Register
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("天空列表註冊", "Sky Register List").ToString(), skinT);
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new EngineGUIString("加入天空", "Add Sky").ToString(), GUILayout.Height(25))) { Target.AddNewSky(); }
            if (GUILayout.Button(new EngineGUIString("刪除天空", "Delete Sky").ToString(), GUILayout.Height(25))) { Target.DeleteLastSky(); }
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel++;
            RenderSkyboxList();
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            #endregion
            /* Ending */
            EditorHelper.EditorOption.EndEclipseEditor(serializedObject);
        }

        #region Render Map List
        private void RenderMapList()
        {
            SerializedProperty SP = serializedObject.FindProperty("MapAssets");
            SP.Next(true);
            int MapArray = SP.arraySize;
            SP.Next(true);
            SP.Next(true);
            for (int i = 0; i < MapArray; i++)
            {
                EditorGUILayout.BeginVertical("GroupBox");
                bool show = EditorGUILayout.PropertyField(SP);
                if (show)
                    RenderMapContent(SP.Copy());
                SP.Next(false);
                EditorGUILayout.EndVertical();
            }
        }

        private void RenderMapContent(SerializedProperty SP)
        {
            EditorGUI.indentLevel++;
            SP.Next(true);
            EditorGUILayout.PropertyField(SP, new GUIContent( new EngineGUIString("地圖 ID", "Map ID").ToString()));
            SP.Next(false);
            EditorGUILayout.PropertyField(SP, new GUIContent(new EngineGUIString("地圖物件", "Map Asset").ToString()));
            EditorGUI.indentLevel--;
        }
        #endregion

        #region Render Skybox List
        private void RenderSkyboxList()
        {
            SerializedProperty SP = serializedObject.FindProperty("skyboxes");
            SP.Next(true);
            int MapArray = SP.arraySize;
            SP.Next(true);
            SP.Next(true);
            for (int i = 0; i < MapArray; i++)
            {
                EditorGUILayout.BeginVertical("GroupBox");
                bool show = EditorGUILayout.PropertyField(SP);
                if (show)
                    RenderSkyboxContent(SP.Copy());
                SP.Next(false);
                EditorGUILayout.EndVertical();
            }
        }

        private void RenderSkyboxContent(SerializedProperty SP)
        {
            EditorGUI.indentLevel++;
            SP.Next(true);
            EditorGUILayout.PropertyField(SP, new GUIContent(new EngineGUIString("天空 ID", "Skybox ID").ToString()));
            SP.Next(false);
            EditorGUILayout.PropertyField(SP, new GUIContent(new EngineGUIString("天空物件", "Skybox Asset").ToString()));
            EditorGUI.indentLevel--;
        }
        #endregion
    }
}