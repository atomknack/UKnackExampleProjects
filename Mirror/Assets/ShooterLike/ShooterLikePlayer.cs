using Mirror;
using TMPro;
using UKnack.Attributes;
using UKnack.Events;
using UnityEngine;

public class ShooterLikePlayer : NetworkBehaviour
{
    [SerializeField]
    [ValidReference]
    [DisableEditingInPlaymode]
    private SOEvent<Vector2> _xzInputEvent;

    [SerializeField]
    [ValidReference]
    [DisableEditingInPlaymode]
    private TMP_Text _floatingPlayerText;

    [SyncVar(hook = nameof(OnNameChanged))]
    private string _playerName;

    [SyncVar(hook = nameof(OnColorChanged))]
    private Color _playerNameColor = Color.white;

    private Vector3 _move = Vector3.zero;

    private void OnNameChanged(string oldValue, string newValue)
    {
        _floatingPlayerText.text = newValue;
    }
    private void OnColorChanged(Color oldValue, Color newValue)
    {
        _floatingPlayerText.color = newValue;
    }


    public void OnEnable()
    {
        if (_floatingPlayerText == null)
            throw new System.ArgumentNullException(nameof(_floatingPlayerText));
        if (_xzInputEvent == null)
            throw new System.ArgumentNullException(nameof(_xzInputEvent));
        _xzInputEvent.Subscribe(OnInputXZ);

    }

    public void OnDisable()
    {
        _xzInputEvent.UnsubscribeNullSafe(OnInputXZ);
    }

    public void OnInputXZ(Vector2 input)
    {
        //Debug.Log(input);
        _move = new Vector3(input.x, 0, input.y);
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = Vector3.zero;

        string name = "Player" + UnityEngine.Random.Range(100, 999);
        Color color = new(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f));
        CmdSetupPlayer(name, color);
    }

    [Command]
    public void CmdSetupPlayer(string playerName, Color nameColor)
    {
        _playerName = playerName;
        _playerNameColor = nameColor;
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        transform.Rotate(0, _move.x * Time.deltaTime * 110.0f, 0);
        transform.Translate(0, 0, _move.z * Time.deltaTime * 4f);
    }
}
