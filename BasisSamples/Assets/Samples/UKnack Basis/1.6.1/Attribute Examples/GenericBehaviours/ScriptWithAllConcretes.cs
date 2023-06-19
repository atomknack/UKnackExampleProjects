using System.Collections;
using System.Collections.Generic;
using UKnack.Attributes;
using UnityEngine;

public class ScriptWithAllConcretes : MonoBehaviour
{
    [SerializeField][ValidReference] private GenericBehaviour<short> shorty;
    [SerializeField][ValidReference] private GenericBehaviour<int> inty;
    [SerializeField][ValidReference] private GenericBehaviour<string> stringy;

    [SerializeField][MarkNullAsPurple] private GenericBehaviour<short> nullIsPurpleShorty;
    [SerializeField][MarkNullAsPurple] private GenericBehaviour<int> nullIsPurpleInty;
    [SerializeField][MarkNullAsPurple] private GenericBehaviour<string> nullIsPurpleStringy;
}
