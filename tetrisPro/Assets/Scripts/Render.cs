using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Render : MonoBehaviour
{
    public GameObject flameBlock;
    public GameObject minoBlock;
    public GameObject emptyBlock;

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
        var field_leftWall = 0;
        var field_rightWall = 11;

        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                if (i == field_leftWall || i == field_rightWall)
                {
                    field[i, j] = (int)FieldValue.WallBlock;
                    Instantiate(flameBlock, new Vector3(i, j, 75), Quaternion.identity);
                }
                if (j == field_bottom)
                {
                    field[i, j] = (int)FieldValue.WallBlock;
                    Instantiate(flameBlock, new Vector3(i, j, 75), Quaternion.identity);
                }
            }
        }
    }

    // fieldの要素数を読み取り、値に沿ったオブジェクトを作成する
    public void Draw(int[,] field)
    {
        for (int i = 0; i < field.GetLength(0); i++)
        {

            for (int j = 0; j < field.GetLength(1); j++)
            {
                // ブロックがないとき
                if (field[i, j] == (int)FieldValue.Empty)
                {
                    Instantiate(emptyBlock, new Vector3(i, j, 75), Quaternion.identity);
                }
                // ブロックがあるとき
                if (field[i, j] == (int)FieldValue.MinoBlock || field[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    Instantiate(minoBlock, new Vector3(i, j, 75), Quaternion.identity);
                }
            }
        }
    }

    // 古いオブジェクトをタグで管理しまとめて削除
    public void Delete()
    {
        GameObject[] DeleteBlocks = GameObject.FindGameObjectsWithTag("DeleteBlocks");

        foreach (GameObject cube in DeleteBlocks)
        {
            Destroy(cube);
        }
    }
}
