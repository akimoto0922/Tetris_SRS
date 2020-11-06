using UnityEngine;
using Tetris.Mino;

public class WallKickData :MonoBehaviour
{
    // T, S, Z, L, Jミノ
    private static readonly Vector2[,] testWallKickForAnyMinos = new Vector2[8, 5]
    {
        // J, L, S, T, Zミノ
        {// ang0 → ang90
            new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(0, -2), new Vector2 (-1, -2)
        },
        {// ang90 → ang180
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, -1), new Vector2(0, 2), new Vector2(1, 2)
        },
        {// ang180 → ang270
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, -2), new Vector2(1, -2)
        },
        {// ang270 → ang360
            new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(0, 2), new Vector2(-1, 2)
        },
        {// ang360 → ang270
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, -2), new Vector2(1, -2)
        },
        {// ang270 → ang180
            new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(0, 2), new Vector2(-1, 2)
        },
        {// ang180 → ang90
            new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(0, -2), new Vector2(-1, -2)
        },
        {// ang90 → ang0
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, -1), new Vector2(0, 2), new Vector2(1, 2)
        },
    };

    // Iミノ
    private static readonly Vector2[,] testWallKickForIMino = new Vector2[8, 5]
    {
        // Iミノ
        {// ang0 → ang90
            new Vector2(0, 0), new Vector2(-2, 0), new Vector2(1, 0), new Vector2(-2, -1), new Vector2(1, 2)
        },
        {// ang90 → ang180
            new Vector2(0, 0), new Vector2(-1, 0), new Vector2(2, 0), new Vector2(-1, 2), new Vector2(2, -1)
        },
        {// ang180 → ang270
            new Vector2(0, 0), new Vector2(2, 0), new Vector2(-1, 0), new Vector2(2, 1), new Vector2(-1, -2)
        },
        {// ang270 → ang360
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(-2, 0), new Vector2(1, -2), new Vector2(-2, 1)
        },
        {// ang360  → ang270
            new Vector2(0, 0), new Vector2(-1, 0), new Vector2(2, 0), new Vector2(-1, 2), new Vector2(2, -1)
        },
        {// ang270 → ang180
            new Vector2(0, 0), new Vector2(-2, 0), new Vector2(1, 0), new Vector2(-2, -1), new Vector2(1, 2)
        },
        {// ang180 → ang90
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(-2, 0), new Vector2(1, -2), new Vector2(-2, 1)
        },
        {// ang90 → ang0
            new Vector2(0, 0), new Vector2(2, 0), new Vector2(-1, 0), new Vector2(2, 1), new Vector2(-1, -2)
        }
    };

    public static Vector2 GetKickData(MinoData.eMinoType type, MinoData.eRotateInfo info, int testNum)
    {
        int i = (int)info;
        if (type == MinoData.eMinoType.I)
        {
            return testWallKickForIMino[i, testNum];
        }
        else
        {
            return testWallKickForAnyMinos[i, testNum];
        }
    }
}
