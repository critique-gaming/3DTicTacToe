using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public char [,,] gameBoard = new char [4,4,4];
    private char currentPlayer = 'X';
    private System.Random random = new System.Random();
    
    void Start() // Start is called before the first frame update
    {
        InitializeGameBoard();
    }

    
    void Update() // Update is called once per frame
    {
        
    }

    void InitializeGameBoard()
    {
        for (int i = 0; i<4; i++)
            for (int j=0; j<4; j++)
                for (int k=0; k<4; k++)
                    gameBoard[i,j,k] ='-';
    }

    public bool CheckWin(char player) 
    {
        
        for (int i = 0; i < 4; i++) // Check rows and columns in each layer, and verticals across layers
        {
            if (CheckLine(player, i, 0, 0, 0, 1, 0) || // Rows in each layer
                CheckLine(player, 0, i, 0, 1, 0, 0) || // Columns in each layer
                CheckLine(player, 0, 0, i, 0, 0, 1))   // Columns across layers
                return true;
        }

            for (int z = 0; z < 4; z++) // Check diagonals in each layer
        {
            if (CheckLine(player, 0, 0, z, 1, 1, 0) || // Diagonal from top-left to bottom-right in each layer
                CheckLine(player, 3, 0, z, -1, 1, 0))  // Diagonal from top-right to bottom-left in each layer
                return true;
        }

        // Check 3D diagonals
        if (CheckLine(player, 0, 0, 0, 1, 1, 1) || // Diagonal from top-left-front to bottom-right-back
            CheckLine(player, 3, 0, 0, -1, 1, 1) || // Diagonal from top-right-front to bottom-left-back
            CheckLine(player, 0, 3, 0, 1, -1, 1) || // Diagonal from bottom-left-front to top-right-back
            CheckLine(player, 3, 3, 0, -1, -1, 1))  // Diagonal from bottom-right-front to top-left-back
            return true;

        // Add checks for vertical lines across layers and any other missing orientations
        for (int x = 0; x < 4; x++)
            for (int y = 0; y < 4; y++)
                if (CheckLine(player, x, y, 0, 0, 0, 1)) // Vertical lines across layers
                    return true;

        return false;
    }

    private bool CheckLine(char player, int startX, int startY, int startZ, int dx, int dy, int dz, bool allowEmpty = false)
    {
        int playerCount = 0;
        int emptyCount = 0;
        for (int i = 0; i < 4; i++)
        {
            char symbol = gameBoard[startX + i * dx, startY + i * dy, startZ + i * dz];
            if (symbol == player) playerCount++;
            else if (symbol == '-') emptyCount++;
            else return false; // Line contains opponent's symbol or is out of bounds
        }
        return allowEmpty ? playerCount > 0 : playerCount == 4;
    }


    public void AIPlay()
    {
        // First, try to block the opponent's immediate win.
        if (TryToBlockOpponent())
            return;

        // If no block is needed, attempt to make a strategic move.
        if (MakeStrategicMove())
            return;

        // Fallback to a random move if no strategic move is found.
        MakeRandomMove();
    }

    private bool TryToBlockOpponent()
    {
        char opponent = currentPlayer == 'X' ? 'Y' : 'X';
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                for (int z = 0; z < 4; z++)
                {
                    if (gameBoard[x, y, z] == '-')
                    {
                        gameBoard[x, y, z] = opponent;
                        if (CheckWin(opponent))
                        {
                            gameBoard[x, y, z] = currentPlayer; // Block the opponent
                            return true;
                        }
                        gameBoard[x, y, z] = '-'; // Reset if not a winning move
                    }
                }
            }
        }
        return false;
    }
    private bool MakeStrategicMove()
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                for (int z = 0; z < 4; z++)
                {
                    if (gameBoard[x, y, z] == '-')
                    {
                        // Temporarily mark the position with the current player's symbol
                        gameBoard[x, y, z] = currentPlayer;
                        // Check all directions from this position to find a strategic move
                        if (IsPartOfWinningLine(x, y, z, currentPlayer))
                        {
                            // No need to revert the mark as it's a valid strategic move
                            return true; // Indicate a strategic move has been made
                        }
                        else
                        {
                            // Revert the temporary mark if it's not a strategic move
                            gameBoard[x, y, z] = '-';
                        }
                    }
                }
            }
        }
        return false; // No strategic move found
    }

     private void MakeRandomMove()
    {
        while (true)
        {
            int x = random.Next(4);
            int y = random.Next(4);
            int z = random.Next(4);
            if (gameBoard[x, y, z] == '-')
            {
                gameBoard[x, y, z] = currentPlayer;
                break;
            }
        }
    }
    private bool IsPartOfWinningLine(int x, int y, int z, char player)
    {
        char opponent = player == 'X' ? 'Y' : 'X';
        // Check horizontal, vertical, and both types of diagonal lines in each layer
        // and the vertical lines across layers for potential winning moves.

        for (int i = 0; i < 4; i++) // Horizontal and vertical lines in layer and across layers
        {
            if (CheckLine(player, x, 0, z, 0, 1, 0, true) || // Horizontal in layer
                CheckLine(player, 0, y, z, 1, 0, 0, true) || // Vertical in layer
                CheckLine(player, x, y, 0, 0, 0, 1, true))   // Vertical across layers
                return true;
        }

        // Diagonals in layer
        if (CheckLine(player, 0, 0, z, 1, 1, 0, true) || // Diagonal from top-left to bottom-right in layer
            CheckLine(player, 0, 3, z, 1, -1, 0, true))  // Diagonal from top-right to bottom-left in layer
            return true;

        // 3D diagonals
        if (CheckLine(player, 0, 0, 0, 1, 1, 1, true) || // Diagonal from top-left-front to bottom-right-back
            CheckLine(player, 3, 0, 0, -1, 1, 1, true) || // Diagonal from top-right-front to bottom-left-back
            CheckLine(player, 0, 3, 0, 1, -1, 1, true) || // Diagonal from bottom-left-front to top-right-back
            CheckLine(player, 0, 0, 3, 1, 1, -1, true) || // Diagonal from bottom-left-back to top-right-front
            CheckLine(player, 3, 3, 0, -1, -1, 1, true) || // Diagonal from bottom-right-front to top-left-back
            CheckLine(player, 3, 0, 3, -1, 1, -1, true) || // Diagonal from bottom-right-back to top-left-front
            CheckLine(player, 0, 3, 3, 1, -1, -1, true) || // Diagonal from bottom-right-back to top-left-front
            CheckLine(player, 3, 3, 3, -1, -1, -1, true))  // Diagonal from bottom-right-back to top-left-front
            return true;

        return false;
    }

    // Call this method to switch the current player
    void SwitchPlayer()
    {
        currentPlayer = currentPlayer == 'X' ? 'Y' : 'X';
    }

   

}


