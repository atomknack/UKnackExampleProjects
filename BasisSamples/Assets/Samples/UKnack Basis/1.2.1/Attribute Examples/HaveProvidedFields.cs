using UKnack.Attributes;
using UnityEngine;

public class HaveProvidedFields : MonoBehaviour, ITest
{
    [SerializeField]
    [ProvidedComponent]
    private MonoBehaviour command;

    [SerializeField]
    [DisableEditingInPlaymode]
    [ProvidedComponent]
    private MonoBehaviour commandPlaymodeDisabled1;

    [SerializeField]
    [ProvidedComponent]
    [DisableEditingInPlaymode]
    private MonoBehaviour commandPlaymodeDisabled2;

    private MonoBehaviour prevValue;
    //[SerializeField]
    //ValueDebugInfo<MonoBehaviour> changedInGame;
    public void Test()
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        prevValue = command;
        command = ProvidedComponentAttribute.Provide(gameObject, command);
        commandPlaymodeDisabled1 = command;
        commandPlaymodeDisabled2 = command;
        //prevValue = command;
    }

    private void Update()
    {
        if (prevValue != command)
        {
            Debug.Log($"changed from '{prevValue}' to '{command}'");
            prevValue = command;
           // changedInGame = command;
        }
    }
}
