using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationQuitCommand : MonoBehaviour
{
    public void Execute()
    {
        Application.Quit();
    }
}
