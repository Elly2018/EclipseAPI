using UnityEngine;
using Eclipse.Base;
using Eclipse.Components.Character;
using Eclipse.Base.Struct;

namespace Eclipse.Components.Controller
{
    public class ControllerBase : ComponentBase
    {
        [SerializeField] public bool MouseControl = true;
        [SerializeField] public bool MovementControl = true;
        [SerializeField] public Vector3 _Cursor = Vector3.zero;

        [SerializeField] public UnityEngine.Camera CharacterCamera;
        [SerializeField] public CharacterBase CB;
        [SerializeField] public Animator anim;
        [SerializeField] public DoubleKeyDetection DoubleKeyDetection = new DoubleKeyDetection();

        public virtual void ControllerInitialize()
        {
            CB = GetComponent<CharacterBase>();
        }

        public virtual void ControllerUpdate()
        {
            DoubleKeyDetection.UpdateAll(Time.deltaTime);
        }

        public void NoClip()
        {
            NoClip(!CB.GetNoClip());
        }

        public void NoClip(bool enable)
        {
            CB.SetNoClip(enable);
            Collider[] c = GetComponentsInChildren<Collider>();
            for(int i = 0; i < c.Length; i++)
            {
                if(!c[i].isTrigger)
                    c[i].enabled = !enable;
            }
            CB.rigi.useGravity = !enable;
        }

        #region Setter And Getter
        public UnityEngine.Camera GetCharacterCamera()
        {
            return CharacterCamera;
        }
        #endregion
    }
}
