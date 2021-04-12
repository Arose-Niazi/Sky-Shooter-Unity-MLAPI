using SWNetwork;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public void OnSpawnerReady(bool alreadyFinishedSceneSetup, SceneSpawner sceneSpawner)
    {
        if (!alreadyFinishedSceneSetup)
        {
            float posX = -16f;
            float posY = Random.Range(-7, 7);

            NetworkClient.Instance.LastSpawner.SpawnForPlayer(0, new Vector3(posX, posY, 0), Quaternion.identity);
            
            NetworkClient.Instance.LastSpawner.PlayerFinishedSceneSetup();
        }
    }
}
