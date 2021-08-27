using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace MyTetris
{
    class Program
    {


        static void Main(string[] args)
        {
            Dictionary<object, ConsoleColor> ColorScheme = new Dictionary<object, ConsoleColor>
            {
                {Tile.EMPTY, ConsoleColor.Black },
                {Tile.WALL, ConsoleColor.Gray },
                {Tile.I, ConsoleColor.Cyan },
                {Tile.O, ConsoleColor.Yellow },
                {Tile.L, ConsoleColor.Red },
                {Tile.J, ConsoleColor.Green },
                {Tile.S, ConsoleColor.White },
                {Tile.Z, ConsoleColor.Magenta },

                {"FOREGROUND_COLOR", ConsoleColor.DarkMagenta },
                {"BACKGROUND_COLOR", ConsoleColor.DarkCyan }
            };
            Dictionary<ConsoleKey, string> Keybinds = new Dictionary<ConsoleKey, string>
            {
                {ConsoleKey.A, "LEFT" },
                {ConsoleKey.D, "RIGHT" },
                {ConsoleKey.S, "DOWN" },
                {ConsoleKey.Spacebar, "ROTATE"}
            };


            // GAME SETTINGS !! change the names to constants !!
            int borderSize = 1;
            int playFieldWidth = 10;
            int playFieldHeight = 20;

            int vanishHeight = 5;
            int totalWidth = borderSize + playFieldWidth + borderSize;
            int totalHeight = borderSize + playFieldHeight + vanishHeight + borderSize;
            float ADVANCE_TETRO_MOD = 0.01f;

            // GAME VARIABLES
            int score = 0;
            int totalClearedRows = 0;
            bool gameOver = false;
            string inputAction = "IDLE";
            KeyboardHandler keyboardHandler = new KeyboardHandler();



            Tile[,] gameField = new Tile[totalWidth, totalHeight];

            //Tetromino waitingTetromino = new Tetromino();
            Tetromino activeTetromino = new Tetromino();


            // METHODS


            // SETUP
            void setup()
            {
                Console.ForegroundColor = ColorScheme["FOREGROUND_COLOR"];
                Console.BackgroundColor = ColorScheme["BACKGROUND_COLOR"];
                Console.CursorVisible = false;

                // Insert Walls
                for (int y = 0; y < totalHeight; y++)
                {
                    for (int x = 0; x < totalWidth; x++)
                    {
                        if (x == 0 || x == totalWidth - 1 || y == totalHeight - 1)
                        {
                            gameField[x, y] = Tile.WALL;
                        }
                        else
                        {
                            gameField[x, y] = Tile.EMPTY;
                        }
                    }
                }
            }

            // INPUT
            void input()
            {
                if (Console.KeyAvailable == false)
                {
                    switch (keyboardHandler.PressedKey)
                    {
                        case ConsoleKey.A:
                            inputAction = "LEFT";
                            keyboardHandler.PressedKey = null;
                            break;

                        case ConsoleKey.D:
                            inputAction = "RIGHT";
                            keyboardHandler.PressedKey = null;
                            break;

                        case ConsoleKey.S:
                            inputAction = "DOWN";
                            keyboardHandler.PressedKey = null;
                            break;

                        case ConsoleKey.Spacebar:
                            inputAction = "ROTATE";
                            keyboardHandler.PressedKey = null;
                            break;

                        default:
                            inputAction = "IDLE";
                            break;
                    }
                }
            }

            // LOGIC (inlcuding time Logic)

            double accumulatedTime = 0;
            void logic(double delta)
            {
                accumulatedTime += delta;

                switch (inputAction)
                {
                    case "LEFT":
                        activeTetromino.GoLeft(gameField);
                        Console.SetCursorPosition(0, 31);
                        Console.WriteLine("LEFT");
                        break;

                    case "RIGHT":
                        activeTetromino.GoRight(gameField);
                        Console.SetCursorPosition(0, 31);
                        Console.WriteLine("RIGHT");
                        break;

                    case "DOWN":
                        bool IsConflict = activeTetromino.Advance(gameField);
                        if (IsConflict)
                        {
                            accumulatedTime = 9999;
                        }
                        else
                        {
                            accumulatedTime = 0;
                        }
                        break;

                    case "ROTATE":
                        activeTetromino.Rotate(gameField);
                        Console.SetCursorPosition(0, 31);
                        Console.WriteLine("ROTATE");
                        break;

                    case "IDLE":
                        Console.SetCursorPosition(0, 31);
                        Console.WriteLine("IDLE");
                        break;
                }


                if (accumulatedTime > (1000 - (1000 * (ADVANCE_TETRO_MOD * totalClearedRows)) ))
                {
                    accumulatedTime = 0;
                    bool IsConflict = activeTetromino.Advance(gameField);
                    if (IsConflict)
                    {
                        // Add activeTetrominos Tiles into the stack.
                        Tile[,] TetroTiles = activeTetromino.Tiles;

                        for (int y = 0; y < TetroTiles.GetLength(1); y++)
                        {
                            for (int x = 0; x < TetroTiles.GetLength(0); x++)
                            {
                                if (TetroTiles[x, y] != Tile.EMPTY)
                                {
                                    gameField[x + activeTetromino.XPos, y + activeTetromino.YPos] = TetroTiles[x, y];
                                }
                            }
                        }


                        // LOGIC TO CLEAR LINES

                        // Find completed rows
                        List<int> rows = new List<int> ();
                        for (int y = vanishHeight; y < totalHeight - borderSize; y++)
                        {
                            int count = 0;
                            for (int x = borderSize; x < totalWidth - borderSize; x++)
                            {
                                Debug.WriteLine($"X: {x}  Y:{y}  Tile state: {gameField[x, y]} ");
                                if (gameField[x, y] != Tile.EMPTY)
                                {
                                    count += 1;
                                }
                            }

                            Debug.WriteLine(count);

                            if (count == totalWidth - 2 * borderSize)
                            {
                                rows.Add(y);
                            }
                        }

                        // Replace filled rows with EMPTY
                        foreach (int row in rows)
                        {
                            // Clear row
                            for (int x = borderSize; x < totalWidth - borderSize; x++)
                            {
                                gameField[x, row] = Tile.EMPTY;
                            }

                            // Drop stack above downwards
                            for (int y = row - 1; y > borderSize; y--)
                            {
                                Debug.WriteLine(y);
                                Debug.WriteLine(y > 1);

                                for (int x = borderSize; x < totalWidth - borderSize; x++)
                                {
                                    Tile tileToDrop = gameField[x, y];
                                    gameField[x, y + 1] = tileToDrop;
                                }
                            }

                            totalClearedRows += 1;
                        }

                        activeTetromino = null;
                        activeTetromino = new Tetromino();
                    }
                };

                Console.SetCursorPosition(0,30);
                Console.WriteLine(delta);
            }

            // RENDERING
            void draw()
            {
                Tile[,] FieldToDraw;

                // Doesn't draw the vanish zone
                void drawGameField()
                {
                    FieldToDraw = (Tile[,])gameField.Clone();
                }
                void drawActivePiece()
                {
                    Tile[,] tetroTiles = activeTetromino.Tiles;
                    for (int y = 0; y < tetroTiles.GetLength(1); y++)
                    {
                        for (int x = 0; x < tetroTiles.GetLength(0); x++)
                        {
                            Tile currentTile = tetroTiles[x, y];

                            if (currentTile != Tile.EMPTY)
                            {
                                FieldToDraw[activeTetromino.XPos + x, activeTetromino.YPos + y] = currentTile;
                            }
                        }
                    }
                }
                void drawInterface()
                {
                    Console.WriteLine(totalClearedRows);
                }


                drawGameField();
                drawActivePiece();


                Console.SetCursorPosition(0, 0);
                Console.BackgroundColor = ColorScheme["BACKGROUND_COLOR"];

                drawInterface();

                for (int y = vanishHeight; y < totalHeight; y++)
                {
                    for (int x = 0; x < totalWidth; x++)
                    {
                        Tile currentTile = FieldToDraw[x, y];
                        Console.SetCursorPosition(x * 2, y);
                        Console.BackgroundColor = ColorScheme[currentTile];
                        Console.Write($"  ");
                    }
                }
            }

            setup();

            //do your work

            DateTime currentTime, lastUpdatedTime;

            lastUpdatedTime = DateTime.Now;

            while (gameOver == false)
            {
                currentTime = DateTime.Now;
                double deltaTime = ((TimeSpan)(currentTime - lastUpdatedTime)).TotalMilliseconds;

                input();
                logic(deltaTime);
                draw();

                Console.CursorVisible = false;

                lastUpdatedTime = currentTime;
            }
        }
    }
}
