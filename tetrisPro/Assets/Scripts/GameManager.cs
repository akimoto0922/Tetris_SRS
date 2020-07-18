using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject GameoverWindow = null;
    [SerializeField] GameObject GameClearWindow = null;

    public bool makeMino = false;  　// ミノの生成

    AudioSource sound1;
    AudioSource sound2;
    AudioSource sound3;

    public int score;              // スコア
    public int currentMino;        // 現在のミノ
    public int nextMino;           // 次のミノ
    public int holdMino;           // ホールド中のミノ
    public int minoDirection;      // ミノの向き
    public bool isActive = true;   // 操作可能かどうか
    bool isGround;　　             // 空中にいるかかどうか

    public int[,] field = new int[12, 22]
    {
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },

    };

    
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
        //Script = roatation.GetComponent<Rotation>();
        AudioSource[] audioSources = GetComponents<AudioSource>();
        sound1 = audioSources[0];
        sound2 = audioSources[1];
        sound3 = audioSources[2];

    }

    // Update is called once per frame
    void Update()
    {
        // 次のミノを出す
        if (makeMino == true)
        {
            minoDirection = (int)MinoAngle._0;
            makeMino = false;
        }

        // ゲームオーバーの判定
        if (field[5, 19] == (int)FieldValue.WallBlock || field[6, 19] == (int)FieldValue.WallBlock)
        {
            isActive = false;
            makeMino = false;
            GameoverWindow.SetActive(true);
        }

        // ゲームクリアの処理
        if (score >= 4000)
        {
            isActive = false;
            makeMino = false;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnRetry();
            }
        }
    }

   
    public void OnRetry()
    {
        GameoverWindow.SetActive(false);
        GameClearWindow.SetActive(false);
    }
}