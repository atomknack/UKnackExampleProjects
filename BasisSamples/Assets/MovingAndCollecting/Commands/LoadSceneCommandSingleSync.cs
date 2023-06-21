using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneCommandSingleSync : MonoBehaviour
{
    [SerializeField]
    private string _sceneName;

    public void Execute()
    {
        Execute(_sceneName);
    }

    public void Execute(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
