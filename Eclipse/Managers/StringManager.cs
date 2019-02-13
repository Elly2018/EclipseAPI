using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Eclipse.Base;
using Eclipse.Base.Struct;
using System.Text.RegularExpressions;
using Eclipse.Components.Language;

namespace Eclipse.Managers
{
    [AddComponentMenu("Eclipse/Managers/String")]
    public class StringManager : ManagerBase
    {
        [SerializeField] private List<string> LanguageTag = new List<string>();
        [SerializeField] private string A_LanguageEnterTag;
        [SerializeField] private string B_LanguageEnterTag;
        [SerializeField] private List<StringCategoryBase> CategoryBases = new List<StringCategoryBase>();
        [SerializeField] private string UseLanguageTag;
        [SerializeField] private int UseLanguageIndex;

        /* String control worker */
        /* The use function for other objects */
        public class StringControl
        {
            public static string GetString(string _category, string _label, string tag)
            {
                StringManager SM = LinkerHelper.ToManager.GetManagerByType<StringManager>();
                StringLabelBase labelBase = SM.GetLabel(_category, _label);
                if (labelBase != null)
                {
                    for (int i = 0; i < labelBase._string.Count; i++)
                    {
                        if (labelBase._string[i].LanguageTag == _label)
                        {
                            return labelBase._string[i].Data;
                        }
                    }
                    EclipseDebug.Log(2, EclipseDebug.DebugState.Log, "國際語言中未找到此項目");
                }
                return null;
            }

            public static string GetLanguageTag()
            {
                StringManager SM = LinkerHelper.ToManager.GetManagerByType<StringManager>();
                return SM.GetLanguageTag();
            }

            public static void UpdateStringInScene()
            {
                LanguageRegister[] SR = GameObject.FindObjectsOfType<LanguageRegister>();
                for (int i = 0; i < SR.Length; i++)
                {
                    SR[i].StringUpdate();
                }
            }
        }
        /* Tag register */
        #region Register Tag
        public void RegisterTag()
        {
            if (!CheckAString(A_LanguageEnterTag)) return;
            for (int i = 0; i < LanguageTag.Count; i++)
            {
                if(LanguageTag[i] == A_LanguageEnterTag)
                {
                    EclipseDebug.Log(1, EclipseDebug.DebugState.Log, "該物件已經存在於語言註冊列表中.");
                    ClearTagEnter();
                    return;
                }
            }
            EclipseDebug.Log(1, EclipseDebug.DebugState.Log, "成功加入新的語言標籤.");
            LanguageTag.Add(A_LanguageEnterTag);
            StringTagUpdate();
        }

        public void DeleteTag()
        {
            if (!CheckAString(A_LanguageEnterTag)) return;
            for(int i = 0; i < LanguageTag.Count; i++)
            {
                if(LanguageTag[i] == A_LanguageEnterTag)
                {
                    EclipseDebug.Log(1, EclipseDebug.DebugState.Log, "成功刪除指定語言標籤.");
                    LanguageTag.RemoveAt(i);
                    ClearTagEnter();
                    StringTagUpdate();
                    return;
                }
            }
            ClearTagEnter();
            EclipseDebug.Log(1, EclipseDebug.DebugState.Log, "無法找到此物件在語言註冊列表中.");
        }

        public void ChangeTag()
        {
            if (!CheckAString(A_LanguageEnterTag)) return;
            if (!CheckAString(B_LanguageEnterTag)) return;
            bool Exist = LanguageTag.Exists(e => e.EndsWith(A_LanguageEnterTag));
            bool BExist = LanguageTag.Exists(e => e.EndsWith(B_LanguageEnterTag));
            if (!Exist) {
                EclipseDebug.Log(1, EclipseDebug.DebugState.Warning, "指定被轉換標籤並不存在於語言註冊列表中.");
                return;
            }
            if (BExist)
            {
                EclipseDebug.Log(1, EclipseDebug.DebugState.Warning, "此轉換的標籤已經存在.");
                return;
            }
            for (int i = 0; i < LanguageTag.Count; i++)
            {
                if(LanguageTag[i] == A_LanguageEnterTag)
                {
                    LanguageTag[i] = B_LanguageEnterTag;
                    EclipseDebug.Log(1, EclipseDebug.DebugState.Warning, "成功將目標標籤轉換.");
                }
            }
            StringTagUpdate();
        }

