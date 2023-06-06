using System.Collections;
using System.Collections.Generic;
using UKnack.Attributes;
using UKnack.Events;
using UnityEngine;
using UnityEngine.UIElements;

public class UpdatePlayerCoordsUI_EventAsIntermediary : MonoBehaviour
{
    private const string NAMEOFUIPLAYERPOSITION = "PlayerPosition";

    [SerializeField]
    [ProvidedComponent]
    [DisableEditingInPlaymode]
    private UIDocument _uiDocument;

    [SerializeField]
    [ValidReference]
    [DisableEditingInPlaymode]
    private SOEvent<Vector3> _playerPosition;

    [SerializeField]
    [DisableEditingInPlaymode]
    [ValidReference]
    SOPublisher<Vector3> _setNewPlayerCoords;


    private Vector3Field _uiVector3;

    private void OnEnable()
    {
        _uiDocument = ProvidedComponentAttribute.Provide(gameObject, _uiDocument);
        _uiVector3 = _uiDocument.rootVisualElement.Q<Vector3Field>(NAMEOFUIPLAYERPOSITION);
        if (_uiVector3 == null)
            throw new System.ArgumentNullException($"{NAMEOFUIPLAYERPOSITION} Vector3Field not found");
        if (_playerPosition == null)
            throw new System.ArgumentNullException(nameof(_playerPosition));
        if (_setNewPlayerCoords == null)
            throw new System.ArgumentException(nameof(_setNewPlayerCoords));
        _playerPosition.Subscribe(OnPlayerPositionChanged);
        _uiVector3.RegisterCallback<ChangeEvent<Vector3>>(OnPositionChangedEvent);
    }

    private void OnPositionChangedEvent(ChangeEvent<Vector3> e)
    {
        _setNewPlayerCoords.Publish(e.newValue);
    }

    private void OnDisable()
    {
        _playerPosition.UnsubscribeNullSafe(OnPlayerPositionChanged);
        _uiVector3.UnregisterCallback<ChangeEvent<Vector3>>(OnPositionChangedEvent);
    }

    private void OnPlayerPositionChanged(Vector3 playerPosition)
    {
        _uiVector3.SetValueWithoutNotify(playerPosition);
    }
}
