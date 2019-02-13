using System.IO;
using UnityEngine;
using Eclipse.Managers;

namespace Eclipse
{
    public class EclipseDebug
    {
        public enum DebugState
        {
            Log, Warning, Error
        }

        public static void Log(int _level, DebugState _state, object _obj)
        {
            if (LinkerHelper.ToManager.GetManagerByType<EngineManager>().GetDebugLevel() > _level) return;
            switch (_state)
            {
                case DebugState.Log:
                    Debug.Log("[Eclipse] " + _obj.ToString());
                    break;
                case DebugState.Warning:
                    Debug.LogWarning("[Eclipse] " + _obj.ToString());
                    break;
                case DebugState.Error:
                    Debug.LogError("[Eclipse] " + _obj.ToString());
                    break;
            }
        }
    }
}