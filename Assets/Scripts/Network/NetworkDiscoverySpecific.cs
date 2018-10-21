using System;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkDiscoverySpecific : NetworkDiscovery {

    
        public override void OnReceivedBroadcast(string fromAddress, string data)
        {
            Debug.Log("Received broadcast from: " + fromAddress + " with the data: " + data);
        }
  

}
