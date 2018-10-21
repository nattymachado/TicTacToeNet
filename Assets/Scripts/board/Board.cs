using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board {

    public Board() {
        InitializePositions();
    }

    private int[,] _positions;


    public int GetPosition(int line, int column)
    {
        return _positions[line, column];
    }

    public int[,] GetPositions()
    {
        return _positions;
    }

    public void SetPosition(int line, int column, int player)
    {
        _positions[line, column] = player;
    }

    public void SetPositions(int[,] positions)
    {
        for (int line = 0; line < 3; line++)
        {
            for (int column = 0; column < 3; column++)
            {
                _positions[line, column] = positions[line, column];
            }
        }
        
    }

    public Dictionary<string, int> GetBoardDimensions()
    {

        Dictionary<string, int> Dimensions = new Dictionary<string, int>();
        Dimensions["lines"] = 3;
        Dimensions["columns"] = 3;
        return Dimensions;
    }


    public void InitializePositions()
    {
        _positions = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
    }

    public bool CheckEmptyPositions()
    {
        for (int line = 0; line < 3; line++)
        {
            for (int column = 0; column < 3; column++)
            {
                if (this.GetPosition(line, column) == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }   
}
