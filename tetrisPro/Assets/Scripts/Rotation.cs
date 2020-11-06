using Tetris.Mino;

namespace Tetris.Rotation
{
    using eMinoType = Tetris.Mino.MinoData.eMinoType;
    using eMinoAngle = Tetris.Mino.MinoData.eMinoAngle;

    public class Rotation
    {
        Move move = new Move();
        public enum eFieldValue : int
        {
            Empty,          // 空の部分
            MinoBlock,      // ミノブロック
            WallBlock,      // 固定されたブロック
        }

        public enum eRotateInfo : int
        {
            // 右回転
            _0To_90,
            _90To_180,
            _180To_270,
            _270To_360,
            // 左回転
            _360To_270,
            _270To_180,
            _180To_90,
            _90To_0
        }

        // ミノのタイプ、ミノの向きをもらって、配列の値を返す
        public void RotatedMino(int[,]field,  ref int[] minoArray, eMinoType currentMino, eMinoAngle minoAngle, ref int minoPosX, ref int minoPosY, bool isRightSpin)
        {
            int[] copyArray;
            const int TEST_NUM = 5;

            // 回転させる
            Rotate(out copyArray, currentMino, minoAngle);

            // SRS利用時の添え字を取得する
            int info = GetRotateInfo(minoAngle, isRightSpin);
            UnityEngine.Debug.Log("RotateInfo = " + (MinoData.eRotateInfo)info);


            // 壁と被らないかチェック
            for (int i = 0; i < TEST_NUM; i++)
            {
                UnityEngine.Debug.Log(i + " "+"回目のテスト");
                // minoPosを(x, y)にどれだけ動かすか、値を取得
                var x = (int)WallKickData.GetKickData(currentMino, (MinoData.eRotateInfo)info, i).x;
                var y = (int)WallKickData.GetKickData(currentMino, (MinoData.eRotateInfo)info, i).y;

                // ミノを取得した座標に動かす
                move.MoveMino(ref minoPosX, ref minoPosY, x, -y);

                // 壁と重なっていないかチェック
                if (move.CheckWall(field, copyArray, minoPosX, minoPosY, currentMino))
                {
                    // 回転後の配列の値を代入し処理を抜ける
                    for (int j = 0; j < minoArray.Length; j++)
                    {
                        minoArray[j] = copyArray[j];
                    }
                    return;
                }
                else
                {
                    // 失敗時は元の座標に戻してから再度計算。
                    UnityEngine.Debug.Log("座標をもとの位置に戻します。");
                    move.MoveMino(ref minoPosX, ref minoPosY, -x, y);
                }
            }
        }

        // ミノを回転させる
        void Rotate(out int[] res, eMinoType type, eMinoAngle angle)
        {
            int t = (int)type;
            int size, sideLength;

            // Iミノのみ特殊 
            if (type == eMinoType.I)
            {
                size = MinoData.I_MINO_SIZE;
                sideLength = MinoData.I_MINO_SIDE_LENGTH;
            }
            else if(type == eMinoType.O)
            {
                size = MinoData.O_MINO_SIZE;
                sideLength = MinoData.O_MINO_SIDE_LENGTH;
            }
            else
            {
                size = MinoData.MINO_SIZE;
                sideLength = MinoData.MINO_SIDE_LENGTH;
            }

            res = new int[size];

            for (int i = 0; i < size; ++i)
            {
                int x, y;
                x = i % sideLength;
                y = i / sideLength;

                res[i] = MinoData.MinoArrays[t][GetRotatedIndex(x, y, angle, sideLength)];
            }
        }

        // 回転時の添え字を取得する（Rotate関数内で使用）
        int GetRotatedIndex(int x, int y, eMinoAngle angle, int side)
        {
            switch (angle)
            {
                case eMinoAngle.ang0: return x + (side * y);
                case eMinoAngle.ang90: return (side * (side - 1)) - (side * x) + y;
                case eMinoAngle.ang180: return ((side * side) - 1) - x - (side * y);
                case eMinoAngle.ang270: return (side - 1) + (side * x) - y;
                default: return -1;
            }
        }

