using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkModeOption
{
    private string _label;
    private int _value;

    public NetworkModeOption(string label, int value)
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


public class NetworkModeOptions {

    public static NetworkModeOption[] options = { new NetworkModeOption("LAN", 1) , new NetworkModeOption("Match Maker", 2) };
    
}
