using System;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;
using MLAPI.SceneManagement;
using UnityEngine.SceneManagement;

public class ConnectionHandle : NetworkBehaviour
{
    public static bool AllowConnections;
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;


        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if(IsServer)
                NetworkManager.StopHost();
        }
    }
    
    private void NetworkSceneManagerOnOnSceneSwitched()
    {
        
    }

    private void HandleServerStarted()
    {
        Debug.Log("Handle Server Started");
        NetworkSceneManager.SwitchScene("Hosting");
        AllowConnections = true;
    }

    private void HandleClientConnected(ulong clientId)
    {
        Debug.Log("Handle Client Connected");
    }
    

    private void HandleClientDisconnect(ulong clientId)
    {
        Debug.Log("Handle Client Disconnect");
    }
    
    private void OnDestroy()
    {
        if (NetworkManager.Singleton == null) { return; }

        NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
        NetworkSceneManager.OnSceneSwitched -= NetworkSceneManagerOnOnSceneSwitched;
    }
}
