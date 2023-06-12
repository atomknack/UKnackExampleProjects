using System.Collections;
using System.Collections.Generic;
using UKnack.Attributes;
using UKnack.Events;
using UnityEngine;

public class PublishStringOnEnable : MonoBehaviour
{
    [SerializeField]
    [ValidReference(nameof(IPublisherStringValidation), typeof(IPublisher<string>))]
    private UnityEngine.Object _publisher;
    [SerializeField]
    private string _stringToPublish;

    private void OnEnable()
    {
        IPublisher<string> ipublisher = UKnack.Common.CommonStatic.ValidCast<IPublisher<string>>(_publisher);
        ipublisher.Publish(_stringToPublish);
    }
    
    private static IPublisher<string> IPublisherStringValidation(UnityEngine.Object obj) =>
        UKnack.Common.CommonStatic.ValidCast<IPublisher<string>>(obj);
}
