using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class JoystickControl : MonoBehaviour
{
        public GameObject movementAreaForMobile;
        public FixedJoystick moveJoystick;
        public FixedButton jumpButton;
        public FixedTouchField touchField;
        FirstPersonController fps;
        private void Awake()
        {
            fps = GetComponent<FirstPersonController>();

            if (fps.currentInputType == InputType.mobile)
            {
                movementAreaForMobile.SetActive(true);
            }
            if (fps.currentInputType == InputType.PC)
            {
                movementAreaForMobile.SetActive(false);
            }
        }

        private void Update()
        {
            fps.RunAxis = moveJoystick.Direction;
            fps.JumpAxis = jumpButton.Pressed;
            fps.m_MouseLook.LookAxis = touchField.TouchDist;
        }
    }

}

