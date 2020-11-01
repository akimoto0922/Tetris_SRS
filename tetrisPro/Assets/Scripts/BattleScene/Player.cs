using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject FlameBlock;
    string playerName;
    public int[,] field = new int[12, 22];
    enum FieldValue : int
    {
        Empty,
        MinoBlock,
        MinoBlock_Axis,
        WallBlock,

    }

    // Start is called before the first frame update
    void Start()
    {
        var parent = this.transform;
        // 枠を作る
        var field_bottom = 0;
        var field_top = 21;
        var field_leftWall = 0;
        var field_rightWall = 11;
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                if (i == field_leftWall || i == field_rightWall)
                {
                    field[i, j] = (int)FieldValue.WallBlock;
                    Instantiate(FlameBlock, new Vector3(i , j, 0), Quaternion.identity,parent);
                }
                if (j == field_bottom || j == field_top)
                {
                    field[i, j] = (int)FieldValue.WallBlock;
                    Instantiate(FlameBlock, new Vector3(i, j, 0), Quaternion.identity, parent);
                }
            }
        }
    }
}
