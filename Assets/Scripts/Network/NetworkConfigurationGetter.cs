using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class NetworkConfigurationGetter {



    public static BoardNetworkConfiguration getConfigurationObject()
    {
        GameObject temp = null;
        Debug.Log("1");
        temp = new GameObject();
        Debug.Log("2");
        Object.DontDestroyOnLoad(temp);
        Debug.Log("3");
        UnityEngine.SceneManagement.Scene dontDestroyOnLoad = temp.scene;
        GameObject[] g = dontDestroyOnLoad.GetRootGameObjects();
        int position = 0;
        for (int i = 0; i < dontDestroyOnLoad.rootCount; i++)
        {
            Debug.Log("Root: " + g[i].name);
            if (g[i].name == "Configuration")
            {
                position = i;
                break;
            }
        }
        return dontDestroyOnLoad.GetRootGameObjects()[position].GetComponent<BoardNetworkConfiguration>();
    }

    public static NetworkManagerSpecific GetLanManager()
    {
        GameObject temp = null;
        temp = new GameObject();
        Object.DontDestroyOnLoad(temp);
        UnityEngine.SceneManagement.Scene dontDestroyOnLoad = temp.scene;
        GameObject[] g = dontDestroyOnLoad.GetRootGameObjects();
        int position = 0;
        for (int i = 0; i < dontDestroyOnLoad.rootCount; i++)
        {
            if (g[i].name == "NetworkManagerController")
            {
                position = i;
                break;
            }
        }
        return dontDestroyOnLoad.GetRootGameObjects()[position].GetComponent<NetworkManagerSpecific>();
    }




}
