using Eclipse.Base.Struct;
using System;
using System.Collections.Generic;

namespace Eclipse.Components.Command
{
    [System.Serializable]
    public class CommandBase
    {
        public string FunctionName;
        public string FunctionComment;
        public PermissionBase permissionBase = new PermissionBase();
        public List<CommandProperty> CP_Lists = new List<CommandProperty>();
        public delegate string[] Call();
        public Call call;

        public CommandBase(string Name, string Comment, PermissionBase PB, List<CommandProperty> List)
        {
            FunctionName = Name;
            FunctionComment = Comment;
            permissionBase = PB;
            CP_Lists = List;
        }

        public CommandBase(string Name, string Comment, List<CommandProperty> List)
        {
            FunctionName = Name;
            FunctionComment = Comment;
            CP_Lists = List;
        }

        public CommandBase()
        {
        }
    }
}
