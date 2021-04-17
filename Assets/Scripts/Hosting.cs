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
        SpawnMeServerRpc(NetworkManager.Singleton.LocalClientId);
        if (IsHost)
        {
            startButton.SetActive(true);
        }
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
        float x = -515 + 280 * ((clientId>0)?(int)clientId-1:0);
        _ship = Instantiate(spaceShip, new Vector3(x, 50, 0), Quaternion.identity).gameObject;
        _ship.GetComponent<NetworkObject>().SpawnWithOwnership(clientId, null, true);
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
 