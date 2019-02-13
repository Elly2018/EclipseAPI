using Eclipse.Backend;
using Eclipse.Base.Struct;
using Eclipse.Components.MapMark;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Eclipse.Components.Command
{
    [AddComponentMenu("Eclipse/Components/Command/Command")]
    public class Command : MapMarkBase
    {
        [SerializeField] private CommandBase commandBase;
        [SerializeField] private string SelectionName;
        [SerializeField] private int SelectionIndex = -1;

        public override void MarkCalling()
        {
            if (commandBase == null) return;
            string Command = string.Empty;
            for(int i = 0; i < commandBase.CP_Lists.Count; i++)
            {
                if(i == commandBase.CP_Lists.Count - 1)
                    Command += commandBase.CP_Lists[i].property.variableValue;
                else
                    Command += commandBase.CP_Lists[i].property.variableValue + " ";
            }
            CommandBackend.InputCommand = Command;
            CommandBackend.CommandRecordEnter(CommandBackend.CommandEnter());
        }

        #region Setter And Getter
        public void SetCommandBase(CommandBase CB) { commandBase = CB; }
        public void SetCommandName(string n) { SelectionName = n; }
        public void SetCommandIndex(int index) { SelectionIndex = index; }
        public CommandBase GetCommandBase() { return commandBase; }
        public string GetCommandName() { return SelectionName; }
        public int GetCommandIndex() { return SelectionIndex; }
        #endregion

    }

    [CustomEditor(typeof(Command))]
    [CanEditMultipleObjects]
    public class CommandEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Command Target = (Command)target;
            CommandBase[] BasicCommandList = CommandHelper.GetAllUseableCommandList();
            GUIStyle skinT = EditorHelper.TypeOption.GetCustomStyle(16, FontStyle.Normal, TextAnchor.MiddleCenter);
            /* Begining */
            EditorHelper.EditorOption.BeginEclipseEditor(
                new EngineGUIString("控制台指令呼叫", "Console Command Calling"), serializedObject);
            /* Silbing index */
            EditorHelper.MarkOption.MarksOrder(Target.transform);
            /* Command object selection */
            #region Command Select
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(new EngineGUIString("指令選擇", "Command Selection").ToString(), skinT);
            EditorGUILayout.Space();
            List<string> CommandNameList = new List<string>();
            for(int i = 0; i < BasicCommandList.Length; i++)
            {
                CommandNameList.Add(BasicCommandList[i].FunctionName);
            }
            Target.SetCommandIndex(
                EditorGUILayout.Popup(new EngineGUIString("指令列表", "Command List").ToString(),
                Target.GetCommandIndex(), CommandNameList.ToArray()));
            if(GUILayout.Button(new EngineGUIString("確認", "Apply").ToString(), GUILayout.Height(20))){
                Target.SetCommandBase(BasicCommandList[Target.GetCommandIndex()]);
            }
            EditorGUILayout.EndVertical();
            #endregion
            /* Command setting */
            #region Command setting
            CommandBase CurrentcommandBase = Target.GetCommandBase();
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField("指令係數說明與設定", skinT);
            if(CurrentcommandBase != null)
            {
                EditorGUILayout.HelpBox(CurrentcommandBase.FunctionComment, MessageType.Info);
                for(int i = 0; i < CurrentcommandBase.CP_Lists.Count; i++)
                {
                    EditorGUILayout.BeginVertical("GroupBox");
                    EditorGUILayout.LabelField(CurrentcommandBase.CP_Lists[i].property.variableType.ToString(), skinT);
                    EditorGUILayout.LabelField(CurrentcommandBase.CP_Lists[i].property.variableName);
                    if(CurrentcommandBase.CP_Lists[i].property.variableType == CommandProperty.VariableType.Static)
                    {
                        EditorGUILayout.LabelField(CurrentcommandBase.CP_Lists[i].property.variableValue.ToString());
                    }
                    else
                    {
                        CurrentcommandBase.CP_Lists[i].property.variableValue =
                        EditorGUILayout.TextField(CurrentcommandBase.CP_Lists[i].property.variableValue);
                    }
                    EditorGUILayout.EndVertical();
                }

            }
            EditorGUILayout.EndVertical();
            #endregion
            /* Ending */
            EditorHelper.EditorOption.EndEclipseEditor(serializedObject);
        }
    }
}
