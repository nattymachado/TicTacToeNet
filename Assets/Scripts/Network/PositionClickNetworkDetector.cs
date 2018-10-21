using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PositionClickNetworkDetector : NetworkBehaviour
{
    public int positionId = 0;

    public void Start()
    {
        
    }

    [Command]
    public void CmdClick(int positionId, bool sentByServer)
    {
        Debug.Log("Posicao clicada:" + positionId);
        BoardNetworkManager boardManager = GetComponentInParent<BoardNetworkManager>();
        boardManager.ClickBehavior(positionId, sentByServer);
    }

    

    



    private void OnMouseDown()
    {

       CmdClick(positionId, isServer);



    }
}
