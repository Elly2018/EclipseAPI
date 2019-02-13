using UnityEngine;

namespace Eclipse.Base
{
    public class ComponentBase : MonoBehaviour
    {
        [SerializeField] private string ComponentName;

        public void SetComponentName(string ID)
        {
            ComponentName = ID;
        }

        public string GetComponentName()
        {
            return ComponentName;
        }
    }
}
