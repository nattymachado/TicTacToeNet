using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class NetworkManagerSpecific : NetworkManager {

    public static event Action<NetworkConnection> onServerConnect;
    public static event Action<NetworkConnection> onClientConnect;

    public static NetworkDiscovery Discovery
    {
        get
        {
            Debug.Log("Getting");
            if (singleton)
            {
                return singleton.GetComponent<NetworkDiscovery>();
            } else
            {
                return null;
            }
            
        }
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        if (conn.address == "localClient")
        {
            return;
        }

        Debug.Log("Client connected! Address: " + conn.address);

        //conn.playerControllers.Count > 1

        if (onServerConnect != null)
        {
            onServerConnect(conn);
        }
    }

    //Detect when a client connects to the Server
    public override void OnClientConnect(NetworkConnection conn)
    {

        //Output text to show the connection on the client side
        Debug.Log(ClientScene.reconnectId);
        Debug.Log("Client Side : Client " + conn.connectionId + " Connected!");
        NetworkManagerSpecific.singleton.ServerChangeScene("WhoStartSceneNetwork");
        
        

    }


    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        base.OnClientError(conn, errorCode);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log("Connection ID:" + conn.connectionId);
        GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        if (conn.connectionId > 0)
        {
            player.tag = "OtherPlayer";
            Debug.Log("Changing tag");
        }
        
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    

    public override void ServerChangeScene(string newSceneName)
    {
        Debug.Log("Trocando de página");
        SceneManager.LoadScene(newSceneName);
        base.ServerChangeScene(newSceneName);
    }

    //Detect when a client connects to the Server
    public override void OnClientSceneChanged(NetworkConnection conn)
    {

       // ClientScene.Ready(conn);
        base.OnClientSceneChanged(conn);
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

    
    public static void StartDiscovery()
    {
        NetworkManagerSpecific.Discovery.Initialize();
        NetworkManagerSpecific.Discovery.StartAsClient();
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


    // Use this for initialization
    void Start () {
        NetworkManagerSpecific.Discovery.Initialize();
        NetworkManagerSpecific.Discovery.StartAsClient();

    }
	/*
	// Update is called once per frame
	void Update () {
		
	}

    

    */

}
