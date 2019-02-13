using Eclipse.Base;
using UnityEngine;

namespace Eclipse.Components
{
    [AddComponentMenu("Eclipse/Components/Register/Audio")]
    public class AudioRegister : ComponentBase
    {
        public enum AudioType
        {
            Music, SFX
        }

        [SerializeField] private AudioType _type = AudioType.Music;
        [SerializeField] private float _duration;
        [SerializeField] private bool _autoDestroy = false;

        private void Update()
        {
            if(_duration > 0.0f) _duration -= Time.deltaTime;
            if (_duration <= 0.0f && _autoDestroy) Destroy(gameObject);
        }
        public void InitliazeAudioRegister(float _dura, bool autodestory)
        {
            _autoDestroy = autodestory;
            _duration = _dura;
        }

        public void SetAudioType(AudioType t) { _type = t; }
        public AudioType GetAudioType() { return _type; }
    }
}
