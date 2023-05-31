//temp until there is will be same in UKnackBasis

using UnityEngine;
using UKnack.Attributes;
using UKnack.Events;

using static UnityEngine.InputSystem.InputAction;

namespace UKnack.Concrete.Events
{
    [AddComponentMenu("UKnack/CallbackContexTo/ValueToBool")]
    internal sealed class CallbackContextValueToBool : MonoBehaviour
    {
        [SerializeField]
        [ValidReference(typeof(IPublisher<bool>), nameof(IPublisher<bool>.Validate), typeof(IPublisher<bool>))]
        [DisableEditingInPlaymode]
        private UnityEngine.Object _iPublisher;

        private IPublisher<bool> _iPublisherAsInterface;

        public void PublishAsBool(CallbackContext ctx)
        {
            float rawvalue = ctx.ReadValue<float>();
            bool value = (1 - rawvalue) < 0.999f; 
            _iPublisherAsInterface.Publish(value);
        }

        private void Awake()
        {
            _iPublisherAsInterface = IPublisher<bool>.Validate(_iPublisher);
        }
    }
}