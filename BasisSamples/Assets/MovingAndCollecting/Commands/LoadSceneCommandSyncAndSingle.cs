using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneCommandSyncAndSingle : MonoBehaviour
{
    [SerializeField]
    private string _sceneName;

    protected void Execute()
    {
        Execute(_sceneName);
    }

    protected void Execute(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
