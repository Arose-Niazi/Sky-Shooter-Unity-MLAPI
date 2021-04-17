using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;

public class PositionHandler : NetworkBehaviour
{
    public NetworkVariableVector3 position = new NetworkVariableVector3(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.Everyone,
        ReadPermission = NetworkVariablePermission.Everyone
    });
    private void Start()
    {
        if(IsOwner)
            position.Value = transform.position;
    }
    

    void Update()
    {
        if(IsOwner)
            position.Value = transform.position;
        else
        {
            transform.position = position.Value;
        }
    }
}

