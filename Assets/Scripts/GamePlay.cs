using MLAPI;
using MLAPI.Messaging;
using MLAPI.SceneManagement;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GamePlay : NetworkBehaviour
{
   public GameObject pausedMenu;
   public Transform player;

   private GameObject _me;
   
   private void Awake()
   {
      Resume();
   }

   private void Start()
   {
      if(IsClient)
         SpawnMeServerRpc(NetworkManager.Singleton.LocalClientId);
      else
      {
         _me = Instantiate(player, new Vector3(-15, 0, 0), quaternion.identity).gameObject;
      }
   }

   public void Resume()
   {
      Time.timeScale = 1f;
      pausedMenu.SetActive(false);
   }

   public void Pause()
   {
      pausedMenu.SetActive(true);
      Time.timeScale = 0f;
   }

   public void Restart()
   {
      SceneManager.LoadScene("Loading");
   }

   public void MainMenu()
   {
      if(IsClient)
         DestroyMeServerRpc();
      if (IsHost)
         NetworkSceneManager.SwitchScene("Hosting");
      else
         SceneManager.LoadScene("MainMenu");
   }
   
   
   [ServerRpc(RequireOwnership = false)]
   private void SpawnMeServerRpc(ulong clientId)
   {
      float x = -515 + 280 * ((clientId>0)?(int)clientId-1:0);
      _me = Instantiate(player, new Vector3(-15, Random.Range(-10,10), 0), Quaternion.identity).gameObject;
      _me.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, null, true);
   }
    
   [ServerRpc(RequireOwnership = false)]
   public void DestroyMeServerRpc()
   {
      _me.GetComponent<NetworkObject>().Despawn(true);
   }

}
