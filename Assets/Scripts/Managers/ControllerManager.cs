using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    public enum InputType
    {
        Move,
        Jump,
        Interact
    }

    public static class ControllerManager
    {
        private static readonly InputSystem_Actions _inputSystem;

        private static InputActionMap _actionMap;

        static ControllerManager()
        {
            _inputSystem = new InputSystem_Actions();

            _inputSystem.Enable();

            _actionMap = _inputSystem.asset.FindActionMap("Player", throwIfNotFound: true);
        }

        /// <summary>
        /// You have to define the Input Type (Move, Look, etc.) <see cref="InputType" />,
        /// and an <see cref="Action" /> function with an <see cref="InputAction.CallbackContext" /> by params.
        /// </summary>
        public static void AddPerformanceEvent(InputType inputType, Action<InputAction.CallbackContext> action)
        {
            var actionName = inputType.ToString();
            var inputAction = _actionMap.FindAction(actionName, throwIfNotFound: false);
            if (inputAction == null)
            {
                Debug.LogWarning($"Can't find \"{actionName}\" in the ActionMap \"Player\".");
                return;
            }

            inputAction.performed += action;
        }

        /// <summary>
        /// You have to define the Input Type (Move, Look, etc.) <see cref="InputType" />,
        /// and an <see cref="Action" /> function with an <see cref="InputAction.CallbackContext" /> by params.
        /// </summary>
        public static void AddCancelEvent(InputType inputType, Action<InputAction.CallbackContext> action)
        {
            var actionName = inputType.ToString();
            var inputAction = _actionMap.FindAction(actionName, throwIfNotFound: false);
            if (inputAction == null)
            {
                Debug.LogWarning($"Can't find \"{actionName}\" in the ActionMap \"Player\".");
                return;
            }

            inputAction.canceled += action;
        }

        //TODO: Add remove for each case
        public static void RemoveEvent(InputType inputType, Action<InputAction.CallbackContext> action)
        {
            var actionName = inputType.ToString();
            var inputAction = _actionMap.FindAction(actionName, throwIfNotFound: false);
            if (inputAction == null)
            {
                Debug.LogWarning($"Can't find \"{actionName}\" in the ActionMap \"Player\".");
                return;
            }

            inputAction.performed -= action;
        }
    }
}