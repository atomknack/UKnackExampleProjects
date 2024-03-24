//temp until there is will be same in UKnackBasis


using UKnack.Attributes;
using UKnack.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace UKnack.Concrete.Events
{
    [AddComponentMenu("UKnack/CallbackContexTo/WithDeviceScaleToVector2")]
    internal class CallbackContextWithDeviceScaleToVector2 : MonoBehaviour
    {
        [SerializeField]
        public Vector2 pointerScale = Vector2.one;

        [SerializeField]
        public Vector2 gamepadScale = Vector2.one;

        [SerializeField]
        private bool _gamepadContinuouslyPublisWhilePressed = false;

        private bool _pressed = false;

        private Vector2 _value = Vector2.zero;

        [SerializeField]
        [ValidReference(typeof(IPublisher<Vector2>), nameof(IPublisher<Vector2>.Validate), typeof(IPublisher<Vector2>))]
        [DisableEditingInPlaymode]
        private UnityEngine.Object _iPublisher;

        private IPublisher<Vector2> _iPublisherAsInterface;

        public void PublishAsVector2(CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>();

            _value = Scale(value, ref context);

            var currentDevice = context.control.device;

            // dirty
            if (_gamepadContinuouslyPublisWhilePressed && currentDevice == Gamepad.current)
            {
                var phase = context.phase;
                if (phase == InputActionPhase.Performed)
                    _pressed = true;
                if (phase == InputActionPhase.Disabled || phase == InputActionPhase.Canceled)
                    _pressed = false;
                return;
            } // end of dirty

            _iPublisherAsInterface.Publish(_value);

            Vector2 Scale(Vector2 value, ref CallbackContext context)
            {
                var currentDevice = context.control.device;
                if (currentDevice == Mouse.current || currentDevice == Touchscreen.current || currentDevice == Pointer.current)
                    return value * pointerScale;

                if (currentDevice == Gamepad.current)
                    return value * gamepadScale;

                return value;
            }
        }

        private void Update()
        {
            if (_pressed)
            {
                _iPublisherAsInterface.Publish(_value*Time.deltaTime);
            }
        }

        private void Awake()
        {
            _iPublisherAsInterface = IPublisher<Vector2>.Validate(_iPublisher);
        }
    }
}