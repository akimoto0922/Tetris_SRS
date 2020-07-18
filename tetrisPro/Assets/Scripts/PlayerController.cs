﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int[,] field = new int[12, 22];  // ミノを扱う配列
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
    public bool isActive;            // ゲーム中かどうか
    public bool isGround;                   // 空中にいるかかどうか
    public bool isHold;                     // ホールド中かどうか
    public bool makeMino;                   // 次のミノを生成するかどうか
    
    // 参照するスクリプト類
    public GameObject move;
    public GameObject render;
    public GameObject hold;
    public GameObject rotation;
    public GameObject next;
    public GameObject minoCreate;
    public GameObject rockDown;
    public GameObject LineClear;
    Move Script_move;
    Render Script_render;
    Hold Script_hold;
    Rotation Script_rotation;
    Next Script_next;
    MinoCreate Script_minoCreate;
    RockDown Script_rockDown;
    LineClear Script_lineClear;

    // デバッグ用
    //public int[,] field = new int[12, 22]
    //{
    //    { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
    //    { 0,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
    //    { 0,3,0,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
    //    { 0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
    //    { 0,3,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
    //    { 0,3,3,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
    //    { 0,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
    //    { 0,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
    //    { 0,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
    //    { 0,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
    //    { 0,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
    //    { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },

    //};
    
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
        Script_move = move.GetComponent<Move>();
        Script_render = render.GetComponent<Render>();
        Script_hold = hold.GetComponent<Hold>();
        Script_rotation = rotation.GetComponent<Rotation>();
        Script_next = next.GetComponent<Next>();
        Script_minoCreate = minoCreate.GetComponent<MinoCreate>();
        Script_rockDown = rockDown.GetComponent<RockDown>();
        Script_lineClear = LineClear.GetComponent<LineClear>();

        Script_render.MakeFlame(field);
    }

    void Start()
    {
        Intialize();
    }

    // Update is called once per frame
    void Update()
    {
        // ゲーム中なら操作可能
        if (isActive == true)
        {
            // 次のミノを生成する
            if (makeMino == true)
            {
                Script_next.NextChanger(ref currentMino, ref nextMino);
                Script_minoCreate.CreateMino(field, currentMino);
                minoDirection = (int)Direction.Up;
                isGround = false;
                makeMino = false;
            }

            // 右移動
            if (Input.GetKeyDown(KeyCode.D))
            {
                time2 = 0;
                if (isGround == true)
                {
                    setCount++;
                }
                Script_move.MoveMino(field, 1, 0, ref isGround);
            }

            // 左移動
            if (Input.GetKeyDown(KeyCode.A))
            {
                time2 = 0;
                if (isGround == true)
                {
                    setCount++;
                }
                Script_move.MoveMino(field, -1, 0, ref isGround);
            }

            // 右回転
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                time2 = 0;
                if (isGround == true)
                {
                    setCount++;
                }
                Script_rotation.RightSpin(ref field, currentMino, ref minoDirection);
            }

            // 左回転
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                time2 = 0;
                if (isGround == true)
                {
                    setCount++;
                }
                Script_rotation.LeftSpin(ref field, currentMino, ref minoDirection);
            }

            // ソフトドロップ
            if (Input.GetKey(KeyCode.S))
            {
                Script_move.MoveMino(field, 0, -1, ref isGround);
                time2 = 0;
            }

            // ハードドロップ
            if (Input.GetKeyDown(KeyCode.W))
            {
                for (int i = 0; i < 20; i++)
                {
                    Script_move.MoveMino(field, 0, -1, ref isGround);
                }
                time = 0;
                time2 = 0;
                Script_rockDown.SetMino(field);
                for (int i = 0; i < 20; i++)
                {
                    Script_lineClear.LineDelete(field, ref score);
                }
                setCount = 0;

                makeMino = true;
                isHold = false;
            }

            // ホールド
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
                    Script_minoCreate.CreateMino(field, currentMino);
                }
                isHold = true;
            }
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
        Script_render.Draw(field);
        
        // 自由落下
        time += Time.deltaTime;
        if (time >= dropInterval)
        {
            Script_move.MoveMino(field, 0, -1, ref isGround);
            time = 0;
        }

        // ロックダウン①,回数制限
        if (setCount >= 15)
        {
            if (isGround == true)
            {
                Script_rockDown.SetMino(field);
                Script_lineClear.LineDelete(field, ref score);
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
                Script_rockDown.SetMino(field);
                setCount = 0;
                Script_lineClear.LineDelete(field, ref score);
                makeMino = true;
                time2 = 0;
                isGround = false;
                isHold = false;
            }
        }

    }

    public void Intialize()
    {
        for (int i = 1; i < field.GetLength(0) - 1; i++)
        {
            for (int j = 1; j < field.GetLength(1) - 1; j++)
            {
                field[i, j] = (int)FieldValue.Empty;
            }
        }
        score = 0;
        isActive = true;
        minoDirection = (int)Direction.Up;
        holdMino = (int)MinoType.Null;
        isHold = false;
        makeMino = true;
    }
}