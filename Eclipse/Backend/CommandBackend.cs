using System;
using System.Text;
using System.Collections.Generic;
using Eclipse.Components.Command;
using UnityEngine;
using Eclipse.Base.Struct;
using Eclipse.Managers;

namespace Eclipse.Backend
{
    public class CommandBackend
    {
        public static string[] CommandRecord = new string[200];
        public static string InputCommand = "";

        public struct ArugmentString
        {
            public string TargetString;
            public string ChangeString;

            public ArugmentString(string targetString, string changeString)
            {
                TargetString = targetString;
                ChangeString = changeString;
            }
        }

        /* Handle mutiple string enter to record */
        public static void CommandRecordEnter(string[] obj)
        {
            if (obj == null) return;
            for(int i = 0; i < obj.Length; i++) {
                CommandEnterSingleString(obj[i]);
            }
        }

        /* Only enter one single string into record */
        private static void CommandEnterSingleString(string obj)
        {
            string[] CR = (string[])CommandRecord.Clone();
            for (int i = 1; i < CommandRecord.Length; i++)
            {
                CommandRecord[i - 1] = CR[i];
            }
            CommandRecord[CommandRecord.Length - 1] = obj;
        }

        /* Execute the command, it will enter all the useable commands and find the match */
        public static string[] CommandEnter(bool Backend = true)
        {
            EclipseDebug.Log(1, EclipseDebug.DebugState.Log, "Command Enter: " + "\"" + CommandBackend.InputCommand + "\"");
            CommandBase[] CBList = CommandHelper.GetAllUseableCommandList();
            List<ArugmentString> ArugString = new List<ArugmentString>();
            PermissionBase CurrentPermission = LinkerHelper.ToManager.GetManagerByType<EngineManager>().GetPermission();

            /* Loop all the useable commands */
            for(int i = 0; i < CBList.Length; i++)
            {
                string[] commandSplit = InputCommand.Split(' ');
                /* Check the length first */
                if (CBList[i].CP_Lists.Count == commandSplit.Length)
                {
                    bool Execute = true;
                    PermissionBase permission = CBList[i].permissionBase;
                    for (int j = 0; j < commandSplit.Length; j++)
                    {
                        /* check if the static string match */
                        if (CBList[i].CP_Lists[j].property.variableType == CommandProperty.VariableType.Static)
                        {
                            /* Not match */
                            if(CBList[i].CP_Lists[j].property.variableValue != commandSplit[j])
                            {
                                Execute = false;
                            }
                        }
                        /* Apply the variable to dynamic field */
                        else if (CBList[i].CP_Lists[j].property.variableType == CommandProperty.VariableType.Dynamic)
                        {
                            CBList[i].CP_Lists[j].property.variableValue = commandSplit[j];

                            /* Add it in a argument */
                            /* For response use */
                            ArugString.Add(new ArugmentString("%" + CBList[i].CP_Lists[j].property.variableName + "%",
                                commandSplit[j]));
                        }
                    }
                    if (Execute)
                    {
                        if (Backend)
                        {
                            return ExecuteCommand(CBList[i], ArugString);
                        }

                        for(int k = 0; k < CurrentPermission.GetSize(); k++)
                        {
                            bool HavePermission = permission.CheckUserExist(CurrentPermission.GetUser(k));
                            if (HavePermission) // Successfully execute
                            {
                                return ExecuteCommand(CBList[i], ArugString);
                            }
                        }
                        InputCommand = "";
                        return new string[1] { "You don't have permission to execute the command." };
                    }
                    else
                    {
                        ArugString.Clear();
                    }
                }
            }
            string co = InputCommand;
            InputCommand = "";
            return new string[2] { "Command Executed Failed: ", "\t" + co};
        }

        /* Change the all the <target_string> to <change_string> */
        private static string GetResponseString(string rawString, ArugmentString[] arugmentStrings)
        {
            if (arugmentStrings == null || rawString == null) return rawString;
            string Result = new string(rawString.ToCharArray());
            for(int i = 0; i < arugmentStrings.Length; i++)
            {
                if (Result.Contains(arugmentStrings[i].TargetString))
                {
                    Result = Result.Replace(arugmentStrings[i].TargetString, arugmentStrings[i].ChangeString);
                }
            }
            return Result;
        }
        /* Execute command */
        private static string[] ExecuteCommand(CommandBase call, List<ArugmentString> arug)
        {
            InputCommand = "";
            string[] Res = call.call();
            if (Res == null) return null;
            for (int l = 0; l < Res.Length; l++)
            {
                Res[l] = GetResponseString(Res[l], arug.ToArray());
            }
            return Res;
        }
    }
}
