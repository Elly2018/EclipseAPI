using UnityEngine;

namespace Eclipse.Base.Struct
{
    [System.Serializable]
    public class AssetBase<T>
    {
        public string AssetID;
        public T Asset;

        public AssetBase(string assetID, T asset)
        {
            AssetID = assetID;
            Asset = asset;
        }
    }
}
