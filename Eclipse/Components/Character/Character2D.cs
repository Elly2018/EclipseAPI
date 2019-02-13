using System.Collections.Generic;
using UnityEngine;

namespace Eclipse.Components.Character
{
    public class Character2D : CharacterBase
    {
        public enum MoveDirection
        {
            Left, None, Right
        }

        [SerializeField] private List<string> StepID = new List<string>();
        [SerializeField] private float WalkSpeed = 3.0f;
        [SerializeField] private MoveDirection Dir = MoveDirection.None;

        private void Start()
        {
            base.CharacterInitialize();
        }

        private void Update()
        {
            InAirDetection();
            BasicCharacterUpdate();
        }

        public override void BasicCharacterUpdate()
        {
            base.BasicCharacterUpdate();
        }

        public void Walk(MoveDirection d)
        {
            Dir = d;
            switch (Dir)
            {
                case MoveDirection.Left:
                    rigi2D.AddForce(new Vector2(-1, 0) * WalkSpeed);
                    break;
                case MoveDirection.Right:
                    rigi2D.AddForce(new Vector2(1, 0) * WalkSpeed);
                    break;
            }
        }
    }
}
