using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AIPlayer:Player

{

    public AIPlayer(int id, PlayerType type, Sprite symbol) :base(id, type, symbol)
    {
        
    }

    public int MakePlay(Game game, DifficultyOptions.Options difficulty)
    {
        Dictionary<string, int> result = GetBestMove(game, -99, 99, 8, difficulty);
        return result["bestMove"];
    }

    private Dictionary<string, int> SetPositionScore(int line, int column, int value)
    {
        Dictionary<string, int> positionScore = new Dictionary<string, int>();
        positionScore["column"] = column;
        positionScore["line"] = line;
        positionScore["value"] = value;
        return positionScore;
    }

    private Dictionary<string, int> GetBestMove(Game game, int alpha, int beta, int depth, DifficultyOptions.Options difficulty)
    {
        Dictionary<string, int> result = new Dictionary<string, int>();

        if (game.IsOver  || depth == 0)
        {
            result["score"] = GetScore(game.Winner, game.Player1.Id, difficulty);
            return result;
        }

        List<int> pMoves = new List<int>();
        int bestScore=0;
        int score;
        int bestMove=0;
        pMoves = game.GetPossibleMoves();
        if (pMoves.Count > 0)
        {
            for (int i = 0; i < pMoves.Count; i++)
            {
                Game cloneGame = game.Clone();
                cloneGame.CurrentPlayer = game.CurrentPlayer;
                cloneGame.MakeMove(pMoves[i]);
                score=GetBestMove(cloneGame, alpha,beta, depth -1, difficulty)["score"];
                if (i == 0)
                {
                    bestScore = score;
                    bestMove = pMoves[i];
                }
                if (game.CurrentPlayer.Type == PlayerType.AIPlayer)
                {
                    //MAX
                    if (bestScore < score)
                    {
                        bestScore = score;
                        bestMove = pMoves[i];
                    }
                    if (score >= beta) break;
                    alpha = alpha > score ? alpha  : score;
                } else
                {
                    //MIN
                    if (bestScore > score)
                    {
                        bestScore = score;
                        bestMove = pMoves[i];
                    }
                    if (score <= alpha) break;
                    beta = beta < score ? beta : score;
                }
            }

            result["bestMove"] = bestMove;
            result["score"] = bestScore;
        } else
        {
            result["score"] = 0;
            
        }

        return result;
    }

    private int getRamdomNumber(DifficultyOptions.Options difficulty, int value)
    {
        int randomNumber;
        if (difficulty == DifficultyOptions.Options.Easy)
        {
            randomNumber = Random.Range(0, 10);
            return randomNumber;
        } else
        {
            if (Random.value <= 0.3)
            {
                return value * -1;
            } else
            {
                return value;
            }

        }
        
    }

    

    private int GetScore(int winner, int aiPlayerIdentifier, DifficultyOptions.Options difficulty)
    {
        int score;
        if (winner == 1)
        {
            score = 10;
            if (difficulty == DifficultyOptions.Options.Easy)
                score = getRamdomNumber(difficulty, score);
            
        } else if (winner == 2)
        {
            score = -10;
            if (difficulty != DifficultyOptions.Options.Hard)
                score = getRamdomNumber(difficulty, score);
        } else
        {
            score = 0;
        }
        return score;
    }

}