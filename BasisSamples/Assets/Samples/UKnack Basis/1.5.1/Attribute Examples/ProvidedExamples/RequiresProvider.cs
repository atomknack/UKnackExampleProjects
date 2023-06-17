using System.Collections;
using System.Collections.Generic;
using UKnack.Attributes;
using UnityEngine;

public class RequiresProvider : MonoBehaviour
{
    [SerializeField]
    [ProvidedComponent] 
    private ProviderBehaviour _provided;

    private void Awake()
    {
        //Will throw in runtime if _provided not set to something and cannot provide some ProviderBehaviour from parent chain
        _provided = ProvidedComponentAttribute.Provide<ProviderBehaviour>(gameObject, _provided);
    }
}
