using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private int _id;
    private PlayerType _type;
    private Sprite _symbol;

    public Player (int id, PlayerType type, Sprite symbol)
    {
        _id = id;
        _type = type;
        _symbol = symbol;
    }

    public int Id
    {
        get
        {
            return _id;

        }
    }

    public PlayerType Type
    {
        get
        {
            return _type;

        }
    }

    public Sprite Symbol
    {
        get
        {
            return _symbol;

        }
    }



}

public enum PlayerType { AIPlayer, HumanPlayer };


public class Game 
{

    private Board _board = null;
    private Player _player1 = null;
    private Player _player2 = null;
    private int _winner = 0;
    private int[] _winnerPositions = null;
    private bool _isOver = false;
    private Player _currentPlayer = null;

    public Game(Player player1, Player player2)
    {
        _board = new Board();
        _player1 = player1;
        _player2 = player2;

    }

    public Game()
    {

    }


    public Player Player1
    {
        get
        {
            return _player1;

        }
    }

    public Player Player2
    {
        get
        {
            return _player2;

        }
    }

    public bool IsOver
    {
        get
        {
            if (!_isOver)
            {
                if ((CheckGameEndingLines() != 0) || (CheckGameEndingDiagonal() != 0) || (CheckGameEndingColumns() != 0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            } else
            {
                return _isOver;
            }
            
        }
    }

    public int Winner
    {
        get
        {
            return _winner;

        }
    }

    public Board Board
    {
        get
        {
            return _board;

        }
    }

    public Player CurrentPlayer
    {
        get
        {
            return _currentPlayer;

        }
        set
        {
            _currentPlayer=value;

        }
    }

    public int[] WinnerPositions
    {
        get
        {
            return _winnerPositions;

        }
    }

    public List<int> GetPossibleMoves()
    {
        List<int> possibleMoves = new List<int>();
        
        for (int line = 0; line < 3; line++)
        {
            for (int column = 0; column < 3; column++)
            {
                if (_board.GetPosition(line, column) == 0)
                {
                    possibleMoves.Add(ConvertLineAndColumnToPosition(line, column));
                }
            }
        }
        return possibleMoves;
    }

    private int ConvertLineAndColumnToPosition(int line,int column)
    {
        return ((line * 3) + column + 1);
    }

    private int CheckGameEndingColumns()
    {
        for (int column = 0; column < 3; column++)
        {
            if ((_board.GetPosition(0, column) != 0) && (_board.GetPosition(0, column) == _board.GetPosition(1, column)) && (_board.GetPosition(1, column) == _board.GetPosition(2, column)))
            {
                _winnerPositions = new int[3] { ConvertLineAndColumnToPosition(0, column), ConvertLineAndColumnToPosition(1, column), ConvertLineAndColumnToPosition(2, column) };
                _winner = _board.GetPosition(0, column);
            }
        }
        return _winner;
    }

    private int CheckGameEndingDiagonal()
    {
        if ((_board.GetPosition(0, 0) != 0) && (_board.GetPosition(0, 0) == _board.GetPosition(1, 1)) && (_board.GetPosition(1, 1) == _board.GetPosition(2, 2)))
        {
            _winnerPositions = new int[3] { ConvertLineAndColumnToPosition(0, 0), ConvertLineAndColumnToPosition(1, 1), ConvertLineAndColumnToPosition(2, 2) };
            _winner = _board.GetPosition(0, 0);
        }
        else if ((_board.GetPosition(0, 2) != 0) && (_board.GetPosition(0, 2) == _board.GetPosition(1, 1)) && (_board.GetPosition(1, 1) == _board.GetPosition(2, 0)))
        {
            _winnerPositions = new int[3] { ConvertLineAndColumnToPosition(0,2), ConvertLineAndColumnToPosition(1,1), ConvertLineAndColumnToPosition(2,0) };
            _winner = _board.GetPosition(0, 2);
        }
        return _winner;
    }

    private int CheckGameEndingLines()
    {
        for (int line = 0; line < 3; line++)
        {

            if ((_board.GetPosition(line, 0) != 0) && (_board.GetPosition(line, 0) == _board.GetPosition(line, 1)) && (_board.GetPosition(line, 1) == _board.GetPosition(line, 2)))
            {
                _winnerPositions = new int[3] { ConvertLineAndColumnToPosition(line, 0), ConvertLineAndColumnToPosition(line, 1), ConvertLineAndColumnToPosition(line, 2) };
                _winner = _board.GetPosition(line, 0);
            }
        }
        return _winner;
    }

    public Game Clone()
    {
        Game newGame = new Game(this.Player1, this.Player2);
        newGame.Board.SetPositions(this.Board.GetPositions());
        return newGame;
    }

    public void MakeMove(int move)
    {
        move = move - 1;
        int line = (move / 3);
        int column = (move % 3);
        Board.SetPosition(line, column, _currentPlayer.Id);
        _currentPlayer = (_currentPlayer == _player1) ? _player2 : _player1;
    }

}
