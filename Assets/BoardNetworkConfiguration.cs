using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardNetworkConfiguration : MonoBehaviour {


    private static BoardNetworkConfiguration singleton;

    private int _starter = 0;
    private string _networkType = "";

    public static BoardNetworkConfiguration Instance
    {
        get
        {
            if (singleton == null) singleton = new BoardNetworkConfiguration();
            return singleton;
        }
    }


    public int Starter
    {
        get
        {
            return _starter;
        }
        set
        {
            _starter = value;
        }
    }

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

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
