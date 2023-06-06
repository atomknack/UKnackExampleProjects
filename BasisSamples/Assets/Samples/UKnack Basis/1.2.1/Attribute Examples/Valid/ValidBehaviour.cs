using System;
using UKnack.Attributes;
using UnityEngine;
using UnityEngine.Events;

public class ValidBehaviour : MonoBehaviour, ITest
{
    [SerializeField]
    [ValidReference(typeof(ITest))]
    private UnityEngine.Object notNullWithInterfacePicker;

    [SerializeField]
    [ValidReference]
    private UnityEngine.Object notNull;

    [SerializeField]
    [ValidReference(nameof(ValidTestPickerInterface), typeof(ITest))]
    private UnityEngine.Object interfaceOnObject;

    [SerializeField]
    [ValidReference(nameof(ValidTestPickerInterface), typeof(ValidBehaviour))] 
    private UnityEngine.Object behaviourOnObject;

    [SerializeField]
    [ValidReference(nameof(ValidBehaviourThatHaveChildGameObjects), typeof(ValidBehaviour))] 
    private MonoBehaviour shouldHaveChild;

    [SerializeField]
    [ValidReference(typeof(ValidBehaviour), nameof(ValidBehaviourThatHaveChildGameObjects),
        typeof(ITest),
        typeof(UnityEngine.Component), 
        typeof(UnityEngine.GameObject), 
        typeof(ValidBehaviour), 
        typeof(ScriptableObject), 
        typeof(TestScriptableObject)
        )]
    private UnityEngine.Object manyPickersForObjectThatHaveChild;

    [SerializeField]
    [ValidReference(
    typeof(ITest),
    typeof(UnityEngine.Component),
    typeof(UnityEngine.GameObject),
    typeof(ValidBehaviour),
    typeof(ScriptableObject),
    typeof(TestScriptableObject)
    )]
    private UnityEngine.Object manyPickersForNotNull;

    [SerializeField]
    [ValidReference]
    private GenericBehaviour<short> shorty;

    static ITest ValidTestPickerInterface(UnityEngine.Object obj)
    {
        if (obj == null)
            throw new ArgumentNullException();
        return (ITest)obj;
    }

    static void ValidBehaviourThatHaveChildGameObjects(UnityEngine.Object obj)
    {
        if (obj == null) throw new ArgumentNullException();
        MonoBehaviour monoBehaviour = obj as MonoBehaviour;
        if (monoBehaviour.gameObject.transform.childCount == 0)
            throw new Exception("This behaviour's gameobject should have at least one child");
    }

    public void Test()
    {
        throw new NotImplementedException();
    }
}
