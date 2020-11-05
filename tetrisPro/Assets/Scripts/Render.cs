using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris.Rotation;
using Tetris.Mino;


public class Render : MonoBehaviour
{
    public GameObject flameBlock;
    public GameObject flameBlockTop;    // 上のフレームだけ少しスケールが大きい
    public GameObject minoBlock;
    public GameObject emptyBlock;

    // 枠組みを作る（見た目は異なるが、成分は固定されたブロックと同じ）
    public void MakeFlame(int[,] field)
    {
        const int FIELDTOP = 22;   
        const int FIELDBOTTOM = 0;
        const int FIELDLEFT = 0;
        const int FIELDRIGHT = 11;

        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                if (i == FIELDLEFT || i == FIELDRIGHT)
                {
                    field[i, j] = (int)Rotation.eFieldValue.WallBlock;
                    Instantiate(flameBlock, new Vector3(i, j, 73), Quaternion.identity);
                }
                if (j == FIELDTOP)
                {
                    field[i, j] = (int)Rotation.eFieldValue.WallBlock;
                    Instantiate(flameBlockTop, new Vector3(i , j - 0.2f, 73), Quaternion.identity);
                }
                if (j == FIELDBOTTOM)
                {
                    field[i, j] = (int)Rotation.eFieldValue.WallBlock;
                    Instantiate(flameBlock, new Vector3(i, j, 73), Quaternion.identity);
                }
            }
        }
    }

    // Fieldを描画
    public void DrawField(int[,] field)
    {
        for (int i = 0; i < field.GetLength(0); i++)
        {

            for (int j = 0; j < field.GetLength(1); j++)
            {
                // ブロックがないとき
                if (field[i, j] == (int)Rotation.eFieldValue.Empty)
                {
                    Instantiate(emptyBlock, new Vector3(i, j, 75), Quaternion.identity);
                }
                // ブロックがあるとき
                if (field[i, j] == (int)Rotation.eFieldValue.WallBlock)
                {
                    Instantiate(minoBlock, new Vector3(i, j, 75), Quaternion.identity);
                }
            }
        }
    }

    // Minoを描画
    public void DrawMino(int xPos, int yPos,ref int[] minoArray, MinoData.eMinoType currentMino)
    {
        int sideLength;
        // Iミノは 4 * 4 マスなため sizeが変わる
        if (currentMino == MinoData.eMinoType.I)
        {
            sideLength = MinoData.I_MINO_SIDE_LENGTH;
        }
        else if(currentMino == MinoData.eMinoType.O)
        {
            sideLength = MinoData.O_MINO_SIDE_LENGTH;
        }
        else
        {
            sideLength = MinoData.MINO_SIDE_LENGTH;
        }

        // 1次元配列のミノのデータを Field の座標に変換
        int count = 0;
        int length = minoArray.Length;
        for (int i = 0; i < length; i++)
        {
            if (minoArray[i] == (int)Rotation.eFieldValue.MinoBlock)
            {
                Instantiate(minoBlock, new Vector3(xPos, yPos, 74), Quaternion.identity);
            }
            xPos++;
            count++;
            if (count == sideLength)
            {
                xPos -= sideLength;
                yPos--;
                count = 0;
            }
        }
    }

    // Field内で更新があった時、古いオブジェクトをタグで管理しまとめて削除
    public void Delete()
    {
        GameObject[] DeleteBlocks = GameObject.FindGameObjectsWithTag("DeleteBlocks");

        foreach (GameObject cube in DeleteBlocks)
        {
            Destroy(cube);
        }
    }

    // Minoの移動や回転時（更新があった時）、古いオブジェクトをタグで管理しまとめて削除
    public void DeleteMino()
    {
        GameObject[] ActiveBlocks = GameObject.FindGameObjectsWithTag("ActiveBlock");

        foreach (GameObject cube in ActiveBlocks)
        {
            Destroy(cube);
        }
    }
}
