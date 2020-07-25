using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
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

    public List<List<>>


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
