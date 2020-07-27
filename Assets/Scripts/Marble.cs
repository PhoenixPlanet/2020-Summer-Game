using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Marble : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public enum BoardObjectType
{
    Empty = 0,
    Wall,
    Portal_1,
    Portal_2,
    Lightning
}

[Serializable]
public enum MarbleType
{
    Silver = 0,
    Gold,
    White,
    Blue,
    Red,
    None
}

[Serializable]
public enum DirectionType
{
    Right,
    Down,
    Left,
    Up,
    Right_Up,
    Right_Down,
    Left_Up,
    Left_Down
}
