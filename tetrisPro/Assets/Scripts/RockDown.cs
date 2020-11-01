public class RockDown
{
    enum FieldValue : int
    {
        Empty,
        MinoBlock,
        MinoBlock_Axis,
        WallBlock,
    }
    
    // アクティブなミノを固定されたミノに変換する
    public void SetMino(int[,] field)
    {
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                if (field[i,j] == (int)FieldValue.MinoBlock || field[i, j] == (int)FieldValue.MinoBlock_Axis)
                {
                    field[i, j] = (int)FieldValue.WallBlock;
                }
            }
        }
    }
}
