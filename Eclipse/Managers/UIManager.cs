using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Eclipse.Base;
using Eclipse.Base.Struct;

namespace Eclipse.Managers
{
    [AddComponentMenu("Eclipse/Managers/UI")]
    public class UIManager : ManagerBase
    {
        [SerializeField] private Transform UIParent;
        [SerializeField] private List<string> StartCallingUIID = new List<string>();
        [SerializeField] private List<GameObjectAsset> Canvases = new List<GameObjectAsset>();

        private void Start()
        {
            for(int i = 0; i < StartCallingUIID.Count; i++)
            {
                UIManager.UIManagerControl.SpawnCanvas(StartCallingUIID[i]);
            }
        }

        /* The worker that control the manager */
        public class UIManagerControl
        {
            public static void SpawnCanvas(string CanvasID)
            {
                GameObjectAsset asset =
                LinkerHelper.ToManager.GetManagerByType<UIManager>().GetCanvasByID(CanvasID);
                if (asset != null)
                {
                    Transform t = LinkerHelper.ToManager.GetManagerByType<UIManager>().GetParent();
                    if (!t) {
                        EclipseDebug.Log(4, EclipseDebug.DebugState.Error, new EngineGUIString("介面管理尚未註冊生成區.", "UI manager didn't register spawn root.").ToString());
                        return;
                    } 
                    GameObject g = GameObject.Instantiate(asset.Asset, t);
                    g.name = asset.AssetID;
                    EclipseDebug.Log(3, EclipseDebug.DebugState.Log, new EngineGUIString("介面生成成功.", "Successfully spawn UI element.").ToString());
                }
                else
                {
                    EclipseDebug.Log(3, EclipseDebug.DebugState.Warning, new EngineGUIString("ID 無法找到在介面列表中.", "Cannot find ID in UI register list.").ToString());
                }
            }

            /* Return if destory */
            public static bool SpawnCanvas(string CanvasID, bool IsExistThenDestroy)
            {
                Transform t = LinkerHelper.ToManager.GetManagerByType<UIManager>().GetParent();
                if (!t)
                {
                    EclipseDebug.Log(4, EclipseDebug.DebugState.Error, new EngineGUIString("介面管理尚未註冊生成區.", "UI manager didn't register spawn root.").ToString());
                    return false;
                }
                for(int i = 0; i < t.childCount; i++)
                {
                    if(t.GetChild(i).name == CanvasID)
                    {
                        Destroy(t.GetChild(i).gameObject);
                        return true;
                    }
                }

                GameObjectAsset asset =
                LinkerHelper.ToManager.GetManagerByType<UIManager>().GetCanvasByID(CanvasID);
                if (asset != null)
                {
                    GameObject g = GameObject.Instantiate(asset.Asset, t);
                    g.name = asset.AssetID;
                }
                return false;
            }

            public static void DeleteCanvas(string CanvasID)
            {
                Transform t = LinkerHelper.ToManager.GetManagerByType<UIManager>().GetParent();
                if (!t)
                {
                    EclipseDebug.Log(4, EclipseDebug.DebugState.Error, new EngineGUIString("介面管理尚未註冊生成區.", "UI manager didn't register spawn root.").ToString());
                    return;
                }
                for (int i = 0; i < t.childCount; i++)
                {
                    if (t.GetChild(i).name == CanvasID) {
                        Destroy(t.GetChild(i).gameObject);
                    } 
                }
            }

            public static void DeleteAllCanvas()
            {
                Transform t = LinkerHelper.ToManager.GetManagerByType<UIManager>().GetParent();
                if (!t)
                {
                    EclipseDebug.Log(4, EclipseDebug.DebugState.Error, new EngineGUIString("介面管理尚未註冊生成區.", "UI manager didn't register spawn root.").ToString());
                    return;
                }
                for (int i = 0; i < t.childCount; i++)
                {
                    Destroy(t.GetChild(i).gameObject);
                }
            }

            public static void MoveToTop(string CanvasID)
            {

            }

            public static void MoveToBottom(string CanvasID)
            {

            }

            public static void MoveUp(string CanvasID)
            {

            }

            public static void MoveDown(string CanvasID)
            {

            }
        }
        /* Editor useful function */
        #region Editor Function
        public void AddNewStart()
        {
            StartCallingUIID.Add("");
        }

