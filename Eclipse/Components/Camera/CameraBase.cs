using Eclipse.Base;
using UnityEngine;

namespace Eclipse.Components.Camera
{
    public class CameraBase : ComponentBase
    {
        public UnityEngine.Camera GetCamera()
        {
            return GetComponent<UnityEngine.Camera>();
        }
    }
}
