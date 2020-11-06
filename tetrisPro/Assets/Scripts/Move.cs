using UnityEngine;
using Tetris.Mino;
using Tetris.Rotation;
using eMinoType = Tetris.Mino.MinoData.eMinoType;

public class Move
{
    // 上下左右の移動
    public void MoveMino(ref int minoPosX,ref int minoPosY, int x, int y)
    {
        minoPosX += x;
        minoPosY += y;
    }

    // 移動後の座標に壁がないかチェック
    public bool CheckWall(int[,] field, int[] minoArray, int minoPosX, int minoPosY, eMinoType type)
    {
        // 移動制限を超えていないか
        if ((0 <= minoPosX && minoPosX < Tetris.Field.FieldData.FIELD_WIDTH) && (0 <= minoPosY && minoPosY < Tetris.Field.FieldData.FIELD_HEIGHT))
        {
            int size;
            int sideLength;

            // IとOミノのみ特殊 
            if (type == eMinoType.I)
            {
                size = MinoData.I_MINO_SIZE;
                sideLength = MinoData.I_MINO_SIDE_LENGTH;
            }
            else if (type == eMinoType.O)
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

            bool canMove;
            int minoCount = 0;
            const int MAX_COUNT = 4; // ミノは4つのブロックからできている

            // 重なってないか比較
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    // ミノの部分が field で空の部分なら
                    if (array[i, j] == (int)Rotation.eFieldValue.MinoBlock)
                    {
                        if (field[i + minoPosY, j + minoPosX] == (int)Rotation.eFieldValue.Empty)
                        {
                            int y = Tetris.Field.FieldData.FIELD_HEIGHT - 1 - minoPosY;
                            //UnityEngine.Debug.DrawLine(new Vector3(j + minoPosX, y - i, 75), new Vector3(j + minoPosX, y - i, - 75), Color.red, 10f);
                            minoCount++;
                        }
                    }
                }
            }
            if (minoCount == MAX_COUNT)
            {
                UnityEngine.Debug.Log("移動、または回転ができます。");
                return true;
            }
            else
            {
                UnityEngine.Debug.Log("壁があるため移動、または回転ができません。");
                return false;
            }
        }
        else
        {
            UnityEngine.Debug.Log("範囲制限のため移動、または回転ができません。");
            return false;
        }
    }
}
