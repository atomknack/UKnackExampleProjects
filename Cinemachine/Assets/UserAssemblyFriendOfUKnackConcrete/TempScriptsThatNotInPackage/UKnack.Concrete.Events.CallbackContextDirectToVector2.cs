//temp until there is will be same in UKnackBasis


using UKnack.Attributes;
using UKnack.Events;
using UnityEngine;

using static UnityEngine.InputSystem.InputAction;

namespace UKnack.Concrete.Events
{
    [AddComponentMenu("UKnack/CallbackContexTo/DirectValueToVector2")]
    internal class CallbackContextDirectToVector2 : MonoBehaviour
    {
        [SerializeField]
        [ValidReference(typeof(IPublisher<Vector2>), nameof(IPublisher<Vector2>.Validate), typeof(IPublisher<Vector2>))]
        [DisableEditingInPlaymode]
        private UnityEngine.Object _iPublisher;

        private IPublisher<Vector2> _iPublisherAsInterface;

        public void PublishAsVector2(CallbackContext ctx)
        {
            Vector2 value = ctx.ReadValue<Vector2>();
            _iPublisherAsInterface.Publish(value);
        }

        private void Awake()
        {
            _iPublisherAsInterface = IPublisher<Vector2>.Validate(_iPublisher);
        }
    }
}