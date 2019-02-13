using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Eclipse.Base;
using Eclipse.Base.Struct;
using Eclipse.Components.Camera;
using Eclipse.Components.Character;
using Eclipse.Components.Controller;

namespace Eclipse.Managers
{
    [AddComponentMenu("Eclipse/Managers/Character")]
    public class CharacterManager : ManagerBase
    {
        [SerializeField] private string DefaultCharacter;
        [SerializeField] private CameraBase DefaultCamera;
        [SerializeField] private List<GameObjectAsset> CharacterList = new List<GameObjectAsset>();
        [SerializeField] private Transform CharacterSpawnRoot;

        /* The worker that control the character manager */
        public class CharacterControl
        {
            public static GameObjectAsset GetCharacterByString(string ID)
            {
                CharacterManager CM = LinkerHelper.ToManager.GetManagerByType<CharacterManager>();
                return CM.GetAssetByID(ID);
            }
            /* Spawn character */
            public static bool SpawnPlayerCharacter(Vector3 pos)
            {
                CharacterManager CM = LinkerHelper.ToManager.GetManagerByType<CharacterManager>();
                if (ControlManager.ControllerAssign.GetCurrentController() != null)
                {
                    EclipseDebug.Log(4, EclipseDebug.DebugState.Error,
                        new EngineGUIString("玩家角色已經在場景內.", "Player character already in the game scene."));
                    return false;
                }
                if (CM.GetRoot() == null)
                {
                    EclipseDebug.Log(4, EclipseDebug.DebugState.Error,
                        new EngineGUIString("你需要先註冊角色生成區.", "You need to register the character spawn root."));
                    return false;
                }

                GameObjectAsset ass = CharacterManager.CharacterControl.GetCharacterByString(CM.GetDefaultCharacter());
                if (ass == null)
                {
                    EclipseDebug.Log(4, EclipseDebug.DebugState.Warning,
                        new EngineGUIString("預設角色物件未註冊.", "Default character object didn't register."));
                    return false;
                }
                /* Spawn */
                GameObject p = GameObject.Instantiate(ass.Asset, CM.GetRoot());
                p.name = "Character: " + ass.Asset.name;
                p.transform.position = pos;

                /* Register controller */
                ControllerBase CB = p.GetComponent<ControllerBase>();
                if(CB)
                    ControlManager.ControllerAssign.RegisterPlayerController(CB);

                /* Update camera */
                SceneCameraUpdate();
                EclipseDebug.Log(4, EclipseDebug.DebugState.Log,
                            new EngineGUIString("生成角色成功.", "Successfully spawn player character."));
                return true;
            }
            public static bool SpawnPlayerCharacter(string CharacterID, Vector3 pos)
            {
                CharacterManager CM = LinkerHelper.ToManager.GetManagerByType<CharacterManager>();
                if (ControlManager.ControllerAssign.GetCurrentController() != null) {
                    EclipseDebug.Log(4, EclipseDebug.DebugState.Error,
                        new EngineGUIString("玩家角色已經在場景內.", "Player character already in the game scene."));
                    return false;
                }
                if (CM.GetRoot() == null) {
                    EclipseDebug.Log(4, EclipseDebug.DebugState.Error,
                        new EngineGUIString("你需要先註冊角色生成區.", "You need to register the character spawn root."));
                    return false;
                }

                GameObjectAsset ass = CM.GetAssetByID(CharacterID);
                if (ass == null || ass.Asset == null)
                {
                    EclipseDebug.Log(4, EclipseDebug.DebugState.Warning,
                        new EngineGUIString("該角色 ID 未註冊.", "Character ID didn't register."));
                    return false;
                }
                /* Spawn */
                GameObject p = GameObject.Instantiate(ass.Asset, CM.GetRoot());
                p.name = "Character: " + ass.AssetID;
                p.transform.position = pos;

                /* Register controller */
                ControllerBase CB = p.GetComponent<ControllerBase>();
                if (CB)
                    ControlManager.ControllerAssign.RegisterPlayerController(CB);

                /* Update camera */
                SceneCameraUpdate();
                EclipseDebug.Log(4, EclipseDebug.DebugState.Log,
                            new EngineGUIString("生成角色成功.", "Successfully spawn player character."));
                return true;
            }
            /* Clean all character */
            public static void KillAll()
            {
                CharacterManager CM = LinkerHelper.ToManager.GetManagerByType<CharacterManager>();
                Transform t = CM.GetRoot();
                if (t == null)
                {
                    EclipseDebug.Log(4, EclipseDebug.DebugState.Error,
                        new EngineGUIString("你需要先註冊角色生成區.", "You need to register the character spawn root."));
                    return;
                }
                for(int i = 0; i < t.childCount; i++)
                {
                    Destroy(t.GetChild(i).gameObject);
                }
                SceneCameraUpdate();
                InGameMouseLock(false);
            }
            /* Update all scene camera */
            public static void SceneCameraUpdate()
            {
                CharacterManager CM = LinkerHelper.ToManager.GetManagerByType<CharacterManager>();
                if(ControlManager.ControllerAssign.GetCurrentController() == null)
                {
                    CameraBase c = CM.GetDefaultCamera();
                    c.GetComponent<AudioListener>().enabled = true;
                    Camera.SetupCurrent(c.GetCamera());
                    InGameMouseLock(false);
                }
                else
                {
                    CameraBase c = CM.GetDefaultCamera();
                    c.GetComponent<AudioListener>().enabled = false;
                    Camera.SetupCurrent(ControlManager.ControllerAssign.GetCurrentController().GetCharacterCamera());
                }
            }

