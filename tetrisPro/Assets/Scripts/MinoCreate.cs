using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinoCreate : MonoBehaviour
{
    enum FieldValue : int
    {
        Empty,
        MinoBlock,
        MinoBlock_Axis,
        WallBlock,

    }

    enum MinoType : int    // ミノは７種類 + Null も含め計８種類
    {
        T,
        S,
        Z,
        L,
        J,
        O,
        I,
        Null,
    }

    public void CreateMino(int[,]field,int createNum)
    {
        switch (createNum)
        {
            case (int)MinoType.T:

                field[4, 19] = (int)FieldValue.MinoBlock;      // □■□
                field[5, 19] = (int)FieldValue.MinoBlock_Axis; // ■■■
                field[5, 20] = (int)FieldValue.MinoBlock;      // □□□
                field[6, 19] = (int)FieldValue.MinoBlock;

                break;

            case (int)MinoType.S:

                field[4, 19] = (int)FieldValue.MinoBlock;        // □■■
                field[5, 19] = (int)FieldValue.MinoBlock_Axis;   // ■■□
                field[5, 20] = (int)FieldValue.MinoBlock;        // □□□
                field[6, 20] = (int)FieldValue.MinoBlock;

                break;

            case (int)MinoType.Z:

                field[4, 20] = (int)FieldValue.MinoBlock;        // ■■□
                field[5, 19] = (int)FieldValue.MinoBlock_Axis;   // □■■
                field[5, 20] = (int)FieldValue.MinoBlock;        // □□□
                field[6, 19] = (int)FieldValue.MinoBlock;

                break;

            case (int)MinoType.L:

                field[4, 19] = (int)FieldValue.MinoBlock;        // □□■
                field[5, 19] = (int)FieldValue.MinoBlock_Axis;   // ■■■
                field[6, 19] = (int)FieldValue.MinoBlock;        // □□□
                field[6, 20] = (int)FieldValue.MinoBlock;

                break;

            case (int)MinoType.J:

                field[4, 19] = (int)FieldValue.MinoBlock;        // ■□□
                field[5, 19] = (int)FieldValue.MinoBlock_Axis;   // ■■■
                field[4, 20] = (int)FieldValue.MinoBlock;        // □□□
                field[6, 19] = (int)FieldValue.MinoBlock;

                break;

            case (int)MinoType.O:

                field[4, 19] = (int)FieldValue.MinoBlock;        // ■■□
                field[5, 19] = (int)FieldValue.MinoBlock;        // ■■□
                field[4, 20] = (int)FieldValue.MinoBlock;        // □□□
                field[5, 20] = (int)FieldValue.MinoBlock;

                break;

            case (int)MinoType.I:

                field[4, 19] = (int)FieldValue.MinoBlock;        // □□□□
                field[5, 19] = (int)FieldValue.MinoBlock_Axis;   // ■■■■
                field[6, 19] = (int)FieldValue.MinoBlock;        // □□□□
                field[7, 19] = (int)FieldValue.MinoBlock;        // □□□□

                break;

        }
    }
}
