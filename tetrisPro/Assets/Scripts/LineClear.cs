using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineClear : MonoBehaviour
{
    enum FieldValue : int
    {
        Empty,
        MinoBlock,
        MinoBlock_Axis,
        WallBlock,
    }
    
    public void LineDelete(int[,]field, ref int score)
    {
        for (int i = 1; i < field.GetLength(1) - 1; i++)
        {
            if (field[1, i] == (int)FieldValue.WallBlock)
            {
                int count = 0;
                for (int j = 1; j < field.GetLength(0) - 1; j++)
                {
                    if (field[j, i] == (int)FieldValue.WallBlock)
                    {
                        count++;
                    }
                }
                if (count == 10)
                {
                    for (int k = 1; k < field.GetLength(0) - 1; k++)
                    {
                        score += 10;
                        field[k, i] = (int)FieldValue.Empty;
                    }
                    LineShift(field);
                }
                count = 0;
            }
        }
    }

    void LineShift(int[,]field)
    {
        for (int i = 1; i < field.GetLength(1) - 1; i++)
        {
            if (field[1, i] == (int)FieldValue.Empty)
            {
                int count = 0;
                for (int j = 1; j < field.GetLength(0) - 1; j++)
                {
                    if (field[j, i] == (int)FieldValue.Empty)
                    {
                        count++;
                    }
                }
                if (count == 10)
                {
                    for (int k = 1; k < field.GetLength(0) - 1; k++)
                    {
                        field[k, i] = field[k, i + 1];
                        field[k, i + 1] = (int)FieldValue.Empty;
                    }
                }
                count = 0;
            }
        }
        // sound1.PlayOneShot(sound3.clip);
    }
}
