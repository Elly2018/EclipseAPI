using UnityEngine;

namespace Eclipse.Base.Struct
{
    [System.Serializable]
    public class AudioAsset : AssetBase<AudioClip>
    {
        public AudioAsset(string assetID, AudioClip asset) : base(assetID, asset)
        {
        }
    }
}
