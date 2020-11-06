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

        int yMax = field.GetLength(0);
        int xMax = field.GetLength(1);

        for (int y = yMax - 1; y >= 0; --y)
        {
            for (int x = xMax - 1; x >= 0; --x)
            {
                if (x == FIELDLEFT || x == FIELDRIGHT)
                {
                    field[y, x] = (int)Rotation.eFieldValue.WallBlock;
                    Instantiate(flameBlock, new Vector3(x, y, 73), Quaternion.identity);
                }
                if (y == FIELDTOP)
                {
                    field[y, x] = (int)Rotation.eFieldValue.WallBlock;
                    Instantiate(flameBlockTop, new Vector3(x, y - 0.2f, 73), Quaternion.identity);
                }
                if (y == FIELDBOTTOM)
                {
                    field[y, x] = (int)Rotation.eFieldValue.WallBlock;
                    Instantiate(flameBlock, new Vector3(x, y, 73), Quaternion.identity);
                }
            }
        }
    }

    // Fieldを描画
    public void DrawField(int[,] field)
    {
        int yMax = field.GetLength(0);
        int xMax = field.GetLength(1);
        for (int y = yMax - 1; y >= 0; --y)
        {
            int idxY = yMax - 1 - y;
            for (int x = 0; x < xMax; ++x)
            {
                // ブロックがないとき
                if (field[idxY, x] == (int)Rotation.eFieldValue.Empty)
                {
                    Instantiate(emptyBlock, new Vector3(x, y, 75), Quaternion.identity);
                    //UnityEngine.Debug.DrawLine(new Vector3(x, y, 75), new Vector3(x, y, -75), Color.red, 10f);
                }
                // ブロックがあるとき
                else if (field[idxY, x] == (int)Rotation.eFieldValue.WallBlock)
                {
                    Instantiate(flameBlock, new Vector3(x, y, 7), Quaternion.identity);
                    //UnityEngine.Debug.DrawLine(new Vector3(x, y, 75), new Vector3(x, y, -75), Color.blue, 10f);
                }
            }
        }
    }

    // Minoを描画
    public void DrawMino(int xPos, int yPos,ref int[] minoArray, MinoData.eMinoType type)
    {
        int size;
        int sideLength;

        yPos = Tetris.Field.FieldData.FIELD_HEIGHT - 1 - yPos;

        // Iミノのみ特殊 
        if (type == MinoData.eMinoType.I)
        {
            size = MinoData.I_MINO_SIZE;
            sideLength = MinoData.I_MINO_SIDE_LENGTH;
        }
        else if (type == MinoData.eMinoType.O)
        {
            size = MinoData.O_MINO_SIZE;
            sideLength = MinoData.O_MINO_SIDE_LENGTH;
        }
        else
        {
            size = MinoData.MINO_SIZE;
            sideLength = MinoData.MINO_SIDE_LENGTH;
        }

        int[,] array = new int[sideLength, sideLength];

        for (int i = 0; i < size; i++)
        {
            // 配列を2次元配列にコピー
            int indexX = i % sideLength;
            int indexY = i / sideLength;

            array[indexY, indexX] = minoArray[i];
        }

        // 1次元配列のミノのデータを Field の座標に変換
        int count = 0;
        int length = minoArray.Length;
        for (int i = 0; i < length; i++)
        {
            int x = i % sideLength;
            int y = i / sideLength;

            if (minoArray[i] == (int)Rotation.eFieldValue.MinoBlock)
            {
                Instantiate(minoBlock, new Vector3(xPos + x, yPos - y, 74), Quaternion.identity);
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
