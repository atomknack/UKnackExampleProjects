//temp until there is will be same in UKnackBasis

using UnityEngine;
using UKnack.Attributes;
using UKnack.Events;

using static UnityEngine.InputSystem.InputAction;

namespace UKnack.Concrete.Events
{

/// This class not intended to be used in code, but only made for ease of creation scriptable object in Unity Editor
[CreateAssetMenu(fileName = "PublisherToSOEvent_Vector2", menuName = "UKnack/Publishers/To Vector2")]
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
internal class SOPublisher_Concrete_Vector2 : SOPublisher<Vector2>
{
    [SerializeField]
    [ValidReference(typeof(IEvent<Vector2>), nameof(IEvent<Vector2>.Validate),
        typeof(SOEvent<Vector2>),
        typeof(SOEvent_Concrete_Vector2)
    )] private SOEvent<Vector2> where;

    public override void Publish(Vector2 v)
    {
        ValidateWhere();
        where.InternalInvoke(v);
    }

    internal void ValidateWhere() =>
        IEvent<Vector2>.Validate(where);

}

}