            public static void InGameMouseLock(bool _lock)
            {
                Cursor.lockState = _lock ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.visible = !_lock;
            }

            public static int GetCharacterSceneIndex(Transform t)
            {
                CharacterManager CM = LinkerHelper.ToManager.GetManagerByType<CharacterManager>();
                Transform root = CM.GetRoot();
                if (CM.GetRoot() == null)
                {
                    EclipseDebug.Log(4, EclipseDebug.DebugState.Error,
                        new EngineGUIString("你需要先註冊角色生成區.", "You need to register the character spawn root."));
                    return -1;
                }
                for(int i = 0; i < root.childCount; i++)
                {
                    if (root.GetChild(i) == t) return i;
                }
                return -1;
            }
            public static CharacterBase GetPlayerCharacter()
            {
                ControllerBase CB = ControlManager.ControllerAssign.GetCurrentController();
                if (!CB) return null;
                return CB.transform.GetComponent<CharacterBase>();
            }
        }
        /* Editor use function */
        #region Editor Function
        public void AddNew()
        {
            CharacterList.Add(new GameObjectAsset("Empty", null));
        }
        public void DeleteLast()
        {
            if (CharacterList.Count > 0)
                CharacterList.RemoveAt(CharacterList.Count - 1);
        }
        #endregion
        /* It handle the set and get */
        #region Setter And Getter
        public Transform GetRoot() { return CharacterSpawnRoot; }
        public void SetDefaultCharacter(string default_character) { DefaultCharacter = default_character; }
        public string GetDefaultCharacter() { return DefaultCharacter; }
        public CameraBase GetDefaultCamera() { return DefaultCamera; }
        public GameObjectAsset GetAssetByID(string id) {
            for(int i = 0; i < CharacterList.Count; i++)
            {
                if (CharacterList[i].AssetID == id) return CharacterList[i];
            }
            return null;
        }
        #endregion
    }

    [CustomEditor(typeof(CharacterManager))]
    [CanEditMultipleObjects]
    public class CharacterManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            CharacterManager Target = (CharacterManager)target;
            /* Begining */
            EditorHelper.EditorOption.BeginEclipseEditor(new EngineGUIString("角色管理腳本", "Character Manager"), serializedObject);
            /* Return */
            EditorHelper.EditorOption.ReturnToEngineManager("");
            /* Setting */
            #region Manager Setting
            EditorGUILayout.BeginVertical("GroupBox"); 
            EditorGUILayout.PropertyField(serializedObject.FindProperty("DefaultCharacter"), new GUIContent(new EngineGUIString("預設角色", "Default Character").ToString()));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("DefaultCamera"), new GUIContent(new EngineGUIString("預設攝影機", "Default Camera").ToString()));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("CharacterSpawnRoot"), new GUIContent(new EngineGUIString("角色生成區", "Character Spawn Root").ToString()));
            EditorGUILayout.EndVertical();
            #endregion
            /* Asset register */
            #region Character register
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new EngineGUIString("加入角色", "Add UI").ToString(), GUILayout.Height(25))) { Target.AddNew(); }
            if (GUILayout.Button(new EngineGUIString("刪除角色", "Delete UI").ToString(), GUILayout.Height(25))) { Target.DeleteLast(); }
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel++;
            RenderCharacterList();
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            #endregion
            /* Ending */
            EditorHelper.EditorOption.EndEclipseEditor(serializedObject);
        }

        #region List Render
        private void RenderCharacterList()
        {
            SerializedProperty SP = serializedObject.FindProperty("CharacterList");
            SP.Next(true);
            int UISize = SP.arraySize;
            SP.Next(true);
            SP.Next(true);
            for (int i = 0; i < UISize; i++)
            {
                EditorGUILayout.BeginVertical("GroupBox");
                bool show = EditorGUILayout.PropertyField(SP);
                if (show)
                    RenderCharacterContent(SP.Copy());
                SP.Next(false);
                EditorGUILayout.EndVertical();
            }
        }

        private void RenderCharacterContent(SerializedProperty SP)
        {
            EditorGUI.indentLevel++;
            SP.Next(true); // Enter in
            EditorGUILayout.PropertyField(SP, new GUIContent(new EngineGUIString("角色 ID", "Character ID").ToString()));
            SP.Next(false);
            EditorGUILayout.PropertyField(SP, new GUIContent(new EngineGUIString("角色物件", "Character Asset").ToString()));
            EditorGUI.indentLevel--;
        }
        #endregion
    }
}
