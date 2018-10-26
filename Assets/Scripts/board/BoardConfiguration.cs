using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardConfiguration : MonoBehaviour {

    private DifficultyOptions.Options _difficulty = DifficultyOptions.Options.Hard;
    private NetworkOptions.Options _network = NetworkOptions.Options.Lan;
    private GameModeOption _gameModeOption = null;
    private int _starter = 0;

    public void Start()
    {
        
    }

    public DifficultyOptions.Options Difficulty
    {
        get
        {
            return _difficulty;
        }
        set
        {
            _difficulty = value;
        }
    }

    public NetworkOptions.Options Network
    {
        get
        {
            return _network;
        }
        set
        {
            _network = value;
        }
    }

    public GameModeOption GameModeOption
    {
        get
        {
            return _gameModeOption;
        }
        set
        {
            _gameModeOption = value;
        }
    }

    public int Starter
    {
        get
        {
            return _starter;
        }
        set
        {
            _starter = value;
        }
    }


}
