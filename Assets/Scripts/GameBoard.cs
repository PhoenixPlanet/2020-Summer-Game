using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GameBoard : MonoBehaviour
{
    LevelData levelData;

    public Vector3Int startPoint;

    public Tilemap board;
    public Tilemap marble;

    public Grid grid;

    public List<BoardObjectPrefab> boardObjectPrefabs;
    public List<MarblePrefab> marblePrefabs;

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

    // 구현 시 알아두어야 할 점입니다.
    // 좌표는 왼쪽, 위 끝이 (0, 0)이고 오른쪽, 아래로 갈 수록 좌표가 증가합니다.
    // 구슬은 벽을 통과할 수 없고, 번개로 이동하면 어떤 구슬이던 바로 사라집니다.
    // 구슬은 포탈에 도달하면 반대편 포탈로 이동합니다.
    // 좌표 (x, y)에 있는 구슬의 종류는 levelData.getMarble(x, y)로 구할 수 있습니다.
    // 좌표 (x, y)에 있는 물체의 종류는 levelData.getBoardObject(x, y)로 구할 수 있습니다.
    // 좌표 (x, y)에 있는 구슬의 종류는 levelData.setMarble(x, y, MarbleType)로 지정할 수 있습니다. 

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

        return posList;
    }

    // 구현해야 할 함수 2
    public void setMarble(Vector3Int pos, MarbleType marbleType)
    {
        // pos 좌표에 있는 구슬의 타입을 marbleType으로 변경합니다.
        // 데이터 뿐 아니라 실제 화면에도 반영해줍니다.
        // 아래 코드가 실제 화면에 반영해주는 코드입니다.
        // marble.SetTile(startPoint + new Vector3Int(pos.x, -pos.y, 0), marblePrefabs.Find(x => x.marbleType == marbleType).marblePrefab);
        // marbleType이 MarbleType.None 이라면 levelData.marbleData.marble_num을 1 감소시켜줍니다.
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
        initBoard(1);

        // 테스트해보세요
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int posInt = grid.LocalToCell(pos);
            
            Debug.Log(posInt);
            Debug.Log(board.GetTile(posInt).name);
        }*/

        // 테스트해보세요
    }
}

[Serializable]
public class LevelData
{
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;

    public Vector2Int board_size
    {
        set
        {
            width = value.x;
            height = value.y;
        }

        get
        {
            return new Vector2Int(width, height);
        }
    }

    public BoardObjectData boardObjectData;
    public MarbleData marbleData;

    public LevelData(int width_, int height_)
    {
        board_size = new Vector2Int(width_, height_);
    }

    public BoardObjectType getBoardObject(int x, int y)
    {
        return boardObjectData.game_board[y].row[x];
    }

    public MarbleType getMarble(int x, int y)
    {
        return marbleData.game_board[y].row[x];
    }

    public void setMarble(int x, int y, MarbleType newMarble)
    {
        marbleData.game_board[y].row[x] = newMarble;
    }
}

[Serializable]
public class BoardObjectData
{
    public List<SerializableBoardObjectRow> game_board = new List<SerializableBoardObjectRow>();

    public void initBoardData(Vector2Int board_size, BoardObjectType init_item)
    {
        for (int i = 0; i < board_size.y; i++)
        {
            SerializableBoardObjectRow t = new SerializableBoardObjectRow();
            for (int j = 0; j < board_size.x; j++)
            {
                t.row.Add(init_item);
            }
            game_board.Add(t);
        }
    }
}

[Serializable]
public class MarbleData
{
    public int marble_num;

    public List<SerializableMarbleRow> game_board = new List<SerializableMarbleRow>();

    public void initBoardData(Vector2Int board_size, MarbleType init_item)
    {
        for (int i = 0; i < board_size.y; i++)
        {
            SerializableMarbleRow t = new SerializableMarbleRow();
            for (int j = 0; j < board_size.x; j++)
            {
                t.row.Add(init_item);
            }
            game_board.Add(t);
        }
    }
}

[Serializable]
public class SerializableMarbleRow
{ 
    public List<MarbleType> row = new List<MarbleType>();
}

[Serializable]
public class SerializableBoardObjectRow
{
    public List<BoardObjectType> row = new List<BoardObjectType>();
}

[Serializable]
public struct BoardObjectPrefab
{
    public BoardObjectType boardObjectType;
    public TileBase boardObjectPrefab;
}

[Serializable]
public struct MarblePrefab
{
    public MarbleType marbleType;
    public TileBase marblePrefab;
}