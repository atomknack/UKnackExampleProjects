using System.Collections;
using System.Collections.Generic;
using UKnack.Attributes;
using UnityEngine;

public class ValidArrayOfGameobjects : MonoBehaviour
{
    [SerializeField]
    [ValidReference(nameof(ValidateGameobjectsNotNull), typeof(GameObject))]
    protected GameObject[] _arrayOfGameobjects;

    /// <summary>
    /// Property Drawer not gets a object itself, and instead is applyed to each element. 
    /// We need a litte bit more complex check that will work both in Editor and Player(therefore also in builded game).
    /// </summary>
    /// <param name="arrayOrGameobject"></param>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="System.Exception"></exception>
    public static void ValidateGameobjectsNotNull(object arrayOrGameobject)
    {
        if (arrayOrGameobject == null)
            throw new System.ArgumentNullException(nameof(arrayOrGameobject));
        if (arrayOrGameobject is IReadOnlyList<GameObject> objectsArray)
        {
            for (var i = 0; i < objectsArray.Count; i++)
            {
                if (objectsArray[i] == null)
                    throw new System.ArgumentNullException($"{i} item is null, nulls not allowed");
            }
            return;
        }
        if (arrayOrGameobject is GameObject)
            return;

        throw new System.Exception("object is not array of gameobjects and not gameobject itself");
    }

    private void Awake()
    {
        //will throw if array or any it element is null
        ValidateGameobjectsNotNull(_arrayOfGameobjects);

    }
}
