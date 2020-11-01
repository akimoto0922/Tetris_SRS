using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Rotation : MonoBehaviour
{   
    int[,] minoArray = new int[12, 22];
    int[,] wallArray = new int[12, 22];
    int[,] initialPosArray = new int[12, 22];
    int[,] array_mino_test = new int[4, 4];

    bool canRotate;
    enum FieldValue : int
    {
        Empty,
        MinoBlock,
        MinoBlock_Axis,
        WallBlock,
    }

    enum MinoAngle : int
    {
        _0,
        _90,
        _180,
        _270
    }

    enum MinoType : int    // ミノは７種類 + Null も含め計8種類
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

    public void RightSpin(ref int[,] field, int currentMino, ref int minoDirection)
    {
        if (currentMino == (int)MinoType.O)
        {
            // O(オー)ミノは回転しないのでテストは行わない。
            return;
        }
        else if (currentMino == (int)MinoType.I) // Iミノ
        {
            switch (minoDirection)
            {
                case 0:
                    IminoTest_0_90(ref field, ref minoDirection);
                    break;

                case 1:
                    IminoTest_90_180(ref field, ref minoDirection);
                    break;

                case 2:
                    IminoTest_180_270(ref field, ref minoDirection);
                    break;

                case 3:
                    IminoTest_270_360(ref field, ref minoDirection);
                    break;
            }
        }
        else　// L,J,S,Z,Tミノ
        {
            switch (minoDirection)
            {
                case 0:
                    Test_0_90(ref field, ref minoDirection);
                    break;

                case 1:
                    Test_90_180(ref field, ref minoDirection);
                    break;

                case 2:
                    Test_180_270(ref field, ref minoDirection);
                    break;

                case 3:
                    Test_270_360(ref field, ref minoDirection);
                    break;
            }
        }
    }

    public void LeftSpin(ref int[,] field, int currentMino, ref int minoDirection)
    {
        if (currentMino == (int)MinoType.O)
        {
            // O(オー)ミノは回転しないのでテストは行わない。
            return;
        }
        else if (currentMino == (int)MinoType.I)  // Iミノ
        {
            switch (minoDirection)
            {
                case 0:
                    IminoTest_360_270(ref field, ref minoDirection);
                    break;

                case 1:
                    IminoTest_90_0(ref field, ref minoDirection);
                    break;

                case 2:
                    IminoTest_180_90(ref field, ref minoDirection);
                    break;

                case 3:
                    IminoTest_270_180(ref field, ref minoDirection);
                    break;
            }
        }
        else // L,J,S,Z,Tミノ
        {
            switch (minoDirection)
            {
                case 0:
                    Test_360_270(ref field, ref minoDirection);
                    break;

                case 1:
                    Test_90_0(ref field, ref minoDirection);
                    
                    break;

                case 2:
                    Test_180_90(ref field, ref minoDirection);
                    break;

                case 3:
                    Test_270_180(ref field, ref minoDirection);
                    break;
            }
        }
    }

    // 右回転のテスト
    void Test_0_90(ref int[,] field, ref int minoDirection)
    {
        // 配列をコピー
        Copy(field, ref minoArray, ref wallArray);

        // 回転させる
        RotateRight(ref minoArray);

        Copy_RespownPos(minoArray, ref initialPosArray);
        // テスト開始
        for (int i = 0; i < 5; i++)
        {
            UnityEngine.Debug.Log(i + 1 + "回目");
            switch (i)
            {
                case 0:

                    break;

                case 1:

                    ForcedMoveMino(ref minoArray, -1, 0);
                    break;

                case 2:

                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, -1, 1);
                    break;

                case 3:

                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 0, -2);
                    break;

                case 4:

                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, -1, -2);
                    break;
            }
            // 配列を比較
            Compare(minoArray, wallArray);

            // 成功なら
            if (canRotate == true)
            {
                minoDirection = 1;
                // fieldのミノを回転させて描画
                Rotate_field(ref field, minoArray);
                // コピーした配列の初期化
                Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
                UnityEngine.Debug.Log("回転成功！");
                return;
            }
        }
        // すべて失敗したら
        Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
        UnityEngine.Debug.Log("回転できない。");
    }
    void Test_90_180(ref int[,] field, ref int minoDirection)
    {
        Copy(field, ref minoArray, ref wallArray);
        RotateRight(ref minoArray);
        Copy_RespownPos(minoArray, ref initialPosArray);

        for (int i = 0; i < 5; i++)
        {
            UnityEngine.Debug.Log(i + 1 + "回目");
            switch (i)
            {
                case 0:

                    break;

                case 1:

                    ForcedMoveMino(ref minoArray, 1, 0);
                    break;

                case 2:

                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 1, -1);
                    break;

                case 3:

                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 0, 2);
                    break;

                case 4:

                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 1, 2);
                    break;
            }
            Compare(minoArray, wallArray);

            if (canRotate == true)
            {
                minoDirection = 2;
                Rotate_field(ref field, minoArray);
                Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
                UnityEngine.Debug.Log("回転成功！");
                return;
            }
        }
        Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
        UnityEngine.Debug.Log("回転できない。");
    }
    void Test_180_270(ref int[,] field, ref int minoDirection)
    {
        Copy(field, ref minoArray, ref wallArray);
        RotateRight(ref minoArray);
        Copy_RespownPos(minoArray, ref initialPosArray);

        for (int i = 0; i < 5; i++)
        {
            UnityEngine.Debug.Log(i + 1 + "回目");
            switch (i)
            {
                case 0:

                    break;

                case 1:
                    ForcedMoveMino(ref minoArray, 1, 0);
                    break;

                case 2:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 1, 1);
                    break;

                case 3:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 0, -2);
                    break;

                case 4:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 1, -2);
                    break;
            }
            Compare(minoArray, wallArray);

            if (canRotate == true)
            {
                minoDirection = 3;
                Rotate_field(ref field, minoArray);
                Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
                UnityEngine.Debug.Log("回転成功！");
                return;
            }
        }
        Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
        UnityEngine.Debug.Log("回転できない。");
    }
    void Test_270_360(ref int[,] field, ref int minoDirection)
    {
        Copy(field, ref minoArray, ref wallArray);
        RotateRight(ref minoArray);
        Copy_RespownPos(minoArray, ref initialPosArray);

        for (int i = 0; i < 5; i++)
        {
            UnityEngine.Debug.Log(i + 1 + "回目");
            switch (i)
            {
                case 0:

                    break;

                case 1:
                    ForcedMoveMino(ref minoArray, -1, 0);
                    break;

                case 2:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, -1, -1);
                    break;

                case 3:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 0, 2);
                    break;

                case 4:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, -1, 2);
                    break;
            }
            Compare(minoArray, wallArray);

            if (canRotate == true)
            {
                minoDirection = 0;
                Rotate_field(ref field, minoArray);
                Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
                UnityEngine.Debug.Log("回転成功！");
                return;
            }
        }
        Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
        UnityEngine.Debug.Log("回転できない。");
    }

    // 左回転のテスト
    void Test_360_270(ref int[,] field, ref int minoDirection)
    {
        Copy(field, ref minoArray, ref wallArray);
        RotateLeft(ref minoArray);
        Copy_RespownPos(minoArray, ref initialPosArray);

        for (int i = 0; i < 5; i++)
        {
            UnityEngine.Debug.Log(i + 1 + "回目");
            switch (i)
            {
                case 0:

                    break;

                case 1:
                    ForcedMoveMino(ref minoArray, 1, 0);
                    break;

                case 2:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 1, 1);
                    break;

                case 3:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 0, -2);
                    break;

                case 4:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 1, -2);
                    break;
            }
            Compare(minoArray, wallArray);

            if (canRotate == true)
            {
                minoDirection = 3;
                Rotate_field(ref field, minoArray);
                Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
                UnityEngine.Debug.Log("回転成功！");
                return;
            }
        }
        Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
        UnityEngine.Debug.Log("回転できない。");
    }
    void Test_270_180(ref int[,] field, ref int minoDirection)
    {
        Copy(field, ref minoArray, ref wallArray);
        RotateLeft(ref minoArray);
        Copy_RespownPos(minoArray, ref initialPosArray);

        for (int i = 0; i < 5; i++)
        {
            UnityEngine.Debug.Log(i + 1 + "回目");
            switch (i)
            {
                case 0:

                    break;

                case 1:
                    ForcedMoveMino(ref minoArray, -1, 0);
                    break;

                case 2:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, -1, -1);
                    break;

                case 3:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 0, 2);
                    break;

                case 4:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, -1, 2);
                    break;
            }
            Compare(minoArray, wallArray);

            if (canRotate == true)
            {
                minoDirection = 2;
                Rotate_field(ref field, minoArray);
                Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
                UnityEngine.Debug.Log("回転成功！");
                return;
            }
        }
        Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
        UnityEngine.Debug.Log("回転できない。");
    }
    void Test_180_90(ref int[,] field, ref int minoDirection)
    {
        Copy(field, ref minoArray, ref wallArray);
        RotateLeft(ref minoArray);
        Copy_RespownPos(minoArray, ref initialPosArray);

        for (int i = 0; i < 5; i++)
        {
            UnityEngine.Debug.Log(i + 1 + "回目");
            switch (i)
            {
                case 0:

                    break;

                case 1:
                    ForcedMoveMino(ref minoArray, -1, 0);
                    break;

                case 2:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, -1, 1);
                    break;

                case 3:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 0, -2);
                    break;

                case 4:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, -1, -2);
                    break;
            }
            Compare(minoArray, wallArray);

            if (canRotate == true)
            {
                minoDirection = 1;
                Rotate_field(ref field, minoArray);
                Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
                UnityEngine.Debug.Log("回転成功！");
                return;
            }
        }
        Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
        UnityEngine.Debug.Log("回転できない。");
    }
    void Test_90_0(ref int[,] field, ref int minoDirection)
    {
        Copy(field, ref minoArray, ref wallArray);
        RotateLeft(ref minoArray);
        Copy_RespownPos(minoArray, ref initialPosArray);

        for (int i = 0; i < 5; i++)
        {
            UnityEngine.Debug.Log(i + 1 + "回目");
            switch (i)
            {
                case 0:

                    break;

                case 1:
                    ForcedMoveMino(ref minoArray, 1, 0);
                    break;

                case 2:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 1, -1);
                    break;

                case 3:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 0, 2);
                    break;

                case 4:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 1, 2);
                    break;
            }
            Compare(minoArray, wallArray);

            if (canRotate == true)
            {
                minoDirection = 0;
                Rotate_field(ref field, minoArray);
                Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
                UnityEngine.Debug.Log("回転成功！");
                return;
            }
        }
        Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
        UnityEngine.Debug.Log("回転できない。");
    }

    // Iミノのテスト 右回転
    void IminoTest_0_90(ref int[,] field, ref int minoDirection)
    {
        Copy(field, ref minoArray, ref wallArray);
        IminoRotate_0_90(ref minoArray);
        Copy_RespownPos(minoArray, ref initialPosArray);

        for (int i = 0; i < 5; i++)
        {
            UnityEngine.Debug.Log(i + 1 + "回目");
            switch (i)
            {
                case 0:

                    break;

                case 1:

                    ForcedMoveMino(ref minoArray, -2, 0);
                    break;

                case 2:

                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 1, 0);
                    break;

                case 3:

                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, -2, -1);
                    break;

                case 4:

                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 1, 2);
                    break;
            }
            Compare(minoArray, wallArray);

            if (canRotate == true)
            {
                minoDirection = 1;
                Rotate_field(ref field, minoArray);
                Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
                UnityEngine.Debug.Log("回転成功！");
                return;
            }
        }
        Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
        UnityEngine.Debug.Log("回転できない。");
    }
    void IminoTest_90_180(ref int[,] field, ref int minoDirection)
    {
        Copy(field, ref minoArray, ref wallArray);
        IminoRotate_90_180(ref minoArray);
        Copy_RespownPos(minoArray, ref initialPosArray);

        for (int i = 0; i < 5; i++)
        {
            UnityEngine.Debug.Log(i + 1 + "回目");
            switch (i)
            {
                case 0:

                    break;

                case 1:

                    ForcedMoveMino(ref minoArray, -1, 0);
                    break;

                case 2:

                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 2, 0);
                    break;

                case 3:

                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, -1, 2);
                    break;

                case 4:

                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 2, -1);
                    break;
            }
            Compare(minoArray, wallArray);

            if (canRotate == true)
            {
                minoDirection = 2;
                Rotate_field(ref field, minoArray);
                Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
                UnityEngine.Debug.Log("回転成功！");
                return;
            }
        }
        Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
        UnityEngine.Debug.Log("回転できない。");
    }
    void IminoTest_180_270(ref int[,] field, ref int minoDirection)
    {
        Copy(field, ref minoArray, ref wallArray);
        IminoRotate_180_270(ref minoArray);
        Copy_RespownPos(minoArray, ref initialPosArray);

        for (int i = 0; i < 5; i++)
        {
            UnityEngine.Debug.Log(i + 1 + "回目");
            switch (i)
            {
                case 0:

                    break;

                case 1:
                    ForcedMoveMino(ref minoArray, 2, 0);
                    break;

                case 2:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, -1, 0);
                    break;

                case 3:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 2, 1);
                    break;

                case 4:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, -1, -2);
                    break;
            }
            Compare(minoArray, wallArray);

            if (canRotate == true)
            {
                minoDirection = 3;
                Rotate_field(ref field, minoArray);
                Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
                UnityEngine.Debug.Log("回転成功！");
                return;
            }
        }
        Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
        UnityEngine.Debug.Log("回転できない。");
    }
    void IminoTest_270_360(ref int[,] field, ref int minoDirection)
    {
        Copy(field, ref minoArray, ref wallArray);
        IminoRotate_270_360(ref minoArray);
        Copy_RespownPos(minoArray, ref initialPosArray);

        for (int i = 0; i < 5; i++)
        {
            UnityEngine.Debug.Log(i + 1 + "回目");
            switch (i)
            {
                case 0:

                    break;

                case 1:
                    ForcedMoveMino(ref minoArray, 1, 0);
                    break;

                case 2:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, -2, 0);
                    break;

                case 3:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 1, -2);
                    break;

                case 4:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, -2, 1);
                    break;
            }
            Compare(minoArray, wallArray);

            if (canRotate == true)
            {
                minoDirection = 0;
                Rotate_field(ref field, minoArray);
                Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
                UnityEngine.Debug.Log("回転成功！");
                return;
            }
        }
        Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
        UnityEngine.Debug.Log("回転できない。");
    }

    // Iミノのテスト 左回転
    void IminoTest_360_270(ref int[,] field, ref int minoDirection)
    {
        Copy(field, ref minoArray, ref wallArray);
        IminoRotate_360_270(ref minoArray);
        Copy_RespownPos(minoArray, ref initialPosArray);

        for (int i = 0; i < 5; i++)
        {
            UnityEngine.Debug.Log(i + 1 + "回目");
            switch (i)
            {
                case 0:

                    break;

                case 1:
                    ForcedMoveMino(ref minoArray, -1, 0);
                    break;

                case 2:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 2, 0);
                    break;

                case 3:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, -1, 2);
                    break;

                case 4:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 2, -1);
                    break;
            }
            Compare(minoArray, wallArray);

            if (canRotate == true)
            {
                minoDirection = 3;
                Rotate_field(ref field, minoArray);
                Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
                UnityEngine.Debug.Log("回転成功！");
                return;
            }
        }
        Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
        UnityEngine.Debug.Log("回転できない。");
    }
    void IminoTest_270_180(ref int[,] field, ref int minoDirection)
    {
        Copy(field, ref minoArray, ref wallArray);
        IminoRotate_270_180(ref minoArray);
        Copy_RespownPos(minoArray, ref initialPosArray);

        for (int i = 0; i < 5; i++)
        {
            UnityEngine.Debug.Log(i + 1 + "回目");
            switch (i)
            {
                case 0:

                    break;

                case 1:
                    ForcedMoveMino(ref minoArray, -2, 0);
                    break;

                case 2:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 1, 0);
                    break;

                case 3:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, -2, -1);
                    break;

                case 4:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 1, 2);
                    break;
            }
            Compare(minoArray, wallArray);

            if (canRotate == true)
            {
                minoDirection = 2;
                Rotate_field(ref field, minoArray);
                Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
                UnityEngine.Debug.Log("回転成功！");
                return;
            }
        }
        Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
        UnityEngine.Debug.Log("回転できない。");
    }
    void IminoTest_180_90(ref int[,] field, ref int minoDirection)
    {
        Copy(field, ref minoArray, ref wallArray);
        IminoRotate_180_90(ref minoArray);
        Copy_RespownPos(minoArray, ref initialPosArray);

        for (int i = 0; i < 5; i++)
        {
            UnityEngine.Debug.Log(i + 1 + "回目");
            switch (i)
            {
                case 0:

                    break;

                case 1:
                    ForcedMoveMino(ref minoArray, 1, 0);
                    break;

                case 2:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, -2, 0);
                    break;

                case 3:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 1, -2);
                    break;

                case 4:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, -2, 1);
                    break;
            }
            Compare(minoArray, wallArray);

            if (canRotate == true)
            {
                minoDirection = 1;
                Rotate_field(ref field, minoArray);
                Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
                UnityEngine.Debug.Log("回転成功！");
                return;
            }
        }
        Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
        UnityEngine.Debug.Log("回転できない。");
    }
    void IminoTest_90_0(ref int[,] field, ref int minoDirection)
    {
        Copy(field, ref minoArray, ref wallArray);
        IminoRotate_90_0(ref minoArray);
        Copy_RespownPos(minoArray, ref initialPosArray);

        for (int i = 0; i < 5; i++)
        {
            UnityEngine.Debug.Log(i + 1 + "回目");
            switch (i)
            {
                case 0:

                    break;

                case 1:
                    ForcedMoveMino(ref minoArray, 2, 0);
                    break;

                case 2:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, -1, 0);
                    break;

                case 3:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, 2, 1);
                    break;

                case 4:
                    Respown(ref minoArray, initialPosArray);
                    ForcedMoveMino(ref minoArray, -1, -2);
                    break;
            }
            Compare(minoArray, wallArray);

            if (canRotate == true)
            {
                minoDirection = 0;
                Rotate_field(ref field, minoArray);
                Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
                UnityEngine.Debug.Log("回転成功！");
                return;
            }
        }
        Initialize_All(ref wallArray, ref minoArray, ref initialPosArray);
        UnityEngine.Debug.Log("回転できない。");
    }



    // テストで使う関数
    // <ミノの強制移動>
    void ForcedMoveMino(ref int[,] minoArray, int minoPos_x, int minoPos_y)
    {
        // ①回転後のミノの座標を仮置きの配列にコピーする
        int[,] copyArray = new int[12, 22];

        for (int i = 0; i < minoArray.GetLength(0); i++)
        {
            for (int j = 0; j < minoArray.GetLength(1); j++)
            {
                if (minoArray[i, j] == (int)FieldValue.MinoBlock || minoArray[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    // 範囲外処理
                    var x = i + minoPos_x;
                    var y = j + minoPos_y;
                    if (x >= minoArray.GetLength(0) || 0 >= x || y >= minoArray.GetLength(1) || 0 >= y)
                    {
                        return;
                    }
                    copyArray[i + minoPos_x, j + minoPos_y] = minoArray[i, j];
                }
            }
        }

        // ②minoArrayを初期化
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {
            for (int j = 0; j < minoArray.GetLength(1); j++)
            {
                if (minoArray[i, j] == (int)FieldValue.MinoBlock || minoArray[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    minoArray[i, j] = (int)FieldValue.Empty;
                }
            }
        }

        // ③仮置きの配列の値をminoArrayに代入する
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {
            for (int j = 0; j < minoArray.GetLength(1); j++)
            {
                if (copyArray[i, j] == (int)FieldValue.MinoBlock || copyArray[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    minoArray[i, j] = copyArray[i, j];
                }
            }
        }

        // ④copyArrayを初期化
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {
            for (int j = 0; j < minoArray.GetLength(1); j++)
            {
                copyArray[i, j]　= 0;
            }
        }
    }

    // <コピー>
    void Copy(int[,] field, ref int[,] minoArray, ref int[,] wallArray)
    {
        // fieldのミノと壁をそれぞれ別の配列にコピーする
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                // ミノ
                if (field[i, j] == (int)FieldValue.MinoBlock || field[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    minoArray[i, j] = field[i, j];
                }

                // 壁
                if (field[i, j] == (int)FieldValue.WallBlock)
                {
                    wallArray[i, j] = field[i, j];
                }
            }
        }
    }

    // <初期位置をコピー>
    void Copy_RespownPos(int[,] minoArray, ref int[,] initialPosArray)
    {
        // fieldのミノと壁をそれぞれ別の配列にコピーする
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {
            for (int j = 0; j < minoArray.GetLength(1); j++)
            {
                // ミノ
                if (minoArray[i, j] == (int)FieldValue.MinoBlock || minoArray[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    initialPosArray[i, j] = minoArray[i, j];
                }
            }
        }
    }

    // <比較>
    void Compare(int[,] minoArray, int[,] wallArray)
    {
        int count = 0;
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {
            for (int j = 0; j < minoArray.GetLength(1); j++)
            {
                if (minoArray[i, j] == (int)FieldValue.MinoBlock_Axis && wallArray[i, j] == (int)FieldValue.Empty)
                {
                    count++;
                    UnityEngine.Debug.Log("count+1 回転軸");
                }
                if (minoArray[i, j] == (int)FieldValue.MinoBlock && wallArray[i, j] == (int)FieldValue.Empty)
                {
                    count++;
                    UnityEngine.Debug.Log("count+1 ほかのブロック");
                }
            }
        }
        if (count >= 4)
        {
            canRotate = true;
        }
        else
        {
            canRotate = false;
        }
    }

    // <初期位置へ移動>
    void Respown(ref int[,] minoArray, int[,] initialPosArray)
    {
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {
            for (int j = 0; j < minoArray.GetLength(1); j++)
            {
                minoArray[i, j] = initialPosArray[i, j];
            }
        }
    }

    // <回転関連>
    // ミノの回転
    void RotateRight(ref int[,] minoArray)
    {
       
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {

            for (int j = 0; j < minoArray.GetLength(1); j++)
            {

                if (minoArray[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    array_mino_test[0, 0] = minoArray[i - 1, j + 1];
                    array_mino_test[0, 1] = minoArray[i, j + 1];
                    array_mino_test[0, 2] = minoArray[i + 1, j + 1];
                    array_mino_test[1, 2] = minoArray[i + 1, j];
                    array_mino_test[2, 2] = minoArray[i + 1, j - 1];
                    array_mino_test[2, 1] = minoArray[i, j - 1];
                    array_mino_test[2, 0] = minoArray[i - 1, j - 1];
                    array_mino_test[1, 0] = minoArray[i - 1, j];

                    minoArray[i + 1, j] = (int)FieldValue.Empty;
                    minoArray[i + 1, j - 1] = (int)FieldValue.Empty;
                    minoArray[i + 1, j + 1] = (int)FieldValue.Empty;
                    minoArray[i, j + 1] = (int)FieldValue.Empty;
                    minoArray[i - 1, j + 1] = (int)FieldValue.Empty;
                    minoArray[i - 1, j] = (int)FieldValue.Empty;
                    minoArray[i - 1, j - 1] = (int)FieldValue.Empty;
                    minoArray[i, j - 1] = (int)FieldValue.Empty;

                    minoArray[i + 1, j + 1] = array_mino_test[0, 0];
                    minoArray[i + 1, j] = array_mino_test[0, 1];
                    minoArray[i + 1, j - 1] = array_mino_test[0, 2];
                    minoArray[i, j - 1] = array_mino_test[1, 2];
                    minoArray[i - 1, j - 1] = array_mino_test[2, 2];
                    minoArray[i - 1, j] = array_mino_test[2, 1];
                    minoArray[i - 1, j + 1] = array_mino_test[2, 0];
                    minoArray[i, j + 1] = array_mino_test[1, 0];
                }
            }
        }

        // 一時的に保存しとくための配列を使用後に初期化する
        for (int i = 0; i < array_mino_test.GetLength(0); i++)
        {
            for (int j = 0; j < array_mino_test.GetLength(1); j++)
            {
                array_mino_test[i, j] = (int)FieldValue.Empty;
            }
        }
    }
    void RotateLeft(ref int[,] minoArray)
    {
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {

            for (int j = 0; j < minoArray.GetLength(1); j++)
            {

                if (minoArray[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    array_mino_test[0, 0] = minoArray[i - 1, j + 1];
                    array_mino_test[0, 1] = minoArray[i, j + 1];
                    array_mino_test[0, 2] = minoArray[i + 1, j + 1];
                    array_mino_test[1, 2] = minoArray[i + 1, j];
                    array_mino_test[2, 2] = minoArray[i + 1, j - 1];
                    array_mino_test[2, 1] = minoArray[i, j - 1];
                    array_mino_test[2, 0] = minoArray[i - 1, j - 1];
                    array_mino_test[1, 0] = minoArray[i - 1, j];

                    minoArray[i + 1, j] = (int)FieldValue.Empty;
                    minoArray[i + 1, j - 1] = (int)FieldValue.Empty;
                    minoArray[i + 1, j + 1] = (int)FieldValue.Empty;
                    minoArray[i, j + 1] = (int)FieldValue.Empty;
                    minoArray[i - 1, j + 1] = (int)FieldValue.Empty;
                    minoArray[i - 1, j] = (int)FieldValue.Empty;
                    minoArray[i - 1, j - 1] = (int)FieldValue.Empty;
                    minoArray[i, j - 1] = (int)FieldValue.Empty;

                    minoArray[i - 1, j - 1] = array_mino_test[0, 0];
                    minoArray[i - 1, j] = array_mino_test[0, 1];
                    minoArray[i - 1, j + 1] = array_mino_test[0, 2];
                    minoArray[i, j + 1] = array_mino_test[1, 2];
                    minoArray[i + 1, j + 1] = array_mino_test[2, 2];
                    minoArray[i + 1, j] = array_mino_test[2, 1];
                    minoArray[i + 1, j - 1] = array_mino_test[2, 0];
                    minoArray[i, j - 1] = array_mino_test[1, 0];
                }
            }
        }

        // 一時的に保存しとくための配列を使用後に初期化する
        for (int i = 0; i < array_mino_test.GetLength(0); i++)
        {
            for (int j = 0; j < array_mino_test.GetLength(1); j++)
            {
                array_mino_test[i, j] = (int)FieldValue.Empty;
            }
        }
    }

    // 'Iミノ'の回転は8パターン(※回転というより、一度消して回転後の値を代入している。)
    // 右回転(4パターン)
    void IminoRotate_0_90(ref int[,] minoArray)
    {
        for (int i = minoArray.GetLength(0) - 1; i > 0; i--)
        {

            for (int j = 0; j < minoArray.GetLength(1); j++)
            {

                if (minoArray[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    minoArray[i, j] = (int)FieldValue.Empty;
                    minoArray[i - 1, j] = (int)FieldValue.Empty;
                    minoArray[i + 1, j] = (int)FieldValue.Empty;
                    minoArray[i + 2, j] = (int)FieldValue.Empty;

                    minoArray[i + 1, j + 1] = (int)FieldValue.MinoBlock;
                    minoArray[i + 1, j] = (int)FieldValue.MinoBlock_Axis;
                    minoArray[i + 1, j - 1] = (int)FieldValue.MinoBlock;
                    minoArray[i + 1, j - 2] = (int)FieldValue.MinoBlock;
                }
            }
        }
    }
    void IminoRotate_90_180(ref int[,] minoArray)
    {
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {

            for (int j = 0; j < minoArray.GetLength(1); j++)
            {

                if (minoArray[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    minoArray[i, j] = (int)FieldValue.Empty;
                    minoArray[i, j + 1] = (int)FieldValue.Empty;
                    minoArray[i, j - 1] = (int)FieldValue.Empty;
                    minoArray[i, j - 2] = (int)FieldValue.Empty;

                    minoArray[i - 2, j - 1] = (int)FieldValue.MinoBlock;
                    minoArray[i - 1, j - 1] = (int)FieldValue.MinoBlock;
                    minoArray[i, j - 1] = (int)FieldValue.MinoBlock_Axis;
                    minoArray[i + 1, j - 1] = (int)FieldValue.MinoBlock;

                }
            }
        }
    }
    void IminoRotate_180_270(ref int[,] minoArray)
    {
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {

            for (int j = 0; j < minoArray.GetLength(1); j++)
            {

                if (minoArray[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    minoArray[i, j] = (int)FieldValue.Empty;
                    minoArray[i - 2, j] = (int)FieldValue.Empty;
                    minoArray[i - 1, j] = (int)FieldValue.Empty;
                    minoArray[i + 1, j] = (int)FieldValue.Empty;

                    minoArray[i - 1, j + 1] = (int)FieldValue.MinoBlock;
                    minoArray[i - 1, j] = (int)FieldValue.MinoBlock_Axis;
                    minoArray[i - 1, j - 1] = (int)FieldValue.MinoBlock;
                    minoArray[i - 1, j + 2] = (int)FieldValue.MinoBlock;
                }
            }
        }
    }
    void IminoRotate_270_360(ref int[,] minoArray)
    {
        for (int i = minoArray.GetLength(0) - 1; i > 0; i--)
        {

            for (int j = minoArray.GetLength(1) - 1; j > 0; j--)
            {

                if (minoArray[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    minoArray[i, j] = (int)FieldValue.Empty;
                    minoArray[i, j + 1] = (int)FieldValue.Empty;
                    minoArray[i, j + 2] = (int)FieldValue.Empty;
                    minoArray[i, j - 1] = (int)FieldValue.Empty;

                    minoArray[i + 2, j + 1] = (int)FieldValue.MinoBlock;
                    minoArray[i + 1, j + 1] = (int)FieldValue.MinoBlock;
                    minoArray[i, j + 1] = (int)FieldValue.MinoBlock_Axis;
                    minoArray[i - 1, j + 1] = (int)FieldValue.MinoBlock;
                }
            }
        }
    }

    // 左回転(4パターン)
    void IminoRotate_360_270(ref int[,] minoArray)
    {
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {

            for (int j = 0; j < minoArray.GetLength(1); j++)
            {

                if (minoArray[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    minoArray[i, j] = (int)FieldValue.Empty;
                    minoArray[i - 1, j] = (int)FieldValue.Empty;
                    minoArray[i + 1, j] = (int)FieldValue.Empty;
                    minoArray[i + 2, j] = (int)FieldValue.Empty;

                    minoArray[i, j + 1] = (int)FieldValue.MinoBlock;
                    minoArray[i, j] = (int)FieldValue.MinoBlock;
                    minoArray[i, j - 1] = (int)FieldValue.MinoBlock_Axis;
                    minoArray[i, j - 2] = (int)FieldValue.MinoBlock;
                }
            }
        }
    }
    void IminoRotate_270_180(ref int[,] minoArray)
    {
        for (int i = minoArray.GetLength(0) - 1; i > 0; i--)
        {

            for (int j = 0; j < minoArray.GetLength(1); j++)
            {

                if (minoArray[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    minoArray[i, j] = (int)FieldValue.Empty;
                    minoArray[i, j + 1] = (int)FieldValue.Empty;
                    minoArray[i, j - 1] = (int)FieldValue.Empty;
                    minoArray[i, j + 2] = (int)FieldValue.Empty;

                    minoArray[i + 2, j] = (int)FieldValue.MinoBlock;
                    minoArray[i - 1, j] = (int)FieldValue.MinoBlock;
                    minoArray[i, j] = (int)FieldValue.MinoBlock;
                    minoArray[i + 1, j] = (int)FieldValue.MinoBlock_Axis;
                }
            }
        }
    }
    void IminoRotate_180_90(ref int[,] minoArray)
    {
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {

            for (int j = minoArray.GetLength(1) - 1; j > 0; j--)
            {

                if (minoArray[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    minoArray[i, j] = (int)FieldValue.Empty;
                    minoArray[i - 2, j] = (int)FieldValue.Empty;
                    minoArray[i - 1, j] = (int)FieldValue.Empty;
                    minoArray[i + 1, j] = (int)FieldValue.Empty;

                    minoArray[i, j + 1] = (int)FieldValue.MinoBlock_Axis;
                    minoArray[i, j] = (int)FieldValue.MinoBlock;
                    minoArray[i, j - 1] = (int)FieldValue.MinoBlock;
                    minoArray[i, j + 2] = (int)FieldValue.MinoBlock;
                }
            }
        }
    }
    void IminoRotate_90_0(ref int[,] minoArray)
    {
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {

            for (int j = minoArray.GetLength(1) - 1; j > 0; j--)
            {

                if (minoArray[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    minoArray[i, j] = (int)FieldValue.Empty;
                    minoArray[i, j + 1] = (int)FieldValue.Empty;
                    minoArray[i, j - 2] = (int)FieldValue.Empty;
                    minoArray[i, j - 1] = (int)FieldValue.Empty;

                    minoArray[i - 2, j] = (int)FieldValue.MinoBlock;
                    minoArray[i + 1, j] = (int)FieldValue.MinoBlock;
                    minoArray[i, j] = (int)FieldValue.MinoBlock;
                    minoArray[i - 1, j] = (int)FieldValue.MinoBlock_Axis;
                }
            }
        }
    }

    void Rotate_field(ref int[,] field, int[,] minoArray) // ここで'field'に値が代入されて画面に反映する
    {
        for (int i = 0; i < field.GetLength(0); i++)
        {

            for (int j = 0; j < field.GetLength(1); j++)
            {

                if (field[i, j] == (int)FieldValue.MinoBlock || field[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    field[i, j] = (int)FieldValue.Empty;
                }
            }
        }

        for (int i = 0; i < minoArray.GetLength(0); i++)
        {

            for (int j = 0; j < minoArray.GetLength(1); j++)
            {

                if (minoArray[i, j] == (int)FieldValue.MinoBlock || minoArray[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    field[i, j] = minoArray[i, j];
                }
            }
        }
    }

    // 利用した配列の初期化
    void Initialize_All(ref int[,] wallArray, ref int[,] minoArray, ref int[,] initialPosArray)
    {
        // テスト用に作った二つの配列を初期化
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {
            for (int j = 0; j < minoArray.GetLength(1); j++)
            {
                wallArray[i, j] = (int)FieldValue.Empty;
                minoArray[i, j] = (int)FieldValue.Empty;
                initialPosArray[i, j] = (int)FieldValue.Empty;
            }
        }
    }

}