using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class BoardNetworkManager: NetworkBehaviour
{


    private BoardNetworkConfiguration _configuration = null;
    private SpriteRenderer[] _positionsRenderer = null;

    private SpriteRenderer _currentPlayerSymbol = null;

    private SpriteRenderer _infoPlayerSymbol = null;
    private SpriteRenderer _restartInfo = null;
    private float _totalTime = 0;
    private string _optionSceneName = "OptionsScene";
    private string _boardSceneName = "BoardNetworkScene";

    public Sprite Circle = null;
    public Sprite Cross = null;
    public Sprite CrossWithCircle = null;
    public Sprite CircleWithCircle = null;

    public SpriteRenderer LoadingSprite = null;
    public SpriteRenderer LoadingText = null;

    private bool _finishingGame = false;
    private  Player _player1 = null;
    private Player _player2 = null;


    private float _timeToPlay = 0;

    
    public int currentPlayerId = 0;

    private Game _game = null;

   


    public void ClickBehavior(int positionId, bool sentByServer)
    {
        int numPlayers = 0;

        if (_configuration && _configuration.NetworkType == NetworkOptions.Options.Lan.ToString())
            numPlayers = NetworkManagerSpecific.singleton.numPlayers;
        else
            numPlayers = NetworkLobbyManagerSpecific.LobbyManager.numPlayers;

        if (!_game.IsOver && numPlayers > 1 && NetworkServer.connections.Count > 1 && NetworkServer.connections[0].isReady && NetworkServer.connections[1].isReady)
        {

            Debug.Log("Player is server:" + sentByServer);
            Debug.Log("Current User:" + _game.CurrentPlayer.Id);
            Board board = _game.Board;
            int line = ((positionId - 1) / 3);
            int column = ((positionId - 1) % 3);
            if (board.GetPosition(line, column) == 0)
            {
                if ((sentByServer && _game.CurrentPlayer.Id == 1) || (!sentByServer && _game.CurrentPlayer.Id == 2))
                {
                    Debug.Log("Positions length:" + _positionsRenderer.Length);
                    Debug.Log("Positions obj:" + _positionsRenderer[positionId - 1]);
                    Debug.Log("Positions game:" + _game);
                    board.SetPosition(line, column, _game.CurrentPlayer.Id);
                    SetPlayerSpriteOnPosition(positionId, _game.CurrentPlayer.Symbol);
                    RpcSetPositions(_game.CurrentPlayer.Id, positionId);
                    Player current = (_game.CurrentPlayer == _game.Player1) ? _game.Player2 : _game.Player1;
                    SetCurrentPlayer(current, _game);

                    if (current.Id == 1)
                    {
                        ChangeAllAuthority("LocalPlayer");
                    }
                    else
                    {
                        ChangeAllAuthority("OtherPlayer");
                    }
                }
            }
        }
    }

    private void SetPlayerSpriteOnPosition(int positionId, Sprite sprite)
    {
        if (_positionsRenderer[positionId - 1].sprite == null)
        {
            _positionsRenderer[positionId - 1].sprite = sprite;
        }
    }

    [ClientRpc]
    public void RpcSetPositions(int currentPlayerId, int positionId)
    {
        if (currentPlayerId == 1)
        {
            _positionsRenderer[positionId - 1].sprite = _player1.Symbol;
        }
         else
        {
            _positionsRenderer[positionId - 1].sprite = _player2.Symbol;
        }

    }

    [ClientRpc]
    public void RpcSetCurrentPlayerId(int currentPlayerIdValue)
    {
        this.currentPlayerId = currentPlayerIdValue;
        LoadNextPlayer();

    }

    [Command]
    public void CmdSetAuth(NetworkIdentity player, NetworkIdentity position)
    {
        var networkIdentity = position;
        var otherOwner = networkIdentity.clientAuthorityOwner;

        if (otherOwner == player.connectionToClient)
        {
            return;
        }
        else
        {
            if (otherOwner != null)
            {
                networkIdentity.RemoveClientAuthority(otherOwner);
            }
            networkIdentity.AssignClientAuthority(player.connectionToClient);
            Debug.Log(networkIdentity);
        }
    }

    private void ChangeAuthority(NetworkIdentity position, string tag)
    {
        StartCoroutine(CheckOtherPlayer(tag, position));
    }

    private void LoadNextPlayer()
    {
       
        Debug.Log("Waiting for other player data...");
        SpriteRenderer currentPlayerSymbol = GameObject.Find("currentInfo").GetComponent<SpriteRenderer>();
        if (this.currentPlayerId == this._player1.Id)
        {
            currentPlayerSymbol.sprite = this._player1.Symbol;
        }
        else
        {
            currentPlayerSymbol.sprite = this._player2.Symbol;
        }
    }
    
    private IEnumerator CheckOtherPlayer(String tag, NetworkIdentity position)
    {
        Debug.Log("Waiting for other player...");
        GameObject player;
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag(tag) != null && GameObject.FindGameObjectWithTag(tag).GetComponent<NetworkIdentity>().connectionToClient.isReady);
        player = GameObject.FindGameObjectWithTag(tag);
        GameObject otherPlayer = GameObject.FindGameObjectWithTag(tag);
        CmdSetAuth(otherPlayer.GetComponent<NetworkIdentity>(), position);
        Debug.Log("Tag:" + tag);
        Debug.Log("PLayer:" + otherPlayer);
        Debug.Log("Other player is ok now!");
    }

    private IEnumerator CheckUser()
    {
        Debug.Log("Waiting for other player...");
        if (_configuration && _configuration.NetworkType == NetworkOptions.Options.Lan.ToString())
        {
            yield return new WaitUntil(() => NetworkManagerSpecific.singleton.numPlayers > 1);
        }
        else
        {
            yield return new WaitUntil(() => NetworkLobbyManagerSpecific.singleton.numPlayers > 1);
        }
        Debug.Log("Other player is ok now!");
    }

    private void Start () {
        _configuration = NetworkConfigurationGetter.getConfigurationObject();
        InitializeBoardPositions();
        _player1 = new Player(1, PlayerType.HumanPlayer, Cross);
        _player2 = new Player(2, PlayerType.HumanPlayer, Circle);

        if (!isServer) {    
            return;
        }

        StartCoroutine(CheckUser());
        _configuration = NetworkConfigurationGetter.getConfigurationObject();
        
        _game = new Game(_player1, _player2);
       
        //_configuration.Starter = 2;
        if (_configuration.Starter == 1)
        {
            SetCurrentPlayer(_player1, _game);
            ChangeAllAuthority("LocalPlayer");
        }
        else
        {
            SetCurrentPlayer(_player2, _game);
            ChangeAllAuthority("OtherPlayer");
        }
        
        _finishingGame = false;
    }

    private void SetCurrentPlayer(Player player, Game game)
    {
        game.CurrentPlayer = player;
        _currentPlayerSymbol = GameObject.Find("currentInfo").GetComponent<SpriteRenderer>();
        _currentPlayerSymbol.sprite = player.Symbol;
        RpcSetCurrentPlayerId(game.CurrentPlayer.Id);
    }

    private void InitializeBoardPositions()
    {
        _positionsRenderer = GameObject.Find("positions").GetComponentsInChildren<SpriteRenderer>();

        for (int position = 0; position < _positionsRenderer.Length; position++)
        {
            _positionsRenderer[position].sprite = null;

        }
    }

    private void ChangeAllAuthority(String playerTag)
    {
        _positionsRenderer = GameObject.Find("positions").GetComponentsInChildren<SpriteRenderer>();

        for (int position = 0; position < _positionsRenderer.Length; position++)
        {
            Debug.Log("Object Position:" + _positionsRenderer[position]);
            Debug.Log("Object Position Identity:" + _positionsRenderer[position].GetComponent<NetworkIdentity>());
            ChangeAuthority(_positionsRenderer[position].GetComponent<NetworkIdentity>(), playerTag);
        }
    }

    private void Update()
    {
        if (ClientScene.ready && LoadingSprite.enabled)
        {
            Debug.Log("Loading finish ...");
            LoadingSprite.enabled = false;
            LoadingText.enabled = false;
            if (isClient)
            {
                _currentPlayerSymbol = GameObject.Find("currentInfo").GetComponent<SpriteRenderer>();
                if (_configuration.Starter == 1)
                {
                    _currentPlayerSymbol.sprite = Cross;
                } else
                {
                    _currentPlayerSymbol.sprite = Circle;
                }
            }
            
           
            return;


        }
        if (_game != null && (_game.IsOver || _game.GetPossibleMoves().Count == 0))
        {
            if (_finishingGame == false)
            {
                StartCoroutine(Timer.WaitATime(5));

                FindingWinner();
                _finishingGame = true;
            }
            EndGame();

        }
    }

    private void FindingWinner()
    {
        Sprite winnerSprite = null;
        if (_game.Winner == 1)
        {
            winnerSprite = CrossWithCircle;
        }
        else
        {
            winnerSprite = CircleWithCircle;
        }
        if (_game.WinnerPositions != null && _game.WinnerPositions.Length > 0)
        {
            for (int position = 0; position < _game.WinnerPositions.Length; position++)
            {
                int winnerPosition = _game.WinnerPositions[position] - 1;
                _positionsRenderer[winnerPosition].sprite = winnerSprite;
                RpcSetWinner(_game.Winner, winnerPosition);
            }
        }
        //_restartInfo.enabled = true;
    }

    private void EndGame()
    {
        //StartCoroutine(Timer.WaitATime(10));
        //NetworkLobbyManagerSpecific.Shutdown();
        Application.Quit();
        //StartCoroutine(SceneLoader.LoadScene(_optionSceneName));
        //StartCoroutine(SceneLoader.UnloadScene(_boardSceneName));

    }

    [ClientRpc]
    public void RpcSetWinner(int winnerId, int position)
    {
        if (winnerId == 1)
        {
            _positionsRenderer[position].sprite = CrossWithCircle;
        }
        else
        {
            _positionsRenderer[position].sprite = CircleWithCircle;
        }

    }




}
