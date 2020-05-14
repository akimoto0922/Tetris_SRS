using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] GameObject EmptyBlock = null;    　   // 空いてる部分
    [SerializeField] GameObject MinoBlock = null;    　    // ミノの部分
    [SerializeField] GameObject FlameBlock = null;    　   // 枠

    float count = 0f;   // deltaTimeを入れる変数
    bool isGround;      // 空中にいるかどうか
    public int minoDirection = 0;         // ミノの向き
    public int currentMino = 0;           // 現在のミノ
    public float fallInterval = 1.0f; // 落下速度
    public float setInterval = -1.0f; // 固定される速度

    public GameObject script;
    RotationBattle Script;

    enum FieldValue : int    // ミノは７種類 + Null で計８種類
    {
        Empty,            // 0
        MinoBlock,        // 1
        MinoBlock_Axis,   // 2
        WallBlock,        // 3
    }

    public int[,] field_player1 = new int[12, 22]
    {
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },

    };

    public int[,] field_player2 = new int[12, 22]
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
    // Start is called before the first frame update
    void Start()
    {
        Script = script.GetComponent<RotationBattle>();
        // 枠を作る
        var bottom = 0;
        var top = 21;
        var leftWall = 0;
        var rightWall = 11;
        for (int i = 0; i < field_player1.GetLength(0); i++)
        {
            for (int j = 0; j < field_player1.GetLength(1); j++)
            {
                if (i == leftWall || i == rightWall)
                {
                    field_player1[i, j] = (int)FieldValue.WallBlock;
                    Instantiate(FlameBlock, new Vector3(i - 12, j, 75), Quaternion.identity);

                    field_player2[i, j] = (int)FieldValue.WallBlock;
                    Instantiate(FlameBlock, new Vector3(i + 14, j, 75), Quaternion.identity);
                }
                if (j == bottom || j == top)
                {
                    field_player1[i, j] = (int)FieldValue.WallBlock;
                    Instantiate(FlameBlock, new Vector3(i - 12, j, 75), Quaternion.identity);

                    field_player2[i, j] = (int)FieldValue.WallBlock;
                    Instantiate(FlameBlock, new Vector3(i + 14, j, 75), Quaternion.identity);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Delete();
        Draw();

        // 落下処理と設置処理とライン消去
        CheckUnder();
        if (isGround == false)
        {
            count += Time.deltaTime;
            if (count >= fallInterval)
            {
                Fall();
                count = 0;
            }
        }
        else
        {
            count -= Time.deltaTime;
            if (count <= setInterval)
            {
                Set();　　　 // 設置する
                Clear();     // ライン消去
                count = 0;
            }
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
        }
    }

    // 描画系の処理
    void Fall()
    {

        for (int i = 0; i < field_player1.GetLength(0); i++)
        {

            for (int j = 0; j < field_player1.GetLength(1); j++)
            {
                if (field_player1[i, j] == (int)FieldValue.MinoBlock)
                {
                    field_player1[i, j] = (int)FieldValue.Empty;
                    field_player1[i, j - 1] = (int)FieldValue.MinoBlock;
                }
                if (field_player1[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    field_player1[i, j] = (int)FieldValue.Empty;
                    field_player1[i, j - 1] = (int)FieldValue.MinoBlock_Axis;
                }
            }
        }
    }
    void Draw()
    {
        for (int i = 0; i < field_player1.GetLength(0); i++)
        {

            for (int j = 0; j < field_player1.GetLength(1); j++)
            {
                if (field_player1[i, j] == (int)FieldValue.Empty)
                {
                    Instantiate(EmptyBlock, new Vector3(i - 12, j, 75), Quaternion.identity);
                }
                if (field_player1[i, j] == (int)FieldValue.MinoBlock || field_player1[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    Instantiate(MinoBlock, new Vector3(i - 12, j, 75), Quaternion.identity);
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
    void CheckUnder()
    {
        int count = 0; // 1回でもtrueならループを抜ける

        for (int i = 0; i < field_player1.GetLength(0); i++)
        {
            for (int j = field_player1.GetLength(1) - 1; j > 0; j--)
            {

                if (field_player1[i, j] == (int)FieldValue.MinoBlock || field_player1[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    if (field_player1[i, j - 1] == (int)FieldValue.WallBlock)
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
    void Set()
    {
        for (int i = 0; i < field_player1.GetLength(0); i++)
        {

            for (int j = 1; j < field_player1.GetLength(1); j++)
            {
                if (field_player1[i, j] == (int)FieldValue.MinoBlock || field_player1[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    field_player1[i, j] = (int)FieldValue.WallBlock;
                }

            }
        }
    }
    void Clear()
    {
        for (int i = 1; i < field_player1.GetLength(1) - 1; i++)
        {
            if (field_player1[1, i] == (int)FieldValue.WallBlock)
            {
                int count = 0;
                for (int s = 1; s < field_player1.GetLength(0) - 1; s++)
                {
                    if (field_player1[s, i] == (int)FieldValue.WallBlock)
                    {
                        count++;
                    }
                }
                if (count == 10)
                {
                    for (int k = 1; k < field_player1.GetLength(0) - 1; k++)
                    {
                        field_player1[k, i] = (int)FieldValue.Empty;
                    }
                    LineShift();
                }
                count = 0;
            }
        }
    }
    void LineShift()
    {
        // 1列全部空白な部分の１個上の配列を持ってくる
        for (int i = 1; i < field_player1.GetLength(1) - 1; i++)
        {
            if (field_player1[1, i] == (int)FieldValue.Empty)
            {
                int count = 0;
                for (int s = 1; s < field_player1.GetLength(0) - 1; s++)
                {
                    if (field_player1[s, i] == (int)FieldValue.Empty)
                    {
                        count++;
                    }
                }
                if (count == 10)
                {
                    for (int k = 1; k < field_player1.GetLength(0) - 1; k++)
                    {
                        field_player1[k, i] = field_player1[k, i + 1];
                        field_player1[k, i + 1] = (int)FieldValue.Empty;
                    }
                }
                count = 0;
            }
        }
    }
}