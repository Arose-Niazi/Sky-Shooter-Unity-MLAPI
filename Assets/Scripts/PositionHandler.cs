using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;

public class PositionHandler : NetworkBehaviour
{
    public NetworkVariableVector3 position = new NetworkVariableVector3(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.OwnerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });
    private void Start()
    {
        Debug.Log("Pos is" + transform.position);
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

