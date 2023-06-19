using UKnack.Attributes.KnackAttributeDrawers;
using UnityEditor;
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(MarkNullAsPurpleAttribute))]
public class MarkNullAsPurpleAttributeDrawer : MarkNullAsColorAttributeDrawer
{

}
#endif