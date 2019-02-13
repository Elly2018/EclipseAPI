using Eclipse.Base.Struct;
using Eclipse.Components.Character;
using Eclipse.Managers;
using UnityEngine;

namespace Eclipse.Components.Controller
{
    public class Character2DController : ControllerBase
    {
        [SerializeField] private float JumpForce;

        private void Start()
        {
            base.ControllerInitialize();
        }

        private void Update()
        {
            if(MovementControl) KeyDetection();
            if(MovementControl && CB.rigi2D && !CB.InAir) JumpDetection();
        }

        private void KeyDetection()
        {
            bool DirectionKeyPressed = false;
            if (Input.GetKey(ControlManager.ControlAssign.GetControlKeycode().
                GetEditorControlKeycodeStructByID<ControlKeycodeBase.MovementControl>("Leftward").keyCode))
            {
                (CB as Character2D).Walk(Character2D.MoveDirection.Left);
                DirectionKeyPressed = true;
            }
            if (Input.GetKey(ControlManager.ControlAssign.GetControlKeycode().
                GetEditorControlKeycodeStructByID<ControlKeycodeBase.MovementControl>("Rightward").keyCode))
            {
                (CB as Character2D).Walk(Character2D.MoveDirection.Right);
                DirectionKeyPressed = true;
            }
            if (!DirectionKeyPressed) (CB as Character2D).Walk(Character2D.MoveDirection.None);
        }

        private void JumpDetection()
        {
            if (Input.GetKeyDown(ControlManager.ControlAssign.GetControlKeycode().
                GetEditorControlKeycodeStructByID<ControlKeycodeBase.MovementControl>("Jump").keyCode))
            {
                CB.rigi2D.AddForce(new Vector2(0, JumpForce));
            }
        }
    }
}
