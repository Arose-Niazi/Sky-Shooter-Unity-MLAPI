using System;
using System.Collections;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.SceneManagement;
using UnityEngine.SceneManagement;

public class Hosting : NetworkBehaviour
{

    public Transform spaceShip;
    public GameObject startButton;
    private GameObject _ship;

    private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
        SpawnMeServerRpc(NetworkManager.Singleton.LocalClientId);
        if (IsHost)
        {
            startButton.SetActive(true);
        }
    }
    
    private void OnDestroy()
    {
        if (NetworkManager.Singleton == null) { return; }
        
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
    }
    
    public void Leave()
    {
        DestroyMeServerRpc();
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
    
    [ServerRpc(RequireOwnership = false)]
    private void SpawnMeServerRpc(ulong clientId)
    {
        float x = -515 + 280 * (NetworkManager.Singleton.ConnectedClients.Count - 1);
        _ship = Instantiate(spaceShip, new Vector3(x, 50, 0), Quaternion.identity).gameObject;
        _ship.GetComponent<NetworkObject>().SpawnWithOwnership(clientId, null, true);
    }

    private void HandleClientDisconnect(ulong clientId)
    {
        SceneManager.LoadScene("MainMenu");
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyMeServerRpc()
    {
        _ship.GetComponent<NetworkObject>().Despawn(true);
    }

    private void Update()
    {
        if(!IsClient) SceneManager.LoadScene("MainMenu");
    }
    
}
 