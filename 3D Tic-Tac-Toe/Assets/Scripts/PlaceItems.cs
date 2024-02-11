using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using System;

public class PlaceItems : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject olivePrefab;
    public GameObject bananaPrefab;
    public char [,,] gameBoard = new char [4,4,4];
    private char currentPlayer = 'X';
    private System.Random random = new System.Random();
    private float secondsToNext = 2;

    float spacing = 10.0f;
    // Start is called before the first frame update
    private void Start()
    {
        InitializeGameBoard();
        
        //Creates a 4*4*4 grid out of cubes
        for (int x = 0; x < 4; x++) 
        {
            for (int y = 0; y < 4; y++) 
            {
                 for (int z = 0; z < 4; z++) 
                {
                    Vector3 positionCube = new Vector3(x, y, z) * spacing;
                    Instantiate(cubePrefab, positionCube, cubePrefab.transform.rotation);
                }
            }
        }

    }

    // Update is called once per frame
  private void Update()
{
   if (currentPlayer =='Y'||currentPlayer=='X') //this is for 2 AIs, will change later
   {
        secondsToNext-=Time.deltaTime;
        if (secondsToNext<=0)
        {
            AIPlay();
            if (CheckWin()||CheckIfDone())
            {
                //DisplayWinMessage
                UnityEditor.EditorApplication.ExitPlaymode();
            }
            SwitchPlayer();
            secondsToNext = 2;
        }
   }
    
}

    void InitializeGameBoard()
    {
        for (int i = 0; i<4; i++)
            for (int j=0; j<4; j++)
                for (int k=0; k<4; k++)
                    gameBoard[i,j,k] ='-';
    }

    public bool CheckWin()
    {
        for (int x = 0; x < 4; x++) 
            {
                for (int y = 0; y < 4; y++) 
                {
                    for (int z = 0; z < 4; z++) 
                    {
                        if (CheckCube(x, y, z)=="win")
                            return true;
                    }
                }
            }
        return false;
    }

        private string CheckCube (int x, int y, int z)
    {
        int XplayerCount=0;
        int YplayerCount=0;
        bool alive = false;
        for (int i=0; i<4; i++) //check line on X axis
        {
            if (gameBoard[i, y, z] =='X')
                XplayerCount++;
            else if (gameBoard[i,y,z]=='Y')
                YplayerCount++;

            if (XplayerCount==4)
                return "Xwin";
            if (YplayerCount==4)
                return "Ywin";
            if (XplayerCount==3 && YplayerCount==0)
                return "Xcritical";
            if (YplayerCount ==3 && YplayerCount ==0)
                return "Ycritical";
            if (i==3 && ((XplayerCount==0 && currentPlayer=='Y') || (YplayerCount==0 && currentPlayer=='X')))
                alive = true;
        }

        XplayerCount=0;
        YplayerCount=0;
        for (int i=0; i<4; i++) //check line on Y axis
        {
            if (gameBoard[x, i, z] =='X')
                XplayerCount++;
            else if (gameBoard[x,i,z]=='Y')
                YplayerCount++;

            if (XplayerCount==4)
                return "Xwin";
            if (YplayerCount==4)
                return "Ywin";
            if (XplayerCount==3 && YplayerCount==0)
                return "Xcritical";
            if (YplayerCount ==3 && YplayerCount ==0)
                return "Ycritical";
            if (i==3 && ((XplayerCount==0 && currentPlayer=='Y') || (YplayerCount==0 && currentPlayer=='X')))
                alive = true;
        }

        XplayerCount=0;
        YplayerCount=0;
        for (int i=0; i<4; i++) //check line on z axis
        {
            if (gameBoard[x, y, i] =='X')
                XplayerCount++;
            else if (gameBoard[x,y,i]=='Y')
                YplayerCount++;

            if (XplayerCount==4)
                return "Xwin";
            if (YplayerCount==4)
                return "Ywin";
            if (XplayerCount==3 && YplayerCount==0)
                return "Xcritical";
            if (YplayerCount ==3 && YplayerCount ==0)
                return "Ycritical";
            if (i==3 && ((XplayerCount==0 && currentPlayer=='Y') || (YplayerCount==0 && currentPlayer=='X')))
                alive = true;
        }

        XplayerCount=0;
        YplayerCount=0;
        if ((x==0 || x==3)&&(y==0||y==3)) //if on a xy diagonal, check it
        {
            for (int i=0; i<4; i++)
            {
                if (gameBoard[Math.Abs(x-i), Math.Abs(y-i), z]=='X')
                    XplayerCount++;
                if (gameBoard[Math.Abs(x-i), Math.Abs(y-i), z]=='Y')
                    YplayerCount++;
            }
            if (XplayerCount==4)
                return "Xwin";
            if (YplayerCount==4)
                return "Ywin";
            if (XplayerCount==3 && YplayerCount==0)
                return "Xcritical";
            if (YplayerCount ==3 && YplayerCount ==0)
                return "Ycritical";
            if ((XplayerCount==0 && currentPlayer=='Y') || (YplayerCount==0 && currentPlayer=='X'))
                alive = true;
        }

        XplayerCount=0;
        YplayerCount=0;
        if ((x==0 || x==3)&&(z==0||z==3)) //if on a xz diagonal, check it
        {
            for (int i=0; i<4; i++)
            {
                if (gameBoard[Math.Abs(x-i), y, Math.Abs(z-i)]=='X')
                    XplayerCount++;
                if (gameBoard[Math.Abs(x-i), y, Math.Abs(z-i)]=='Y')
                    YplayerCount++;
            }
            if (XplayerCount==4)
                return "Xwin";
            if (YplayerCount==4)
                return "Ywin";
            if (XplayerCount==3 && YplayerCount==0)
                return "Xcritical";
            if (YplayerCount ==3 && YplayerCount ==0)
                return "Ycritical";
            if ((XplayerCount==0 && currentPlayer=='Y') || (YplayerCount==0 && currentPlayer=='X'))
                alive = true;
        }

        XplayerCount=0;
        YplayerCount=0;
        if ((y==0 || y==3)&&(z==0||z==3)) //if on a yz diagonal, check it
        {
            for (int i=0; i<4; i++)
            {
                if (gameBoard[x, Math.Abs(y-i), Math.Abs(z-i)]=='X')
                    XplayerCount++;
                if (gameBoard[x, Math.Abs(y-i), Math.Abs(z-i)]=='Y')
                    YplayerCount++;
            }
            if (XplayerCount==4)
                return "Xwin";
            if (YplayerCount==4)
                return "Ywin";
            if (XplayerCount==3 && YplayerCount==0)
                return "Xcritical";
            if (YplayerCount ==3 && YplayerCount ==0)
                return "Ycritical";
            if ((XplayerCount==0 && currentPlayer=='Y') || (YplayerCount==0 && currentPlayer=='X'))
                alive = true;
        }

        XplayerCount=0;
        YplayerCount=0;
        if ((x==0 || x==3)&&(z==0||z==3)&&(y==0||y==3)) //it on a 3D diagonal
        {
            for (int i=0; i<4; i++)
            {
                if (gameBoard[Math.Abs(x-i), Math.Abs(y-i), Math.Abs(z-i)]=='X')
                    XplayerCount++;
                if (gameBoard[Math.Abs(x-i), Math.Abs(y-i), Math.Abs(z-i)]=='Y')
                    YplayerCount++;
            }
            if (XplayerCount==4)
                return "Xwin";
            if (YplayerCount==4)
                return "Ywin";
            if (XplayerCount==3 && YplayerCount==0)
                return "Xcritical";
            if (YplayerCount ==3 && YplayerCount ==0)
                return "Ycritical";
            if ((XplayerCount==0 && currentPlayer=='Y') || (YplayerCount==0 && currentPlayer=='X'))
                alive = true;
        }
        
        if (alive == true)
            return "alive";
            else return "dead";
    }



   private void AIPlay()
    {
        int moveCoordinates=CriticalMove();
        if (moveCoordinates!=0)
        {
            
            gameBoard[moveCoordinates/100, moveCoordinates/10%10, moveCoordinates%10] = currentPlayer;
            PlaceSymbol(moveCoordinates/100,moveCoordinates/10%10,moveCoordinates%10);
        }
        else 
        {
            moveCoordinates = AliveMove();
            if (moveCoordinates!=0)
            {
                gameBoard[moveCoordinates/100, moveCoordinates/10%10, moveCoordinates%10] = currentPlayer;
                PlaceSymbol(moveCoordinates/100,moveCoordinates/10%10,moveCoordinates%10);
            }
            else MakeRandomMove();
        }
        
    }

    private int CriticalMove()
{
    for (int x = 0; x < 4; x++) 
        {
            for (int y = 0; y < 4; y++) 
            {
                 for (int z = 0; z < 4; z++) 
                {
                    if (gameBoard[x,y,z]=='-')
                        if (CheckCube(x, y, z)=="Xcritical"||CheckCube(x,y,z)=="YCritical")
                            {
                                return x*100+y*10+z;
                            }
                }
            }
        }
    return 0;
}
    private int AliveMove()
{
    int count = 0;
    int x = 0;
    int y=0;
    int z = 0;
    do
    {
        count++;
        x = random.Next(4);
        y = random.Next(4);
        z = random.Next(4);
        if ((currentPlayer == 'X' && CheckCube(x,y,z)=="Xalive")||(currentPlayer=='Y' && CheckCube(x,y,z)=="Yalive"))
            return x+100+y*10+z;
    }   while (count<=120);
            return 0;   
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
                PlaceSymbol(x, y, z);
                break;
            }
        }
    }
    // Call this method to switch the current player
    void SwitchPlayer()
    {
        currentPlayer = currentPlayer == 'X' ? 'Y' : 'X';
    }

    IEnumerator Waiter()
    {
        yield return new WaitForSecondsRealtime(4);
    }

    void PlaceSymbol(int x, int y , int z)
    {
        if(gameBoard[x,y,z] == 'X')
        {
            Vector3 positionOlive= new Vector3(x*spacing, y*spacing-8, z*spacing+5);
            Instantiate(olivePrefab, positionOlive, cubePrefab.transform.rotation);
        }
        else if(gameBoard[x,y,z]== 'Y')
        {
            Vector3 positionBanana= new Vector3(x*spacing, y*spacing-8, z*spacing+5);
            Instantiate(bananaPrefab, positionBanana, cubePrefab.transform.rotation);
        }
    }

    bool CheckIfDone() 
    {
        foreach (char symbol in gameBoard)
        {
            if (symbol=='-')
                return false;
        }
        return true;
    }
}
