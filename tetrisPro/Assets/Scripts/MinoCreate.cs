

namespace Tetris.Mino
{
    public class MinoCreate
    {
        // minoArrayに生成するミノのデータを代入する
        public void SetMinoData(out int[] minoArray, MinoData.eMinoType type)
        {
            if (type == MinoData.eMinoType.MAX)
            {
                minoArray = null;
                return;
            }

            int size;
            // Iミノは 4 * 4 の配列なので size が変わる
            if (type == MinoData.eMinoType.I)
            {
                size = MinoData.I_MINO_SIZE;
                
            }
            // Oミノは 2 * 2 の配列なので size が変わる
            else if (type == MinoData.eMinoType.O)
            {
                size = MinoData.O_MINO_SIZE;
            }
            else
            {
                size = MinoData.MINO_SIZE;
            }

            minoArray = new int[size];

            MinoData.MinoArrays[(int)type].CopyTo(minoArray, 0);
        }
    }
}