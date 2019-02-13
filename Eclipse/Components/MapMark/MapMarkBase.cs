using UnityEngine;
using Eclipse.Base;

namespace Eclipse.Components.MapMark
{
    public class MapMarkBase : ComponentBase
    {
        public virtual void MapMarkInitialize()
        {
            if(GetComponent<MeshRenderer>()) GetComponent<MeshRenderer>().enabled = false;
            if (GetComponent<BoxCollider>()) GetComponent<BoxCollider>().enabled = false;
            if (GetComponent<SphereCollider>()) GetComponent<SphereCollider>().enabled = false;
            if (GetComponent<Collider>()) GetComponent<Collider>().enabled = false;
            if (GetComponent<CapsuleCollider>()) GetComponent<CapsuleCollider>().enabled = false;
            if (GetComponent<MeshCollider>()) GetComponent<MeshCollider>().enabled = false;
        }
        public virtual void MarkCalling() { }
    }
}
