using MLAPI;
using MLAPI.Messaging;
using UnityEngine;
using MLAPI.SceneManagement;

public class ConnectionHandle : NetworkBehaviour
{
    private bool _allowConnections;
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
    }

    private void HandleServerStarted()
    {
        Debug.Log("Handle Server Started");
        // Temporary workaround to treat host as client
        NetworkSceneManager.SwitchScene("Hosting");
        _allowConnections = true;
    }

    private void HandleClientConnected(ulong clientId)
    {
        Debug.Log("Handle Client Connected");
        //ConnectionCheckServerRpc();
    }
    

    private void HandleClientDisconnect(ulong clientId)
    {
        Debug.Log("Handle Client Disconnect");
        // Are we the client that is disconnecting?
    }
    
    private void OnDestroy()
    {
        // Prevent error in the editor
        if (NetworkManager.Singleton == null) { return; }

        NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
    }

    [ServerRpc]
    private void ConnectionCheckServerRpc()
    {
        ConnectClientRpc(_allowConnections);
    }

    [ClientRpc]
    private void ConnectClientRpc(bool allow)
    {
        if (!IsOwner) return;

        if(!allow)
            NetworkManager.Singleton.StopClient();
    }
}
