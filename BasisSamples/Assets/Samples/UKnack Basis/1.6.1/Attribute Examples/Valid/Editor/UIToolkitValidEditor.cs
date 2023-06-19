#if UNITY_EDITOR
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[UnityEditor.CustomEditor(typeof(UIToolkitValid))]
[UnityEditor.CanEditMultipleObjects]
public class UIToolkitValidEditor : UnityEditor.Editor
{
    VisualElement _editorRoot;
    public override VisualElement CreateInspectorGUI()
    {
        _editorRoot = new VisualElement();

        // Add a simple label
        _editorRoot.Add(new Label("UI Toolkit custom inspector:"));
        DrawProperty("notNullWithInterfacePicker");
        DrawProperty("notNull");
        DrawProperty("interfaceOnObject");
        DrawProperty("behaviourOnObject");
        DrawProperty("shouldHaveChild");
        DrawProperty("manyPickersForObjectThatHaveChild");
        DrawProperty("manyPickersForNotNull");
        DrawProperty("shorty");
        return _editorRoot;
    }

    private void DrawProperty(string name)
    {
        _editorRoot.Add(new PropertyField(serializedObject.FindProperty(name)));
    }
}
#endif