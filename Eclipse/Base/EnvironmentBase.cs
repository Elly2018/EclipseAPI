using Eclipse;
using UnityEditor;
using UnityEngine;

namespace Eclipse.Base
{
    public class EnvironmentBase : MonoBehaviour
    {
        public enum EnvironmentType
        {
            Physics, Lighting
        }
        [SerializeField] protected EnvironmentType environmentType = EnvironmentType.Lighting;

        public EnvironmentType GetEnviromentType()
        {
            return environmentType;
        }

        private void OnDestroy()
        {
            if (environmentType == EnvironmentType.Lighting)
                RenderSettings.skybox = null;
        }
    }
}
