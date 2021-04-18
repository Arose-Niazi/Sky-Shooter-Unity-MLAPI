using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentHandler : MonoBehaviour
{
    private GameObject _parent;
    // Start is called before the first frame update
    void Start()
    {
        _parent = GameObject.FindWithTag("MainParentForSpawn");
        transform.SetParent(_parent.transform, false);
    }
    
}
