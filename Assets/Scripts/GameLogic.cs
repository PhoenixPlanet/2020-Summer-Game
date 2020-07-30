using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameLogic : MonoBehaviour
{
    GameBoard gameBoard;

    public Color moveTargetColor;
    public Color defaultColor;

    public AudioSource audioSource;

    public AudioClip marbleSelectClip;
    public AudioClip wrongSelectClip;
    public AudioClip marbleMoveClip;

    public event Action OnMarbleClear;

    private List<Vector3Int> movablePosList;
    private Vector3Int selectedMarblePos;
    private MarblePressedState marblePressedState = MarblePressedState.NotSelected;

    private int currentLevel;

    public void OnBoardPressed(Vector3Int pos)
    {
        if (marblePressedState == MarblePressedState.NotSelected)
        {
            if (gameBoard.isMarble(pos))
            {
                selectedMarblePos = pos;
                movablePosList = gameBoard.getMovablePosList(pos);

                if (movablePosList.Count == 0)
                {
                    audioSource.clip = wrongSelectClip;
                    audioSource.Play();
                    return;
                }

                foreach (Vector3Int p in movablePosList)
                {
                    gameBoard.setTileColor(p, moveTargetColor);
                }
                marblePressedState = MarblePressedState.MarbleSelected;

                audioSource.clip = marbleSelectClip;
                audioSource.Play();
            }
            else
            {
                audioSource.clip = wrongSelectClip;
                audioSource.Play();
            }
        } else if (marblePressedState == MarblePressedState.MarbleSelected)
        {
            if (movablePosList.Exists(x => x == pos))
            {
                gameBoard.moveMarble(selectedMarblePos, pos);

                audioSource.clip = marbleMoveClip;
                audioSource.Play();
            }
            else
            {
                audioSource.clip = wrongSelectClip;
                audioSource.Play();
            }

            marblePressedState = MarblePressedState.NotSelected;
            foreach (Vector3Int p in movablePosList)
            {
                gameBoard.setTileColor(p, defaultColor);
            }
        }
    }

    public void startLevel(int level)
    {
        currentLevel = level;
        gameBoard.initBoard(level);
        gameBoard.OnBoardPressed += OnBoardPressed;
        gameBoard.OnMarbleClear += endLevel;
    }

    public void endLevel()
    {
        gameBoard.OnBoardPressed -= OnBoardPressed;
        gameBoard.OnMarbleClear -= endLevel;

        OnMarbleClear();
    }

    public void refreshLevel()
    {
        gameBoard.initBoard(currentLevel);
    }

    void Awake()
    {
        gameBoard = GetComponent<GameBoard>();
        audioSource = GetComponent<AudioSource>();
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
