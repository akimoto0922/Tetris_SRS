using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Tetris.Mino;
using Tetris.Rotation;
using Tetris.Field;

public class PlayerController : MonoBehaviour
{

    public int[,] field = new int[FieldData.FIELD_HEIGHT, FieldData.FIELD_WIDTH];  // フィールドの配列
    public int[] minoArray;                                    // ミノの配列
    public int minoPosX = FieldData.RESPAWN_MINO_POS_X;
    public int minoPosY = FieldData.RESPAWN_MINO_POS_Y;

    private MinoData.eMinoType currentMino;
    private MinoData.eMinoType nextMino;
    private MinoData.eMinoType holdMino;

    private MinoData.eMinoAngle _minoAngle;
    private MinoData.eMinoAngle minoAngle
    {
        get
        {
            return _minoAngle;
        }
        set
        {
            _minoAngle = value;
            UnityEngine.Debug.Log(_minoAngle.ToString());
        }
    }

    //    public int currentMino;                 // 現在のミノ
    //    public int nextMino;                    // 次のミノ 
    //    public int holdMino;                    // ホールド中のミノ
    //public int minoDirection;               // ミノの向き
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
    public bool canMove;
    
    // 参照するスクリプト類
    public GameObject render;
    public GameObject hold;
    public GameObject next;
    public GameObject lineClear;
    
    Render Script_render;
    Hold Script_hold;
    Next Script_next;
    LineClear Script_lineClear;
    Move move = new Move();
    Rotation rotation = new Rotation();
    RockDown rockDown = new RockDown();
    MinoCreate minoCreate = new MinoCreate();

    void Awake()
    {
        // 使用するスクリプトを GetComponent する
        GetScript();

        // フレームを生成する
        //Script_render.MakeFlame(field);
    }

