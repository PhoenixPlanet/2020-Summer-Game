using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GameBoard1 : MonoBehaviour
{
    public static int a;

    LevelData levelData;

    public Vector3Int startPoint;

    public Tilemap board;
    public Tilemap marble;

    public Grid grid;

    public List<BoardObjectPrefab> boardObjectPrefabs;
    public List<MarblePrefab> marblePrefabs;

    // 추가 구현 부분
    public event Action OnMarbleClear;
    public delegate void OnPressed(Vector3Int pos);
    public event OnPressed OnBoardPressed;

    public void loadBoardData(string path, string file_name)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", path, file_name), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string json_board_data = Encoding.UTF8.GetString(data);
        levelData = JsonUtility.FromJson<LevelData>(json_board_data);
    }

    public void saveBoardData(string path, string file_name)
    {
        string json_board_data = JsonUtility.ToJson(levelData);

        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", path, file_name), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(json_board_data);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close(); 
    }

    public void drawBoard()
    {
        for (int i = 0; i < levelData.board_size.y; i++)
        {
            for (int j = 0; j < levelData.board_size.x; j++)
            {
                board.SetTile(startPoint + new Vector3Int(j, -i, 0), boardObjectPrefabs.Find(x => x.boardObjectType == levelData.getBoardObject(j, i)).boardObjectPrefab);
                marble.SetTile(startPoint + new Vector3Int(j, -i, 0), marblePrefabs.Find(x => x.marbleType == levelData.getMarble(j, i)).marblePrefab);
            }
        }
    }

    public void initBoard(int level)
    {
        loadBoardData(Application.dataPath + "/levelData", "Board_" + level);
        drawBoard();
    }

    // 추가 구현 함수
    public int getMarbleNum()
    {
        return levelData.marbleData.marble_num;
    }

    public Vector3Int posToWorld(Vector3Int pos)
    {
        return startPoint + new Vector3Int(pos.x, -pos.y, 0);
    }

    public Vector3Int worldToPos(Vector3Int pos)
    {
        Vector3Int tilePos = pos - startPoint;
        return new Vector3Int(tilePos.x, -tilePos.y, 0);
    }

    public bool isMarble(Vector3Int pos)
    {
        if (levelData.getMarble(pos.x, pos.y) == MarbleType.None)
        {
            return false;
        } else
        {
            return true;
        }
    }

    public void setTileColor(Vector3Int pos, Color color)
    {
        pos = posToWorld(pos);
        board.SetTileFlags(pos, TileFlags.None);
        board.SetColor(pos, color);
    }

    // 구현 시 알아두어야 할 점입니다.
    // 좌표는 왼쪽, 위 끝이 (0, 0)이고 오른쪽, 아래로 갈 수록 좌표가 증가합니다.
    // 구슬은 벽을 통과할 수 없고, 번개로 이동하면 어떤 구슬이던 바로 사라집니다.
    // 구슬은 포탈에 도달하면 반대편 포탈로 이동합니다.
    // 좌표 (x, y)에 있는 구슬의 종류는 levelData.getMarble(x, y)로 구할 수 있습니다.
    // 좌표 (x, y)에 있는 물체의 종류는 levelData.getBoardObject(x, y)로 구할 수 있습니다.

    // 구현해야 할 함수 1
    public List<Vector3Int> getMovablePosList(Vector3Int currentPos)
    {
        List<Vector3Int> posList = new List<Vector3Int>();

        // currentPos 위치에 있는 현재 구슬이 갈 수 있는 좌표들의 list를 반환합니다.
        // 리스트에는 posList.Add(좌표) 함수로 추가합니다.
        // 유니티에서 (x, y)좌표를 나타낼 때는 Vector3Int(x, y, 0)으로 나타냅니다.
        // Vector3Int pos = new Vector3Int(x, y, 0); 
        // 위와 같은 형식으로 좌표 변수를 선언할 수 있습니다.
        // pos.x, pos.y와 같이 접근합니다.
        posList.Add(new Vector3Int(2, 3, 0));

        return posList;
    }

    // 구현해야 할 함수 2
    public void setMarble(Vector3Int pos, MarbleType marbleType)
    {
        // pos 좌표에 있는 구슬의 타입을 marbleType으로 변경합니다.
        // 데이터 뿐 아니라 실제 화면에도 반영해줍니다.
        // 아래 코드가 실제 화면에 반영해주는 코드입니다.
        // marble.SetTile(startPoint + new Vector3Int(pos.x, -pos.y, 0), marblePrefabs.Find(x => x.marbleType == marbleType).marblePrefab);
    }

    public void changeMarble(Vector3Int pos)
    {
        // pos 좌표에 있는 구슬의 타입을 규칙에 따라 변환합니다.
        // 금색 구슬은 흰색 구슬로, 나머지 구슬은 없애면 됩니다.
        // setMarble 함수를 활용합니다.
        // 역시 실제 화면에도 반영해줍니다.
    }

    // 구현해야 할 함수 3
    public bool moveMarble(Vector3Int start, Vector3Int end)
    {
        // start 좌표의 구슬을 end로 옮깁니다.
        // end가 옮기는 것이 가능한 위치라면 옮기고 true를 반환합니다.
        // 불가능한 위치라면 false를 반환합니다.

        return false;
    }

    void Awake()
    {
        levelData = new LevelData(11, 11);
        levelData.boardObjectData = new BoardObjectData();
        levelData.marbleData = new MarbleData();
    }

    // Start()는 최초 1회만 실행되는 함수입니다.
    // Update()는 Start()실행 이후 계속해서 실행되는 함수입니다.
    // 여기서 위에서 만든 함수를 테스트해보세요.
    // Start is called before the first frame update
    void Start()
    {
        //board.SetTile(new Vector3Int(-4, -4, 0), boardObjectPrefabs.Find(x => x.boardObjectType == BoardObjectType.Wall).boardObjectPrefab);
        //initBoard(1);

        // 테스트해보세요
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int posInt = grid.WorldToCell(pos);
            Vector3Int tilePosInt = worldToPos(posInt);

            if (tilePosInt.x >= 0 && tilePosInt.x <= 10 && tilePosInt.y >= 0 && tilePosInt.y <= 10)
            {
                OnBoardPressed?.Invoke(tilePosInt);
            }
            
            //Debug.Log(posInt);
            //Debug.Log(board.GetTile(posInt).name);
        }

        // 테스트해보세요

        // 추가 구현 부분
        if (getMarbleNum() == 1)
        {
            OnMarbleClear?.Invoke();
        }
    }
}