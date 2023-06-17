using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestScriptableObject", menuName = "UKnackExamples/Attributes/ITestScriptableObject", order = 110)]
public class TestScriptableObject : ScriptableObject, ITest
{
    public void Test()
    {
        throw new System.NotImplementedException();
    }
}