        public void DeleteLastStart()
        {
            if (StartCallingUIID.Count > 0)
                StartCallingUIID.RemoveAt(Canvases.Count - 1);
        }
        public void AddNew()
        {
            Canvases.Add(new GameObjectAsset("Empty-Map", null));
        }

        public void DeleteLast()
        {
            if (Canvases.Count > 0)
                Canvases.RemoveAt(Canvases.Count - 1);
        }
        #endregion
        /* The set and get for this worker */
        #region Setter And Getter
        public void SetAsset(List<GameObjectAsset> assets) { Canvases = assets; }
        public List<GameObjectAsset> GetAsset() { return Canvases; }
        public Transform GetParent() { return UIParent; }
        public GameObjectAsset GetCanvasByID(string ID)
        {
            for(int i = 0; i < Canvases.Count; i++)
            {
                if (Canvases[i].AssetID == ID) return Canvases[i];
            }
            return null;
        }
        #endregion
    }

    [CustomEditor(typeof(UIManager))]
    [CanEditMultipleObjects]
    public class UIManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            UIManager Target = (UIManager)target;
            GUIStyle skinT = EditorHelper.TypeOption.GetCustomStyle(16, FontStyle.Normal, TextAnchor.MiddleCenter);
            /* Begining */
            EditorHelper.EditorOption.BeginEclipseEditor(new EngineGUIString("介面管理腳本", "UI Manager"), serializedObject);
            /* Return */
            EditorHelper.EditorOption.ReturnToEngineManager(new EngineGUIString("你能透過呼叫 ID 的方式去呼叫與控制介面", "You Can Control UI By Calling ID."));
            /* Setting and parent register */
            #region Setting
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("介面設定", "UI Setting").ToString(), skinT);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("UIParent"), new GUIContent(new EngineGUIString("介面生成區", "UI Root Transform").ToString()));
            EditorGUILayout.EndVertical();
            #endregion
            /* Start id */
            #region Start ID
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("介面起始呼叫", "UI Begining").ToString(), skinT);
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new EngineGUIString("加入起始介面", "Add Begining UI").ToString(), GUILayout.Height(25))) { Target.AddNewStart(); }
            if (GUILayout.Button(new EngineGUIString("刪除起始介面", "Delete Begining UI").ToString(), GUILayout.Height(25))) { Target.DeleteLastStart(); }
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel++;
            RenderMapStartList();
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("StartCallingUIID"), new GUIContent(new EngineGUIString("介面起始呼叫", "UI Begining ID").ToString()));
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            #endregion
            /* List register */
            #region List Render
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("介面列表註冊", "UI List Register").ToString(), skinT);
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new EngineGUIString("加入介面", "Add UI").ToString(), GUILayout.Height(25))) { Target.AddNew(); }
            if (GUILayout.Button(new EngineGUIString("刪除介面", "Delete UI").ToString(), GUILayout.Height(25))) { Target.DeleteLast(); }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;
            RenderMapList();
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            #endregion
            /* Ending */
            EditorHelper.EditorOption.EndEclipseEditor(serializedObject);
        }

        /* Map asset list render function */
        #region List Render
        private void RenderMapStartList()
        {
            SerializedProperty SP = serializedObject.FindProperty("StartCallingUIID");
            SP.Next(true);
            int UISize = SP.arraySize;
            SP.Next(true);
            SP.Next(true);
            for (int i = 0; i < UISize; i++)
            {
                EditorGUILayout.PropertyField(SP);
                SP.Next(false);
            }
        }

        private void RenderMapList()
        {
            SerializedProperty SP = serializedObject.FindProperty("Canvases");
            SP.Next(true);
            int UISize = SP.arraySize;
            SP.Next(true);
            SP.Next(true);
            for (int i = 0; i < UISize; i++)
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
            SP.Next(true); // Enter in
            EditorGUILayout.PropertyField(SP, new GUIContent(new EngineGUIString("介面 ID", "UI ID").ToString()));
            SP.Next(false);
            EditorGUILayout.PropertyField(SP, new GUIContent(new EngineGUIString("介面物件", "UI Asset").ToString()));
            EditorGUI.indentLevel--;
        }
        #endregion
    }
}
