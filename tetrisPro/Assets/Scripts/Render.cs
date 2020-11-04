using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Render : MonoBehaviour
{

    public GameObject flameBlock;
    public GameObject flameBlockTop;    // 上のフレームだけ少しスケールが小さい
    public GameObject minoBlock;
    public GameObject emptyBlock;

    enum eFieldValue : int
    {
        Empty,
        MinoBlock,
        WallBlock
    }

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
                    field[i, j] = (int)eFieldValue.WallBlock;
                    Instantiate(flameBlock, new Vector3(i, j, 73), Quaternion.identity);
                }
                if (j == FIELDTOP)
                {
                    field[i, j] = (int)eFieldValue.WallBlock;
                    Instantiate(flameBlockTop, new Vector3(i , j - 0.2f, 73), Quaternion.identity);
                }
                if (j == FIELDBOTTOM)
                {
                    field[i, j] = (int)eFieldValue.WallBlock;
                    Instantiate(flameBlock, new Vector3(i, j, 73), Quaternion.identity);
                }
            }
        }
    }

    // Fieldを描画
    public void DrawField(int[,] field)
    {
        for (int i = 1; i < field.GetLength(0) - 1; i++)
        {

            for (int j = 1; j < field.GetLength(1) - 1; j++)
            {
                // ブロックがないとき
                if (field[i, j] == (int)eFieldValue.Empty)
                {
                    Instantiate(emptyBlock, new Vector3(i, j, 75), Quaternion.identity);
                }
                // ブロックがあるとき
                if (field[i, j] == (int)eFieldValue.WallBlock)
                {
                    Instantiate(minoBlock, new Vector3(i, j, 75), Quaternion.identity);
                }
            }
        }
    }

    // Minoを描画
    public void DrawMino(int xPos, int yPos,ref int[] minoArray, int currentMino)
    {
        int sideLength;
        // Iミノは 4 * 4 マスなため sizeが変わる
        if (currentMino == (int)Rotation.eMinoType.I)
        {
            sideLength = Rotation.I_MINO_SIDE_LENGTH;
        }
        else
        {
            sideLength = Rotation.MINO_SIDE_LENGTH;
        }

        // 1次元配列のミノのデータを Field の座標に変換
        int count = 0;
        yPos -= 2;
        for (int i = 0; i < sideLength * sideLength; i++)
        {
            if (minoArray[i] == (int)eFieldValue.MinoBlock)
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
