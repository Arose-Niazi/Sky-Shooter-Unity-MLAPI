using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using UnityEngine.SceneManagement;

public class Hosting : MonoBehaviour
{

    public Transform spaceShip;
    public float spaceShipXOff;
    public GameObject startButton;
    public Transform mainPanel;

    private Transform _spaceShip;
    
    
    private void Start()
    {
        Debug.Log("Start Function");
        NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;

        HandleServerStarted();
        HandleClientConnected(NetworkManager.Singleton.ServerClientId);

    }
    
    public void Leave()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.StopHost();
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.StopClient();
        }

        SceneManager.LoadScene("MainMenu");
    }
    
    private void OnDestroy()
    {
        // Prevent error in the editor
        if (NetworkManager.Singleton == null) { return; }

        NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
    }
    
    private void HandleServerStarted()
    {
        Debug.Log("Handle Server Started");
        // Temporary workaround to treat host as client
        if (NetworkManager.Singleton.IsHost)
        {
            startButton.SetActive(true);
        }
    }

    private void HandleClientConnected(ulong clientId)
    {
        Debug.Log("Handle Client Connected");
        // Are we the client that is connecting?
        if (NetworkManager.Singleton.IsHost)
        {
            float x = -515 + 280 * (NetworkManager.Singleton.ConnectedClients.Count - 1); 
            _spaceShip = Instantiate(spaceShip, 
                new Vector3(x, 50), 
                spaceShip.rotation);
            _spaceShip.SetParent(mainPanel, false);
            _spaceShip.GetComponent<NetworkObject>().Spawn();
        }
    }

    private void HandleClientDisconnect(ulong clientId)
    {
        Debug.Log("Handle Client Disconnect");
        // Are we the client that is disconnecting?
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            _spaceShip.GetComponent<NetworkObject>().Despawn();
            Destroy(_spaceShip);
        }
    }
}
