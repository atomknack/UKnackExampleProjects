using Cinemachine;
using UKnack.Attributes;
using UKnack.Events;
using UnityEngine;

namespace UKnack.Concrete.Cinemachine
{

    [AddComponentMenu("UKnackCinemachine/UKnackCinemachineEventInputProvider")]
    public class UKnackCinemachineEventInputProvider : MonoBehaviour, AxisState.IInputAxisProvider, ISubscriberToEvent<Vector2>, ISubscriberToEvent<float>
    {
        [MarkNullAsColor(0.5f, 0.5f, 0, "If this reference is null, then no XY will be 0")]
        [DisableEditingInPlaymode]
        [Tooltip("SOEvent<Vector2> for XY movement")]
        public SOEvent<Vector2> _XYAxisEvent;

        [MarkNullAsColor(0.5f, 0.5f, 0, "If this reference is null, then no Z will be 0")]
        [DisableEditingInPlaymode]
        [Tooltip("SOEvent<float> for Z movement")]
        public SOEvent<float> _ZAxisEvent;

        private Vector2 _XYAxis = Vector2.zero;
        private float _ZAxis = 0;

        public string Description => throw new System.NotImplementedException();

        public virtual float GetAxisValue(int axis)
        {
            if (enabled)
            {
                switch (axis)
                {
                    case 0: return _XYAxis.x;
                    case 1: return _XYAxis.y;
                    case 2: return _ZAxis;
                }
            }
            return 0;
        }

        public void OnEventNotification(Vector2 xy) =>
            _XYAxis = xy;

        public void OnEventNotification(float z) =>
            _ZAxis = z;

        private void OnEnable()
        {
            if (_XYAxisEvent != null)
            {
                _XYAxisEvent.Subscribe(this);
            }
            if (_ZAxisEvent != null)
                _ZAxisEvent.Subscribe(this);
        }

        private void OnDisable()
        {
            _XYAxisEvent.UnsubscribeNullSafe(this);
            _ZAxisEvent.UnsubscribeNullSafe(this);
        }
    }
}