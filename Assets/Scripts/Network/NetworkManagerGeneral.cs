using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;


public class NetworkManagerGeneral : MonoBehaviour {

    public NetworkLobbyManagerSpecific LobbyManagerSpecific;
    public NetworkManagerSpecific DiscoveryManagerSpecific;

    public void Start()
    {
        LobbyManagerSpecific = new NetworkLobbyManagerSpecific();
        DiscoveryManagerSpecific = new NetworkManagerSpecific();
    }

    public void Awake()
    {
        DontDestroyOnLoad(this);
    }

}
