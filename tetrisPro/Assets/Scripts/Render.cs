using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Render : MonoBehaviour
{
    public GameObject FlameBlock;
    public GameObject MinoBlock;
    public GameObject EmptyBlock;

    enum FieldValue : int
    {
        Empty,
        MinoBlock,
        MinoBlock_Axis,
        WallBlock,

    }

    public void MakeFlame(int[,] field)
    {
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
                    Instantiate(FlameBlock, new Vector3(i, j, 75), Quaternion.identity);
                }
                if (j == field_bottom || j == field_top)
                {
                    field[i, j] = (int)FieldValue.WallBlock;
                    Instantiate(FlameBlock, new Vector3(i, j, 75), Quaternion.identity);
                }
            }
        }
    }

    public void Draw(int[,] field)
    {
        for (int i = 0; i < field.GetLength(0); i++)
        {

            for (int j = 0; j < field.GetLength(1); j++)
            {
                if (field[i, j] == (int)FieldValue.Empty)
                {
                    Instantiate(EmptyBlock, new Vector3(i, j, 75), Quaternion.identity);
                }
                if (field[i, j] == (int)FieldValue.MinoBlock || field[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    Instantiate(MinoBlock, new Vector3(i, j, 75), Quaternion.identity);
                }
            }
        }
    }

    public void Delete()
    {
        GameObject[] DeleteBlocks = GameObject.FindGameObjectsWithTag("DeleteBlocks");

        foreach (GameObject cube in DeleteBlocks)
        {
            Destroy(cube);
        }
    }
}
