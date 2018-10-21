using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionClickDetector : MonoBehaviour
{
    public int positionId = 0;
    public GameObject board;

    private void OnMouseDown()
    {
       BoardManager boardManager = board.GetComponent<BoardManager>();
       boardManager.ClickBehavior(positionId);
    }
}
