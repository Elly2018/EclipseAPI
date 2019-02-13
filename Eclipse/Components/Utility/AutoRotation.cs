using Eclipse.Base;
using UnityEngine;

namespace Eclipse.Components.Utility
{
    [AddComponentMenu("Eclipse/Components/Utility/Audo Rotation")]
    public class AutoRotation : ComponentBase
    {
        [SerializeField] private Vector3 Value;

        private void FixedUpdate()
        {
            transform.Rotate(Value);
        }
    }
}