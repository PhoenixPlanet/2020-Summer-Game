using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameLogic : MonoBehaviour
{
    public static GameLogic instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameLogic>();
            }

            return m_instance;
        }
    }

    private static GameLogic m_instance;

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
            gameBoard.board.RefreshAllTiles();
        }
    }

    public void startLevel(int level)
    {
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

    public void refreshLevel(int level)
    {
        audioSource.clip = marbleMoveClip;
        audioSource.Play();
        gameBoard.initBoard(level);
    }

    void Awake()
    {
        gameBoard = GetComponent<GameBoard>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