    void Start()
    {
        // 初期化
        Intialize();

        field[8,  2] = 2;
        field[9,  2] = 2;
        field[10, 2] = 2;
        field[8,  4] = 2;
        field[9,  5] = 2;
        field[10, 4] = 2;
        field[7,  2] = 2;
        field[6,  2] = 2;
        field[6,  3] = 2;
        field[11, 2] = 2;
        field[11, 3] = 2;
        field[11, 4] = 2;

        Script_render.Delete();
        Script_render.DrawField(field);
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
        
        //// 自由落下
        //time += Time.deltaTime;
        //if (time >= dropInterval)
        //{
        //    move.MoveMino(field, minoArray, ref minoPosX, ref minoPosY, 0, -1, currentMino);
        //    time = 0;
        //    Script_render.DeleteMino();
        //    Script_render.DrawMino(minoPosX, minoPosY, ref minoArray, currentMino);
        //}

        // ブロック設置の条件①,移動回数制限
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

                Script_render.Delete();
                Script_render.DrawField(field);
            }
        }

        // ブロック設置の条件②,地面についてる間の秒数制限
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

                Script_render.Delete();
                Script_render.DrawField(field);
            }
        }
    }
    
    // 次のミノを生成する
    private void MakeMino()
    {
        if (makeMino == true)
        {
            Script_next.NextChanger(ref currentMino, ref nextMino);
            minoCreate.SetMinoData(out minoArray, currentMino);
            minoPosX = FieldData.RESPAWN_MINO_POS_X;
            minoPosY = FieldData.RESPAWN_MINO_POS_Y;
            Script_render.DeleteMino();
            Script_render.DrawMino(minoPosX, minoPosY, ref minoArray, currentMino);
            minoAngle = MinoData.eMinoAngle.ang0;
            isGround = false;
            makeMino = false;
        }
    }

    //------------------------------------
    // 入力処理
    //------------------------------------
    // ホールド
    private void Hold()
    {
        if (Input.GetKeyDown(KeyCode.Q) && isHold == false)
        {
            if (holdMino == MinoData.eMinoType.MAX) // ホールドしてるミノがないとき
            {
                holdMino = currentMino;
                Script_hold.HoldMino(field, ref currentMino, ref holdMino);
                makeMino = true;
            }
            else
            {
                MinoData.eMinoType createType = holdMino;
                holdMino = currentMino;
                currentMino = createType;
                Script_hold.HoldMino(field, ref currentMino, ref holdMino);
                Script_render.DeleteMino();
                minoCreate.SetMinoData(out minoArray, currentMino);
                Script_render.DrawMino(minoPosX, minoPosY, ref minoArray, currentMino);
            }
            isHold = true;
        }
    }
    
    // 移動
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

            // 移動可能ならば、移動させて画面を更新
            if (move.CheckWall(field, minoArray, minoPosX + 1, minoPosY, currentMino))
            {
                move.MoveMino(ref minoPosX, ref minoPosY, 1, 0);
                Script_render.DeleteMino();
                Script_render.DrawMino(minoPosX, minoPosY, ref minoArray, currentMino);
            }
        }

        // 左移動
        if (Input.GetKeyDown(KeyCode.A))
        {
            //time2 = 0;
            //if (isGround == true)
            //{
            //    setCount++;
            //}

            // 移動可能ならば、移動させて画面を更新
            if (move.CheckWall(field, minoArray, minoPosX - 1, minoPosY, currentMino))
            {
                move.MoveMino(ref minoPosX, ref minoPosY, -1, 0);
                Script_render.DeleteMino();
                Script_render.DrawMino(minoPosX, minoPosY, ref minoArray, currentMino);
            }
        }

        // 下移動（ソフトドロップ）
        if (Input.GetKey(KeyCode.S))
        {
            // 移動可能ならば、移動させて画面を更新
            if (move.CheckWall(field, minoArray, minoPosX, minoPosY + 1, currentMino))
            {
                move.MoveMino(ref minoPosX, ref minoPosY, 0, 1);
                Script_render.DeleteMino();
                Script_render.DrawMino(minoPosX, minoPosY, ref minoArray, currentMino);
            }
        }
        // 上移動
        if (Input.GetKey(KeyCode.W))
        {
            // 移動可能ならば、移動させて画面を更新
            if (move.CheckWall(field, minoArray, minoPosX, minoPosY - 1, currentMino))
            {
                move.MoveMino(ref minoPosX, ref minoPosY, 0, -1);
                Script_render.DeleteMino();
                Script_render.DrawMino(minoPosX, minoPosY, ref minoArray, currentMino);
            }
        }

        //// 高速落下、ミノの設置までする（ハードドロップ）
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    for (int i = 0; i < 20; i++)
        //    {
        //       // move.MoveMino(field, 0, -1, ref isGround);
        //    }
        //    time = 0;
        //    time2 = 0;
        //    rockDown.SetMino(field);
        //    for (int i = 0; i < 20; i++)
        //    {
        //        Script_lineClear.LineDelete(field, ref score);
        //    }
        //    setCount = 0;

        //    makeMino = true;
        //    isHold = false;
        //}
    }
    
    // 回転
    private void Rotate()
    {
        // 右回転
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // ミノの向きを変える
            // 左向き(3)の時は上向き(0)にする
            if (minoAngle == MinoData.eMinoAngle.ang270)
            {
                minoAngle = MinoData.eMinoAngle.ang0;
            }
            else
            {
                minoAngle++;
            }
            rotation.RotatedMino(field, ref minoArray, currentMino, minoAngle, ref minoPosX, ref minoPosY, true);
            Script_render.DeleteMino();
            Script_render.DrawMino(minoPosX, minoPosY, ref minoArray, currentMino);
            // TODO : 回転が失敗だった時、 minoAngleをもとに戻す処理

        }

        // 左回転
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // ミノの向きを変える
            // 上向き(0)の時は左向き(3)にする
            if (minoAngle == MinoData.eMinoAngle.ang0)
            {
                minoAngle = MinoData.eMinoAngle.ang270;
            }
            else
            {
                minoAngle--;
            }
            rotation.RotatedMino(field, ref minoArray, currentMino, minoAngle,ref minoPosX,ref minoPosY, false);
            Script_render.DeleteMino();
            Script_render.DrawMino(minoPosX, minoPosY, ref minoArray, currentMino);
            // TODO : 回転が失敗だった時、 minoAngleをもとに戻す処理

        }
    }

    // ゲームクリアの判定
    private void CheckGameClear()
    {
        if (score >= 4000)
        {
            isActive = false;
        }
    }

    // ゲームオーバーの判定
    private void CheckGameOver()
    {
        for (int i = 0; i < field.GetLength(0); i++)
        {
            if (field[i, FieldData.RESPAWN_MINO_POS_Y] == (int)Rotation.eFieldValue.WallBlock)
            {
                isActive = false;
            }
        }
    }
    
    // Scriptの取得
    private void GetScript()
    {
        Script_render = render.GetComponent<Render>();
        Script_hold = hold.GetComponent<Hold>();
        Script_next = next.GetComponent<Next>();
        Script_lineClear = lineClear.GetComponent<LineClear>();
    }
    
    // 初期化
    private void Intialize()
    {
        // field を初期化
        for (int i = 0; i < FieldData.FIELD_HEIGHT; i++)
        {
            if(i == FieldData.FIELD_HEIGHT - 1)
            {
                for (int j = 0; j < FieldData.FIELD_WIDTH; j++)
                {
                    field[i, j] = (int)Rotation.eFieldValue.WallBlock;
                }
            }
            else
            {
                for (int j = 0; j < FieldData.FIELD_WIDTH; j++)
                {
                    if(j == 0 || j == FieldData.FIELD_WIDTH - 1)
                    {
                        field[i, j] = (int)Rotation.eFieldValue.WallBlock;
                    }
                    else
                    {
                        field[i, j] = (int)Rotation.eFieldValue.Empty;
                    }
                }
            }
        }
        score = 0;                          // スコア
        isActive = true;                    // 操作可能かどうか
        minoAngle = MinoData.eMinoAngle.ang0;  // ミノの向き
        _minoAngle = minoAngle;
        holdMino = MinoData.eMinoType.MAX;      // ホールド中のミノ
        isHold = false;                     // ホールド可能かどうか
        makeMino = true;                    // 次のミノを生成

    }
}
