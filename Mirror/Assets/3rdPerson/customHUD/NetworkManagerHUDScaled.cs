// vis2k: GUILayout instead of spacey += ...; removed Update hotkeys to avoid
// confusion if someone accidentally presses one.
using TMPro;
using UnityEngine;

namespace Mirror
{
    /// <summary>Shows NetworkManager controls in a GUI at runtime.</summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/Network Manager HUD Scaled")]
    [RequireComponent(typeof(NetworkManager))]
    [HelpURL("https://mirror-networking.gitbook.io/docs/components/network-manager-hud")]
    public class NetworkManagerHUDScaled : MonoBehaviour
    {
        NetworkManager manager;

        public float scaledOffsetX = 0.1f;
        public float scaledOffsetY = 0.05f;

        public float sizeScaleX = 1f;
        public float sizeScaleY = 1f;

        public float elementHeightScaled = 1f;

        void Awake()
        {
            manager = GetComponent<NetworkManager>();
        }

        void OnGUI()
        {

            var style = new GUIStyle();
            style.stretchWidth = true;
            style.stretchHeight = true;

            float elementHeight = Mathf.Max(Screen.height * elementHeightScaled * 0.1f, 16);
            elementHeight *= 0.7f;

            var buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = (int)(elementHeight * 0.5f);
            buttonStyle.fixedHeight = elementHeight;

            var textfieldStyle = new GUIStyle(GUI.skin.textField);
            textfieldStyle.fontSize = buttonStyle.fontSize;
            textfieldStyle.fixedHeight = buttonStyle.fixedHeight;

            var labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontSize = buttonStyle.fontSize;
            labelStyle.fixedHeight = buttonStyle.fixedHeight;

            GUILayout.BeginArea(new Rect(scaledOffsetX*Screen.width, scaledOffsetY*Screen.height, 
                0.3f*sizeScaleX*Screen.width, sizeScaleY*Screen.height), style);

            if (!NetworkClient.isConnected && !NetworkServer.active)
            {
                StartButtons(buttonStyle, textfieldStyle);
            }
            else
            {
                StatusLabels(labelStyle);
            }

            // client ready
            if (NetworkClient.isConnected && !NetworkClient.ready)
            {

                if (GUILayout.Button("Client Ready", buttonStyle))
                {
                    NetworkClient.Ready();
                    if (NetworkClient.localPlayer == null)
                    {
                        NetworkClient.AddPlayer();
                    }
                }
            }

            StopButtons(buttonStyle);

            GUILayout.EndArea();
        }

        void StartButtons(GUIStyle buttonStyle, GUIStyle textfieldStyle)
        {
            if (!NetworkClient.active)
            {
                // Server + Client
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    if (GUILayout.Button("Host (Server + Client)", buttonStyle))
                    {
                        manager.StartHost();
                    }
                }

                // Client + IP
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Client", buttonStyle))
                {
                    manager.StartClient();
                }
                // This updates networkAddress every frame from the TextField
                manager.networkAddress = GUILayout.TextField(manager.networkAddress, textfieldStyle);
                GUILayout.EndHorizontal();

                // Server Only
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    // cant be a server in webgl build
                    GUILayout.Box("(  WebGL cannot be server  )");
                }
                else
                {
                    if (GUILayout.Button("Server Only", buttonStyle)) manager.StartServer();
                }
            }
            else
            {
                // Connecting
                GUILayout.Label($"Connecting to {manager.networkAddress}..");
                if (GUILayout.Button("Cancel Connection Attempt", buttonStyle))
                {
                    manager.StopClient();
                }
            }
        }

        void StatusLabels(GUIStyle labelStyle)
        {
            // host mode
            // display separately because this always confused people:
            //   Server: ...
            //   Client: ...
            if (NetworkServer.active && NetworkClient.active)
            {
                GUILayout.Label($"<b>Host</b>: running via {Transport.active}", labelStyle);
            }
            // server only
            else if (NetworkServer.active)
            {
                GUILayout.Label($"<b>Server</b>: running via {Transport.active}", labelStyle);
            }
            // client only
            else if (NetworkClient.isConnected)
            {
                GUILayout.Label($"<b>Client</b>: connected to {manager.networkAddress} via {Transport.active}", labelStyle);
            }
        }

        void StopButtons(GUIStyle buttonStyle)
        {
            // stop host if host mode
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Stop Host", buttonStyle))
                {
                    manager.StopHost();
                }
                if (GUILayout.Button("Stop Client", buttonStyle))
                {
                    manager.StopClient();
                }
                GUILayout.EndHorizontal();
            }
            // stop client if client-only
            else if (NetworkClient.isConnected)
            {
                if (GUILayout.Button("Stop Client", buttonStyle))
                {
                    manager.StopClient();
                }
            }
            // stop server if server-only
            else if (NetworkServer.active)
            {
                if (GUILayout.Button("Stop Server", buttonStyle))
                {
                    manager.StopServer();
                }
            }
        }
    }
}
