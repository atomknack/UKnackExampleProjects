using System.Collections;
using System.Collections.Generic;
using UKnack.Attributes;
using UnityEngine;

public class NeedForIDummy : MonoBehaviour
{
    [SerializeReference][ValidReference(typeof(IDummy))] public MonoBehaviour dummy;
}
