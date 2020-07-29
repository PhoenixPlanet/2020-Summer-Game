using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    GameBoard1 gameBoard;

    private MarblePressedState marblePressedState = MarblePressedState.NotSelected;

    public Color moveTargetColor;

    public void OnBoardPressed(Vector3Int pos)
    {
        if (marblePressedState == MarblePressedState.NotSelected)
        {
            if (gameBoard.isMarble(pos))
            {
                List<Vector3Int> movablePosList = gameBoard.getMovablePosList(pos);

                foreach (Vector3Int p in movablePosList)
                {
                    gameBoard.setTileColor(p, moveTargetColor);
                }
            }
        }
    }

    public void startLevel(int level)
    {
        gameBoard.initBoard(level);
        gameBoard.OnBoardPressed += OnBoardPressed;
    }

    void Awake()
    {
        gameBoard = GetComponent<GameBoard1>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startLevel(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

enum MarblePressedState
{
    NotSelected,
    MarbleSelected,
    NewLocateSelected
}
