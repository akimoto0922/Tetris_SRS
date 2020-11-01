using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hold : MonoBehaviour
{
    Image m_Image;
    public Sprite[] m_Sprite;

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
        m_Image = gameObject.GetComponent<Image>();
    }

    public void HoldMino(int[,] field, ref int currentMino, ref int holdMino)
    {
        for (int i = 1; i < field.GetLength(0) - 1; i++)
        {
            for (int j = 1; j < field.GetLength(1) - 1; j++)
            {
                // fieldにある
                if (field[i, j] == (int)FieldValue.MinoBlock || field[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    field[i, j] = (int)FieldValue.Empty;
                }
            }
        }

        // ホールドの画像を入れ替える
        m_Image.sprite = m_Sprite[holdMino];
    }
}
