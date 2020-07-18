using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Next : MonoBehaviour
{
    public GameObject next1;        // nextを表示するオブジェクト
    public GameObject next2;
    public GameObject next3;
    public GameObject next4;
    public GameObject next5;
    bool makeMino;
    int[] nextMino = new int[14];
    int[] minoBox = new int[7];

    Image m_Image;
    Image Image_next1;
    Image Image_next2;
    Image Image_next3;
    Image Image_next4;
    Image Image_next5;

    public Sprite[] m_Sprite;
    // Start is called before the first frame update
    void Awake()
    {
        m_Image = gameObject.GetComponent<Image>();
        Image_next1 = next1.GetComponent<Image>();
        Image_next2 = next2.GetComponent<Image>();
        Image_next3 = next3.GetComponent<Image>();
        Image_next4 = next4.GetComponent<Image>();
        Image_next5 = next5.GetComponent<Image>();

        // ネクストミノの生成　→　7個 * 2回で14個の配列に値を入れる
        MinoDraw();
        for (int i = 0; i < minoBox.Length; i++)
        {
            nextMino[i] = minoBox[i];
        }
        MinoDraw();
        for (int i = 0; i < minoBox.Length; i++)
        {
            nextMino[i + 7] = minoBox[i];
        }

    }

    // ガチャの中身を作る
    void MinoDraw()
    {
        for (int i = 0; i < minoBox.Length; i++)
        {
            minoBox[i] = i;
        }
        for (int i = 0; i < 10; i++)
        {
            var num1 = Random.Range(0, 7);
            var num2 = Random.Range(0, 7);
            var copy = minoBox[num1];

            minoBox[num1] = minoBox[num2];
            minoBox[num2] = copy;
        }
    }

    public void NextChanger(ref int currentMino,ref int nextMino)
    {
        currentMino = this.nextMino[0];
        nextMino = this.nextMino[1];
        // nextMinoの中身を繰り上げ配列の最後を空にする
        var empty = 7;
        for (int i = 0; i < this.nextMino.Length - 1; i++)
        {
            this.nextMino[i] = this.nextMino[i + 1];
        }
        this.nextMino[13] = empty;

        // 7個消費したらネクストミノを補充
        if (this.nextMino[7] == empty)
        {
            MinoDraw();
            for (int i = 0; i < minoBox.Length; i++)
            {
                this.nextMino[7 + i] = minoBox[i];
            }
        }
        // スプライトを変更する処理
        switch (this.nextMino[0])
        {
            case 0: // T

                Image_next1.sprite = m_Sprite[0];

                break;

            case 1: // S

                Image_next1.sprite = m_Sprite[1];

                break;

            case 2: // Z

                Image_next1.sprite = m_Sprite[2];

                break;

            case 3: // L

                Image_next1.sprite = m_Sprite[3];

                break;

            case 4: // J

                Image_next1.sprite = m_Sprite[4];

                break;

            case 5: // O

                Image_next1.sprite = m_Sprite[5];

                break;

            case 6: // I

                Image_next1.sprite = m_Sprite[6];

                break;
        }
        switch (this.nextMino[1])
        {
            case 0: // T

                Image_next2.sprite = m_Sprite[0];

                break;

            case 1: // S

                Image_next2.sprite = m_Sprite[1];

                break;

            case 2: // Z

                Image_next2.sprite = m_Sprite[2];

                break;

            case 3: // L

                Image_next2.sprite = m_Sprite[3];

                break;

            case 4: // J

                Image_next2.sprite = m_Sprite[4];

                break;

            case 5: // O

                Image_next2.sprite = m_Sprite[5];

                break;

            case 6: // I

                Image_next2.sprite = m_Sprite[6];

                break;
        }
        switch (this.nextMino[2])
        {
            case 0: // T

                Image_next3.sprite = m_Sprite[0];

                break;

            case 1: // S

                Image_next3.sprite = m_Sprite[1];

                break;

            case 2: // Z

                Image_next3.sprite = m_Sprite[2];

                break;

            case 3: // L

                Image_next3.sprite = m_Sprite[3];

                break;

            case 4: // J

                Image_next3.sprite = m_Sprite[4];

                break;

            case 5: // O

                Image_next3.sprite = m_Sprite[5];

                break;

            case 6: // I

                Image_next3.sprite = m_Sprite[6];

                break;
        }
        switch (this.nextMino[3])
        {
            case 0: // T

                Image_next4.sprite = m_Sprite[0];

                break;

            case 1: // S

                Image_next4.sprite = m_Sprite[1];

                break;

            case 2: // Z

                Image_next4.sprite = m_Sprite[2];

                break;

            case 3: // L

                Image_next4.sprite = m_Sprite[3];

                break;

            case 4: // J

                Image_next4.sprite = m_Sprite[4];

                break;

            case 5: // O

                Image_next4.sprite = m_Sprite[5];

                break;

            case 6: // I

                Image_next4.sprite = m_Sprite[6];

                break;
        }
        switch (this.nextMino[4])
        {
            case 0: // T

                Image_next5.sprite = m_Sprite[0];

                break;

            case 1: // S

                Image_next5.sprite = m_Sprite[1];

                break;

            case 2: // Z

                Image_next5.sprite = m_Sprite[2];

                break;

            case 3: // L

                Image_next5.sprite = m_Sprite[3];

                break;

            case 4: // J

                Image_next5.sprite = m_Sprite[4];

                break;

            case 5: // O

                Image_next5.sprite = m_Sprite[5];

                break;

            case 6: // I

                Image_next5.sprite = m_Sprite[6];

                break;
        }
    }
}
