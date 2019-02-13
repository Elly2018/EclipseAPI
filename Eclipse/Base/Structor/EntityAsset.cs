using UnityEngine;

namespace Eclipse.Base.Struct
{
    [System.Serializable]
    public class EntityAsset : GameObjectAsset
    {
        [SerializeField] private bool EnableSpawn = true; 

        public EntityAsset(string assetID, GameObject asset, bool Enable) : base(assetID, asset)
        {
            EnableSpawn = Enable;
        }
    }
}
