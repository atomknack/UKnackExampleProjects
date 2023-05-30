//temp until there is will be same in UKnackBasis

using UnityEngine;
using UnityEngine.Events;
using UKnack.Events;

using static UnityEngine.InputSystem.InputAction;

namespace UKnack.Concrete.Events
{

    /// This class not intended to be used in code, but only made for ease of creation scriptable object in Unity Editor
    [CreateAssetMenu(fileName = "SOEvent_Vector2", menuName = "UKnack/SOEvents/SOEvent_Vector2")]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    internal sealed class SOEvent_Concrete_Vector2 : SOEvent<Vector2>
    {
        [SerializeField] private UnityEvent<Vector2> _beforeSubscribers;
        [SerializeField] private UnityEvent<Vector2> _afterSubscribers;

        internal override void InternalInvoke(Vector2 v)
        {
            _beforeSubscribers.Invoke(v);
            base.InternalInvoke(v);
            _afterSubscribers.Invoke(v);
        }
    }
}


