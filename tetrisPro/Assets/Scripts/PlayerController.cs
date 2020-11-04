using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const int respownMinoPosX = 4;          // ミノが生成される座標の X 成分
    const int respownMinoPosY = 23;         // ミノが生成される座標の Y 成分

    public int[,] field = new int[12, 23];  // ミノを扱う配列
    public int[] minoArray = new int[9];
    public int minoPosX;
    public int minoPosY;
    public int currentMino;                 // 現在のミノ
    public int nextMino;                    // 次のミノ 
    public int holdMino;                    // ホールド中のミノ
    public int minoDirection;               // ミノの向き
    public int setCount;                    // 設置されるまでの動かせる回数
    public int score = 0;                   // プレイヤーのスコア
    public float setLimit = 0.5f;           // ミノが設置されるまでの時間
    public float dropInterval = 1.0f;       // 落ちる速度
    public float time = 0;                  // deltaTimeを入れる変数
    public float time2 = 0;                 // deltaTimeを入れる変数    後で名前変える
    public bool isActive;                   // ゲーム中かどうか
    public bool isGround;                   // 空中にいるかかどうか
    public bool isHold;                     // ホールド中かどうか
    public bool makeMino;                   // 次のミノを生成するかどうか
    
    // 参照するスクリプト類
    public GameObject render;
    public GameObject hold;
    public GameObject next;
    public GameObject minoCreate;
    public GameObject lineClear;
    
    Render Script_render;
    Hold Script_hold;
    Next Script_next;
    MinoCreate Script_minoCreate;
    LineClear Script_lineClear;
    Move move = new Move();
    Rotation rotation = new Rotation();
    RockDown rockDown = new RockDown();

    Rotation.eMinoType currentMinoType;
    Rotation.eMinoAngle currentMinoAngle = Rotation.eMinoAngle.ang0;

    enum Direction : int
    {
        Up,
        Right,
        Down,
        Left
    }
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

    void Awake()
    {
        // 使用するスクリプトを GetComponent する
        GetScript();

        // フレームを生成する
        Script_render.MakeFlame(field);
    }

    void Start()
    {
        // 初期化
        Intialize();
    }

    // Update is called once per frame
    void Update()
    {
        // ゲーム中なら操作可能
        if (isActive == true)
        {
            // fieldに次のミノを生成する
            MakeMino();

            // プレイヤーの入力処理
            Hold();     // Q           ホールド…現在のミノをストックし、必要な時に取り出すことができる
            Move();     // W, A, S, D　ミノの移動
            Rotate();   // ←, →　    ミノの回転

        }

        // ゲームオーバー
        if (field[6,18] == (int)FieldValue.WallBlock || field[7, 18] == (int)FieldValue.WallBlock)
        {
            isActive = false;
        }
        // ゲームクリア
        if (score >= 4000)
        {
            isActive = false;
        }

        Script_render.Delete();
        Script_render.DrawField(field);
        
        // 自由落下
        time += Time.deltaTime;
        if (time >= dropInterval)
        {
            // move.MoveMino(field, 0, -1, ref isGround);
            time = 0;
        }

        // ロックダウン①,回数制限
        if (setCount >= 15)
        {
            if (isGround == true)
            {
                rockDown.SetMino(field);
                for (int i = 0; i < 20; i++)
                {
                    Script_lineClear.LineDelete(field, ref score);
                }
                setCount = 0;
                makeMino = true;
                isGround = false;
                isHold = false;
            }
        }
        // ロックダウン②,秒数制限
        if (isGround == true)
        {
            time2 += Time.deltaTime;
            if (time2 >= setLimit)
            {
                rockDown.SetMino(field);
                setCount = 0;
                for (int i = 0; i < 20; i++)
                {
                    Script_lineClear.LineDelete(field, ref score);
                }
                makeMino = true;
                time2 = 0;
                isGround = false;
                isHold = false;
            }
        }
    }
    
    private void MakeMino()
    {
        if (makeMino == true)
        {
            Script_next.NextChanger(ref currentMino, ref nextMino);
            Script_minoCreate.SetMinoData(ref minoArray, currentMino);
            minoPosX = respownMinoPosX;
            minoPosY = respownMinoPosY;
            Script_render.DeleteMino();
            Script_render.DrawMino(minoPosX, minoPosY, ref minoArray, currentMino);
            minoDirection = (int)Direction.Up;
            isGround = false;
            makeMino = false;
        }
    }

    private void Hold()
    {
        if (Input.GetKeyDown(KeyCode.Q) && isHold == false)
        {
            if (holdMino == (int)MinoType.Null) // ホールドしてるミノがないとき
            {
                holdMino = currentMino;
                Script_hold.HoldMino(field, ref currentMino, ref holdMino);
                makeMino = true;
            }
            else
            {
                int createNum = holdMino;
                holdMino = currentMino;
                currentMino = createNum;
                Script_hold.HoldMino(field, ref currentMino, ref holdMino);
                Script_render.DeleteMino();
                Script_minoCreate.SetMinoData(ref minoArray, currentMino);
                Script_render.DrawMino(minoPosX, minoPosY, ref minoArray, currentMino);
            }
            isHold = true;
        }
    }
    
    private void Move()
    {
        // 右移動
        if (Input.GetKeyDown(KeyCode.D))
        {
            //time2 = 0;
            //if (isGround == true)
            //{
            //    setCount++;
            //}
            minoPosX = move.MoveMino(minoPosX, 1);
            Script_render.DeleteMino();
            Script_render.DrawMino(minoPosX, minoPosY,ref minoArray, currentMino);
        }

        // 左移動
        if (Input.GetKeyDown(KeyCode.A))
        {
            //time2 = 0;
            //if (isGround == true)
            //{
            //    setCount++;
            //}
            minoPosX = move.MoveMino(minoPosX, -1);
            Script_render.DeleteMino();
            Script_render.DrawMino(minoPosX, minoPosY, ref minoArray, currentMino);
        }

        // 下移動（ソフトドロップ）
        if (Input.GetKey(KeyCode.S))
        {
           // move.MoveMino(field, 0, -1, ref isGround);
            time2 = 0;
        }

        // 高速落下、ミノの設置までする（ハードドロップ）
        if (Input.GetKeyDown(KeyCode.W))
        {
            for (int i = 0; i < 20; i++)
            {
               // move.MoveMino(field, 0, -1, ref isGround);
            }
            time = 0;
            time2 = 0;
            rockDown.SetMino(field);
            for (int i = 0; i < 20; i++)
            {
                Script_lineClear.LineDelete(field, ref score);
            }
            setCount = 0;

            makeMino = true;
            isHold = false;
        }
    }
    
    private void Rotate()
    {
        // 右回転
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // ミノの向きを変える
            // 左向き(3)の時は上向き(0)にする
            if (minoDirection == (int)Direction.Left)
            {
                minoDirection = (int)Direction.Up;
            }
            else
            {
                minoDirection++;
            }
            rotation.RotatedMino(ref minoArray, (Rotation.eMinoType)currentMino, (Rotation.eMinoAngle)minoDirection);
            Script_render.DeleteMino();
            Script_render.DrawMino(minoPosX, minoPosY, ref minoArray, currentMino);
            // TODO : 回転が失敗だった時、 minoDirectionをもとに戻す処理

        }

        // 左回転
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // ミノの向きを変える
            // 上向き(0)の時は左向き(3)にする
            if (minoDirection == (int)Direction.Up)
            {
                minoDirection = (int)Direction.Left;
            }
            else
            {
                minoDirection--;
            }
            rotation.RotatedMino(ref minoArray, (Rotation.eMinoType)currentMino, (Rotation.eMinoAngle)minoDirection);
            Script_render.DeleteMino();
            Script_render.DrawMino(minoPosX, minoPosY, ref minoArray, currentMino);
            // TODO : 回転が失敗だった時、 minoDirectionをもとに戻す処理

        }
    }
    
    private void GetScript()
    {
        Script_render = render.GetComponent<Render>();
        Script_hold = hold.GetComponent<Hold>();
        Script_next = next.GetComponent<Next>();
        Script_minoCreate = minoCreate.GetComponent<MinoCreate>();
        Script_lineClear = lineClear.GetComponent<LineClear>();
    }
    
    private void Intialize()
    {
        // field を初期化
        for (int i = 1; i < field.GetLength(0) - 1; i++)
        {
            for (int j = 1; j < field.GetLength(1); j++)
            {
                field[i, j] = (int)FieldValue.Empty;
            }
        }
        score = 0;                          // スコア
        isActive = true;                    // 操作可能かどうか
        minoDirection = (int)Direction.Up;  // ミノの向き
        holdMino = (int)MinoType.Null;      // ホールド中のミノ
        isHold = false;                     // ホールド可能かどうか
        makeMino = true;                    // 次のミノを生成

    }
}
