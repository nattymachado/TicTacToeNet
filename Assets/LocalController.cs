using System;
using UnityEngine;
using UnityEngine.Networking;

public class LocalController : MonoBehaviour
{

    private string _networkType = "";

    public string NetworkType
    {
        get
        {
            return _networkType;
        }
        set
        {
            _networkType = value;
        }
    }

    
    

}