        // SRSのテスト時の添え字を取得する
        int GetRotateInfo(eMinoAngle angle, bool isRightSpin) 
        {
            if (isRightSpin)
            {
                switch (angle)
                {
                    case eMinoAngle.ang0:   return (int)eRotateInfo._270To_360;
                    case eMinoAngle.ang90:  return (int)eRotateInfo._0To_90;
                    case eMinoAngle.ang180: return (int)eRotateInfo._90To_180;
                    case eMinoAngle.ang270: return (int)eRotateInfo._180To_270;
                    default:                return -1;
                }
            }
            else
            {
                switch (angle)
                {
                    case eMinoAngle.ang0:   return (int)eRotateInfo._90To_0;
                    case eMinoAngle.ang90:  return (int)eRotateInfo._180To_90;
                    case eMinoAngle.ang180: return (int)eRotateInfo._270To_180;
                    case eMinoAngle.ang270: return (int)eRotateInfo._360To_270;
                    default:                return -1;
                }
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------
        /*
        bool canRotate;     // 回転可能かどうか

        public void RightSpin(ref int[,] field, int currentMino, ref int minoDirection)
        {
            if (currentMino == (int)eMinoType.O)
            {
                // O(オー)ミノは回転しないのでテストは行わない。
                return;
            }
            else if (currentMino == (int)eMinoType.I) // Iミノ
            {
                switch (minoDirection)
                {
                    case (int)eMinoAngle.ang0:
                        IminoTest_0_90(ref field, ref minoDirection);
                        break;

                    case (int)eMinoAngle.ang90:
                        IminoTest_90_180(ref field, ref minoDirection);
                        break;

                    case (int)eMinoAngle.ang180:
                        IminoTest_180_270(ref field, ref minoDirection);
                        break;

                    case (int)eMinoAngle.ang270:
                        IminoTest_270_360(ref field, ref minoDirection);
                        break;
                }
            }
            else　// L,J,S,Z,Tミノ
            {
                switch (minoDirection)
                {
                    case (int)eMinoAngle.ang0:
                        //Test_0_90(ref field, ref minoDirection);
                        break;
                    case (int)eMinoAngle.ang90:
                        //Test_90_180(ref field, ref minoDirection);
                        break;
                    case (int)eMinoAngle.ang180:
                        //Test_180_270(ref field, ref minoDirection);
                        break;
                    case (int)eMinoAngle.ang270:
                        //Test_270_360(ref field, ref minoDirection);
                        break;
                }
            }
        }

        public void LeftSpin(ref int[,] field, int currentMino, ref int minoDirection)
        {
            if (currentMino == (int)eMinoType.O)
            {
                // O(オー)ミノは回転しないのでテストは行わない。
                return;
            }
            else if (currentMino == (int)eMinoType.I)  // Iミノ
            {
                switch (minoDirection)
                {
                    case 0:
                        IminoTest_360_270(ref field, ref minoDirection);
                        break;

                    case 1:
                        IminoTest_90_0(ref field, ref minoDirection);
                        break;

                    case 2:
                        IminoTest_180_90(ref field, ref minoDirection);
                        break;

                    case 3:
                        IminoTest_270_180(ref field, ref minoDirection);
                        break;
                }
            }
            else // L,J,S,Z,Tミノ
            {
                switch (minoDirection)
                {
                    case 0:
                        Test_360_270(ref field, ref minoDirection);
                        break;

                    case 1:
                        Test_90_0(ref field, ref minoDirection);

                        break;

                    case 2:
                        Test_180_90(ref field, ref minoDirection);
                        break;

                    case 3:
                        Test_270_180(ref field, ref minoDirection);
                        break;
                }
            }
        }

         //memo1
         //minoDirection はそのうち enum でしっかり書く。
         //0 → Up       生成されたときの初期位置
         //1 → Right
         //2 → Down
         //3 → Left


        // 右回転のテスト
        void Test_0_90(ref int[,] field, ref int minoDirection)
        {
            // 配列をコピー
            Copy(field, ref fieldArray, ref wallArray);

            // 回転させる
            RotateRight(ref fieldArray);

            Copy_RespownPos(fieldArray, ref initialPosArray);
            // テスト開始
            for (int i = 0; i < 5; i++)
            {
                UnityEngine.Debug.Log(i + 1 + "回目");
                switch (i)
                {
                    case 0:

                        break;

                    case 1:

                        ForcedMoveMino(ref fieldArray, -1, 0);
                        break;

                    case 2:

                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, -1, 1);
                        break;

                    case 3:

                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 0, -2);
                        break;

                    case 4:

                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, -1, -2);
                        break;
                }
                // 配列を比較
                Compare(fieldArray, wallArray);

                // 成功なら
                if (canRotate == true)
                {
                    minoDirection = 1;
                    // fieldのミノを回転させて描画
                    Rotate_field(ref field, fieldArray);
                    // コピーした配列の初期化
                    Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
                    UnityEngine.Debug.Log("回転成功！");
                    return;
                }
            }
            // すべて失敗したら
            Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
            UnityEngine.Debug.Log("回転できない。");
        }
        void Test_90_180(ref int[,] field, ref int minoDirection)
        {
            Copy(field, ref fieldArray, ref wallArray);
            RotateRight(ref fieldArray);
            Copy_RespownPos(fieldArray, ref initialPosArray);

            for (int i = 0; i < 5; i++)
            {
                UnityEngine.Debug.Log(i + 1 + "回目");
                switch (i)
                {
                    case 0:

                        break;

                    case 1:

                        ForcedMoveMino(ref fieldArray, 1, 0);
                        break;

                    case 2:

                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 1, -1);
                        break;

                    case 3:

                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 0, 2);
                        break;

                    case 4:

                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 1, 2);
                        break;
                }
                Compare(fieldArray, wallArray);

                if (canRotate == true)
                {
                    minoDirection = 2;
                    Rotate_field(ref field, fieldArray);
                    Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
                    UnityEngine.Debug.Log("回転成功！");
                    return;
                }
            }
            Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
            UnityEngine.Debug.Log("回転できない。");
        }
        void Test_180_270(ref int[,] field, ref int minoDirection)
        {
            Copy(field, ref fieldArray, ref wallArray);
            RotateRight(ref fieldArray);
            Copy_RespownPos(fieldArray, ref initialPosArray);

            for (int i = 0; i < 5; i++)
            {
                UnityEngine.Debug.Log(i + 1 + "回目");
                switch (i)
                {
                    case 0:

                        break;

                    case 1:
                        ForcedMoveMino(ref fieldArray, 1, 0);
                        break;

                    case 2:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 1, 1);
                        break;

                    case 3:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 0, -2);
                        break;

                    case 4:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 1, -2);
                        break;
                }
                Compare(fieldArray, wallArray);

                if (canRotate == true)
                {
                    minoDirection = 3;
                    Rotate_field(ref field, fieldArray);
                    Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
                    UnityEngine.Debug.Log("回転成功！");
                    return;
                }
            }
            Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
            UnityEngine.Debug.Log("回転できない。");
        }
        void Test_270_360(ref int[,] field, ref int minoDirection)
        {
            Copy(field, ref fieldArray, ref wallArray);
            RotateRight(ref fieldArray);
            Copy_RespownPos(fieldArray, ref initialPosArray);

            for (int i = 0; i < 5; i++)
            {
                UnityEngine.Debug.Log(i + 1 + "回目");
                switch (i)
                {
                    case 0:

                        break;

                    case 1:
                        ForcedMoveMino(ref fieldArray, -1, 0);
                        break;

                    case 2:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, -1, -1);
                        break;

                    case 3:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 0, 2);
                        break;

                    case 4:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, -1, 2);
                        break;
                }
                Compare(fieldArray, wallArray);

                if (canRotate == true)
                {
                    minoDirection = 0;
                    Rotate_field(ref field, fieldArray);
                    Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
                    UnityEngine.Debug.Log("回転成功！");
                    return;
                }
            }
            Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
            UnityEngine.Debug.Log("回転できない。");
        }

        // 左回転のテスト
        void Test_360_270(ref int[,] field, ref int minoDirection)
        {
            Copy(field, ref fieldArray, ref wallArray);
            RotateLeft(ref fieldArray);
            Copy_RespownPos(fieldArray, ref initialPosArray);

            for (int i = 0; i < 5; i++)
            {
                UnityEngine.Debug.Log(i + 1 + "回目");
                switch (i)
                {
                    case 0:

                        break;

                    case 1:
                        ForcedMoveMino(ref fieldArray, 1, 0);
                        break;

                    case 2:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 1, 1);
                        break;

                    case 3:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 0, -2);
                        break;

                    case 4:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 1, -2);
                        break;
                }
                Compare(fieldArray, wallArray);

                if (canRotate == true)
                {
                    minoDirection = 3;
                    Rotate_field(ref field, fieldArray);
                    Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
                    UnityEngine.Debug.Log("回転成功！");
                    return;
                }
            }
            Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
            UnityEngine.Debug.Log("回転できない。");
        }
        void Test_270_180(ref int[,] field, ref int minoDirection)
        {
            Copy(field, ref fieldArray, ref wallArray);
            RotateLeft(ref fieldArray);
            Copy_RespownPos(fieldArray, ref initialPosArray);

            for (int i = 0; i < 5; i++)
            {
                UnityEngine.Debug.Log(i + 1 + "回目");
                switch (i)
                {
                    case 0:

                        break;

                    case 1:
                        ForcedMoveMino(ref fieldArray, -1, 0);
                        break;

                    case 2:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, -1, -1);
                        break;

                    case 3:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 0, 2);
                        break;

                    case 4:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, -1, 2);
                        break;
                }
                Compare(fieldArray, wallArray);

                if (canRotate == true)
                {
                    minoDirection = 2;
                    Rotate_field(ref field, fieldArray);
                    Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
                    UnityEngine.Debug.Log("回転成功！");
                    return;
                }
            }
            Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
            UnityEngine.Debug.Log("回転できない。");
        }
        void Test_180_90(ref int[,] field, ref int minoDirection)
        {
            Copy(field, ref fieldArray, ref wallArray);
            RotateLeft(ref fieldArray);
            Copy_RespownPos(fieldArray, ref initialPosArray);

            for (int i = 0; i < 5; i++)
            {
                UnityEngine.Debug.Log(i + 1 + "回目");
                switch (i)
                {
                    case 0:

                        break;

                    case 1:
                        ForcedMoveMino(ref fieldArray, -1, 0);
                        break;

                    case 2:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, -1, 1);
                        break;

                    case 3:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 0, -2);
                        break;

                    case 4:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, -1, -2);
                        break;
                }
                Compare(fieldArray, wallArray);

                if (canRotate == true)
                {
                    minoDirection = 1;
                    Rotate_field(ref field, fieldArray);
                    Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
                    UnityEngine.Debug.Log("回転成功！");
                    return;
                }
            }
            Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
            UnityEngine.Debug.Log("回転できない。");
        }
        void Test_90_0(ref int[,] field, ref int minoDirection)
        {
            Copy(field, ref fieldArray, ref wallArray);
            RotateLeft(ref fieldArray);
            Copy_RespownPos(fieldArray, ref initialPosArray);

            for (int i = 0; i < 5; i++)
            {
                UnityEngine.Debug.Log(i + 1 + "回目");
                switch (i)
                {
                    case 0:

                        break;

                    case 1:
                        ForcedMoveMino(ref fieldArray, 1, 0);
                        break;

                    case 2:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 1, -1);
                        break;

                    case 3:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 0, 2);
                        break;

                    case 4:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 1, 2);
                        break;
                }
                Compare(fieldArray, wallArray);

                if (canRotate == true)
                {
                    minoDirection = 0;
                    Rotate_field(ref field, fieldArray);
                    Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
                    UnityEngine.Debug.Log("回転成功！");
                    return;
                }
            }
            Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
            UnityEngine.Debug.Log("回転できない。");
        }

        // Iミノのテスト 右回転
        void IminoTest_0_90(ref int[,] field, ref int minoDirection)
        {
            Copy(field, ref fieldArray, ref wallArray);
            IminoRotate_0_90(ref fieldArray);
            Copy_RespownPos(fieldArray, ref initialPosArray);

            for (int i = 0; i < 5; i++)
            {
                UnityEngine.Debug.Log(i + 1 + "回目");
                switch (i)
                {
                    case 0:

                        break;

                    case 1:

                        ForcedMoveMino(ref fieldArray, -2, 0);
                        break;

                    case 2:

                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 1, 0);
                        break;

                    case 3:

                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, -2, -1);
                        break;

                    case 4:

                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 1, 2);
                        break;
                }
                Compare(fieldArray, wallArray);

                if (canRotate == true)
                {
                    minoDirection = 1;
                    Rotate_field(ref field, fieldArray);
                    Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
                    UnityEngine.Debug.Log("回転成功！");
                    return;
                }
            }
            Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
            UnityEngine.Debug.Log("回転できない。");
        }
        void IminoTest_90_180(ref int[,] field, ref int minoDirection)
        {
            Copy(field, ref fieldArray, ref wallArray);
            IminoRotate_90_180(ref fieldArray);
            Copy_RespownPos(fieldArray, ref initialPosArray);

            for (int i = 0; i < 5; i++)
            {
                UnityEngine.Debug.Log(i + 1 + "回目");
                switch (i)
                {
                    case 0:

                        break;

                    case 1:

                        ForcedMoveMino(ref fieldArray, -1, 0);
                        break;

                    case 2:

                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 2, 0);
                        break;

                    case 3:

                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, -1, 2);
                        break;

                    case 4:

                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 2, -1);
                        break;
                }
                Compare(fieldArray, wallArray);

                if (canRotate == true)
                {
                    minoDirection = 2;
                    Rotate_field(ref field, fieldArray);
                    Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
                    UnityEngine.Debug.Log("回転成功！");
                    return;
                }
            }
            Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
            UnityEngine.Debug.Log("回転できない。");
        }
        void IminoTest_180_270(ref int[,] field, ref int minoDirection)
        {
            Copy(field, ref fieldArray, ref wallArray);
            IminoRotate_180_270(ref fieldArray);
            Copy_RespownPos(fieldArray, ref initialPosArray);

            for (int i = 0; i < 5; i++)
            {
                UnityEngine.Debug.Log(i + 1 + "回目");
                switch (i)
                {
                    case 0:

                        break;

                    case 1:
                        ForcedMoveMino(ref fieldArray, 2, 0);
                        break;

                    case 2:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, -1, 0);
                        break;

                    case 3:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 2, 1);
                        break;

                    case 4:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, -1, -2);
                        break;
                }
                Compare(fieldArray, wallArray);

                if (canRotate == true)
                {
                    minoDirection = 3;
                    Rotate_field(ref field, fieldArray);
                    Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
                    UnityEngine.Debug.Log("回転成功！");
                    return;
                }
            }
            Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
            UnityEngine.Debug.Log("回転できない。");
        }
        void IminoTest_270_360(ref int[,] field, ref int minoDirection)
        {
            Copy(field, ref fieldArray, ref wallArray);
            IminoRotate_270_360(ref fieldArray);
            Copy_RespownPos(fieldArray, ref initialPosArray);

            for (int i = 0; i < 5; i++)
            {
                UnityEngine.Debug.Log(i + 1 + "回目");
                switch (i)
                {
                    case 0:

                        break;

                    case 1:
                        ForcedMoveMino(ref fieldArray, 1, 0);
                        break;

                    case 2:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, -2, 0);
                        break;

                    case 3:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 1, -2);
                        break;

                    case 4:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, -2, 1);
                        break;
                }
                Compare(fieldArray, wallArray);

                if (canRotate == true)
                {
                    minoDirection = 0;
                    Rotate_field(ref field, fieldArray);
                    Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
                    UnityEngine.Debug.Log("回転成功！");
                    return;
                }
            }
            Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
            UnityEngine.Debug.Log("回転できない。");
        }

        // Iミノのテスト 左回転
        void IminoTest_360_270(ref int[,] field, ref int minoDirection)
        {
            Copy(field, ref fieldArray, ref wallArray);
            IminoRotate_360_270(ref fieldArray);
            Copy_RespownPos(fieldArray, ref initialPosArray);

            for (int i = 0; i < 5; i++)
            {
                UnityEngine.Debug.Log(i + 1 + "回目");
                switch (i)
                {
                    case 0:

                        break;

                    case 1:
                        ForcedMoveMino(ref fieldArray, -1, 0);
                        break;

                    case 2:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 2, 0);
                        break;

                    case 3:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, -1, 2);
                        break;

                    case 4:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 2, -1);
                        break;
                }
                Compare(fieldArray, wallArray);

                if (canRotate == true)
                {
                    minoDirection = 3;
                    Rotate_field(ref field, fieldArray);
                    Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
                    UnityEngine.Debug.Log("回転成功！");
                    return;
                }
            }
            Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
            UnityEngine.Debug.Log("回転できない。");
        }
        void IminoTest_270_180(ref int[,] field, ref int minoDirection)
        {
            Copy(field, ref fieldArray, ref wallArray);
            IminoRotate_270_180(ref fieldArray);
            Copy_RespownPos(fieldArray, ref initialPosArray);

            for (int i = 0; i < 5; i++)
            {
                UnityEngine.Debug.Log(i + 1 + "回目");
                switch (i)
                {
                    case 0:

                        break;

                    case 1:
                        ForcedMoveMino(ref fieldArray, -2, 0);
                        break;

                    case 2:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 1, 0);
                        break;

                    case 3:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, -2, -1);
                        break;

                    case 4:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 1, 2);
                        break;
                }
                Compare(fieldArray, wallArray);

                if (canRotate == true)
                {
                    minoDirection = 2;
                    Rotate_field(ref field, fieldArray);
                    Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
                    UnityEngine.Debug.Log("回転成功！");
                    return;
                }
            }
            Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
            UnityEngine.Debug.Log("回転できない。");
        }
        void IminoTest_180_90(ref int[,] field, ref int minoDirection)
        {
            Copy(field, ref fieldArray, ref wallArray);
            IminoRotate_180_90(ref fieldArray);
            Copy_RespownPos(fieldArray, ref initialPosArray);

            for (int i = 0; i < 5; i++)
            {
                UnityEngine.Debug.Log(i + 1 + "回目");
                switch (i)
                {
                    case 0:

                        break;

                    case 1:
                        ForcedMoveMino(ref fieldArray, 1, 0);
                        break;

                    case 2:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, -2, 0);
                        break;

                    case 3:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 1, -2);
                        break;

                    case 4:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, -2, 1);
                        break;
                }
                Compare(fieldArray, wallArray);

                if (canRotate == true)
                {
                    minoDirection = 1;
                    Rotate_field(ref field, fieldArray);
                    Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
                    UnityEngine.Debug.Log("回転成功！");
                    return;
                }
            }
            Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
            UnityEngine.Debug.Log("回転できない。");
        }
        void IminoTest_90_0(ref int[,] field, ref int minoDirection)
        {
            Copy(field, ref fieldArray, ref wallArray);
            IminoRotate_90_0(ref fieldArray);
            Copy_RespownPos(fieldArray, ref initialPosArray);

            for (int i = 0; i < 5; i++)
            {
                UnityEngine.Debug.Log(i + 1 + "回目");
                switch (i)
                {
                    case 0:

                        break;

                    case 1:
                        ForcedMoveMino(ref fieldArray, 2, 0);
                        break;

                    case 2:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, -1, 0);
                        break;

                    case 3:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, 2, 1);
                        break;

                    case 4:
                        Respown(ref fieldArray, initialPosArray);
                        ForcedMoveMino(ref fieldArray, -1, -2);
                        break;
                }
                Compare(fieldArray, wallArray);

                if (canRotate == true)
                {
                    minoDirection = 0;
                    Rotate_field(ref field, fieldArray);
                    Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
                    UnityEngine.Debug.Log("回転成功！");
                    return;
                }
            }
            Initialize_All(ref wallArray, ref fieldArray, ref initialPosArray);
            UnityEngine.Debug.Log("回転できない。");
        }



        // テストで使う関数
        // <ミノの強制移動>
        void ForcedMoveMino(ref int[,] minoArray, int minoPos_x, int minoPos_y)
        {
            // ①回転後のミノの座標を仮置きの配列にコピーする
            int[,] copyArray = new int[12, 22];

            for (int i = 0; i < minoArray.GetLength(0); i++)
            {
                for (int j = 0; j < minoArray.GetLength(1); j++)
                {
                    if (minoArray[i, j] == (int)eFieldValue.MinoBlock || minoArray[i, j] == (int)eFieldValue.MinoBlock_Axis)
                    {
                        // 範囲外処理
                        var x = i + minoPos_x;
                        var y = j + minoPos_y;
                        if (x >= minoArray.GetLength(0) || 0 >= x || y >= minoArray.GetLength(1) || 0 >= y)
                        {
                            return;
                        }
                        copyArray[i + minoPos_x, j + minoPos_y] = minoArray[i, j];
                    }
                }
            }

            // ②minoArrayを初期化
            for (int i = 0; i < minoArray.GetLength(0); i++)
            {
                for (int j = 0; j < minoArray.GetLength(1); j++)
                {
                    if (minoArray[i, j] == (int)eFieldValue.MinoBlock || minoArray[i, j] == (int)eFieldValue.MinoBlock_Axis)
                    {
                        minoArray[i, j] = (int)eFieldValue.Empty;
                    }
                }
            }

            // ③仮置きの配列の値をminoArrayに代入する
            for (int i = 0; i < minoArray.GetLength(0); i++)
            {
                for (int j = 0; j < minoArray.GetLength(1); j++)
                {
                    if (copyArray[i, j] == (int)eFieldValue.MinoBlock || copyArray[i, j] == (int)eFieldValue.MinoBlock_Axis)
                    {
                        minoArray[i, j] = copyArray[i, j];
                    }
                }
            }

            // ④copyArrayを初期化
            for (int i = 0; i < minoArray.GetLength(0); i++)
            {
                for (int j = 0; j < minoArray.GetLength(1); j++)
                {
                    copyArray[i, j]　= 0;
                }
            }
        }

        // <コピー>
        void Copy(int[,] field, ref int[,] minoArray, ref int[,] wallArray)
        {
            // fieldのミノと壁をそれぞれ別の配列にコピーする
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    // ミノ
                    if (field[i, j] == (int)eFieldValue.MinoBlock || field[i, j] == (int)eFieldValue.MinoBlock_Axis)
                    {
                        minoArray[i, j] = field[i, j];
                    }

                    // 壁
                    if (field[i, j] == (int)eFieldValue.WallBlock)
                    {
                        wallArray[i, j] = field[i, j];
                    }
                }
            }
        }

        // <初期位置をコピー>
        void Copy_RespownPos(int[,] minoArray, ref int[,] initialPosArray)
        {
            // fieldのミノと壁をそれぞれ別の配列にコピーする
            for (int i = 0; i < minoArray.GetLength(0); i++)
            {
                for (int j = 0; j < minoArray.GetLength(1); j++)
                {
                    // ミノ
                    if (minoArray[i, j] == (int)eFieldValue.MinoBlock || minoArray[i, j] == (int)eFieldValue.MinoBlock_Axis)
                    {
                        initialPosArray[i, j] = minoArray[i, j];
                    }
                }
            }
        }

        // <比較>
        void Compare(int[,] minoArray, int[,] wallArray)
        {
            int count = 0;
            for (int i = 0; i < minoArray.GetLength(0); i++)
            {
                for (int j = 0; j < minoArray.GetLength(1); j++)
                {
                    if (minoArray[i, j] == (int)eFieldValue.MinoBlock_Axis && wallArray[i, j] == (int)eFieldValue.Empty)
                    {
                        count++;
                        UnityEngine.Debug.Log("count+1 回転軸");
                    }
                    if (minoArray[i, j] == (int)eFieldValue.MinoBlock && wallArray[i, j] == (int)eFieldValue.Empty)
                    {
                        count++;
                        UnityEngine.Debug.Log("count+1 ほかのブロック");
                    }
                }
            }
            if (count >= 4)
            {
                canRotate = true;
            }
            else
            {
                canRotate = false;
            }
        }

        // <初期位置へ移動>
        void Respown(ref int[,] minoArray, int[,] initialPosArray)
        {
            for (int i = 0; i < minoArray.GetLength(0); i++)
            {
                for (int j = 0; j < minoArray.GetLength(1); j++)
                {
                    minoArray[i, j] = initialPosArray[i, j];
                }
            }
        }

        // <回転関連>
        // ミノの回転
        void RotateRight(ref int[,] minoArray)
        {

            for (int i = 0; i < minoArray.GetLength(0); i++)
            {

                for (int j = 0; j < minoArray.GetLength(1); j++)
                {

                    if (minoArray[i, j] == (int)eFieldValue.MinoBlock_Axis)
                    {
                        array_mino_test[0, 0] = minoArray[i - 1, j + 1];
                        array_mino_test[0, 1] = minoArray[i, j + 1];
                        array_mino_test[0, 2] = minoArray[i + 1, j + 1];
                        array_mino_test[1, 2] = minoArray[i + 1, j];
                        array_mino_test[2, 2] = minoArray[i + 1, j - 1];
                        array_mino_test[2, 1] = minoArray[i, j - 1];
                        array_mino_test[2, 0] = minoArray[i - 1, j - 1];
                        array_mino_test[1, 0] = minoArray[i - 1, j];

                        minoArray[i + 1, j] = (int)eFieldValue.Empty;
                        minoArray[i + 1, j - 1] = (int)eFieldValue.Empty;
                        minoArray[i + 1, j + 1] = (int)eFieldValue.Empty;
                        minoArray[i, j + 1] = (int)eFieldValue.Empty;
                        minoArray[i - 1, j + 1] = (int)eFieldValue.Empty;
                        minoArray[i - 1, j] = (int)eFieldValue.Empty;
                        minoArray[i - 1, j - 1] = (int)eFieldValue.Empty;
                        minoArray[i, j - 1] = (int)eFieldValue.Empty;

                        minoArray[i + 1, j + 1] = array_mino_test[0, 0];
                        minoArray[i + 1, j] = array_mino_test[0, 1];
                        minoArray[i + 1, j - 1] = array_mino_test[0, 2];
                        minoArray[i, j - 1] = array_mino_test[1, 2];
                        minoArray[i - 1, j - 1] = array_mino_test[2, 2];
                        minoArray[i - 1, j] = array_mino_test[2, 1];
                        minoArray[i - 1, j + 1] = array_mino_test[2, 0];
                        minoArray[i, j + 1] = array_mino_test[1, 0];
                    }
                }
            }

            // 一時的に保存しとくための配列を使用後に初期化する
            for (int i = 0; i < array_mino_test.GetLength(0); i++)
            {
                for (int j = 0; j < array_mino_test.GetLength(1); j++)
                {
                    array_mino_test[i, j] = (int)eFieldValue.Empty;
                }
            }
        }
        void RotateLeft(ref int[,] minoArray)
        {
            for (int i = 0; i < minoArray.GetLength(0); i++)
            {

                for (int j = 0; j < minoArray.GetLength(1); j++)
                {

                    if (minoArray[i, j] == (int)eFieldValue.MinoBlock_Axis)
                    {
                        array_mino_test[0, 0] = minoArray[i - 1, j + 1];
                        array_mino_test[0, 1] = minoArray[i, j + 1];
                        array_mino_test[0, 2] = minoArray[i + 1, j + 1];
                        array_mino_test[1, 2] = minoArray[i + 1, j];
                        array_mino_test[2, 2] = minoArray[i + 1, j - 1];
                        array_mino_test[2, 1] = minoArray[i, j - 1];
                        array_mino_test[2, 0] = minoArray[i - 1, j - 1];
                        array_mino_test[1, 0] = minoArray[i - 1, j];

                        minoArray[i + 1, j] = (int)eFieldValue.Empty;
                        minoArray[i + 1, j - 1] = (int)eFieldValue.Empty;
                        minoArray[i + 1, j + 1] = (int)eFieldValue.Empty;
                        minoArray[i, j + 1] = (int)eFieldValue.Empty;
                        minoArray[i - 1, j + 1] = (int)eFieldValue.Empty;
                        minoArray[i - 1, j] = (int)eFieldValue.Empty;
                        minoArray[i - 1, j - 1] = (int)eFieldValue.Empty;
                        minoArray[i, j - 1] = (int)eFieldValue.Empty;

                        minoArray[i - 1, j - 1] = array_mino_test[0, 0];
                        minoArray[i - 1, j] = array_mino_test[0, 1];
                        minoArray[i - 1, j + 1] = array_mino_test[0, 2];
                        minoArray[i, j + 1] = array_mino_test[1, 2];
                        minoArray[i + 1, j + 1] = array_mino_test[2, 2];
                        minoArray[i + 1, j] = array_mino_test[2, 1];
                        minoArray[i + 1, j - 1] = array_mino_test[2, 0];
                        minoArray[i, j - 1] = array_mino_test[1, 0];
                    }
                }
            }

            // 一時的に保存しとくための配列を使用後に初期化する
            for (int i = 0; i < array_mino_test.GetLength(0); i++)
            {
                for (int j = 0; j < array_mino_test.GetLength(1); j++)
                {
                    array_mino_test[i, j] = (int)eFieldValue.Empty;
                }
            }
        }

        // 'Iミノ'の回転は8パターン(※回転というより、一度消して回転後の値を代入している。)
        // 右回転(4パターン)
        void IminoRotate_0_90(ref int[,] minoArray)
        {
            for (int i = minoArray.GetLength(0) - 1; i > 0; i--)
            {

                for (int j = 0; j < minoArray.GetLength(1); j++)
                {

                    if (minoArray[i, j] == (int)eFieldValue.MinoBlock_Axis)
                    {
                        minoArray[i, j] = (int)eFieldValue.Empty;
                        minoArray[i - 1, j] = (int)eFieldValue.Empty;
                        minoArray[i + 1, j] = (int)eFieldValue.Empty;
                        minoArray[i + 2, j] = (int)eFieldValue.Empty;

                        minoArray[i + 1, j + 1] = (int)eFieldValue.MinoBlock;
                        minoArray[i + 1, j] = (int)eFieldValue.MinoBlock_Axis;
                        minoArray[i + 1, j - 1] = (int)eFieldValue.MinoBlock;
                        minoArray[i + 1, j - 2] = (int)eFieldValue.MinoBlock;
                    }
                }
            }
        }
        void IminoRotate_90_180(ref int[,] minoArray)
        {
            for (int i = 0; i < minoArray.GetLength(0); i++)
            {

                for (int j = 0; j < minoArray.GetLength(1); j++)
                {

                    if (minoArray[i, j] == (int)eFieldValue.MinoBlock_Axis)
                    {
                        minoArray[i, j] = (int)eFieldValue.Empty;
                        minoArray[i, j + 1] = (int)eFieldValue.Empty;
                        minoArray[i, j - 1] = (int)eFieldValue.Empty;
                        minoArray[i, j - 2] = (int)eFieldValue.Empty;

                        minoArray[i - 2, j - 1] = (int)eFieldValue.MinoBlock;
                        minoArray[i - 1, j - 1] = (int)eFieldValue.MinoBlock;
                        minoArray[i, j - 1] = (int)eFieldValue.MinoBlock_Axis;
                        minoArray[i + 1, j - 1] = (int)eFieldValue.MinoBlock;

                    }
                }
            }
        }
        void IminoRotate_180_270(ref int[,] minoArray)
        {
            for (int i = 0; i < minoArray.GetLength(0); i++)
            {

                for (int j = 0; j < minoArray.GetLength(1); j++)
                {

                    if (minoArray[i, j] == (int)eFieldValue.MinoBlock_Axis)
                    {
                        minoArray[i, j] = (int)eFieldValue.Empty;
                        minoArray[i - 2, j] = (int)eFieldValue.Empty;
                        minoArray[i - 1, j] = (int)eFieldValue.Empty;
                        minoArray[i + 1, j] = (int)eFieldValue.Empty;

                        minoArray[i - 1, j + 1] = (int)eFieldValue.MinoBlock;
                        minoArray[i - 1, j] = (int)eFieldValue.MinoBlock_Axis;
                        minoArray[i - 1, j - 1] = (int)eFieldValue.MinoBlock;
                        minoArray[i - 1, j + 2] = (int)eFieldValue.MinoBlock;
                    }
                }
            }
        }
        void IminoRotate_270_360(ref int[,] minoArray)
        {
            for (int i = minoArray.GetLength(0) - 1; i > 0; i--)
            {

                for (int j = minoArray.GetLength(1) - 1; j > 0; j--)
                {

                    if (minoArray[i, j] == (int)eFieldValue.MinoBlock_Axis)
                    {
                        minoArray[i, j] = (int)eFieldValue.Empty;
                        minoArray[i, j + 1] = (int)eFieldValue.Empty;
                        minoArray[i, j + 2] = (int)eFieldValue.Empty;
                        minoArray[i, j - 1] = (int)eFieldValue.Empty;

                        minoArray[i + 2, j + 1] = (int)eFieldValue.MinoBlock;
                        minoArray[i + 1, j + 1] = (int)eFieldValue.MinoBlock;
                        minoArray[i, j + 1] = (int)eFieldValue.MinoBlock_Axis;
                        minoArray[i - 1, j + 1] = (int)eFieldValue.MinoBlock;
                    }
                }
            }
        }

        // 左回転(4パターン)
        void IminoRotate_360_270(ref int[,] minoArray)
        {
            for (int i = 0; i < minoArray.GetLength(0); i++)
            {

                for (int j = 0; j < minoArray.GetLength(1); j++)
                {

                    if (minoArray[i, j] == (int)eFieldValue.MinoBlock_Axis)
                    {
                        minoArray[i, j] = (int)eFieldValue.Empty;
                        minoArray[i - 1, j] = (int)eFieldValue.Empty;
                        minoArray[i + 1, j] = (int)eFieldValue.Empty;
                        minoArray[i + 2, j] = (int)eFieldValue.Empty;

                        minoArray[i, j + 1] = (int)eFieldValue.MinoBlock;
                        minoArray[i, j] = (int)eFieldValue.MinoBlock;
                        minoArray[i, j - 1] = (int)eFieldValue.MinoBlock_Axis;
                        minoArray[i, j - 2] = (int)eFieldValue.MinoBlock;
                    }
                }
            }
        }
        void IminoRotate_270_180(ref int[,] minoArray)
        {
            for (int i = minoArray.GetLength(0) - 1; i > 0; i--)
            {

                for (int j = 0; j < minoArray.GetLength(1); j++)
                {

                    if (minoArray[i, j] == (int)eFieldValue.MinoBlock_Axis)
                    {
                        minoArray[i, j] = (int)eFieldValue.Empty;
                        minoArray[i, j + 1] = (int)eFieldValue.Empty;
                        minoArray[i, j - 1] = (int)eFieldValue.Empty;
                        minoArray[i, j + 2] = (int)eFieldValue.Empty;

                        minoArray[i + 2, j] = (int)eFieldValue.MinoBlock;
                        minoArray[i - 1, j] = (int)eFieldValue.MinoBlock;
                        minoArray[i, j] = (int)eFieldValue.MinoBlock;
                        minoArray[i + 1, j] = (int)eFieldValue.MinoBlock_Axis;
                    }
                }
            }
        }
        void IminoRotate_180_90(ref int[,] minoArray)
        {
            for (int i = 0; i < minoArray.GetLength(0); i++)
            {

                for (int j = minoArray.GetLength(1) - 1; j > 0; j--)
                {

                    if (minoArray[i, j] == (int)eFieldValue.MinoBlock_Axis)
                    {
                        minoArray[i, j] = (int)eFieldValue.Empty;
                        minoArray[i - 2, j] = (int)eFieldValue.Empty;
                        minoArray[i - 1, j] = (int)eFieldValue.Empty;
                        minoArray[i + 1, j] = (int)eFieldValue.Empty;

                        minoArray[i, j + 1] = (int)eFieldValue.MinoBlock_Axis;
                        minoArray[i, j] = (int)eFieldValue.MinoBlock;
                        minoArray[i, j - 1] = (int)eFieldValue.MinoBlock;
                        minoArray[i, j + 2] = (int)eFieldValue.MinoBlock;
                    }
                }
            }
        }
        void IminoRotate_90_0(ref int[,] minoArray)
        {
            for (int i = 0; i < minoArray.GetLength(0); i++)
            {

                for (int j = minoArray.GetLength(1) - 1; j > 0; j--)
                {

                    if (minoArray[i, j] == (int)eFieldValue.MinoBlock_Axis)
                    {
                        minoArray[i, j] = (int)eFieldValue.Empty;
                        minoArray[i, j + 1] = (int)eFieldValue.Empty;
                        minoArray[i, j - 2] = (int)eFieldValue.Empty;
                        minoArray[i, j - 1] = (int)eFieldValue.Empty;

                        minoArray[i - 2, j] = (int)eFieldValue.MinoBlock;
                        minoArray[i + 1, j] = (int)eFieldValue.MinoBlock;
                        minoArray[i, j] = (int)eFieldValue.MinoBlock;
                        minoArray[i - 1, j] = (int)eFieldValue.MinoBlock_Axis;
                    }
                }
            }
        }

        void Rotate_field(ref int[,] field, int[,] minoArray) // ここで'field'に値が代入されて画面に反映する
        {
            for (int i = 0; i < field.GetLength(0); i++)
            {

                for (int j = 0; j < field.GetLength(1); j++)
                {

                    if (field[i, j] == (int)eFieldValue.MinoBlock || field[i, j] == (int)eFieldValue.MinoBlock_Axis)
                    {
                        field[i, j] = (int)eFieldValue.Empty;
                    }
                }
            }

            for (int i = 0; i < minoArray.GetLength(0); i++)
            {

                for (int j = 0; j < minoArray.GetLength(1); j++)
                {

                    if (minoArray[i, j] == (int)eFieldValue.MinoBlock || minoArray[i, j] == (int)eFieldValue.MinoBlock_Axis)
                    {
                        field[i, j] = minoArray[i, j];
                    }
                }
            }
        }

        // 利用した配列の初期化
        void Initialize_All(ref int[,] wallArray, ref int[,] minoArray, ref int[,] initialPosArray)
        {
            // テスト用に作った二つの配列を初期化
            for (int i = 0; i < minoArray.GetLength(0); i++)
            {
                for (int j = 0; j < minoArray.GetLength(1); j++)
                {
                    wallArray[i, j] = (int)eFieldValue.Empty;
                    minoArray[i, j] = (int)eFieldValue.Empty;
                    initialPosArray[i, j] = (int)eFieldValue.Empty;
                }
            }
        }
        */
    }
}