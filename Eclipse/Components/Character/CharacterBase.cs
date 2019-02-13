using System.Collections.Generic;
using UnityEngine;
using Eclipse.Base;
using Eclipse.Base.Struct;
using Eclipse.Managers;
using Eclipse.Components.Controller;

namespace Eclipse.Components.Character
{
    public class CharacterBase : ComponentBase
    {
        /* This variable can catch in backend */
        [SerializeField] public bool InAir = true;
        [SerializeField] public bool NoClip = false;

        [SerializeField] public List<GroundDetection> InAirCollision = new List<GroundDetection>();
        [SerializeField] public Animator anim;
        [SerializeField] public Rigidbody rigi;
        [SerializeField] public Rigidbody2D rigi2D;
        [SerializeField] public ControllerBase control;

        public virtual void CharacterInitialize()
        {
            anim = GetComponent<Animator>();
            rigi = GetComponent<Rigidbody>();
            rigi2D = GetComponent<Rigidbody2D>();
            control = GetComponent<ControllerBase>();
        }

        public virtual void BasicCharacterUpdate()
        {
            if (transform.position.y < -300.0f) // Death cause falling out of the map
            {
                UnityEngine.Camera[] cc = GetComponentsInChildren<UnityEngine.Camera>();
                for(int i = 0; i < cc.Length; i++)
                {
                    Destroy(cc[i].gameObject);
                }
                CharacterManager.CharacterControl.SceneCameraUpdate();
                Destroy(this.gameObject);
            }
        }

        public virtual void InAirDetection()
        {
            bool result = true;
            for (int i = 0; i < InAirCollision.Count; i++)
            {
                if (InAirCollision[i].IsCollider) result = false;
            }
            InAir = result;
        }

        public bool GetNoClip()
        {
            return NoClip;
        }
        public void SetNoClip(bool noclip)
        {
            NoClip = noclip;
        }

        public int GetCharacterSceneIndex()
        {
            return CharacterManager.CharacterControl.GetCharacterSceneIndex(transform);
        }
    }
}
