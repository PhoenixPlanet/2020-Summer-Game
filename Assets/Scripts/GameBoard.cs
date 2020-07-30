using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GameBoard : MonoBehaviour
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
        }
        else
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
        MarbleType marbleType = levelData.getMarble(currentPos.x, currentPos.y);
        return NewMethod(currentPos, posList, marbleType);
    }

    private List<Vector3Int> NewMethod(Vector3Int currentPos, List<Vector3Int> posList, MarbleType marbleType)
    {
        if (marbleType == MarbleType.Silver || marbleType == MarbleType.Gold || marbleType == MarbleType.White)
        {
            if (currentPos.x >= 2) if (levelData.getMarble(currentPos.x - 1, currentPos.y) != MarbleType.None && levelData.getMarble(currentPos.x - 2, currentPos.y) == MarbleType.None && levelData.getBoardObject(currentPos.x - 2, currentPos.y) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x - 2, currentPos.y, 0));
            if (currentPos.x <= 8) if (levelData.getMarble(currentPos.x + 1, currentPos.y) != MarbleType.None && levelData.getMarble(currentPos.x + 2, currentPos.y) == MarbleType.None && levelData.getBoardObject(currentPos.x + 2, currentPos.y) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x + 2, currentPos.y, 0));
            if (currentPos.y >= 2) if (levelData.getMarble(currentPos.x, currentPos.y - 1) != MarbleType.None && levelData.getMarble(currentPos.x, currentPos.y - 2) == MarbleType.None && levelData.getBoardObject(currentPos.x, currentPos.y - 2) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x, currentPos.y - 2, 0));
            if (currentPos.y <= 8) if (levelData.getMarble(currentPos.x, currentPos.y + 1) != MarbleType.None && levelData.getMarble(currentPos.x, currentPos.y + 2) == MarbleType.None && levelData.getBoardObject(currentPos.x, currentPos.y + 2) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x, currentPos.y + 2, 0));
        }//상하좌우 1개만 뛰어넘을 수 있는 구슬
        else if (marbleType == MarbleType.Blue)
        {
            if (currentPos.x >= 2) if (levelData.getMarble(currentPos.x - 1, currentPos.y) != MarbleType.None && levelData.getMarble(currentPos.x - 2, currentPos.y) == MarbleType.None && levelData.getBoardObject(currentPos.x - 2, currentPos.y) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x - 2, currentPos.y, 0));
            if (currentPos.x <= 8) if (levelData.getMarble(currentPos.x + 1, currentPos.y) != MarbleType.None && levelData.getMarble(currentPos.x + 2, currentPos.y) == MarbleType.None && levelData.getBoardObject(currentPos.x + 2, currentPos.y) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x + 2, currentPos.y, 0));
            if (currentPos.y >= 2) if (levelData.getMarble(currentPos.x, currentPos.y - 1) != MarbleType.None && levelData.getMarble(currentPos.x, currentPos.y - 2) == MarbleType.None && levelData.getBoardObject(currentPos.x, currentPos.y - 2) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x, currentPos.y - 2, 0));
            if (currentPos.y <= 8) if (levelData.getMarble(currentPos.x, currentPos.y + 1) != MarbleType.None && levelData.getMarble(currentPos.x, currentPos.y + 2) == MarbleType.None && levelData.getBoardObject(currentPos.x, currentPos.y + 2) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x, currentPos.y + 2, 0));
            if (currentPos.x <= 8 && currentPos.y <= 8) if (levelData.getMarble(currentPos.x + 1, currentPos.y + 1) != MarbleType.None && levelData.getMarble(currentPos.x + 2, currentPos.y + 2) == MarbleType.None && levelData.getBoardObject(currentPos.x + 2, currentPos.y + 2) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x + 2, currentPos.y + 2, 0));
            if (currentPos.x >= 2 && currentPos.y <= 8) if (levelData.getMarble(currentPos.x - 1, currentPos.y + 1) != MarbleType.None && levelData.getMarble(currentPos.x - 2, currentPos.y + 2) == MarbleType.None && levelData.getBoardObject(currentPos.x - 2, currentPos.y + 2) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x - 2, currentPos.y + 2, 0));
            if (currentPos.x >= 2 && currentPos.y >= 2) if (levelData.getMarble(currentPos.x - 1, currentPos.y - 1) != MarbleType.None && levelData.getMarble(currentPos.x - 2, currentPos.y - 2) == MarbleType.None && levelData.getBoardObject(currentPos.x - 2, currentPos.y - 2) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x - 2, currentPos.y - 2, 0));
            if (currentPos.x <= 8 && currentPos.y >= 2) if (levelData.getMarble(currentPos.x + 1, currentPos.y - 1) != MarbleType.None && levelData.getMarble(currentPos.x + 2, currentPos.y - 2) == MarbleType.None && levelData.getBoardObject(currentPos.x + 2, currentPos.y - 2) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x + 2, currentPos.y - 2, 0));
        }//8방향으로 1개 뛰어넘을 수 있는 구슬
        else if (marbleType == MarbleType.Red)
        {
            if (currentPos.x >= 2) if (levelData.getMarble(currentPos.x - 1, currentPos.y) != MarbleType.None && levelData.getMarble(currentPos.x - 2, currentPos.y) == MarbleType.None && levelData.getBoardObject(currentPos.x - 2, currentPos.y) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x - 2, currentPos.y, 0));
            if (currentPos.x <= 8) if (levelData.getMarble(currentPos.x + 1, currentPos.y) != MarbleType.None && levelData.getMarble(currentPos.x + 2, currentPos.y) == MarbleType.None && levelData.getBoardObject(currentPos.x + 2, currentPos.y) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x + 2, currentPos.y, 0));
            if (currentPos.y >= 2) if (levelData.getMarble(currentPos.x, currentPos.y - 1) != MarbleType.None && levelData.getMarble(currentPos.x, currentPos.y - 2) == MarbleType.None && levelData.getBoardObject(currentPos.x, currentPos.y - 2) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x, currentPos.y - 2, 0));
            if (currentPos.y <= 8) if (levelData.getMarble(currentPos.x, currentPos.y + 1) != MarbleType.None && levelData.getMarble(currentPos.x, currentPos.y + 2) == MarbleType.None && levelData.getBoardObject(currentPos.x, currentPos.y + 2) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x, currentPos.y + 2, 0));
            //상단은 기본 구슬과 같음 하단은 2칸을 뛰는 경우
            //red|marble|something|end
            if (currentPos.x >= 3) if (levelData.getMarble(currentPos.x - 1, currentPos.y) != MarbleType.None && levelData.getMarble(currentPos.x - 3, currentPos.y) == MarbleType.None && levelData.getBoardObject(currentPos.x - 3, currentPos.y) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x - 3, currentPos.y, 0));
            if (currentPos.x <= 7) if (levelData.getMarble(currentPos.x + 1, currentPos.y) != MarbleType.None && levelData.getMarble(currentPos.x + 3, currentPos.y) == MarbleType.None && levelData.getBoardObject(currentPos.x + 3, currentPos.y) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x + 3, currentPos.y, 0));
            if (currentPos.y >= 2) if (levelData.getMarble(currentPos.x, currentPos.y - 1) != MarbleType.None && levelData.getMarble(currentPos.x, currentPos.y - 3) == MarbleType.None && levelData.getBoardObject(currentPos.x, currentPos.y - 3) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x, currentPos.y - 3, 0));
            if (currentPos.y <= 7) if (levelData.getMarble(currentPos.x, currentPos.y + 1) != MarbleType.None && levelData.getMarble(currentPos.x, currentPos.y + 3) == MarbleType.None && levelData.getBoardObject(currentPos.x, currentPos.y + 3) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x, currentPos.y + 3, 0));
            //red|not marble|marble|end
            if (currentPos.x >= 3) if (levelData.getMarble(currentPos.x - 1, currentPos.y) == MarbleType.None && levelData.getMarble(currentPos.x - 2, currentPos.y) != MarbleType.None && levelData.getMarble(currentPos.x - 3, currentPos.y) == MarbleType.None && levelData.getBoardObject(currentPos.x - 3, currentPos.y) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x - 3, currentPos.y, 0));
            if (currentPos.x <= 7) if (levelData.getMarble(currentPos.x + 1, currentPos.y) == MarbleType.None && levelData.getMarble(currentPos.x + 2, currentPos.y) != MarbleType.None && levelData.getMarble(currentPos.x + 3, currentPos.y) == MarbleType.None && levelData.getBoardObject(currentPos.x + 3, currentPos.y) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x + 3, currentPos.y, 0));
            if (currentPos.y >= 3) if (levelData.getMarble(currentPos.x, currentPos.y - 1) == MarbleType.None && levelData.getMarble(currentPos.x, currentPos.y - 2) != MarbleType.None && levelData.getMarble(currentPos.x, currentPos.y - 3) == MarbleType.None && levelData.getBoardObject(currentPos.x, currentPos.y - 3) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x, currentPos.y - 3, 0));
            if (currentPos.y <= 7) if (levelData.getMarble(currentPos.x, currentPos.y + 1) == MarbleType.None && levelData.getMarble(currentPos.x, currentPos.y + 2) != MarbleType.None && levelData.getMarble(currentPos.x, currentPos.y + 3) == MarbleType.None && levelData.getBoardObject(currentPos.x, currentPos.y + 3) != BoardObjectType.Wall) posList.Add(new Vector3Int(currentPos.x, currentPos.y + 3, 0));
        }//상하좌우 2개까지 뛰어넘을 수 있는 구슬 - 
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
        MarbleType check = levelData.getMarble(pos.x, pos.y);
        levelData.setMarble(pos.x, pos.y, marbleType);
        marble.SetTile(startPoint + new Vector3Int(pos.x, -pos.y, 0), marblePrefabs.Find(x => x.marbleType == marbleType).marblePrefab);
        if (marbleType == MarbleType.None && check != MarbleType.None) levelData.marbleData.marble_num--;
        // 원래 None인 곳을 None으로 바꾸었을 때 구슬 개수를 1개 감산하지 않기 위해 check변수와 if문 추가함
    }

    public void changeMarble(Vector3Int pos)
    {
        // pos 좌표에 있는 구슬의 타입을 규칙에 따라 변환합니다.
        // 금색 구슬은 흰색 구슬로, 나머지 구슬은 없애면 됩니다.
        // setMarble 함수를 활용합니다.
        // 역시 실제 화면에도 반영해줍니다.
        if (levelData.getMarble(pos.x, pos.y) == MarbleType.Gold)
            setMarble(pos, MarbleType.Silver);
        else
            setMarble(pos, MarbleType.None);
    }

    // 구현해야 할 함수 3
    public bool moveMarble(Vector3Int start, Vector3Int end)
    {
        // start 좌표의 구슬을 end로 옮깁니다.
        // end가 옮기는 것이 가능한 위치라면 옮기고 true를 반환합니다.
        // 불가능한 위치라면 false를 반환합니다.
        if (levelData.getMarble(start.x, start.y) == MarbleType.None) return false;//start에 구슬이 없을 경우
        List<Vector3Int> possibleToMove = new List<Vector3Int>();
        possibleToMove = getMovablePosList(start);//현재 위치에서 이동 할 수 있는 위치 목록
        int possible = 0;
        foreach (Vector3Int i in possibleToMove)
        {
            if (i == end) possible = 1;
        }
        if (possible == 0) return false;//end가 이동 가능 목록에 있는지 확인 후 없다면 false 리턴
        MarbleType color = levelData.getMarble(start.x, start.y);
        if (color == MarbleType.Silver || color == MarbleType.Gold || color == MarbleType.White || color == MarbleType.Blue)
        {
            setMarble(end, color);
            setMarble(start, MarbleType.None);
            changeMarble(new Vector3Int((start.x + end.x) / 2, (start.y + end.y) / 2, 0));//뛰어넘은 구슬
            if (color == MarbleType.White) changeMarble(end);
        }//은, 금, 흰, 파란 구슬의 경우
        else if (color == MarbleType.Red)
        {
            setMarble(end, color);
            setMarble(start, MarbleType.None);
            int len = (start.x - end.x) + (start.y - end.y);
            if (len < 0) len *= -1;
            //len = 1칸 뛰었는지 2칸 뛰었는지 판별
            if (len == 2)
            {
                changeMarble(new Vector3Int((start.x + end.x) / 2, (start.y + end.y) / 2, 0));//뛰어넘은 구슬
            }
            else if (len == 3)
            {
                changeMarble(new Vector3Int((start.x + end.x) / 2, (start.y + end.y) / 2, 0));//뛰어넘은 구슬
                changeMarble(new Vector3Int((start.x + end.x) / 2 + 1, (start.y + end.y) / 2 + 1, 0));//뛰어넘은 구슬
            }
        }//빨간 구슬의 경우
        if (levelData.getBoardObject(end.x, end.y) == BoardObjectType.Lightning)
        {
            changeMarble(end);
            changeMarble(end);//금색 구슬일 경우도 사라지게 하기 위해 2번 적용
        }//도착지가 번개인 경우
        else if (levelData.getBoardObject(end.x, end.y) == BoardObjectType.Portal_In)
        {
            int x = -1;
            int y = -1;
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (levelData.getBoardObject(i, j) == BoardObjectType.Portal_Out)
                    {
                        x = i;
                        y = j;
                    } //포탈 출구 위치 저장
                }
            }

            if ((x != -1 && y != -1) && levelData.getMarble(x, y) == MarbleType.None)
            {
                setMarble(new Vector3Int(x, y, 0), color);
                setMarble(end, MarbleType.None);
            }//출구에 구슬이 없을 경우 이동, 출구에 구슬이 있으면 이동하지 않음
        }//도착지가 포탈 입구인 경우
        return true;
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