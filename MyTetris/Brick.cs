using System;

namespace MyTetris
{
    public class Brick
    {
        int[,] felder;
        ConsoleColor farbe = ConsoleColor.Black;

        public Brick(BrickType brickType)
        {
            switch (brickType)
            {
                case BrickType.I:
                    farbe = ConsoleColor.Cyan;                 
                    felder = new int[4,4] {
                            { 1, 0, 0 ,0},
                            { 1, 1, 0, 0},
                            { 1, 0, 0, 0},
                            { 1, 1, 0, 0}};
                    break;
                case BrickType.O:
                    felder = new int[4, 4] {
                            { 0, 1, 0 ,0},
                            { 0, 1, 0, 0},
                            { 0, 1, 0, 0},
                            { 0, 1, 0, 0}};
                    break;
                case BrickType.L:
                    break;
                case BrickType.J:
                    break;
                case BrickType.S:
                    break;
                case BrickType.Z:
                    break;
                default:
                    felder = new int[4, 4] {
                        { 1, 0, 0 ,1},
                        { 0, 1, 0, 0},
                        { 0, 0, 1, 0},
                        { 0, 0, 0, 1}};
                    break;
            }
        }

        internal void RotateRight()
        {
            int[,] clone = felder.Clone() as int[,];
            var length = GetLength;

            // Diagonal spiegeln
            //for (int row = 0; row < length; row++)
            //    for (int column = 0; column < length; column++)
            //    {
            //        var source = clone[column, row];
            //        felder[row,column] = source;
            //    }
            // Horizontal spiegeln
            felder[3, 0] = clone[0, 0];
            felder[3, 1] = clone[1, 0];
            felder[3, 2] = clone[2, 0];
            felder[3, 3] = clone[3, 0];

            felder[2, 0] = clone[0, 1];
            felder[2, 1] = clone[1, 1];
            felder[2, 2] = clone[2, 1];
            felder[2, 3] = clone[3, 1];

            felder[1, 0] = clone[0, 2];
            felder[1, 1] = clone[1, 2];
            felder[1, 2] = clone[2, 2];
            felder[1, 3] = clone[3, 2];

            felder[0, 0] = clone[0, 3];
            felder[0, 1] = clone[1, 3];
            felder[0, 2] = clone[2, 3];
            felder[0, 3] = clone[3, 3];
        }

        public int GetLength => felder.GetLength(0);

        internal ConsoleColor GetColor(int x, int y)
        {
            return (felder[x, y]==1) ? farbe : ConsoleColor.Gray;
        }
    }
}