using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinoCreate : MonoBehaviour
{
    enum eMinoType : int    // ミノは７種類 + Null も含め計８種類
    {
        T,
        S,
        Z,
        L,
        J,
        O,
        I,
        MAX
    }

    // minoArrayにミノのデータを代入する
    public void SetMinoData(ref int[]minoArray, int createNum)
    {
        int size;
        // Iミノは 4 * 4 の配列なので size が変わる
        if (createNum == (int)eMinoType.I)
        {
            size = Rotation.I_MINO_SIZE;
            minoArray = new int[size];
        }
        else
        {
            size = Rotation.MINO_SIZE;
            minoArray = new int[size];
        }

        switch (createNum)
        {
            case (int)eMinoType.T:
                minoArray[0] = 0;
                minoArray[1] = 1;
                minoArray[2] = 0;
                minoArray[3] = 1;
                minoArray[4] = 1;
                minoArray[5] = 1;
                minoArray[6] = 0;
                minoArray[7] = 0;
                minoArray[8] = 0;
                break;
            case (int)eMinoType.S:
                minoArray[0] = 0;
                minoArray[1] = 1;
                minoArray[2] = 1;
                minoArray[3] = 1;
                minoArray[4] = 1;
                minoArray[5] = 0;
                minoArray[6] = 0;
                minoArray[7] = 0;
                minoArray[8] = 0;
                break;
            case (int)eMinoType.Z:
                minoArray[0] = 1;
                minoArray[1] = 1;
                minoArray[2] = 0;
                minoArray[3] = 0;
                minoArray[4] = 1;
                minoArray[5] = 1;
                minoArray[6] = 0;
                minoArray[7] = 0;
                minoArray[8] = 0;
                break;
            case (int)eMinoType.L:
                minoArray[0] = 0;
                minoArray[1] = 0;
                minoArray[2] = 1;
                minoArray[3] = 1;
                minoArray[4] = 1;
                minoArray[5] = 1;
                minoArray[6] = 0;
                minoArray[7] = 0;
                minoArray[8] = 0;
                break;
            case (int)eMinoType.J:
                minoArray[0] = 1;
                minoArray[1] = 0;
                minoArray[2] = 0;
                minoArray[3] = 1;
                minoArray[4] = 1;
                minoArray[5] = 1;
                minoArray[6] = 0;
                minoArray[7] = 0;
                minoArray[8] = 0;
                break;
            case (int)eMinoType.O:
                minoArray[0] = 1;
                minoArray[1] = 1;
                minoArray[2] = 0;
                minoArray[3] = 1;
                minoArray[4] = 1;
                minoArray[5] = 0;
                minoArray[6] = 0;
                minoArray[7] = 0;
                minoArray[8] = 0;
                break;
            case (int)eMinoType.I:
                minoArray[0] = 0;
                minoArray[1] = 0;
                minoArray[2] = 0;
                minoArray[3] = 0;
                minoArray[4] = 1;
                minoArray[5] = 1;
                minoArray[6] = 1;
                minoArray[7] = 1;
                minoArray[8] = 0;
                minoArray[9] = 0;
                minoArray[10] = 0;
                minoArray[11] = 0;
                minoArray[12] = 0;
                minoArray[13] = 0;
                minoArray[14] = 0;
                minoArray[15] = 0;
                break;
        }
    }
}
