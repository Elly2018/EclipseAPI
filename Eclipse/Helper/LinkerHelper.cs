using System;
using UnityEngine;
using Eclipse.Base;
using Eclipse.Components.MapState;

namespace Eclipse
{
    public class LinkerHelper
    {
        public class ToManager
        {
            /* Get the manager script */
            public static T GetManagerByType<T>() where T : ManagerBase
            {
                return GetScriptByType<T>();
            }

            /* Get the manager object */
            public static GameObject GetManagerObjectByType<T>() where T : ManagerBase
            {
                if (GetScriptByType<T>())
                    return GetScriptByType<T>().gameObject;
                else
                    return null;
            }

            private static T GetScriptByType<T>() where T : ManagerBase
            {
                ManagerBase[] bases = GameObject.FindObjectsOfType<ManagerBase>();
                for (int i = 0; i < bases.Length; i++)
                {
                    if (typeof(T) == bases[i].GetType())
                    {
                        return bases[i] as T;
                    }
                }
                return null;
            }
        }

        public class ToComponent
        {
            public static MapState GetMapState()
            {
                return GameObject.FindObjectOfType<MapState>();
            }
        }
    }
}
