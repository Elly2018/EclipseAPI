using UnityEngine;

namespace Eclipse.Base.Struct
{
    [System.Serializable]
    public class MaterialAsset : AssetBase<Material>
    {
        public MaterialAsset(string assetID, Material asset) : base(assetID, asset)
        {
        }
    }
}