        private bool CheckAString(string ta)
        {
            if (ta == "")
            {
                EclipseDebug.Log(1, EclipseDebug.DebugState.Warning, "你並沒有輸入任何標籤.");
                return false;
            }
            else if (ta.Length > 4)
            {
                EclipseDebug.Log(1, EclipseDebug.DebugState.Warning, "輸入標籤字串大小應該小於 4.");
                return false;
            }else if(Regex.IsMatch(ta, @"^-?\d+$"))
            {
                EclipseDebug.Log(1, EclipseDebug.DebugState.Warning, "標籤不應該有數字在其中.");
                return false;
            }
            return true;
        }

        private void ClearTagEnter()
        {
            A_LanguageEnterTag = "";
            B_LanguageEnterTag = "";
        }

        public string GetLanguageTag()
        {
            return UseLanguageTag;
        }
        #endregion
        /* String register */
        #region String Register 
        public void AddNewStringCategory()
        {
            StringCategoryBase buffer = new StringCategoryBase();
            buffer._category_unlocalizeName = "Empty Category";
            CategoryBases.Add(buffer);
            EclipseDebug.Log(1, EclipseDebug.DebugState.Log, "國際語言成功新增新的類別.");
        }

        public void DeleteStringCategory()
        {
            if (CategoryBases.Count > 1) {
                CategoryBases.RemoveAt(CategoryBases.Count - 1);
                EclipseDebug.Log(1, EclipseDebug.DebugState.Log, "國際語言成功刪除最後一項類別.");
            }
            else
            {
                EclipseDebug.Log(1, EclipseDebug.DebugState.Log, "國際語言刪除失敗, 類別列表目前大小為 0.");
            }
        }

        public void AddNewStringLabel(string CategoryID)
        {
            StringCategoryBase TargetCate = GetCategory(CategoryID);
            if(TargetCate != null)
            {
                StringLabelBase buffer = new StringLabelBase();
                buffer._label_unlocalizeName = "Empty Label";
                for(int i = 0; i < LanguageTag.Count; i++)
                {
                    LabelDataBase LDB = new LabelDataBase();
                    LDB.LanguageTag = LanguageTag[i];
                    LDB.Data = "Empty";
                    buffer._string.Add(LDB);
                }
                TargetCate.labelBases.Add(buffer);
                EclipseDebug.Log(1, EclipseDebug.DebugState.Log, "國際語言成功新增項目.");
            }
            else
            {
                EclipseDebug.Log(1, EclipseDebug.DebugState.Log, "國際語言新增項目失敗，找不到類別: " + CategoryID);
            }
        }

        public void DeleteStringLabel(string CategoryID)
        {
            StringCategoryBase TargetCate = GetCategory(CategoryID);
            if (TargetCate != null)
            {
                if(TargetCate.labelBases.Count > 0)
                {
                    TargetCate.labelBases.RemoveAt(TargetCate.labelBases.Count - 1);
                    EclipseDebug.Log(1, EclipseDebug.DebugState.Log, "國際語言刪除項目成功");
                }
                else
                {
                    EclipseDebug.Log(1, EclipseDebug.DebugState.Log, "國際語言刪除項目失敗，類別內的項目列表大小已經是 0: " + CategoryID);
                }
            }
            else
            {
                EclipseDebug.Log(1, EclipseDebug.DebugState.Log, "國際語言刪除項目失敗，找不到類別: " + CategoryID);
            }
        }

        private StringCategoryBase GetCategory(string categoryID)
        {
            for(int i = 0; i < CategoryBases.Count; i++)
            {
                if(CategoryBases[i]._category_unlocalizeName == categoryID)
                {
                    return CategoryBases[i];
                }
            }
            return null;
        }

        private StringLabelBase GetLabel(string categoryID, string labelID)
        {
            StringCategoryBase cate = GetCategory(categoryID);
            if(cate != null)
            {
                for (int i = 0; i < cate.labelBases.Count; i++)
                {
                    if (cate.labelBases[i]._label_unlocalizeName == labelID)
                    {
                        return cate.labelBases[i];
                    }
                }
            }
            return null;
        }

        private void StringTagUpdate()
        {
            for(int i = 0; i < CategoryBases.Count; i++)
            {
                for(int k = 0; k < CategoryBases[i].labelBases.Count; k++)
                {
                    /* Store into buffer */
                    List<LabelDataBase> LDB = CategoryBases[i].labelBases[k]._string;
                    CategoryBases[i].labelBases[k]._string.Clear();
                    for(int j = 0; j < LanguageTag.Count; j++)
                    {
                        LabelDataBase addbuffer = new LabelDataBase();
                        addbuffer.LanguageTag = LanguageTag[j];
                        addbuffer.Data = "Empty";
                        CategoryBases[i].labelBases[k]._string.Add(addbuffer);
                    }

                    /* ADjustment */
                    for(int j = 0; j < CategoryBases[i].labelBases[k]._string.Count; j++)
                    {
                        for(int o = 0; o < LDB.Count; o++)
                        {
                            if(CategoryBases[i].labelBases[k]._string[j].LanguageTag ==
                                LDB[o].LanguageTag)
                            {
                                CategoryBases[i].labelBases[k]._string[j].Data = LDB[o].Data;
                            }
                        }
                    }
                }
            }
        }
        #endregion
        /* Getter */
    }

