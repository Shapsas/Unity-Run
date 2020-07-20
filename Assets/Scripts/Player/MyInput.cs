using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyInput : MonoBehaviour
{
    public static MyInput myInput;

    private InputMaster inputMaster;
    private StrafeMovement strafeMovement;
    public bool inputIsAvailable = true;

    private void Awake()
    {
        myInput = this;
        inputMaster = new InputMaster();
        strafeMovement = GetComponent<StrafeMovement>();

        EnableInput();

        inputMaster.Player.Movement.performed += Movement_performed;
        inputMaster.Player.Movement.canceled += Movement_canceled;

        if(LevelManager.levelManager.checkpoints[0].isTutorial)
        {
            inputMaster.Player.JumpHold.performed += JumpHold_performed;
            inputMaster.Player.JumpHold.canceled -= JumpHold_canceled;
        }
    }

    private void JumpHold_canceled(InputAction.CallbackContext obj)
    {

    }

    private void JumpHold_performed(InputAction.CallbackContext obj)
    {
        var value = obj.ReadValue<float>();
        if(value >= 0.9f)
        {
            TutorialManager.TM.FinishQuest3();
        }
    }

    public void EnableInput()
    {
        inputIsAvailable = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void DisableInput()
    {
        inputIsAvailable = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            GameManager.GM.ConfirmQuit();
            DisableInput();
        }

        if(inputIsAvailable)
        {
            if (Keyboard.current.spaceKey.isPressed)
            {
                strafeMovement.lastJumpPress = Time.time;
            }
            else
            {
                if (Keyboard.current.leftCtrlKey.wasPressedThisFrame)
                    strafeMovement.StartCrouch();
                if (Keyboard.current.leftCtrlKey.wasReleasedThisFrame)
                    strafeMovement.StopCrouch();
            }
            strafeMovement.crouching = Keyboard.current.leftCtrlKey.isPressed;
        }
    }

    private void OnEnable()
    {
        inputMaster.Enable();
    }

    private void OnDisable()
    {
        inputMaster.Disable();
    }

    private void Movement_canceled(InputAction.CallbackContext obj)
    {
        var value = obj.ReadValue<Vector2>();
        strafeMovement.SetValues(value.x, value.y);
    }

    private void Movement_performed(InputAction.CallbackContext obj)
    {
        var value = obj.ReadValue<Vector2>();
        strafeMovement.SetValues(value.x, value.y);
    }
}
