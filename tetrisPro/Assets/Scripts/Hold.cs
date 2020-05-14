using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hold : MonoBehaviour
{
    public GameObject gamemanager;
    public int currentMino;
    public int holdMino;

    Image m_Image;
    public Sprite[] m_Sprite;
    // Start is called before the first frame update
    void Start()
    {
        m_Image = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        currentMino = gamemanager.GetComponent<GameManager>().currentMino;
        holdMino = gamemanager.GetComponent<GameManager>().holdMino;

        // スプライトを変更する処理
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
