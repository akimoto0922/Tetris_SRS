using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public GameObject gamemanager;
    public int minoDirection; // ミノの向き
    int currentMino;          // 現在のミノ
　　int count;                // テストをするときに使う
    int minoPos_x;            // ミノを強制移動させるときに使う
    int minoPos_y;            // ミノを強制移動させるときに使う

    int[,] field = new int[12, 22];
    int[,] fieldArray = new int[12, 22];    // fieldの壁の場所のコピー
    int[,] minoArray = new int[12, 22];     // fieldのミノの場所コピー
    int[,] array_mino_test = new int[4, 4]; // 回転させるときに使う

    void Update()
    {
        minoDirection = gamemanager.GetComponent<GameManager>().minoDirection;
    }
    // テストの数は4種類
    // ＜右回転＞
    // ・J,L,S,Z,Tミノ
    // A→B→C→D
    // ＜左回転＞
    // ・J,L,S,Z,Tミノ
    // C→D→A→B

    public void RightSpin()
    {
        // ①値を参照する
        field = gamemanager.GetComponent<GameManager>().field;
        currentMino = gamemanager.GetComponent<GameManager>().currentMino;
        minoDirection = gamemanager.GetComponent<GameManager>().minoDirection;

        if (currentMino == 5)
        {
            // O(オー)ミノは回転しないのでテストは行わない。
        }
        else if (currentMino == 6) // Iミノ
        {
            switch (minoDirection)
            {
                case 0:
                    IminoTest_270_360();
                    break;

                case 1:
                    IminoTest_0_90();
                    break;

                case 2:
                    IminoTest_90_180();
                    break;

                case 3:
                    IminoTest_180_270();
                    break;
            }
        }
        else　// L,J,S,Z,Tミノ
        {
            switch (minoDirection)
            {
                case 0:
                    Test_270_360();
                    break;

                case 1:
                    Test_0_90();
                    break;

                case 2:
                    Test_90_180();
                    break;

                case 3:
                    Test_180_270();
                    break;
            }
        }
    }

    public void LeftSpin()
    {
        field = gamemanager.GetComponent<GameManager>().field;
        currentMino = gamemanager.GetComponent<GameManager>().currentMino;
        minoDirection = gamemanager.GetComponent<GameManager>().minoDirection;

        if (currentMino == 5)
        {
            // Oミノは回転しないのでテストは行わない。
        }
        else if (currentMino == 6)  // Iミノ
        {
            switch (minoDirection)
            {
                case 0:
                    IminoTest_90_0();
                    break;

                case 1:
                    IminoTest_180_90();
                    break;

                case 2:
                    IminoTest_270_180();
                    break;

                case 3:
                    IminoTest_360_270();
                    break;
            }
        }
        else // L,J,S,Z,Tミノ
        {
            switch (minoDirection)
            {
                case 0:
                    Test_90_0();
                    break;

                case 1:
                    Test_180_90();
                    break;

                case 2:
                    Test_270_180();
                    break;

                case 3:
                    Test_360_270();
                    break;
            }
        }
    }

    //　＜テストの手順＞
    // ①現在のfieldのミノの位置と壁の位置をそれぞれ別の配列にコピー。(minoArray,fieldArray)
    // ②minoArrayでミノを回転させる
    // ③minoArrayとfieldArrayを比較し、ミノの位置がfieldArrayですべて空白のブロックなら成功。
    // ④失敗したら、ミノの位置を強制的に動かしもう一度比較する、これを5回繰り返しすべてに失敗したら回転はできない。
    //
    // wiki参考 https://tetris.wiki/Super_Rotation_System

    // 右回転のテスト
    void Test_0_90()
    {
        // 配列をコピー
        Copy();

        // 回転させる
        RotateRight_minoArray();

        // 配列を比較
        Compare();

        // 成功なら
        if (count == 4) // ←この4はテトリミノが4つのブロックからできているから。
        {
            // fieldのミノを回転させて描画
            Rotate_field();
            count = 0;
            // コピーした配列の初期化
            Initialize_All();
            Debug.Log("回転成功！");
        }
        else
        {
            Debug.Log("回転失敗；；　次のテストへ");
            // 2回目
            count = 0;
            // ミノの位置を指定し(今回は(-1,0))強制的に移動
            MoveLeft_minoArray();
            // 改めて比較する
            Compare();

            if (count == 4)
            {
                Rotate_field();
                count = 0;
                Initialize_All();
                Debug.Log("左に1列ずらしたら回転成功！");
            }
            else
            {
                Debug.Log("回転失敗；；　次のテストへ");
                // 3回目
                count = 0;
                MoveUp_minoArray();
                Compare();

                if (count == 4)
                {
                    Rotate_field();
                    count = 0;
                    Initialize_All();
                    Debug.Log("左に1列ずらし、上に1列ずらしたら回転成功！");
                }
                else
                {
                    Debug.Log("回転失敗；；　次のテストへ");
                    // 4回目
                    count = 0;
                    MoveRight_minoArray();
                    MoveDown_minoArray();
                    MoveDown_minoArray();
                    MoveDown_minoArray();
                    Compare();

                    if (count == 4)
                    {
                        Rotate_field();
                        count = 0;
                        Initialize_All();
                        Debug.Log("下に2列ずらしたら回転成功！");
                    }
                    else
                    {
                        Debug.Log("回転失敗；；　次のテストへ");
                        // 5回目
                        count = 0;
                        MoveLeft_minoArray();
                        Compare();

                        if (count == 4)
                        {
                            Rotate_field();
                            count = 0;
                            Initialize_All();
                            Debug.Log("左に1列ずらし、下に2列ずらしたら回転成功！");
                        }
                        else
                        {
                            Debug.Log("回転できない；；");
                            minoDirection = 0;
                            gamemanager.GetComponent<GameManager>().minoDirection = minoDirection;
                            count = 0;
                            Initialize_All();
                        }
                    }
                }
            }
        }
    }
    void Test_90_180()
    {
        Copy();
        RotateRight_minoArray();
        Compare();

        if (count == 4)
        {
            Rotate_field();
            count = 0;
            Initialize_All();
            Debug.Log("回転成功！");
        }
        else
        {
            Debug.Log("回転失敗；；　次のテストへ");
            count = 0;
            MoveRight_minoArray();
            Compare();

            if (count == 4)
            {
                Rotate_field();
                count = 0;
                Initialize_All();
                Debug.Log("右に1列ずらしたら回転成功！");
            }
            else
            {
                Debug.Log("回転失敗；；　次のテストへ");
                count = 0;
                MoveDown_minoArray();
                Compare();

                if (count == 4)
                {
                    Rotate_field();
                    count = 0;
                    Initialize_All();
                    Debug.Log("右に1列ずらし、下に1列ずらしたら回転成功！");
                }
                else
                {
                    Debug.Log("回転失敗；；　次のテストへ");
                    count = 0;
                    MoveLeft_minoArray();
                    MoveUp_minoArray();
                    MoveUp_minoArray();
                    MoveUp_minoArray();
                    Compare();

                    if (count == 4)
                    {
                        Rotate_field();
                        count = 0;
                        Initialize_All();
                        Debug.Log("上に2列ずらしたら回転成功！");
                    }
                    else
                    {
                        Debug.Log("回転失敗；；　次のテストへ");
                        count = 0;
                        MoveRight_minoArray();
                        Compare();

                        if (count == 4)
                        {
                            Rotate_field();
                            count = 0;
                            Initialize_All();
                            Debug.Log("右に1列ずらし、上に2列ずらしたら回転成功！");
                        }
                        else
                        {
                            minoDirection --;
                            gamemanager.GetComponent<GameManager>().minoDirection = minoDirection;

                            Debug.Log("回転できない；；");
                            count = 0;
                            Initialize_All();
                        }
                    }
                }
            }
        }
    }
    void Test_180_270()
    {
        Copy();
        RotateRight_minoArray();
        Compare();

        if (count == 4)
        {
            Rotate_field();
            count = 0;
            Initialize_All();
            Debug.Log("回転成功！");
        }
        else
        {
            Debug.Log("回転失敗；；　次のテストへ");
            count = 0;
            MoveRight_minoArray();
            Compare();

            if (count == 4)
            {
                Rotate_field();
                count = 0;
                Initialize_All();
                Debug.Log("右に1列ずらしたら回転成功！");
            }
            else
            {
                Debug.Log("回転失敗；；　次のテストへ");
                count = 0;
                MoveUp_minoArray();
                Compare();

                if (count == 4)
                {
                    Rotate_field();
                    count = 0;
                    Initialize_All();
                    Debug.Log("右に1列ずらし、下に1列ずらしたら回転成功！");
                }
                else
                {
                    Debug.Log("回転失敗；；　次のテストへ");
                    count = 0;
                    MoveLeft_minoArray();
                    MoveDown_minoArray();
                    MoveDown_minoArray();
                    MoveDown_minoArray();
                    Compare();

                    if (count == 4)
                    {
                        Rotate_field();
                        count = 0;
                        Initialize_All();
                        Debug.Log("上に2列ずらしたら回転成功！");
                    }
                    else
                    {
                        Debug.Log("回転失敗；；　次のテストへ");
                        count = 0;
                        MoveRight_minoArray();
                        Compare();

                        if (count == 4)
                        {
                            Rotate_field();
                            count = 0;
                            Initialize_All();
                            Debug.Log("右に1列ずらし、上に2列ずらしたら回転成功！");
                        }
                        else
                        {
                            Debug.Log("回転できない；；");
                            minoDirection = 2;
                            gamemanager.GetComponent<GameManager>().minoDirection = minoDirection;
                            count = 0;
                            Initialize_All();
                        }
                    }
                }
            }
        }
    }
    void Test_270_360()
    {
        Copy();
        RotateRight_minoArray();
        Compare();

        if (count == 4)
        {
            Rotate_field();
            count = 0;
            Initialize_All();
            Debug.Log("回転成功！");
        }
        else
        {
            Debug.Log("回転失敗；；　次のテストへ");
            count = 0;
            MoveLeft_minoArray();
            Compare();

            if (count == 4)
            {
                Rotate_field();
                count = 0;
                Initialize_All();
                Debug.Log("左に1列ずらしたら回転成功！");
            }
            else
            {
                Debug.Log("回転失敗；；　次のテストへ");
                count = 0;
                MoveDown_minoArray();
                Compare();

                if (count == 4)
                {
                    Rotate_field();
                    count = 0;
                    Initialize_All();
                    Debug.Log("左に1列ずらし、上に1列ずらしたら回転成功！");
                }
                else
                {
                    Debug.Log("回転失敗；；　次のテストへ");
                    count = 0;
                    MoveRight_minoArray();
                    MoveUp_minoArray();
                    MoveUp_minoArray();
                    MoveUp_minoArray();
                    Compare();

                    if (count == 4)
                    {
                        Rotate_field();
                        count = 0;
                        Initialize_All();
                        Debug.Log("下に2列ずらしたら回転成功！");
                    }
                    else
                    {
                        Debug.Log("回転失敗；；　次のテストへ");
                        count = 0;
                        MoveLeft_minoArray();
                        Compare();

                        if (count == 4)
                        {
                            Rotate_field();
                            count = 0;
                            Initialize_All();
                            Debug.Log("左に1列ずらし、下に2列ずらしたら回転成功！");
                        }
                        else
                        {
                            Debug.Log("回転できない；；");
                            minoDirection  = 3;
                            gamemanager.GetComponent<GameManager>().minoDirection = minoDirection;
                            count = 0;
                            Initialize_All();
                        }
                    }
                }
            }
        }
    }

    // 左回転のテスト
    void Test_360_270()
    {
        Copy();
        RotateLeft_minoArray();
        Compare();

        if (count == 4)
        {
            Rotate_field();
            count = 0;
            Initialize_All();
            Debug.Log("回転成功！");
        }
        else
        {
            Debug.Log("回転失敗；；　次のテストへ");
            count = 0;
            MoveRight_minoArray();
            Compare();

            if (count == 4)
            {
                Rotate_field();
                count = 0;
                Initialize_All();
                Debug.Log("右に1列ずらしたら回転成功！");
            }
            else
            {
                Debug.Log("回転失敗；；　次のテストへ");
                count = 0;
                MoveUp_minoArray();
                Compare();

                if (count == 4)
                {
                    Rotate_field();
                    count = 0;
                    Initialize_All();
                    Debug.Log("右に1列ずらし、下に1列ずらしたら回転成功！");
                }
                else
                {
                    Debug.Log("回転失敗；；　次のテストへ");
                    count = 0;
                    MoveLeft_minoArray();
                    MoveDown_minoArray();
                    MoveDown_minoArray();
                    MoveDown_minoArray();
                    Compare();

                    if (count == 4)
                    {
                        Rotate_field();
                        count = 0;
                        Initialize_All();
                        Debug.Log("上に2列ずらしたら回転成功！");
                    }
                    else
                    {
                        Debug.Log("回転失敗；；　次のテストへ");
                        count = 0;
                        MoveRight_minoArray();
                        Compare();

                        if (count == 4)
                        {
                            Rotate_field();
                            count = 0;
                            Initialize_All();
                            Debug.Log("右に1列ずらし、上に2列ずらしたら回転成功！");
                        }
                        else
                        {
                            Debug.Log("回転できない；；");
                            minoDirection = 0;
                            gamemanager.GetComponent<GameManager>().minoDirection = minoDirection;
                            count = 0;
                            Initialize_All();
                        }
                    }
                }
            }
        }
    }
    void Test_270_180()
    {
        Copy();
        RotateLeft_minoArray();
        Compare();

        if (count == 4)
        {
            Rotate_field();
            count = 0;
            Initialize_All();
            Debug.Log("回転成功！");
        }
        else
        {
            Debug.Log("回転失敗；；　次のテストへ");
            count = 0;
            MoveLeft_minoArray();
            Compare();

            if (count == 4)
            {
                Rotate_field();
                count = 0;
                Initialize_All();
                Debug.Log("左に1列ずらしたら回転成功！");
            }
            else
            {
                Debug.Log("回転失敗；；　次のテストへ");
                count = 0;
                MoveDown_minoArray();
                Compare();

                if (count == 4)
                {
                    Rotate_field();
                    count = 0;
                    Initialize_All();
                    Debug.Log("左に1列ずらし、上に1列ずらしたら回転成功！");
                }
                else
                {
                    Debug.Log("回転失敗；；　次のテストへ");
                    count = 0;
                    MoveRight_minoArray();
                    MoveUp_minoArray();
                    MoveUp_minoArray();
                    MoveUp_minoArray();
                    Compare();

                    if (count == 4)
                    {
                        Rotate_field();
                        count = 0;
                        Initialize_All();
                        Debug.Log("下に2列ずらしたら回転成功！");
                    }
                    else
                    {
                        Debug.Log("回転失敗；；　次のテストへ");
                        count = 0;
                        MoveLeft_minoArray();
                        Compare();

                        if (count == 4)
                        {
                            Rotate_field();
                            count = 0;
                            Initialize_All();
                            Debug.Log("左に1列ずらし、下に2列ずらしたら回転成功！");
                        }
                        else
                        {
                            Debug.Log("回転できない；；");
                            minoDirection = 3;
                            gamemanager.GetComponent<GameManager>().minoDirection = minoDirection;
                            count = 0;
                            Initialize_All();
                        }
                    }
                }
            }
        }
    }
    void Test_180_90()
    {
        Copy();
        RotateLeft_minoArray();
        Compare();

        if (count == 4)
        {
            Rotate_field();
            count = 0;
            Initialize_All();
            Debug.Log("回転成功！");
        }
        else
        {
            Debug.Log("回転失敗；；　次のテストへ");
            count = 0;
            MoveLeft_minoArray();
            Compare();

            if (count == 4)
            {
                Rotate_field();
                count = 0;
                Initialize_All();
                Debug.Log("左に1列ずらしたら回転成功！");
            }
            else
            {
                Debug.Log("回転失敗；；　次のテストへ");
                count = 0;
                MoveUp_minoArray();
                Compare();

                if (count == 4)
                {
                    Rotate_field();
                    count = 0;
                    Initialize_All();
                    Debug.Log("左に1列ずらし、上に1列ずらしたら回転成功！");
                }
                else
                {
                    Debug.Log("回転失敗；；　次のテストへ");
                    count = 0;
                    MoveRight_minoArray();
                    MoveDown_minoArray();
                    MoveDown_minoArray();
                    MoveDown_minoArray();
                    Compare();

                    if (count == 4)
                    {
                        Rotate_field();
                        count = 0;
                        Initialize_All();
                        Debug.Log("下に2列ずらしたら回転成功！");
                    }
                    else
                    {
                        Debug.Log("回転失敗；；　次のテストへ");
                        count = 0;
                        MoveLeft_minoArray();
                        Compare();

                        if (count == 4)
                        {
                            Rotate_field();
                            count = 0;
                            Initialize_All();
                            Debug.Log("左に1列ずらし、下に2列ずらしたら回転成功！");
                        }
                        else
                        {
                            Debug.Log("回転できない；；");
                            minoDirection = 2;
                            gamemanager.GetComponent<GameManager>().minoDirection = minoDirection;
                            count = 0;
                            Initialize_All();
                        }
                    }
                }
            }
        }
    }
    void Test_90_0()
    {
        Copy();
        RotateLeft_minoArray();
        Compare();

        if (count == 4)
        {
            Rotate_field();
            count = 0;
            Initialize_All();
            Debug.Log("回転成功！");
        }
        else
        {
            Debug.Log("回転失敗；；　次のテストへ");
            count = 0;
            MoveRight_minoArray();
            Compare();

            if (count == 4)
            {
                Rotate_field();
                count = 0;
                Initialize_All();
                Debug.Log("右に1列ずらしたら回転成功！");
            }
            else
            {
                Debug.Log("回転失敗；；　次のテストへ");
                count = 0;
                MoveDown_minoArray();
                Compare();

                if (count == 4)
                {
                    //MoveMino_fieldArray_2();
                    Rotate_field();
                    count = 0;
                    Initialize_All();
                    Debug.Log("右に1列ずらし、下に1列ずらしたら回転成功！");
                }
                else
                {
                    Debug.Log("回転失敗；；　次のテストへ");
                    // 4回目
                    count = 0;
                    MoveLeft_minoArray();
                    MoveUp_minoArray();
                    MoveUp_minoArray();
                    MoveUp_minoArray();
                    Compare();

                    if (count == 4)
                    {
                        Rotate_field();
                        count = 0;
                        Initialize_All();
                        Debug.Log("上に2列ずらしたら回転成功！");
                    }
                    else
                    {
                        Debug.Log("回転失敗；；　次のテストへ");
                        // 5回目
                        count = 0;
                        MoveRight_minoArray();
                        Compare();

                        if (count == 4)
                        {
                            Rotate_field();
                            count = 0;
                            Initialize_All();
                            Debug.Log("右に1列ずらし、上に2列ずらしたら回転成功！");
                        }
                        else
                        {
                            Debug.Log("回転できない；；");
                            minoDirection = 1;
                            gamemanager.GetComponent<GameManager>().minoDirection = minoDirection;
                            count = 0;
                            Initialize_All();
                        }
                    }
                }
            }
        }
    }

    // Iミノのテスト 右回転
    void IminoTest_0_90()
    {
        Copy();
        IminoRotate_0_90_minoArray();
        Compare();

        if (count == 4)
        {
            Rotate_field();
            count = 0;
            Initialize_All();
            Debug.Log("回転成功！");
        }
        else
        {
            Debug.Log("回転失敗；；　次のテストへ");
            count = 0;
            MoveLeft_minoArray();
            MoveLeft_minoArray();
            Compare();

            if (count == 4)
            {
                Rotate_field();
                count = 0;
                Initialize_All();
                Debug.Log("左に2列ずらしたら回転成功！");
            }
            else
            {
                Debug.Log("回転失敗；；　次のテストへ");
                count = 0;
                MoveRight_minoArray();
                MoveRight_minoArray();
                MoveRight_minoArray();
                Compare();

                if (count == 4)
                {
                    Rotate_field();
                    count = 0;
                    Initialize_All();
                    Debug.Log("右に1列ずらしたら回転成功！");
                }
                else
                {
                    Debug.Log("回転失敗；；　次のテストへ");
                    count = 0;
                    MoveUp_minoArray();
                    MoveUp_minoArray();
                    Compare();

                    if (count == 4)
                    {
                        Rotate_field();
                        count = 0;
                        Initialize_All();
                        Debug.Log("右に1列ずらし,上に2列ずらしたら回転成功！");
                    }
                    else
                    {
                        Debug.Log("回転失敗；；　次のテストへ");
                        count = 0;
                        MoveLeft_minoArray();
                        MoveLeft_minoArray();
                        MoveLeft_minoArray();
                        MoveDown_minoArray();
                        MoveDown_minoArray();
                        MoveDown_minoArray();
                        Compare();

                        if (count == 4)
                        {
                            Rotate_field();
                            count = 0;
                            Initialize_All();
                            Debug.Log("左に2列ずらし、下に1列ずらしたら回転成功！");
                        }
                        else
                        {
                            minoDirection--;
                            gamemanager.GetComponent<GameManager>().minoDirection = minoDirection;

                            Debug.Log("回転できない；；");
                            count = 0;
                            Initialize_All();
                        }
                    }
                }
            }
        }
    }
    void IminoTest_90_180()
    {
        Copy();
        IminoRotate_90_180_minoArray();
        Compare();

        if (count == 4)
        {
            Rotate_field();
            count = 0;
            Initialize_All();
            Debug.Log("回転成功！");
        }
        else
        {
            Debug.Log("回転失敗；；　次のテストへ");
            count = 0;
            MoveLeft_minoArray();
            Compare();

            if (count == 4)
            {
                Rotate_field();
                count = 0;
                Initialize_All();
                Debug.Log("右に1列ずらしたら回転成功！");
            }
            else
            {
                Debug.Log("回転失敗；；　次のテストへ");
                count = 0;
                MoveRight_minoArray();
                MoveRight_minoArray();
                MoveRight_minoArray();
                Compare();

                if (count == 4)
                {
                    Rotate_field();
                    count = 0;
                    Initialize_All();
                    Debug.Log("右に1列ずらし、下に1列ずらしたら回転成功！");
                }
                else
                {
                    Debug.Log("回転失敗；；　次のテストへ");
                    count = 0;
                    MoveLeft_minoArray();
                    MoveLeft_minoArray();
                    MoveLeft_minoArray();
                    MoveUp_minoArray();
                    MoveUp_minoArray();
                    Compare();

                    if (count == 4)
                    {
                        Rotate_field();
                        count = 0;
                        Initialize_All();
                        Debug.Log("上に2列ずらしたら回転成功！");
                    }
                    else
                    {
                        Debug.Log("回転失敗；；　次のテストへ");
                        count = 0;
                        MoveRight_minoArray();
                        MoveRight_minoArray();
                        MoveRight_minoArray();
                        MoveDown_minoArray();
                        MoveDown_minoArray();
                        MoveDown_minoArray();
                        Compare();

                        if (count == 4)
                        {
                            Rotate_field();
                            count = 0;
                            Initialize_All();
                            Debug.Log("右に1列ずらし、上に2列ずらしたら回転成功！");
                        }
                        else
                        {
                            minoDirection--;
                            gamemanager.GetComponent<GameManager>().minoDirection = minoDirection;

                            Debug.Log("回転できない；；");
                            count = 0;
                            Initialize_All();
                        }
                    }
                }
            }
        }
    }
    void IminoTest_180_270()
    {
        Copy();
        IminoRotate_180_270_minoArray();
        Compare();

        if (count == 4)
        {
            Rotate_field();
            count = 0;
            Initialize_All();
            Debug.Log("回転成功！");
        }
        else
        {
            Debug.Log("回転失敗；；　次のテストへ");
            count = 0;
            MoveRight_minoArray();
            MoveRight_minoArray();
            Compare();

            if (count == 4)
            {
                Rotate_field();
                count = 0;
                Initialize_All();
                Debug.Log("右に2列ずらしたら回転成功！");
            }
            else
            {
                Debug.Log("回転失敗；；　次のテストへ");
                count = 0;
                MoveLeft_minoArray();
                MoveLeft_minoArray();
                MoveLeft_minoArray();
                Compare();

                if (count == 4)
                {
                    Rotate_field();
                    count = 0;
                    Initialize_All();
                    Debug.Log("左に1列ずらしたら回転成功！");
                }
                else
                {
                    Debug.Log("回転失敗；；　次のテストへ");
                    count = 0;
                    MoveRight_minoArray();
                    MoveRight_minoArray();
                    MoveRight_minoArray();
                    MoveUp_minoArray();
                    Compare();

                    if (count == 4)
                    {
                        Rotate_field();
                        count = 0;
                        Initialize_All();
                        Debug.Log("右に2列ずらし、上に1列ずらしたら回転成功！");
                    }
                    else
                    {
                        Debug.Log("回転失敗；；　次のテストへ");
                        count = 0;
                        MoveLeft_minoArray();
                        MoveLeft_minoArray();
                        MoveLeft_minoArray();
                        MoveDown_minoArray();
                        MoveDown_minoArray();
                        MoveDown_minoArray();
                        Compare();

                        if (count == 4)
                        {
                            Rotate_field();
                            count = 0;
                            Initialize_All();
                            Debug.Log("左に1列ずらし、下に2列ずらしたら回転成功！");
                        }
                        else
                        {
                            minoDirection--;
                            gamemanager.GetComponent<GameManager>().minoDirection = minoDirection;

                            Debug.Log("回転できない；；");
                            count = 0;
                            Initialize_All();
                        }
                    }
                }
            }
        }
    }
    void IminoTest_270_360()
    {
        Copy();
        IminoRotate_270_360_minoArray();
        Compare();

        if (count == 4)
        {
            Rotate_field();
            count = 0;
            Initialize_All();
            Debug.Log("回転成功！");
        }
        else
        {
            Debug.Log("回転失敗；；　次のテストへ");
            count = 0;
            MoveRight_minoArray();
            Compare();

            if (count == 4)
            {
                Rotate_field();
                count = 0;
                Initialize_All();
                Debug.Log("右に1列ずらしたら回転成功！");
            }
            else
            {
                Debug.Log("回転失敗；；　次のテストへ");
                count = 0;
                MoveLeft_minoArray();
                MoveLeft_minoArray();
                MoveLeft_minoArray();
                Compare();

                if (count == 4)
                {
                    Rotate_field();
                    count = 0;
                    Initialize_All();
                    Debug.Log("左に2列ずらしたら回転成功！");
                }
                else
                {
                    Debug.Log("回転失敗；；　次のテストへ");
                    count = 0;
                    MoveRight_minoArray();
                    MoveRight_minoArray();
                    MoveRight_minoArray();
                    MoveDown_minoArray();
                    MoveDown_minoArray();
                    Compare();

                    if (count == 4)
                    {
                        Rotate_field();
                        count = 0;
                        Initialize_All();
                        Debug.Log("右に1列ずらし、下に2列ずらしたら回転成功！");
                    }
                    else
                    {
                        Debug.Log("回転失敗；；　次のテストへ");
                        count = 0;
                        MoveLeft_minoArray();
                        MoveLeft_minoArray();
                        MoveLeft_minoArray();
                        MoveUp_minoArray();
                        MoveUp_minoArray();
                        MoveUp_minoArray();
                        Compare();

                        if (count == 4)
                        {
                            Rotate_field();
                            count = 0;
                            Initialize_All();
                            Debug.Log("左に2列ずらし、上に1列ずらしたら回転成功！");
                        }
                        else
                        {
                            minoDirection--;
                            gamemanager.GetComponent<GameManager>().minoDirection = minoDirection;

                            Debug.Log("回転できない；；");
                            count = 0;
                            Initialize_All();
                        }
                    }
                }
            }
        }
    }

    // Iミノのテスト 左回転
    void IminoTest_360_270()
    {
        Copy();
        IminoRotate_360_270_minoArray();
        Compare();

        if (count == 4)
        {
            Rotate_field();
            count = 0;
            Initialize_All();
            Debug.Log("回転成功！");
        }
        else
        {
            Debug.Log("回転失敗；；　次のテストへ");
            count = 0;
            MoveLeft_minoArray();
            Compare();

            if (count == 4)
            {
                Rotate_field();
                count = 0;
                Initialize_All();
                Debug.Log("右に1列ずらしたら回転成功！");
            }
            else
            {
                Debug.Log("回転失敗；；　次のテストへ");
                count = 0;
                MoveRight_minoArray();
                MoveRight_minoArray();
                MoveRight_minoArray();
                Compare();

                if (count == 4)
                {
                    Rotate_field();
                    count = 0;
                    Initialize_All();
                    Debug.Log("右に1列ずらし、下に1列ずらしたら回転成功！");
                }
                else
                {
                    Debug.Log("回転失敗；；　次のテストへ");
                    count = 0;
                    MoveLeft_minoArray();
                    MoveLeft_minoArray();
                    MoveLeft_minoArray();
                    MoveUp_minoArray();
                    MoveUp_minoArray();
                    Compare();

                    if (count == 4)
                    {
                        Rotate_field();
                        count = 0;
                        Initialize_All();
                        Debug.Log("上に2列ずらしたら回転成功！");
                    }
                    else
                    {
                        Debug.Log("回転失敗；；　次のテストへ");
                        count = 0;
                        MoveRight_minoArray();
                        MoveRight_minoArray();
                        MoveRight_minoArray();
                        MoveDown_minoArray();
                        MoveDown_minoArray();
                        MoveDown_minoArray();
                        Compare();

                        if (count == 4)
                        {
                            Rotate_field();
                            count = 0;
                            Initialize_All();
                            Debug.Log("右に1列ずらし、上に2列ずらしたら回転成功！");
                        }
                        else
                        {
                            minoDirection--;
                            gamemanager.GetComponent<GameManager>().minoDirection = minoDirection;

                            Debug.Log("回転できない；；");
                            count = 0;
                            Initialize_All();
                        }
                    }
                }
            }
        }
    }
    void IminoTest_270_180()
    {
        Copy();
        IminoRotate_270_180_minoArray();
        Compare();

        if (count == 4)
        {
            Rotate_field();
            count = 0;
            Initialize_All();
            Debug.Log("回転成功！");
        }
        else
        {
            Debug.Log("回転失敗；；　次のテストへ");
            count = 0;
            MoveLeft_minoArray();
            MoveLeft_minoArray();
            Compare();

            if (count == 4)
            {
                Rotate_field();
                count = 0;
                Initialize_All();
                Debug.Log("左に2列ずらしたら回転成功！");
            }
            else
            {
                Debug.Log("回転失敗；；　次のテストへ");
                count = 0;
                MoveRight_minoArray();
                MoveRight_minoArray();
                MoveRight_minoArray();
                Compare();

                if (count == 4)
                {
                    Rotate_field();
                    count = 0;
                    Initialize_All();
                    Debug.Log("右に1列ずらしたら回転成功！");
                }
                else
                {
                    Debug.Log("回転失敗；；　次のテストへ");
                    count = 0;
                    MoveUp_minoArray();
                    MoveUp_minoArray();
                    Compare();

                    if (count == 4)
                    {
                        Rotate_field();
                        count = 0;
                        Initialize_All();
                        Debug.Log("右に1列ずらし,上に2列ずらしたら回転成功！");
                    }
                    else
                    {
                        Debug.Log("回転失敗；；　次のテストへ");
                        count = 0;
                        MoveLeft_minoArray();
                        MoveLeft_minoArray();
                        MoveLeft_minoArray();
                        MoveDown_minoArray();
                        MoveDown_minoArray();
                        MoveDown_minoArray();
                        Compare();

                        if (count == 4)
                        {
                            Rotate_field();
                            count = 0;
                            Initialize_All();
                            Debug.Log("左に2列ずらし、下に1列ずらしたら回転成功！");
                        }
                        else
                        {
                            minoDirection--;
                            gamemanager.GetComponent<GameManager>().minoDirection = minoDirection;

                            Debug.Log("回転できない；；");
                            count = 0;
                            Initialize_All();
                        }
                    }
                }
            }
        }
    }
    void IminoTest_180_90()
    {
        Copy();
        IminoRotate_180_90_minoArray();
        Compare();

        if (count == 4)
        {
            Rotate_field();
            count = 0;
            Initialize_All();
            Debug.Log("回転成功！");
        }
        else
        {
            Debug.Log("回転失敗；；　次のテストへ");
            count = 0;
            MoveRight_minoArray();
            Compare();

            if (count == 4)
            {
                Rotate_field();
                count = 0;
                Initialize_All();
                Debug.Log("右に1列ずらしたら回転成功！");
            }
            else
            {
                Debug.Log("回転失敗；；　次のテストへ");
                count = 0;
                MoveLeft_minoArray();
                MoveLeft_minoArray();
                MoveLeft_minoArray();
                Compare();

                if (count == 4)
                {
                    Rotate_field();
                    count = 0;
                    Initialize_All();
                    Debug.Log("左に2列ずらしたら回転成功！");
                }
                else
                {
                    Debug.Log("回転失敗；；　次のテストへ");
                    count = 0;
                    MoveRight_minoArray();
                    MoveRight_minoArray();
                    MoveRight_minoArray();
                    MoveDown_minoArray();
                    MoveDown_minoArray();
                    Compare();

                    if (count == 4)
                    {
                        Rotate_field();
                        count = 0;
                        Initialize_All();
                        Debug.Log("右に1列ずらし、下に2列ずらしたら回転成功！");
                    }
                    else
                    {
                        Debug.Log("回転失敗；；　次のテストへ");
                        count = 0;
                        MoveLeft_minoArray();
                        MoveLeft_minoArray();
                        MoveLeft_minoArray();
                        MoveUp_minoArray();
                        MoveUp_minoArray();
                        MoveUp_minoArray();
                        Compare();

                        if (count == 4)
                        {
                            Rotate_field();
                            count = 0;
                            Initialize_All();
                            Debug.Log("左に2列ずらし、上に1列ずらしたら回転成功！");
                        }
                        else
                        {
                            minoDirection--;
                            gamemanager.GetComponent<GameManager>().minoDirection = minoDirection;

                            Debug.Log("回転できない；；");
                            count = 0;
                            Initialize_All();
                        }
                    }
                }
            }
        }
    }
    void IminoTest_90_0()
    {
        Copy();
        IminoRotate_90_0_minoArray();
        Compare();

        if (count == 4)
        {
            Rotate_field();
            count = 0;
            Initialize_All();
            Debug.Log("回転成功！");
        }
        else
        {
            Debug.Log("回転失敗；；　次のテストへ");
            count = 0;
            MoveRight_minoArray();
            MoveRight_minoArray();
            Compare();

            if (count == 4)
            {
                Rotate_field();
                count = 0;
                Initialize_All();
                Debug.Log("右に1列ずらしたら回転成功！");
            }
            else
            {
                Debug.Log("回転失敗；；　次のテストへ");
                count = 0;
                MoveLeft_minoArray();
                MoveLeft_minoArray();
                MoveLeft_minoArray();
                Compare();

                if (count == 4)
                {
                    Rotate_field();
                    count = 0;
                    Initialize_All();
                    Debug.Log("右に1列ずらし、下に1列ずらしたら回転成功！");
                }
                else
                {
                    Debug.Log("回転失敗；；　次のテストへ");
                    count = 0;
                    MoveRight_minoArray();
                    MoveRight_minoArray();
                    MoveRight_minoArray();
                    MoveUp_minoArray();
                    Compare();

                    if (count == 4)
                    {
                        Rotate_field();
                        count = 0;
                        Initialize_All();
                        Debug.Log("上に2列ずらしたら回転成功！");
                    }
                    else
                    {
                        Debug.Log("回転失敗；；　次のテストへ");
                        count = 0;
                        MoveLeft_minoArray();
                        MoveLeft_minoArray();
                        MoveLeft_minoArray();
                        MoveDown_minoArray();
                        MoveDown_minoArray();
                        MoveDown_minoArray();
                        Compare();

                        if (count == 4)
                        {
                            Rotate_field();
                            count = 0;
                            Initialize_All();
                            Debug.Log("右に1列ずらし、上に2列ずらしたら回転成功！");
                        }
                        else
                        {
                            minoDirection--;
                            gamemanager.GetComponent<GameManager>().minoDirection = minoDirection;

                            Debug.Log("回転できない；；");
                            count = 0;
                            Initialize_All();
                        }
                    }
                }
            }
        }
    }

    // ミノの強制移動
    void MoveLeft_minoArray()
    {
        minoPos_x = -1;
        minoPos_y = 0;

        for (int i = 0; i < minoArray.GetLength(0); i++)
        {
            for (int j = 0; j < minoArray.GetLength(1); j++)
            {
                if (minoArray[i, j] == 1 || minoArray[i, j] == 2)
                {
                    var x = i + minoPos_x;
                    if (x < 1)
                    {
                        // 範囲外処理
                        minoPos_x = 0;

                    }
                    minoArray[i + minoPos_x, j + minoPos_y] = minoArray[i, j];
                    minoArray[i, j] = 0;
                }
            }
        }
    }
    void MoveRight_minoArray()
    {
        minoPos_x = 1;
        minoPos_y = 0;

        for (int i = minoArray.GetLength(0) - 1; i >= 0; i--)
        {
            for (int j = 0; j < minoArray.GetLength(1); j++)
            {
                if (minoArray[i, j] == 1 || minoArray[i, j] == 2)
                {
                    var x = i + minoPos_x;
                    if (x >= minoArray.GetLength(0))
                    {
                        // 範囲外処理
                        minoPos_x = 0;

                    }
                    minoArray[i + minoPos_x, j + minoPos_y] = minoArray[i, j];
                    minoArray[i, j] = 0;
                }
            }
        }
    }
    void MoveUp_minoArray()
    {
        minoPos_x = 0;
        minoPos_y = 1;

        for (int i = 0; i < minoArray.GetLength(0); i++)
        {
            for (int j = minoArray.GetLength(1) - 1; j >= 0 ; j--)
            {
                if (minoArray[i, j] == 1 || minoArray[i, j] == 2)
                {
                    var y = j + minoPos_y;
                    if (y >= minoArray.GetLength(1))
                    {
                        minoPos_y = 0;
                    }
                    minoArray[i + minoPos_x, j + minoPos_y] = minoArray[i, j];
                    minoArray[i, j] = 0;
                }
            }
        }

    }
    void MoveDown_minoArray()
    {
        minoPos_x = 0;
        minoPos_y = -1;

        for (int i = 0; i < minoArray.GetLength(0); i++)
        {
            for (int j = 0; j < minoArray.GetLength(1); j++)
            {
                if (minoArray[i, j] == 1 || minoArray[i, j] == 2)
                {
                    var y = j + minoPos_y;
                    if (y < 0 )
                    {
                        minoPos_y = 0;
                    }
                    minoArray[i + minoPos_x, j + minoPos_y] = minoArray[i, j];
                    minoArray[i, j] = 0;
                }
            }
        }
    }

    // <コピー>
    void Copy()
    {
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                // ミノ
                if (field[i, j] == 1 || field[i, j] == 2)
                {
                    minoArray[i, j] = field[i, j];
                }

                // 壁
                if (field[i, j] == 3)
                {
                    fieldArray[i, j] = field[i, j];
                }
            }
        }
    } 

    // <比較>
    void Compare()
    {
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {
            for (int j = 0; j < minoArray.GetLength(1); j++)
            {
                if (minoArray[i, j] == 2 && fieldArray[i, j] == 0)
                {
                    count++;
                    Debug.Log("count+1 回転軸");
                }
                if (minoArray[i, j] == 1 && fieldArray[i, j] == 0)
                {
                    count++;
                    Debug.Log("count+1 ほかのブロック");
                }
            }
        }
    }

    // <回転関連>
    // ミノの回転
    void RotateRight_minoArray()
    {
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {

            for (int j = 0; j < minoArray.GetLength(1); j++)
            {

                if (minoArray[i, j] == 2)
                {
                    array_mino_test[0, 0] = minoArray[i - 1, j + 1];
                    array_mino_test[0, 1] = minoArray[i, j + 1];
                    array_mino_test[0, 2] = minoArray[i + 1, j + 1];
                    array_mino_test[1, 2] = minoArray[i + 1, j];
                    array_mino_test[2, 2] = minoArray[i + 1, j - 1];
                    array_mino_test[2, 1] = minoArray[i, j - 1];
                    array_mino_test[2, 0] = minoArray[i - 1, j - 1];
                    array_mino_test[1, 0] = minoArray[i - 1, j];

                    minoArray[i + 1, j] = 0;
                    minoArray[i + 1, j - 1] = 0;
                    minoArray[i + 1, j + 1] = 0;
                    minoArray[i, j + 1] = 0;
                    minoArray[i - 1, j + 1] = 0;
                    minoArray[i - 1, j] = 0;
                    minoArray[i - 1, j - 1] = 0;
                    minoArray[i, j - 1] = 0;

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

        for (int i = 0; i < array_mino_test.GetLength(0); i++)
        {
            for (int j = 0; j < array_mino_test.GetLength(1); j++)
            {
                array_mino_test[i,j] = 0;
            }
        }
    }
    void RotateLeft_minoArray()
    {
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {

            for (int j = 0; j < minoArray.GetLength(1); j++)
            {

                if (minoArray[i, j] == 2)
                {
                    array_mino_test[0, 0] = minoArray[i - 1, j + 1];
                    array_mino_test[0, 1] = minoArray[i, j + 1];
                    array_mino_test[0, 2] = minoArray[i + 1, j + 1];
                    array_mino_test[1, 2] = minoArray[i + 1, j];
                    array_mino_test[2, 2] = minoArray[i + 1, j - 1];
                    array_mino_test[2, 1] = minoArray[i, j - 1];
                    array_mino_test[2, 0] = minoArray[i - 1, j - 1];
                    array_mino_test[1, 0] = minoArray[i - 1, j];

                    minoArray[i + 1, j] = 0;
                    minoArray[i + 1, j - 1] = 0;
                    minoArray[i + 1, j + 1] = 0;
                    minoArray[i, j + 1] = 0;
                    minoArray[i - 1, j + 1] = 0;
                    minoArray[i - 1, j] = 0;
                    minoArray[i - 1, j - 1] = 0;
                    minoArray[i, j - 1] = 0;

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

        for (int i = 0; i < array_mino_test.GetLength(0); i++)
        {
            for (int j = 0; j < array_mino_test.GetLength(1); j++)
            {
                array_mino_test[i,j] = 0; 
            }
        }
    }

    // Iミノの右回転
    void IminoRotate_0_90_minoArray()
    {
        for (int i =  minoArray.GetLength(0) - 1; i > 0; i--)
        {

            for (int j = 0; j < minoArray.GetLength(1); j++)
            {

                if (minoArray[i, j] == 2)
                {
                    minoArray[i, j] = 0;
                    minoArray[i - 1, j] = 0;
                    minoArray[i + 1, j] = 0;
                    minoArray[i + 2, j] = 0;

                    minoArray[i + 1, j + 1] = 1;
                    minoArray[i + 1, j] = 2;
                    minoArray[i + 1, j - 1] = 1;
                    minoArray[i + 1, j - 2] = 1;
                }
            }
        }
    }
    void IminoRotate_90_180_minoArray()
    {
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {

            for (int j = 0; j < minoArray.GetLength(1); j++)
            {

                if (minoArray[i, j] == 2)
                {
                    minoArray[i, j] = 0;
                    minoArray[i, j + 1] = 0;
                    minoArray[i, j - 1] = 0;
                    minoArray[i, j - 2] = 0;

                    minoArray[i - 2, j - 1] = 1;
                    minoArray[i - 1, j - 1] = 1;
                    minoArray[i, j - 1] = 2;
                    minoArray[i + 1, j - 1] = 1;
                }
            }
        }
    }
    void IminoRotate_180_270_minoArray()
    {
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {

            for (int j = 0; j < minoArray.GetLength(1); j++)
            {

                if (minoArray[i, j] == 2)
                {
                    minoArray[i, j] = 0;
                    minoArray[i - 2, j] = 0;
                    minoArray[i - 1, j] = 0;
                    minoArray[i + 1, j] = 0;

                    minoArray[i - 1, j + 1] = 1;
                    minoArray[i - 1, j] = 2;
                    minoArray[i - 1, j - 1] = 1;
                    minoArray[i - 1, j + 2] = 1;
                }
            }
        }
    }
    void IminoRotate_270_360_minoArray()
    {
        for (int i = minoArray.GetLength(0) - 1; i > 0; i--)
        {

            for (int j = minoArray.GetLength(1) - 1; j > 0; j--)
            {

                if (minoArray[i, j] == 2)
                {
                    minoArray[i, j] = 0;
                    minoArray[i, j + 1] = 0;
                    minoArray[i, j + 2] = 0;
                    minoArray[i, j - 1] = 0;

                    minoArray[i + 2, j + 1] = 1;
                    minoArray[i + 1, j + 1] = 1;
                    minoArray[i, j + 1] = 2;
                    minoArray[i - 1, j + 1] = 1;
                }
            }
        }
    }

    // Iミノの左回転
    void IminoRotate_360_270_minoArray()
    {
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {

            for (int j = 0; j < minoArray.GetLength(1); j++)
            {

                if (minoArray[i, j] == 2)
                {
                    minoArray[i, j] = 0;
                    minoArray[i - 1, j] = 0;
                    minoArray[i + 1, j] = 0;
                    minoArray[i + 2, j] = 0;

                    minoArray[i, j + 1] = 1;
                    minoArray[i, j] = 1;
                    minoArray[i, j - 1] = 2;
                    minoArray[i, j - 2] = 1;
                }
            }
        }
    }
    void IminoRotate_270_180_minoArray()
    {
        for (int i = minoArray.GetLength(0) - 1; i > 0; i--)
        {

            for (int j = 0; j < minoArray.GetLength(1); j++)
            {

                if (minoArray[i, j] == 2)
                {
                    minoArray[i, j] = 0;
                    minoArray[i, j + 1] = 0;
                    minoArray[i, j - 1] = 0;
                    minoArray[i, j + 2] = 0;

                    minoArray[i + 2, j] = 1;
                    minoArray[i - 1, j] = 1;
                    minoArray[i, j] = 1;
                    minoArray[i + 1, j] = 2;
                }
            }
        }
    }
    void IminoRotate_180_90_minoArray()
    {
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {

            for (int j = minoArray.GetLength(1) - 1; j > 0 ; j--)
            {

                if (minoArray[i, j] == 2)
                {
                    minoArray[i, j] = 0;
                    minoArray[i - 2, j] = 0;
                    minoArray[i - 1, j] = 0;
                    minoArray[i + 1, j] = 0;

                    minoArray[i, j + 1] = 2;
                    minoArray[i, j] = 1;
                    minoArray[i, j - 1] = 1;
                    minoArray[i, j + 2] = 1;
                }
            }
        }
    }
    void IminoRotate_90_0_minoArray()
    {
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {

            for (int j = minoArray.GetLength(1) - 1; j > 0; j--)
            {

                if (minoArray[i, j] == 2)
                {
                    minoArray[i, j] = 0;
                    minoArray[i, j + 1] = 0;
                    minoArray[i, j - 2] = 0;
                    minoArray[i, j - 1] = 0;

                    minoArray[i - 2, j] = 1;
                    minoArray[i + 1, j] = 1;
                    minoArray[i, j] = 1;
                    minoArray[i - 1, j] = 2;
                }
            }
        }
    }

    void Rotate_field()
    {
        // 実際には回転というより消して代入している
        // ①fieldの中のミノを消去
        for (int i = 0; i < field.GetLength(0); i++)
        {

            for (int j = 0; j < field.GetLength(1); j++)
            {

                if (field[i, j] == 2 || field[i,j] == 1)
                {
                    field[i,j] = 0;
                }
            }
        }
        // ②minoArrayのミノの位置情報をfieldに与える
        for (int i = 0; i < minoArray.GetLength(0); i++)
        {

            for (int j = 0; j < minoArray.GetLength(1); j++)
            {

                if (minoArray[i, j] == 2 || minoArray[i, j] == 1)
                {
                    field[i, j] = minoArray[i, j];
                }
            }
        }
    }

    // 配列の初期化
    void Initialize_All()
    {
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                fieldArray[i, j] = 0;
                minoArray[i, j] = 0;
            }
        }
    }
    // あると便利かと思って作ったけど下2つ使ってない
    void Initialize_fieldArray()
    {
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                fieldArray[i, j] = 0;
            }
        }
    }
    void Initialize_minoArray()
    {
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                minoArray[i, j] = 0;
            }
        }
    }
}