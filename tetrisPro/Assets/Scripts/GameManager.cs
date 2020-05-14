using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject EmptyBlock = null;    　   // 空いてる部分
    [SerializeField] GameObject MinoBlock = null;    　    // ミノの部分
    [SerializeField] GameObject FlameBlock = null;    　   // 枠

    [SerializeField] GameObject GameoverWindow = null;
    [SerializeField] GameObject GameClearWindow = null;

    [SerializeField] public bool makeMino = false;  　// ミノの生成
    [SerializeField] bool moveLeft = true;　　// 左に壁があるかどうか
    [SerializeField] bool moveRight = true;　 // 右に壁があるかどうか
    [SerializeField] float count = 0f;
    [SerializeField] float checkcount = 0f;
    [SerializeField] float overTime = 3.0f;
    [SerializeField] float overTime2 = 0.8f;

    AudioSource sound1;
    AudioSource sound2;
    AudioSource sound3;

    public int firstNum;
    public int score;        // スコア
    public int currentMino;  // 現在のミノ
    public int nextMino;     // 次のミノ
    public int holdMino;     // ホールド中のミノ
    public int minoDirection;// ミノの向き
    bool isGround;　　       // 空中にいるかかどうか
    public bool isActive = true;    // 操作可能かどうか
    bool isHold = false;     // ホールド中かどうか

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

    public int rotateCount; // 回転した回数
    public int minoType;　　// ミノの種類

    public GameObject script;
    Rotation Script;

    enum MinoType : int    // ミノは７種類 + Null で計８種類
    {
        T,     // 0
        S,     // 1
        Z,     // 2
        L,     // 3
        J,     // 4
        O,     // 5
        I,     // 6
        Null,  // 7
    }

    enum FieldValue : int    // ミノは７種類 + Null で計８種類
    {
        Empty,            // 0
        MinoBlock,        // 1
        MinoBlock_Axis,   // 2
        WallBlock,        // 3
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        sound1 = audioSources[0];
        sound2 = audioSources[1];
        sound3 = audioSources[2];

        Script = script.GetComponent<Rotation>();
        score = 0;

        // ホールドを初期化
        holdMino = (int) MinoType.Null;

        // 枠を作る
        var field_bottom = 0;
        var field_top = 21;
        var field_leftWall = 0;
        var field_rightWall = 11;    
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                if (i == field_leftWall || i == field_rightWall)
                {
                    field[i, j] = (int)FieldValue.WallBlock;
                    Instantiate(FlameBlock, new Vector3(i, j, 75), Quaternion.identity);
                }
                if (j == field_bottom || j == field_top)
                {
                    field[i, j] = (int)FieldValue.WallBlock;
                    Instantiate(FlameBlock, new Vector3(i, j, 75), Quaternion.identity);
                }
            }
        }

        switch (firstNum)
        {
            case 0: // T

                currentMino = (int)MinoType.T;

                field[4, 19] = 1; // □■□
                field[5, 19] = 2; // ■■■
                field[5, 20] = 1; // □□□
                field[6, 19] = 1;

                break;

            case 1: // S

                currentMino = (int)MinoType.S;

                field[4, 19] = 1; // □■■
                field[5, 19] = 2; // ■■□
                field[5, 20] = 1; // □□□
                field[6, 20] = 1;

                break;

            case 2: // Z

                currentMino = (int)MinoType.Z;

                field[4, 20] = 1; // ■■□
                field[5, 19] = 2; // □■■
                field[5, 20] = 1; // □□□
                field[6, 19] = 1;

                break;

            case 3: // L

                currentMino = (int)MinoType.L;

                field[4, 19] = 1; // □□■
                field[5, 19] = 2; // ■■■
                field[6, 19] = 1; // □□□
                field[6, 20] = 1;

                break;

            case 4: // J

                currentMino = (int)MinoType.J;

                field[4, 19] = 1; // ■□□
                field[5, 19] = 2; // ■■■
                field[4, 20] = 1; // □□□
                field[6, 19] = 1;

                break;

            case 5: // O

                currentMino = (int)MinoType.O;

                field[4, 19] = 1; // ■■□
                field[5, 19] = 1; // ■■□
                field[4, 20] = 1; // □□□
                field[5, 20] = 1;

                break;

            case 6: // I

                currentMino = (int)MinoType.I;

                field[4, 19] = 1; // □□□□
                field[5, 19] = 2; // ■■■■
                field[6, 19] = 1; // □□□□
                field[7, 19] = 1; // □□□□

                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        // cubeを出したり消したりして描画してる。
        Delete();
        Draw();

        if (isActive == true)
        {
            // 次のミノを出す
            if (makeMino == true)
            {
                minoDirection = 0;
                Create();
                makeMino = false;
            }
            // 右回転
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (minoDirection == 3)
                {
                    minoDirection = 0;
                }
                else
                {
                    minoDirection++;
                }
                Script.RightSpin();
                sound1.PlayOneShot(sound1.clip);
            }
            // 左回転
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (minoDirection == 0)
                {
                    minoDirection = 3;
                }
                else
                {
                    minoDirection--;
                }
                Script.LeftSpin();
                sound1.PlayOneShot(sound1.clip);
            }
            // 右移動
            if (Input.GetKeyDown(KeyCode.D))
            {
                CheckRight();
                if (moveRight == true)
                {
                    MoveRight();
                }
            }
            // 左移動
            if (Input.GetKeyDown(KeyCode.A))
            {
                CheckLeft();
                if (moveLeft == true)
                {
                    MoveLeft();
                }
            }
            // 下移動
            if (Input.GetKey(KeyCode.S) && isGround == false)
            {
                count = 0; // 自由落下の処理が被るとバグるのを防ぐ
                Fall();
            }
            // ハードドロップ
            if (Input.GetKeyDown(KeyCode.W))
            {
                while (!makeMino)
                {
                    CheckUnder();

                    if (isGround == true)
                    {
                        Put();
                        checkcount = 0;
                    }
                    Fall();
                }
                sound1.PlayOneShot(sound1.clip);
            }
            // ホールド
            if (Input.GetKeyDown(KeyCode.Q) && isHold == false)
            {
                minoDirection = 0;
                Hold();
                isHold = true;
                sound2.PlayOneShot(sound2.clip);
            }
        }


        // 落下
        count += Time.deltaTime;
        if (count >= overTime && isGround == false)
        {
            Fall();
            count = 0;
        }

        // 設置までの時間
        if (isGround == true && makeMino == false)
        {
            checkcount += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
            {
                checkcount -= 0.01f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                checkcount += 0.01f;
            }
            if (checkcount >= overTime2)
            {
                Put();
                checkcount = 0;
            }
        }

        // 設置判定 → 今はここで makeMino = true にしている
        CheckUnder();
        
        // 消えるかどうかの処理
        Clear();
        // ゲームオーバーの判定
        if (field[5, 19] == 3 || field[6, 19] == 3)
        {
            isActive = false;
            makeMino = false;
            GameoverWindow.SetActive(true);
        }


        // ゲームクリアの処理
        if (score >= 4000)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnRetry();
            }
        }
    }

    // 描画系の処理
    void Fall()
    {

        for (int i = 0; i < field.GetLength(0); i++)
        {

            for (int j = 0; j < field.GetLength(1); j++)
            {
                if (field[i, j] == (int)FieldValue.MinoBlock)
                {
                    field[i, j] = (int)FieldValue.Empty;
                    field[i, j - 1] = (int)FieldValue.MinoBlock;
                }
                if (field[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    field[i, j] = (int)FieldValue.Empty;
                    field[i, j - 1] = (int)FieldValue.MinoBlock_Axis;
                }
            }
        }
    }
    void Draw()
    {
        for (int i = 0; i < field.GetLength(0); i++)
        {

            for (int j = 0; j < field.GetLength(1); j++)
            {
                if (field[i, j] == (int)FieldValue.Empty)
                {
                    Instantiate(EmptyBlock, new Vector3(i, j, 75), Quaternion.identity);
                }
                if (field[i, j] == (int)FieldValue.MinoBlock || field[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    Instantiate(MinoBlock, new Vector3(i, j, 75), Quaternion.identity);
                }
            }
        }
    }
    void Delete()
    {
        GameObject[] DeleteBlocks = GameObject.FindGameObjectsWithTag("DeleteBlocks");

        foreach (GameObject cube in DeleteBlocks)
        {
            Destroy(cube);
        }
    }
    // 
    void Put()
    {
        for (int i = 0; i < field.GetLength(0); i++)
        {

            for (int j = 1; j < field.GetLength(1); j++)
            {
                if (field[i, j] == (int)FieldValue.MinoBlock || field[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    field[i, j] = (int)FieldValue.WallBlock;
                }
                
            }
        }
        makeMino = true;
        isHold = false;
    }
    void CheckUnder()
    {
        int count = 0;

        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = field.GetLength(1) - 1; j > 0; j--)
            {

                if (field[i, j] == (int)FieldValue.MinoBlock || field[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    if (field[i, j - 1] == (int)FieldValue.WallBlock)
                    {
                        isGround = true;
                        count++;
                    }
                    else
                    {
                        isGround = false;
                    }
                }
            }
            if (count >= 1)
            {
                break;
            } 
        }
    }
    void CheckRight()
    {
        int count = 0;

        for (int i = 0; i < field.GetLength(1); i++)
        {

            for (int j = 0; j < field.GetLength(0); j++)
            {

                if (field[j, i] == (int)FieldValue.MinoBlock || field[j, i] == (int)FieldValue.MinoBlock_Axis)
                {
                    if (field[j + 1, i] == (int)FieldValue.WallBlock)
                    {
                        moveRight = false;
                        count++;
                    }
                    else
                    {
                        moveRight = true;
                    }
                }
            }

            if (count >= 1)
            {
                break;
            }
        }
    }
    void CheckLeft()
    {
        int count = 0;

        for (int i = 0; i < field.GetLength(1); i++)
        {
            for (int j = field.GetLength(0) - 1; j > 0; j--)
            {

                if (field[j, i] == 1 || field[j, i] == 2)
                {
                    if (field[j - 1, i] == 3)
                    {
                        moveLeft = false;
                        count++;
                    }
                    else
                    {
                        moveLeft = true;
                    }
                }
            }

            if (count >= 1)
            {
                break;
            }
        }
    }
    void MoveRight()
    {
        for (int i = field.GetLength(0) - 1; i > 0; i--)
        {

            for (int j = 0; j < field.GetLength(1); j++)
            {
                if (field[i, j] == 1)
                {
                    field[i, j] = 0;
                    field[i + 1, j] = 1;
                }
                if (field[i, j] == 2)
                {
                    field[i, j] = 0;
                    field[i + 1, j] = 2;
                }
            }
        }
    }
    void MoveLeft()
    {

        for (int i = 0; i < field.GetLength(0); i++)
        {

            for (int j = 0; j < field.GetLength(1); j++)
            {
                if (field[i, j] == 1)
                {
                    field[i, j] = 0;
                    field[i - 1, j] = 1;
                }
                if (field[i, j] == 2)
                {
                    field[i, j] = 0;
                    field[i - 1, j] = 2;
                }
            }
        }
    }
    void Clear()
    {
        // 1列全部数値が3なら消してShiftを呼び出す
        // まずは左の1列目だけを探索する
        for (int i = 1; i < field.GetLength(1) - 1; i++)
        {
            if (field[1, i] == 3)
            {
                int count = 0;
                for (int s = 1; s < field.GetLength(0) - 1; s++)
                {
                    if (field[s, i] == 3)
                    {
                        count++;
                    }
                }
                if (count == 10)
                {
                    for (int k = 1; k < field.GetLength(0) - 1; k++)
                    {
                        field[k, i] = 0;
                    }
                    LineShift();
                    score += 100;
                }
                count = 0;
            }
        }
    }
    void LineShift()
    {
        // 1列全部空白な部分の１個上の配列を持ってくる
        for (int i = 1; i < field.GetLength(1) - 1; i++)
        {
            if (field[1, i] == 0)
            {
                int count = 0;
                for (int s = 1; s < field.GetLength(0) - 1; s++)
                {
                    if (field[s, i] == 0)
                    {
                        count++;
                    }
                }
                if (count == 10)
                {
                    for (int k = 1; k < field.GetLength(0) - 1; k++)
                    {
                        field[k, i] = field[k, i + 1];
                        field[k, i + 1] = 0;
                    }
                }
                count = 0;
            }
        }
        sound1.PlayOneShot(sound3.clip);
    }
    void Create()
    {
        switch (nextMino)
        {
            case 0: // T

                currentMino = (int)MinoType.T;

                field[4, 19] = 1; // □■□
                field[5, 19] = 2; // ■■■
                field[5, 20] = 1; // □□□
                field[6, 19] = 1;

                break;

            case 1: // S

                currentMino = (int)MinoType.S;

                field[4, 19] = 1; // □■■
                field[5, 19] = 2; // ■■□
                field[5, 20] = 1; // □□□
                field[6, 20] = 1;

                break;

            case 2: // Z

                currentMino = (int)MinoType.Z;

                field[4, 20] = 1; // ■■□
                field[5, 19] = 2; // □■■
                field[5, 20] = 1; // □□□
                field[6, 19] = 1;

                break;

            case 3: // L

                currentMino = (int)MinoType.L;

                field[4, 19] = 1; // □□■
                field[5, 19] = 2; // ■■■
                field[6, 19] = 1; // □□□
                field[6, 20] = 1;

                break;

            case 4: // J

                currentMino = (int)MinoType.J;

                field[4, 19] = 1; // ■□□
                field[5, 19] = 2; // ■■■
                field[4, 20] = 1; // □□□
                field[6, 19] = 1;

                break;

            case 5: // O

                currentMino = (int)MinoType.O;

                field[4, 19] = 1; // ■■□
                field[5, 19] = 1; // ■■□
                field[4, 20] = 1; // □□□
                field[5, 20] = 1;

                break;

            case 6: // I

                currentMino = (int)MinoType.I;

                field[4, 19] = 1; // □□□□
                field[5, 19] = 2; // ■■■■
                field[6, 19] = 1; // □□□□
                field[7, 19] = 1; // □□□□

                break;

        }
    }
    void HoldCreate()
    {
        int createNum;
        createNum = holdMino;
        holdMino = currentMino;

        switch (createNum)
        {
            case 0: // T

                currentMino = 0;

                field[4, 19] = 1; // □■□
                field[5, 19] = 2; // ■■■
                field[5, 20] = 1; // □□□
                field[6, 19] = 1;

                break;

            case 1: // S

                currentMino = (int)MinoType.S;

                field[4, 19] = 1; // □■■
                field[5, 19] = 2; // ■■□
                field[5, 20] = 1; // □□□
                field[6, 20] = 1;

                break;

            case 2: // Z

                currentMino = (int)MinoType.Z;

                field[4, 20] = 1; // ■■□
                field[5, 19] = 2; // □■■
                field[5, 20] = 1; // □□□
                field[6, 19] = 1;

                break;

            case 3: // L

                currentMino = (int)MinoType.L;

                field[4, 19] = 1; // □□■
                field[5, 19] = 2; // ■■■
                field[6, 19] = 1; // □□□
                field[6, 20] = 1;

                break;

            case 4: // J

                currentMino = (int)MinoType.J;

                field[4, 19] = 1; // ■□□
                field[5, 19] = 2; // ■■■
                field[4, 20] = 1; // □□□
                field[6, 19] = 1;

                break;

            case 5: // O

                currentMino = (int)MinoType.O;

                field[4, 19] = 1; // ■■□
                field[5, 19] = 1; // ■■□
                field[4, 20] = 1; // □□□
                field[5, 20] = 1;

                break;

            case 6: // I

                currentMino = (int)MinoType.I;

                field[4, 19] = 1; // □□□□
                field[5, 19] = 2; // ■■■■
                field[6, 19] = 1; // □□□□
                field[7, 19] = 1;

                break;

        }
    }
    void Hold()
    {
        for (int i = 1; i < field.GetLength(0) - 1; i++)
        {

            for (int j = 1; j < field.GetLength(1) - 1; j++)
            {
                if (field[i, j] == 1 || field[i, j] == 2)
                {
                    field[i, j] = 0;
                }
            }
        }


        if (holdMino == (int)MinoType.Null)
        {
            holdMino = currentMino;
            makeMino = true;
        }
        else
        {
            HoldCreate();
        }
    }
    public void OnRetry()
    {
        GameoverWindow.SetActive(false);
        GameClearWindow.SetActive(false);

        for (int i = 1; i < field.GetLength(0) - 1; i++)
        {

            for (int j = 1; j < field.GetLength(1) - 1; j++)
            {
                field[i, j] = 0;
            }
        }
        score = 0;
        holdMino = 7;
        makeMino = true;
        isActive = true;
    }
}