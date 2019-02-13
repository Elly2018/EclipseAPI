using System.Collections.Generic;

namespace Eclipse.Base.Struct
{
    public class PermissionBase
    {
        private List<string> permissionUsers = new List<string>();

        public PermissionBase()
        {
            permissionUsers.Add("User");
        }

        public PermissionBase(string[] list)
        {
            permissionUsers = new List<string>(list);
        }

        public int GetSize()
        {
            return permissionUsers.Count;
        }

        public string GetUser(int index)
        {
            return permissionUsers[index];
        }

        public bool AddUser(string user)
        {
            if (!CheckUserExist(user))
            {
                permissionUsers.Add(user);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteUser(string user)
        {
            if (CheckUserExist(user))
            {
                permissionUsers.Remove(user);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckUserExist(string user)
        {
            for(int i = 0; i < permissionUsers.Count; i++)
            {
                if (permissionUsers[i] == user) return true;
            }
            return false;
        }
    }
}
