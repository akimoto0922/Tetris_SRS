using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    bool canMove;   // 移動可能かどうか

    enum FieldValue : int
    {
        Empty,
        MinoBlock,
        MinoBlock_Axis,
        WallBlock,
    }

    public void MoveMino(int[,] field, int x, int y, ref bool isGround)
    {
        CheckWall(field, x, y, ref isGround);

        if (canMove == true)
        {
            int[,] array = new int[12, 22]; // 値を仮置きしておくための配列

            // ①仮置きする配列を初期化
            for (int i = 0; i < field.GetLength(0); i++)
            {

                for (int j = 0; j < field.GetLength(1); j++)
                {
                    array[i, j] = 0;
                }

            }

            // ②移動後のミノの座標を仮置きの配列に保存して、もとの部分を空きブロックにする
            for (int i = 0; i < field.GetLength(0); i++)
            {

                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (field[i, j] == (int)FieldValue.MinoBlock || field[i, j] == (int)FieldValue.MinoBlock_Axis)
                    {
                        array[i + x, j + y] = field[i, j];
                        field[i, j] = (int)FieldValue.Empty;

                    }
                }
            }

            // ③仮置き配列のミノの部分をfieldにコピーする
            for (int i = 0; i < array.GetLength(0); i++)
            {

                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j] == (int)FieldValue.MinoBlock || array[i, j] == (int)FieldValue.MinoBlock_Axis)
                    {
                        field[i, j] = array[i, j];
                    }
                }
            }
        }
    }

    void CheckWall(int[,] field, int x, int y, ref bool isGround)
    {
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                if (field[i, j] == (int)FieldValue.MinoBlock || field[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    if (field[i + x, j + y] == (int)FieldValue.WallBlock)
                    {
                        canMove = false;
                        if (y == -1)
                        {
                            isGround = true;
                        }
                        return;
                    }
                    else
                    {
                        canMove = true;
                        if (y == -1)
                        {
                            isGround = false;
                        }
                    }

                }
            }
        }

    }
}
