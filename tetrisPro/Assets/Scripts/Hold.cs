using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tetris.Mino;
using Tetris.Rotation;

public class Hold : MonoBehaviour
{
    Image m_Image;
    public Sprite[] m_Sprite;

    void Awake()
    {
        m_Image = gameObject.GetComponent<Image>();
    }

    public void HoldMino(int[,] field, ref MinoData.eMinoType currentMino, ref MinoData.eMinoType holdMino)
    {
        for (int i = 1; i < field.GetLength(0) - 1; i++)
        {
            for (int j = 1; j < field.GetLength(1) - 1; j++)
            {
                // fieldにある
                if (field[i, j] == (int)Rotation.eFieldValue.MinoBlock)
                {
                    field[i, j] = (int)Rotation.eFieldValue.Empty;
                }
            }
        }

        // ホールドの画像を入れ替える
        m_Image.sprite = m_Sprite[(int)holdMino];
    }
}
