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
                            { 0, 1, 0 ,1},
                            { 1, 1, 1, 1},
                            { 0, 0, 0, 0},
                            { 0, 0, 0, 0}};
                    break;
                case BrickType.O:
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
                        { 0, 0, 0, 0},
                        { 0, 0, 0, 0},
                        { 1, 0, 0, 1}};
                    break;
            }
        }

        internal void RotateRight()
        {
            int[,] clone = felder.Clone() as int[,];
            var length = GetLength;

            // Diagonal spiegeln
            for (int row = 0; row < length; row++)
                for (int column = 0; column < length; column++)
                {
                    var source = clone[column, row];
                    felder[row,column] = source;
                }
            // Horizontal spiegeln
        }

        public int GetLength => felder.GetLength(0);

        internal ConsoleColor GetColor(int x, int y)
        {
            return (felder[y, x]==1) ? farbe : ConsoleColor.Gray;
        }
    }
}