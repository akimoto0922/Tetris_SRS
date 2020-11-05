using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tetris.Mino
{

    // ミノのデータを管理するクラス
    public class MinoData
    {
        public enum eMinoType : int    // ミノは７種類 + Null も含め計8種類
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

        public enum eMinoAngle : int
        {
            ang0,
            ang90,
            ang180,
            ang270
        }

        // ミノの辺の長さ
        public const int MINO_SIDE_LENGTH = 3;
        // ミノの配列のサイズ
        public const int MINO_SIZE = MINO_SIDE_LENGTH * MINO_SIDE_LENGTH;

        // Oミノの辺の長さ
        public const int O_MINO_SIDE_LENGTH = 2;
        // Oミノの配列のサイズ
        public const int O_MINO_SIZE = O_MINO_SIDE_LENGTH * O_MINO_SIDE_LENGTH;

        // Iミノの辺の長さ
        public const int I_MINO_SIDE_LENGTH = 4;
        // Iミノの配列のサイズ
        public const int I_MINO_SIZE = I_MINO_SIDE_LENGTH * I_MINO_SIDE_LENGTH;

        // ミノの配列
        public static readonly int[][] MinoArrays = new int[(int)eMinoType.MAX][]
        {
            // T
            new int[MINO_SIZE]
            {
              0,1,0,
              1,1,1,
              0,0,0,
            },
            // S
            new int[MINO_SIZE]
            {
              0,1,1,
              1,1,0,
              0,0,0,
            },
            // Z
            new int[MINO_SIZE]
            {
              1,1,0,
              0,1,1,
              0,0,0,
            },
            // L
            new int[MINO_SIZE]
            {
              0,0,1,
              1,1,1,
              0,0,0,
            },
            // J
            new int[MINO_SIZE]
            {
              1,0,0,
              1,1,1,
              0,0,0,
            },
            // O
            new int[O_MINO_SIZE]
            {
              1,1,
              1,1,
            },
            new int[I_MINO_SIZE]
            {
              0,1,0,0,
              0,1,0,0,
              0,1,0,0,
              0,1,0,0,
            }
        };

        public static void CheckArray(in int[] array, in eMinoType type)
        {
            int side, length;
            if(type == eMinoType.I)
            {
                side = I_MINO_SIDE_LENGTH;
                length = I_MINO_SIZE;
            }
            else if(type == eMinoType.O)
            {
                return;
            }
            else
            {
                side = MINO_SIDE_LENGTH;
                length = MINO_SIZE;
            }

            string str = "";

            for(int i = 0; i < side; ++i)
            {
                if (type == eMinoType.I)
                {
                    int idx = i * side;
                    UnityEngine.Debug.Log(array[idx] + " " + array[idx + 1] + " " + array[idx + 2] + " " + array[i + 3]);
                }
                else
                {
                    int idx = i * side;
                    UnityEngine.Debug.Log(array[idx] + " " + array[idx + 1] + " " + array[idx + 2]);
                }
            }
        }
    }
}
