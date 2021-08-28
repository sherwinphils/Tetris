using System;
using System.Collections.Generic;

namespace MyTetris
{
    internal class GameBoard
    {
        Tile[,] gameField;
        List<Brick> bricks = new List<Brick>();
        
        public int Width { get; }
        public int Height { get; }
        int xNull = 1;
        int yNull = 1;

        public GameBoard(int width = 10, int height = 20)
        {
            Width = width;
            Height = height;
            gameField = new Tile[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                gameField[0, y] = Tile.WALL;
                gameField[Width - 1, y] = Tile.WALL;
            }
            for (int x = 0; x < Width; x++)
            {
                gameField[x, Height-1] = Tile.WALL;
            }

            bricks.Add(new Brick(BrickType.I));
        }

        public void Play()
        {
            Brick brick = new Brick(BrickType.I);
            Draw(0, 0, brick);
            
            brick.RotateRight();
            //brick.RotateRight();
            Draw(0, 5, brick);
        }

        private void Draw(int xBrick, int yBrick, Brick brick)
        {
            for (int y = 0; y < brick.GetLength; y++)
            {
                for (int x = 0; x < brick.GetLength; x++)
                {
                    int xPos = (xNull + xBrick + x)*2;
                    int yPos = yNull + yBrick + y;
                    Console.SetCursorPosition(xPos, yPos);
                    Console.BackgroundColor = brick.GetColor(x,y);
                    Console.Write($"  ");
                }
            }
        }

        public void DrawFrame(int xOff=0, int yOff=0)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var tile = gameField[x, y];
                    var xPos = (xOff + x) * 2;
                    var yPos = (yOff + y);

                    Console.SetCursorPosition(xPos, yPos);
                    Console.BackgroundColor = ColorScheme[tile];
                    Console.Write($"  ");
                }
            }
        }

        Dictionary<object, ConsoleColor> ColorScheme = new Dictionary<object, ConsoleColor>
            {
                {Tile.EMPTY, ConsoleColor.Black },
                {Tile.WALL, ConsoleColor.Yellow },
                {Tile.I, ConsoleColor.Cyan },
                {Tile.O, ConsoleColor.Yellow },
                {Tile.L, ConsoleColor.Red },
                {Tile.J, ConsoleColor.Green },
                {Tile.S, ConsoleColor.White },
                {Tile.Z, ConsoleColor.Magenta },

                {"FOREGROUND_COLOR", ConsoleColor.DarkMagenta },
                {"BACKGROUND_COLOR", ConsoleColor.DarkCyan }
            };
    }
}