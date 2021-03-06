﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Networking.Types;
using System.Collections;

public class NetworkLobbyManagerSpecific : NetworkLobbyManager {

    private Dropdown networkMatchesDropwork;
    private int _playersOnLobby = 0;
    private Dictionary<string, MatchInfoSnapshot> _matchesData = new Dictionary<string, MatchInfoSnapshot>();
    private List<Dropdown.OptionData> _optionMatchesList = new List<Dropdown.OptionData>();

    public void Start()
    {
        MMStart();
    }

    public int GetPlayersOnLobby()
    {
        int numberPlayers = 0;
        foreach (NetworkLobbyPlayer player in LobbyManager.lobbySlots)
        {
            if (player != null)
            {
                numberPlayers += 1;
            }

        }
        return numberPlayers;
    }
    
    public static NetworkLobbyManagerSpecific LobbyManager
    {
        get
        {
            if (singleton)
            {
                return singleton.GetComponent<NetworkLobbyManagerSpecific>();
            } else
            {
                return null;
            }
            
        }
    }

    public void MMStart()
    {
        Debug.Log("@ MMStart");
        this.StartMatchMaker();
    }

    public void MMListMaches(Dropdown listMatches)
    {
        Debug.Log("@ MMListMatches");
        if (!networkMatchesDropwork)
        {
            networkMatchesDropwork = listMatches;
        }
        this.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
    }

    private void DestroyDontDestroy()
    {
        GameObject temp = new GameObject();
        UnityEngine.Object.DontDestroyOnLoad(temp);
        UnityEngine.SceneManagement.Scene dontDestroyOnLoad = temp.scene;
        GameObject[] g = dontDestroyOnLoad.GetRootGameObjects();
        for (int i = 0; i < dontDestroyOnLoad.rootCount; i++)
        {
            Destroy(g[i]);
        }
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        DestroyDontDestroy();
        Application.Quit();

    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        DestroyDontDestroy();
        Application.Quit();
    }

    public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        Debug.Log("@ OnMatchList");
        base.OnMatchList(success, extendedInfo, matchList);

        if (!success)
        {
            Debug.Log("Failed OnMatchList:" + extendedInfo);
           
        } else
        {

            _optionMatchesList.Clear();
            foreach (var match in matchList)
            {
                Debug.Log(match.name);
                if (_matchesData.ContainsKey(match.name))
                {
                    continue;
                }

                _optionMatchesList.Add(new Dropdown.OptionData(match.name));

                _matchesData[match.name] = match;
            }

            networkMatchesDropwork.AddOptions(_optionMatchesList);
        }
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        Debug.Log("@ServerConnect");
        NetworkLobbyManagerSpecific.singleton.ServerChangeScene("WhoStartSceneNetwork");
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("@ClientConnect");
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        Debug.Log("@ClentReady");
        base.OnServerReady(conn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log("Connection ID:" + conn.connectionId);
        GameObject player = (GameObject)Instantiate(gamePlayerPrefab, Vector3.zero, Quaternion.identity);
        
        if (conn.connectionId != 0)
        {
            player.tag = "OtherPlayer";
            Debug.Log("Changing tag");
        }
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        return base.OnLobbyServerCreateGamePlayer(conn, playerControllerId);
    }

    public void MMJoin(string matchName)
    {
        if (matchName != null && matchName != "") {
            Debug.Log("@ MMJoinMatch");
            MatchInfoSnapshot match = _matchesData[matchName];
            this.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, OnMatchJoined);
        }
        
    }

    public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        Debug.Log("@ OnMathcJoined");
        base.OnMatchJoined(success, extendedInfo, matchInfo);

        if (!success)
        {
            Debug.Log("Failed OnMatchJoined:" + extendedInfo);
        }
        else
        {
            Debug.Log("Joined:" + matchInfo.networkId);
        }
    }

    public void MMCreateMatch(String rooomName)
    {
        Debug.Log("@ MMCreateMatch");
        this.matchMaker.CreateMatch(rooomName, 15, true, "", "", "", 0, 0, OnMatchCreate);
    }


    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        Debug.Log("@ OnMatchCreate");
        base.OnMatchCreate(success, extendedInfo, matchInfo);

        if (!success)
        {
            Debug.Log("Failed to create match");
        }
        else
        {
            Debug.Log("Match Created");
        }
    }

}
