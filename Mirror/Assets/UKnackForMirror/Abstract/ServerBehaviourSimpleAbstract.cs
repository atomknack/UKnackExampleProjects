using Mirror;

public abstract class ServerBehaviourSimpleAbstract : NetworkBehaviour
{

    private bool _serverRunning = false;

    protected abstract void NetworkBehaviourOnEnable();
    protected abstract void NetworkBehaviourOnDisable();

    public override void OnStartServer()
    {
        NetworkBehaviourOnEnable();
        _serverRunning = true;
    }

    public override void OnStopServer() 
    {
        if (_serverRunning == false)
            return;
        NetworkBehaviourOnDisable();
        _serverRunning = false;
    }

    public void OnDisable() 
    {
        OnStopServer();
        OnStopClient();
    }

}
