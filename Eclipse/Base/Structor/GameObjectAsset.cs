using UnityEngine;

namespace Eclipse.Base.Struct
{
    [System.Serializable]
    public class GameObjectAsset : AssetBase<GameObject>
    {
        public GameObjectAsset(string assetID, GameObject asset) : base(assetID, asset)
        {
        }
    }
}
