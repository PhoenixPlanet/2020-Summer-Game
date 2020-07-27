using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using System.Text;

public class GameBoard : MonoBehaviour
{
    BoardData<BoardObjectType> board_data;
    BoardData<MarbleType> marble_data;

    public void loadBoardData(string path, string file_name)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", path, file_name), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string json_board_data = Encoding.UTF8.GetString(data);
        board_data = JsonUtility.FromJson<BoardData<BoardObjectType>>(json_board_data);
    }

    public void saveBoardData(string path, string file_name)
    {
        string json_board_data = JsonUtility.ToJson(board_data);

        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", path, file_name), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(json_board_data);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    public 

    public bool moveMarble(int x, int y, DirectionType dr)
    {   
        return false;
    }

    void Awake()
    {
        board_data = new BoardData<BoardObjectType>();
    }

    // Start is called before the first frame update
    void Start()
    {
        board_data.initBoardData(11, 11);
        saveBoardData(Application.dataPath, "ex");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class BoardData<T> where T : System.Enum
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

    public int marble_num;

    public List<SerializableRow<T>> game_board = new List<SerializableRow<T>>();

    public void initBoardData(int width, int height, T init_item)
    {
        board_size = new Vector2Int(width, height);
        for (int i = 0; i < height; i++)
        {
            SerializableRow<T> t = new SerializableRow<T>();
            for (int j = 0; j < width; j++)
            {
                t.row.Add(init_item);
            }
            game_board.Add(t);
        }
    }
}

[Serializable]
public class SerializableRow<T> where T : System.Enum
{
    public List<T> row = new List<T>();
}
