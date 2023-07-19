using Mirror;
using UKnack.Commands;
using UnityEngine;

namespace UKnack.Concrete.Commands.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ServerChangeSceneCommand", menuName = "UKnack/Commands/MirrorServer/ChangeScene")]
    public class ServerChangeSceneCommand : CommandScriptableObject, ICommand<string>
    {
        [SerializeField]
        private string _sceneName;

        public override void Execute()
        {
            Execute(_sceneName);
        }

        public void Execute(string sceneName)
        {
            NetworkManager.singleton.ServerChangeScene(sceneName);
        }

    }
}

