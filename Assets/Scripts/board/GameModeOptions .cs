using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeOption
{
    private string _label;
    private int _value;

    public GameModeOption(string label, int value)
    {
        _label = label;
        _value = value;

    }

    public string Label
    {
        get
        {
            return _label;
        }
        set
        {
            _label = value;
        }
    }

    public int Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
        }
    }
}


public class GameModeOptions {

    public static GameModeOption[] options = { new GameModeOption("Player X Computer", 1) , new GameModeOption("Player X Player", 2) };
    
}
