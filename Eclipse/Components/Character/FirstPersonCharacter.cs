using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Eclipse.Base.Struct;
using Eclipse.Managers;

namespace Eclipse.Components.Character
{
    [AddComponentMenu("Eclipse/Components/Character/First Person")]
    public class FirstPersonCharacter : CharacterBase
    {
        [SerializeField] private List<string> StepID = new List<string>();

        private Vector3 Dist;

        private void Start()
        {
            base.CharacterInitialize();
        }

        private void Update()
        {
            InAirDetection();
            BasicCharacterUpdate();
        }

        private void FixedUpdate()
        {
            
        }

        public override void BasicCharacterUpdate()
        {
            base.BasicCharacterUpdate();
            if (InAir && rigi) { rigi.drag = 0.5f; }
            if (!InAir && rigi) { rigi.drag = 2.0f; }
        }
        /* Moving when character is on ground */
        #region Character Moving
        public void CharacterJump(Vector3 pos)
        {
            rigi.AddForce(pos);
        }
        public void CharacterMoving(Vector3 pos, float StepLength, float RunningMultiply, bool Running)
        {
            rigi.AddForce(pos * 150.0f);
            if (NoClip)
            {
                return;
            }
            if (!Running)
            {
                if (rigi.velocity.x > StepLength) { rigi.velocity = new Vector3(StepLength, rigi.velocity.y, rigi.velocity.z); }
                if (rigi.velocity.x < -StepLength) { rigi.velocity = new Vector3(-StepLength, rigi.velocity.y, rigi.velocity.z); }
                if (rigi.velocity.z > StepLength) { rigi.velocity = new Vector3(rigi.velocity.x, rigi.velocity.y, StepLength); }
                if (rigi.velocity.z < -StepLength) { rigi.velocity = new Vector3(rigi.velocity.x, rigi.velocity.y, -StepLength); }
            }
            else
            {
                if (rigi.velocity.x > StepLength * RunningMultiply) { rigi.velocity = new Vector3(StepLength * RunningMultiply, rigi.velocity.y, rigi.velocity.z); }
                if (rigi.velocity.x < -StepLength * RunningMultiply) { rigi.velocity = new Vector3(-StepLength * RunningMultiply, rigi.velocity.y, rigi.velocity.z); }
                if (rigi.velocity.z > StepLength * RunningMultiply) { rigi.velocity = new Vector3(rigi.velocity.x, rigi.velocity.y, StepLength * RunningMultiply); }
                if (rigi.velocity.z < -StepLength * RunningMultiply) { rigi.velocity = new Vector3(rigi.velocity.x, rigi.velocity.y, -StepLength * RunningMultiply); }
            }
        }
        #endregion
        /* Audio part */
        #region Audio
        public void PlayStepSoundEffect()
        {
            if (InAir) return;
            if (StepID.Count == 0) return;
            int ran = Random.Range(0, StepID.Count);
            AudioManager.SFXControl.PlaySFX(StepID[ran], transform.position - new Vector3(0, 1, 0));
        }
        #endregion
    }

    [CustomEditor(typeof(FirstPersonCharacter))]
    [CanEditMultipleObjects]
    public class FirstPersonCharacterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            /* Begining */
            EditorHelper.EditorOption.BeginEclipseEditor(new EngineGUIString("第一人稱元件", "First Person Component"), serializedObject);
            base.DrawDefaultInspector();
            /* Ending */
            EditorHelper.EditorOption.EndEclipseEditor(serializedObject);
        }
    }
}
