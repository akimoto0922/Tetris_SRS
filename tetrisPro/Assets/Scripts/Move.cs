using Tetris.Mino;
using Tetris.Rotation;
using eMinoType = Tetris.Mino.MinoData.eMinoType;

public class Move
{
    // 上下左右の移動
    public void MoveMino(int[,] field, int[]minoArray, ref int minoPosX,ref int minoPosY, int x, int y, MinoData.eMinoType type)
    {
        int sideLength;

        // Iミノのみ特殊 
        if (type == eMinoType.I)
        {
            sideLength = MinoData.I_MINO_SIDE_LENGTH;
        }
        else if (type == eMinoType.O)
        {
            sideLength = MinoData.O_MINO_SIDE_LENGTH;
        }
        else
        {
            sideLength = MinoData.MINO_SIDE_LENGTH;
        }

        // 移動後の座標
        int movedPosX = minoPosX + x;
        int movedPosY = minoPosY + y;

        // 移動制限を超えていないか
        if (0 <= movedPosX && movedPosX < 12 - (sideLength - 1) && 0 + (sideLength - 2) <= movedPosY && movedPosY <= 24)
        {
            // 動けるかどうかチェック
            if (CheckWall(field, minoArray, movedPosX, movedPosY, type))
            {
                minoPosX = movedPosX;
                minoPosY = movedPosY;

                UnityEngine.Debug.Log("移動できます。");
            }
            else
            {
                UnityEngine.Debug.Log("壁があり移動できません。");
            }
        }
        else
        {
            UnityEngine.Debug.Log("範囲制限のため移動できません。");
        }
    }

    // 移動後の座標に壁がないかチェック
    public static  bool CheckWall(int[,] field, int[] minoArray, int minoPosX, int minoPosY, MinoData.eMinoType type)
    {
        int size;
        int sideLength;

        // Iミノのみ特殊 
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

            array[indexX, indexY] = minoArray[i];
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
                    if (field[i + minoPosX, j + minoPosY] == (int)Rotation.eFieldValue.Empty)
                    {
                        minoCount++;
                    }
                }
            }
        }
        if (minoCount == MAX_COUNT)
        {
            canMove = true;
            return canMove;
        }
        else
        {
            canMove = false;
            return canMove;
        }
    }

}
