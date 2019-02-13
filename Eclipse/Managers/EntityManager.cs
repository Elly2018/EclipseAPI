using UnityEditor;
using UnityEngine;
using Eclipse.Base;
using Eclipse.Base.Struct;
using System.Collections.Generic;

namespace Eclipse.Managers
{
    [AddComponentMenu("Eclipse/Managers/Spawner")]
    public class EntityManager : ManagerBase
    {
        [SerializeField] private Transform EntityRoot;
        [SerializeField] private List<EntityAsset> assets = new List<EntityAsset>();

        public class EntityControl
        {
            /* Spawn entity to the position we pass in */
            public static bool SpawnEntity(string ID, Vector3 pos)
            {
                EntityManager EM = LinkerHelper.ToManager.GetManagerByType<EntityManager>();
                Transform root = EM.GetRoot();
                if (!MapManager.MapControl.IfMapLoad())
                {
                    EclipseDebug.Log(2, EclipseDebug.DebugState.Error, new EngineGUIString("尚未載入任何地圖.", "Map is not load.").ToString());
                    return false;
                }
                if (!root)
                {
                    EclipseDebug.Log(2, EclipseDebug.DebugState.Error, new EngineGUIString("實體物件生成區尚未註冊.", "Entity root is not register.").ToString());
                    return false;
                }
                EntityAsset targetEntity = EM.GetEntityAssetByID(ID);
                if (targetEntity == null || targetEntity.Asset == null)
                {
                    EclipseDebug.Log(2, EclipseDebug.DebugState.Warning, new EngineGUIString("實體物件 ID 未找到: ", "Entity ID not find: ").ToString() + ID);
                    return false;
                }

                GameObject g = GameObject.Instantiate(targetEntity.Asset, root);
                g.name = targetEntity.AssetID;
                return true;
            }
            /* Spawn enetity for player */
            public static bool SpawnEntity(string ID)
            {
                EntityManager EM = LinkerHelper.ToManager.GetManagerByType<EntityManager>();
                Transform root = EM.GetRoot();
                if (!MapManager.MapControl.IfMapLoad())
                {
                    EclipseDebug.Log(2, EclipseDebug.DebugState.Error, new EngineGUIString("尚未載入任何地圖.", "Map is not load.").ToString());
                    return false;
                }
                if (!root)
                {
                    EclipseDebug.Log(2, EclipseDebug.DebugState.Error, new EngineGUIString("實體物件生成區尚未註冊.", "Entity root is not register.").ToString());
                    return false;
                }
                EntityAsset targetEntity = EM.GetEntityAssetByID(ID);
                if (targetEntity == null || targetEntity.Asset == null)
                {
                    EclipseDebug.Log(2, EclipseDebug.DebugState.Warning, new EngineGUIString("實體物件 ID 未找到: ", "Entity ID not find: ").ToString() + ID);
                    return false;
                }

                Transform playerTrans = CharacterManager.CharacterControl.GetPlayerCharacter().transform;
                if (!playerTrans)
                {
                    EclipseDebug.Log(2, EclipseDebug.DebugState.Warning, new EngineGUIString("在場景中找不到玩家角色.", "Cannot find player in scene").ToString());
                    return false;
                }

                GameObject g = GameObject.Instantiate(targetEntity.Asset, root);
                g.transform.position = ControlManager.ControllerAssign.GetCurrentController()._Cursor;
                g.name = "Entity: " + targetEntity.AssetID;
                return true;
            }
            /* Kill all entity in scene */
            public static void CleanAllEntity()
            {
                EntityManager EM = LinkerHelper.ToManager.GetManagerByType<EntityManager>();
                Transform root = EM.GetRoot();
                if (!root)
                {
                    EclipseDebug.Log(2, EclipseDebug.DebugState.Error, new EngineGUIString("實體物件生成區尚未註冊.", "Entity root is not register.").ToString());
                    return;
                }
                for(int i = 0; i < root.childCount; i++)
                {
                    Destroy(root.GetChild(i).gameObject);
                }
                EclipseDebug.Log(2, EclipseDebug.DebugState.Log, new EngineGUIString("成功刪除所有實體物件.", "Successfully delete all entity in game.").ToString());
            }
        }
        /* Editor function */
        #region Editor Function
        public void AddNew()
        {
            assets.Add(new EntityAsset("Empty", null, true));
        }
        public void DeleteLast()
        {
            if (assets.Count > 0)
                assets.RemoveAt(assets.Count - 1);
        }
        #endregion
        /* Objects setter and getter */
        #region Setter And Getter
        public Transform GetRoot() { return EntityRoot; }
        public EntityAsset GetEntityAssetByID(string ID)
        {
            for(int i = 0; i < assets.Count; i++)
            {
                if (assets[i].AssetID == ID) return assets[i];
            }
            return null;
        }
        #endregion
    }

    [CustomEditor(typeof(EntityManager))]
    [CanEditMultipleObjects]
    public class EntityManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EntityManager EM = (EntityManager)target;
            GUIStyle skinT = EditorHelper.TypeOption.GetCustomStyle(16, FontStyle.Normal, TextAnchor.MiddleCenter);
            /* Begining */
            EditorHelper.EditorOption.BeginEclipseEditor(new EngineGUIString("實體物件管理腳本", "Entity Manager"), serializedObject);
            /* Return */
            EditorHelper.EditorOption.ReturnToEngineManager("");
            /* Entity spawn root */
            #region Entity Spawn Root
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("實體物件生成區", "Spawn Root Register").ToString(), skinT);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("EntityRoot"), new GUIContent(new EngineGUIString("生成區", "Spawn Root").ToString()));
            EditorGUILayout.EndVertical();
            #endregion
            /* List render */
            #region Entity Object Register
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("實體物件列表註冊", "Entity List Register").ToString(), skinT);
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new EngineGUIString("加入實體物件", "Add Entity").ToString(), GUILayout.Height(25))) { EM.AddNew(); }
            if (GUILayout.Button(new EngineGUIString("刪除實體物件", "Delete Entity").ToString(), GUILayout.Height(25))) { EM.DeleteLast(); }
            EditorGUILayout.EndHorizontal();
            RenderList();
            EditorGUILayout.EndVertical();
            #endregion
            /* Ending */
            EditorHelper.EditorOption.EndEclipseEditor(serializedObject);
        }

        private void RenderList()
        {
            SerializedProperty SP = serializedObject.FindProperty("assets");
            SP.Next(true);
            int EntityArray = SP.arraySize;
            SP.Next(true);
            SP.Next(true);
            for(int i = 0; i < EntityArray; i++)
            {
                EditorGUILayout.BeginVertical("GroupBox");
                EditorGUI.indentLevel++;
                bool show = EditorGUILayout.PropertyField(SP);
                if (show)
                    RenderEntityContent(SP.Copy());
                SP.Next(false);
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
        }

        private void RenderEntityContent(SerializedProperty SP)
        {
            EditorGUI.indentLevel++;
            SP.Next(true);
            EditorGUILayout.PropertyField(SP);
            SP.Next(false);
            EditorGUILayout.PropertyField(SP);
            SP.Next(false);
            EditorGUILayout.PropertyField(SP);
            EditorGUI.indentLevel--;
        }
    }
}
