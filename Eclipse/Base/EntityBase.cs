using UnityEngine;

namespace Eclipse.Base
{
    public class EntityBase : MonoBehaviour
    {
        public void EntityUpdate()
        {
            if (transform.position.y < -500.0f) Destroy(this.gameObject);
        }
    }
}
