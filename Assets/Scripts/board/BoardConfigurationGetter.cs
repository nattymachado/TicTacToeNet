using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class BoardConfigurationGetter {


    private static string _configurationZoneName = "ConfigurationZone";

    public static BoardConfiguration getConfigurationObject()
    {

       
        Scene configurationZone = SceneManager.GetSceneByName(_configurationZoneName);
        
        BoardConfiguration _boardConfiguration = null;
        if (configurationZone.GetRootGameObjects().Length > 0)
        {
            GameObject configurationObject = configurationZone.GetRootGameObjects()[0];
            _boardConfiguration =  configurationObject.GetComponent<BoardConfiguration>();
            
        }
        
        return _boardConfiguration;
    }


}
