using UnityEngine;
using UnityEngine.Networking;


public class BoardManager: MonoBehaviour {


    private BoardConfiguration _configuration = null;
    private SpriteRenderer[] _positionsRenderer = null;
    private SpriteRenderer _currentPlayerSymbol = null;
    private SpriteRenderer _infoPlayerSymbol = null;
    private SpriteRenderer _restartInfo = null;
    private float _totalTime = 0;
    private string _optionSceneName = "OptionsScene";
    private string _boardSceneName = "BoardScene";

    public Sprite Circle = null;
    public Sprite Cross = null;
    public Sprite CrossWithCircle = null;
    public Sprite CircleWithCircle = null;
    public Sprite InfoP1P2 = null;
    public Sprite InfoP1Computer = null;
    private bool _finishingGame = false;
    private Game _game = null;
    private float _timeToPlay = 0;
    

    private void Start () {
        
        
        
        _configuration = BoardConfigurationGetter.getConfigurationObject();
        _configuration.DisabledGeneralAudio();
        Player player1 = null;
        Player player2 = null;
        _restartInfo = GameObject.Find("restartInfo").GetComponent<SpriteRenderer>();
        _restartInfo.enabled = false;
        _infoPlayerSymbol = GameObject.Find("infoPlayerSymbol").GetComponent<SpriteRenderer>();
        if (_configuration.GameModeOption.Value == 1)
        {
            player1 = new AIPlayer(1, PlayerType.AIPlayer, Cross);
            _infoPlayerSymbol.sprite = InfoP1Computer;

        } else
        {
            player1 = new Player(1, PlayerType.HumanPlayer, Cross);
            _infoPlayerSymbol.sprite = InfoP1P2;

        }
        player2 = new Player(2, PlayerType.HumanPlayer, Circle);
        _game = new Game(player1, player2);
        _currentPlayerSymbol = GameObject.Find("currentInfo").GetComponent<SpriteRenderer>();
        if (_configuration.Starter == 1)
        {
            SetCurrentPlayer(player1);
        } else
        {
            SetCurrentPlayer(player2);
        }
        _finishingGame = false;
        InitializeBoardPositions();
    }

    private void SetCurrentPlayer(Player player)
    {
        _game.CurrentPlayer = player;
        _currentPlayerSymbol.sprite = player.Symbol;
    }

    private void InitializeBoardPositions()
    {
        _positionsRenderer = GameObject.Find("positions").GetComponentsInChildren<SpriteRenderer>();

        for (int position=0; position<_positionsRenderer.Length; position++)
        {
            _positionsRenderer[position].sprite = null;
        }
    }

    private void FindingWinner()
    {
        Sprite winnerSprite = null;
        if (_game.Winner == 1)
        {
            winnerSprite = CrossWithCircle;
        } else
        {
            winnerSprite = CircleWithCircle;
        }
        if (_game.WinnerPositions != null && _game.WinnerPositions.Length > 0)
        {
            for (int position = 0; position < _game.WinnerPositions.Length; position++)
            {
                _positionsRenderer[_game.WinnerPositions[position]-1].sprite = winnerSprite;
            }
        }
        _restartInfo.enabled = true;
    }

    private void EndGame()
    {
        _totalTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            StartCoroutine(SceneLoader.LoadScene(_optionSceneName));
            StartCoroutine(SceneLoader.UnloadScene(_boardSceneName));
        }
    }

        
    public void ClickBehavior(int positionId)
    {
        if (!_game.IsOver)
        {
            Board board = _game.Board;
            int line = ((positionId - 1) / 3);
            int column = ((positionId - 1) % 3);
            if (board.GetPosition(line, column) == 0)
            {
                if (_game.CurrentPlayer.Type == PlayerType.HumanPlayer)
                {
                    SetPlayerSpriteOnPosition(positionId, _game.CurrentPlayer.Symbol);
                    board.SetPosition(line, column, _game.CurrentPlayer.Id);
                    Player current = (_game.CurrentPlayer == _game.Player1) ? _game.Player2 : _game.Player1;
                    SetCurrentPlayer(current);

                }
            }
        }
        
    }

    private void SetPlayerSpriteOnPosition(int positionId, Sprite sprite)
    {
        if (_positionsRenderer[positionId-1].sprite == null)
        {
            _positionsRenderer[positionId-1].sprite = sprite;
        }
    }
    
    private void Update () {
        if (_game != null && (_game.IsOver || _game.GetPossibleMoves().Count == 0))
        {
            if (_finishingGame == false)
            {
                StartCoroutine(Timer.WaitATime(5));
                
                FindingWinner();
                _finishingGame = true;
            }
            EndGame();
            
        } else
        {
            if (_game != null && _game.CurrentPlayer.Type == PlayerType.AIPlayer)
            {
                _timeToPlay += Time.deltaTime;
                if (_timeToPlay > 0.5)
                {
                    _timeToPlay = 0;
                    AIPlayer aiPlayer = (AIPlayer)_game.CurrentPlayer;
                    int bestChoice = aiPlayer.MakePlay(_game, _configuration.Difficulty);
                    SetPlayerSpriteOnPosition(bestChoice, _game.CurrentPlayer.Symbol);
                    bestChoice = bestChoice - 1;
                    _game.Board.SetPosition((bestChoice / 3), (bestChoice % 3), 1);
                    Player current = (_game.CurrentPlayer == _game.Player1) ? _game.Player2 : _game.Player1;
                    SetCurrentPlayer(current);
                }
                
            }
        }
        
        

    }


    
}