    [CustomEditor(typeof(StringManager))]
    [CanEditMultipleObjects]
    public class StringManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            StringManager Target = (StringManager)target;
            GUIStyle skinT = EditorHelper.TypeOption.GetCustomStyle(16, FontStyle.Normal, TextAnchor.MiddleCenter);
            /* Begining */
            EditorHelper.EditorOption.BeginEclipseEditor(new EngineGUIString("國際語言管理腳本", "Language Manager"), serializedObject);
            /* Return */
            EditorHelper.EditorOption.ReturnToEngineManager("");
            /* Default setting */
            #region Default Setting
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("國際語言設定值", "Language Default Setting").ToString(), skinT);
            EditorGUILayout.Space();
            if (GetLanguageTagLists().Count - 1 < serializedObject.FindProperty("UseLanguageIndex").intValue)
                serializedObject.FindProperty("UseLanguageIndex").intValue = 0;
            serializedObject.FindProperty("UseLanguageIndex").intValue =
            EditorGUILayout.Popup(serializedObject.FindProperty("UseLanguageIndex").intValue, GetLanguageTagLists().ToArray(), GUILayout.Height(20));
            serializedObject.FindProperty("UseLanguageTag").stringValue = GetLanguageTagLists()
                [serializedObject.FindProperty("UseLanguageIndex").intValue];
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
            #endregion
            /* Register language tag */
            #region Register Tag
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("語言註冊", "Language Register").ToString(), skinT);
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(new EngineGUIString("註冊 : 將標籤打入 [語言標誌註冊] 並按下註冊進行增加語言標籤的動作. \n" +
                "刪除 : 將標籤打入 [語言標誌註冊] 並按下刪除進行搜尋語言標籤，如果找到配對將會刪除. \n" +
                "變更 : 將標籤打入 [語言標誌註冊] 將更換的語言輸入至 [語言標誌更換成] 點選變更.",
                "Register : Enter Your Tag To [Register Slot] And Press [Register] To Do The Register. \n" +
                "Delete : Enter Your Target Tag To [Register Slot] And Press [Delete], It Will Find The Result. \n" +
                "Replace : Enter Your Change Target Tag To [Register Slot] And Enter Your Replace Tag To [Replace Slot], It Will Find The Result.").ToString(), MessageType.Info);
            EditorGUILayout.HelpBox(new EngineGUIString("目前註冊的語言有: ", "Current Register Language Have: ").ToString() + GetTheAlreadyLanguageTagString(GetLanguageTagLists()), MessageType.Info);
            EditorGUILayout.BeginHorizontal("GroupBox");
            if (GUILayout.Button(new EngineGUIString("註冊", "Register").ToString(), GUILayout.Height(28))) { Target.RegisterTag(); }
            if (GUILayout.Button(new EngineGUIString("刪除", "Delete").ToString(), GUILayout.Height(28))) { Target.DeleteTag(); }
            if (GUILayout.Button(new EngineGUIString("變更", "Replace").ToString(), GUILayout.Height(28))) { Target.ChangeTag(); }
            EditorGUILayout.EndHorizontal();
            serializedObject.FindProperty("A_LanguageEnterTag").stringValue = 
                EditorGUILayout.TextField(new EngineGUIString("語言標誌註冊", "Register Slot").ToString(), serializedObject.FindProperty("A_LanguageEnterTag").stringValue, GUILayout.Height(20));
            serializedObject.FindProperty("B_LanguageEnterTag").stringValue =
                EditorGUILayout.TextField(new EngineGUIString("語言標誌更換成", "Replace Slot").ToString(), serializedObject.FindProperty("B_LanguageEnterTag").stringValue, GUILayout.Height(20));
            EditorGUILayout.EndVertical();
            #endregion
            /* Register area */
            #region Content List
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("內容註冊", "Content Register").ToString(), skinT);
            EditorGUILayout.BeginHorizontal("GroupBox");
            if (GUILayout.Button(new EngineGUIString("新增類別", "Add New Category").ToString(), GUILayout.Height(28))) { Target.AddNewStringCategory(); }
            if (GUILayout.Button(new EngineGUIString("刪除類別", "Delete Last Category").ToString(), GUILayout.Height(28))) { Target.DeleteStringCategory(); }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUI.indentLevel++;
            ShowRegisterStringList();
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
            #endregion
            /* Ending */
            EditorHelper.EditorOption.EndEclipseEditor(serializedObject);
        }

        #region Editor Function
        /* Render the string register list */
        #region String List Render
        private void ShowRegisterStringList()
        {
            SerializedProperty SP = serializedObject.FindProperty("CategoryBases");
            SP.Next(true); // Skip header
            int CategorySize = SP.arraySize;
            SP.Next(true); // Size field skip
            SP.Next(true); // Items
            for (int i = 0; i < CategorySize; i++)
            {
                /* Get the header name */
                SerializedProperty PreSP = SP.Copy();
                PreSP.Next(true);
                string Catename = PreSP.stringValue;
                /* Category header */
                bool show = EditorGUILayout.PropertyField(SP);
                if (show)
                {
                    RenderStringCategory(SP, Catename);
                }
                SP.Next(false);
            }
        }

        private void RenderStringCategory(SerializedProperty SP, string Catename)
        {
            StringManager Target = (StringManager)target;
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.BeginHorizontal("GroupBox");
            if (GUILayout.Button(new EngineGUIString("新增標籤", "Add New Label").ToString(), GUILayout.Height(28))) { Target.AddNewStringLabel(Catename); }
            if (GUILayout.Button(new EngineGUIString("刪除標籤", "Delete Last Label").ToString(), GUILayout.Height(28))) { Target.DeleteStringLabel(Catename); }
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel++;
            SerializedProperty SP2 = SP.Copy();
            SP2.Next(true);
            /* Category data */
            EditorGUILayout.PropertyField(SP2, new GUIContent(new EngineGUIString("類別名稱", "Category Name").ToString()));
            SP2.Next(false);
            /* Label header */
            bool labelshow = EditorGUILayout.PropertyField(SP2, new GUIContent(new EngineGUIString("標籤列表", "Label List").ToString()));
            if (labelshow)
            {
                RenderStringLabel(SP2);
            }
            SP2.Next(false);

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        private void RenderStringLabel(SerializedProperty SP)
        {
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUI.indentLevel++;
            SerializedProperty SP2 = SP.Copy();
            SP2.Next(true); // size
            int LabelSize = SP2.arraySize;
            SP2.Next(true); // items
            SP2.Next(true);
            /* Label list */
            for (int i = 0; i < LabelSize; i++)
            {
                bool labelDataShow = EditorGUILayout.PropertyField(SP2);
                if (labelDataShow)
                {
                    RenderStringLabelData(SP2);
                }
                SP2.Next(false);
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        private void RenderStringLabelData(SerializedProperty SP)
        {
            SerializedProperty SP2 = SP.Copy();
            SP2.Next(true);
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUI.indentLevel++;
            /* Label data info */
            EditorGUILayout.PropertyField(SP2);
            SP2.Next(false);
            for (int k = 0; k < GetLanguangeTagLength(); k++)
            {
                SerializedProperty SP3 = SP2.Copy();
                SP3.Next(true);
                SP3.Next(true);
                SP3.Next(true);
                for (int i = 0; i < k; i++)
                {
                    SP3.Next(false);
                }
                SP3.Next(true);
                /* Data */
                string ta = SP3.stringValue;
                SP3.Next(false);
                SP3.stringValue = EditorGUILayout.TextField(ta, SP3.stringValue);
                SP3.Next(false);
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
        #endregion
        /* Useful function */
        #region Useful
        private int GetLanguangeTagLength()
        {
            SerializedProperty SP = serializedObject.FindProperty("LanguageTag");
            SP.Next(true);
            return SP.arraySize;
        }

        private string GetTheAlreadyLanguageTagString(List<string> languageLists)
        {
            string result = "";
            for(int i = 0; i < languageLists.Count; i++)
            {
                result += languageLists[i] + ", ";
            }
            return result;
        }

        private List<string> GetLanguageTagLists()
        {
            List<string> result = new List<string>();
            SerializedProperty list = serializedObject.FindProperty("LanguageTag");
            list.Next(true); // Skip header
            int size = list.arraySize;
            list.Next(true); // Size field skip
            list.Next(true); // Items
            for (int i = 0; i < size; i++)
            {
                result.Add(list.stringValue);
                list.Next(false);
            }
            return result;
        }
        #endregion
        #endregion
    }
}
