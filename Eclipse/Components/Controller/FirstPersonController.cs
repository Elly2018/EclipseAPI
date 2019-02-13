using System;
using Eclipse.Base.Struct;
using Eclipse.Components.Character;
using Eclipse.Managers;
using UnityEngine;

namespace Eclipse.Components.Controller
{
    public class FirstPersonController : ControllerBase
    {
        [SerializeField] private Transform CameraPot;
        [SerializeField] private bool WiggleCamera;

        [SerializeField] private float MouseSensitivity = 0.5f;
        [SerializeField] private float WalkingSpeed = 1.0f;
        [SerializeField] private float RunningMultiply = 1.5f;
        [SerializeField] private float StepLength = 0.5f;
        [SerializeField] private float JumpingForce = 200.0f;

        bool ClickAnyDirectionKey = false;
        bool Running = false;

        private void Start()
        {
            base.ControllerInitialize();
        }

        private void Update()
        {
            _Cursor = CharacterCamera.transform.TransformPoint(new Vector3(0, 0, 3));
            FirstPersonCharacter FPC = (FirstPersonCharacter)CB;
            if (!FPC) return;
            if (ControlManager.ControllerAssign.CheckPairController(this))
            {
                CharacterManager.CharacterControl.InGameMouseLock(MouseControl);
                CameraRotation();
            }
        }

        private void FixedUpdate()
        {
            DoubleKeyDetection.UpdateAll(Time.deltaTime);
            FirstPersonCharacter FPC = (FirstPersonCharacter)CB;
            if (!FPC) return;
            if (ControlManager.ControllerAssign.CheckPairController(this))
            {
                if (MovementControl && !FPC.InAir && !FPC.GetNoClip()) ControllerMovementUpdate();
                if (MovementControl && FPC.GetNoClip()) NoClipMovement();
                if (MovementControl && !FPC.InAir) CameraMovingAnim();
                if (MovementControl && !FPC.InAir) ControllerJumpUpdate();
            }
        }

        #region Movement
        private void NoClipMovement()
        {
            FirstPersonCharacter FPC = (FirstPersonCharacter)CB;
            ControlKeycodeBase CKB = ControlManager.ControlAssign.GetControlKeycode();
            ClickAnyDirectionKey = false;
            Vector3 MovingPos = Vector3.zero;
            if (Input.GetKey(CKB.GetEditorControlKeycodeStructByID<ControlKeycodeBase.MovementControl>("Forward").keyCode))
            {
                ClickAnyDirectionKey = true;
                MovingPos += new Vector3(0, 0, 1);
            }
            if (Input.GetKey(CKB.GetEditorControlKeycodeStructByID<ControlKeycodeBase.MovementControl>("Backward").keyCode))
            {
                ClickAnyDirectionKey = true;
                MovingPos += new Vector3(0, 0, -1);
            }
            if (Input.GetKey(CKB.GetEditorControlKeycodeStructByID<ControlKeycodeBase.MovementControl>("Leftward").keyCode))
            {
                ClickAnyDirectionKey = true;
                MovingPos += new Vector3(-1, 0, 0);
            }
            if (Input.GetKey(CKB.GetEditorControlKeycodeStructByID<ControlKeycodeBase.MovementControl>("Rightward").keyCode))
            {
                ClickAnyDirectionKey = true;
                MovingPos += new Vector3(1, 0, 0);
            }
            Running = Input.GetKey(CKB.GetEditorControlKeycodeStructByID<ControlKeycodeBase.MovementControl>("Running").keyCode);
            if (ClickAnyDirectionKey)
            {
                FPC.CharacterMoving(CharacterCamera.transform.TransformPoint(MovingPos) - transform.position, StepLength, RunningMultiply, Running);
            }
                
        }
        private void ControllerMovementUpdate()
        {
            FirstPersonCharacter FPC = (FirstPersonCharacter)CB;
            ControlKeycodeBase CKB = ControlManager.ControlAssign.GetControlKeycode();
            Vector3 MovingPos = Vector3.zero;
            ClickAnyDirectionKey = false;
            if (Input.GetKey(CKB.GetEditorControlKeycodeStructByID<ControlKeycodeBase.MovementControl>("Forward").keyCode))
            {
                ClickAnyDirectionKey = true;
                MovingPos += new Vector3(0, 0, 1);
            }
            if (Input.GetKey(CKB.GetEditorControlKeycodeStructByID<ControlKeycodeBase.MovementControl>("Backward").keyCode))
            {
                ClickAnyDirectionKey = true;
                MovingPos += new Vector3(0, 0, -1);
            }
            if (Input.GetKey(CKB.GetEditorControlKeycodeStructByID<ControlKeycodeBase.MovementControl>("Leftward").keyCode))
            {
                ClickAnyDirectionKey = true;
                MovingPos += new Vector3(-1, 0, 0);
            }
            if (Input.GetKey(CKB.GetEditorControlKeycodeStructByID<ControlKeycodeBase.MovementControl>("Rightward").keyCode))
            {
                ClickAnyDirectionKey = true;
                MovingPos += new Vector3(1, 0, 0);
            }
            Running = Input.GetKey(CKB.GetEditorControlKeycodeStructByID<ControlKeycodeBase.MovementControl>("Running").keyCode);
            if (ClickAnyDirectionKey)
                FPC.CharacterMoving(transform.TransformPoint(MovingPos) - transform.position, StepLength, RunningMultiply, Running);
        }
        private void CameraMovingAnim()
        {
            anim.SetBool("IsRunning", Running);
            if (ClickAnyDirectionKey)
                anim.SetBool("IsMoving", true);
            else
                anim.SetBool("IsMoving", false);
        }
        #endregion

        #region Action
        private void ControllerJumpUpdate()
        {
            FirstPersonCharacter FPC = (FirstPersonCharacter)CB;
            if (!FPC) return;
            KeyCode Jump = ControlManager.ControlAssign.GetControlKeycode().GetEditorControlKeycodeStructByID<ControlKeycodeBase.MovementControl>("Jump").keyCode;
            if (Input.GetKeyDown(Jump))
            {
                FPC.CharacterJump(new Vector3(0, 1, 0) * JumpingForce);
            }
        }
        #endregion

        #region Camera
        private void CameraRotation()
        {
            transform.localEulerAngles += new Vector3(0, Input.GetAxis("Mouse X"), 0) * MouseSensitivity * 5;

            /* Store the transform eular angle value */
            Vector3 Eular = new Vector3(); Eular = CameraPot.localEulerAngles;
            Eular += new Vector3(-Input.GetAxis("Mouse Y"), 0, 0) * MouseSensitivity * 5;
            /* Get the dotg product between direction vector to up vector */
            float dotProduct = Vector3.Dot(new Vector3(0, 1, 0).normalized, Quaternion.Euler(Eular.x, Eular.y, Eular.z) * Vector3.forward);
            /* Transform to absolute value */
            dotProduct = Mathf.Abs(dotProduct);
            /* if it's not bigger than 0.9f, just apply the eular angle */
            if (dotProduct < 0.9f)
            {
                CameraPot.localEulerAngles = Eular;
            }
        }
        #endregion
    }
}
