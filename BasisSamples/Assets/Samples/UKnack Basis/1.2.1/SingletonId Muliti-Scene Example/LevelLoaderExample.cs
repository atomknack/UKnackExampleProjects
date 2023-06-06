using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UKnack.Preconcrete.UI.SimpleToolkit;

public class LevelLoaderExample : EffortlessButtonClick
{
    [SerializeField]
    private string _sceneName;

    protected override void ButtonClicked()
    {
        if (string.IsNullOrWhiteSpace(_sceneName))
            throw new Exception("scene name should not be empty or whitespaces only");

        SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
    }
}
