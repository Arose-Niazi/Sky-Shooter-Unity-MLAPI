using System.Collections;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine.SceneManagement;

public class Hosting : NetworkBehaviour
{

    public Transform spaceShip;
    public GameObject startButton;
    public Transform mainPanel;

    private GameObject _ship;

    private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
        Debug.Log("Start Function");
        SpawnMeServerRpc(NetworkManager.Singleton.LocalClientId);

        if (IsHost)
        {
            startButton.SetActive(true);
        }
        
    }
    
    private void OnDestroy()
    {
        // Prevent error in the editor
        if (NetworkManager.Singleton == null) { return; }
        
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
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
    
    [ServerRpc(RequireOwnership = false)]
    private void SpawnMeServerRpc(ulong clientId)
    {
        Debug.Log("Spawn Me Server");
        float x = -515 + 280 * (NetworkManager.Singleton.ConnectedClients.Count - 1);
        _ship = Instantiate(spaceShip, new Vector3(x, 50, 0), Quaternion.identity).gameObject;
        _ship.GetComponent<NetworkObject>().SpawnWithOwnership(clientId, null, true);
    }

    private void HandleClientDisconnect(ulong clientId)
    {
        DestroyMeServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyMeServerRpc()
    {
        _ship.GetComponent<NetworkObject>().Despawn();
    }
    
    [ClientRpc]
    private void DestroyMeClientRpc()
    {
        if (IsOwner)
        {
            _ship.GetComponent<NetworkObject>().Despawn();
        }
    }
}
 