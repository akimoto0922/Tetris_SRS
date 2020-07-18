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
                if (field[i, j] == (int)FieldValue.MinoBlock || field[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    field[i, j] = (int)FieldValue.Empty;
                }
            }
        }

        switch (holdMino)
        {
            case 0: // T

                m_Image.sprite = m_Sprite[0];

                break;

            case 1: // S

                m_Image.sprite = m_Sprite[1];

                break;

            case 2: // Z

                m_Image.sprite = m_Sprite[2];

                break;

            case 3: // L

                m_Image.sprite = m_Sprite[3];

                break;

            case 4: // J

                m_Image.sprite = m_Sprite[4];

                break;

            case 5: // O

                m_Image.sprite = m_Sprite[5];

                break;

            case 6: // I

                m_Image.sprite = m_Sprite[6];

                break;

            case 7: // Null

                m_Image.sprite = m_Sprite[7];

                break;
        }
    }
}
