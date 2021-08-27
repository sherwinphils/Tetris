using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTetris
{
    public class Tetromino
    {
        private Dictionary<TetrominoType, int[,]> tilesTemplates = new Dictionary<TetrominoType, int[,]>
            {
                { TetrominoType.I, new int[4, 4]
                    {
                        { 0, 0, 0 ,0},
                        { 1, 1, 1, 1},
                        { 0, 0, 0, 0},
                        { 0, 0, 0, 0}
                    }
                },

                { TetrominoType.O, new int[4,4]
                    {
                        { 0, 0, 0 ,0},
                        { 0, 1, 1 ,0},
                        { 0, 1, 1, 0},
                        { 0, 0, 0, 0}
                    }
                },

                { TetrominoType.L, new int[3,3]
                    {
                        { 0, 0, 0},
                        { 0, 0, 1},
                        { 1, 1, 1},
                    }
                },

                { TetrominoType.J, new int[3,3]
                    {
                        { 0, 0, 0},
                        { 1, 1, 1},
                        { 0, 0, 1},
                    }
                },
                { TetrominoType.S, new int[3,3]
                    {
                        { 0, 0, 0},
                        { 0, 1, 1},
                        { 1, 1, 0},
                    }
                },

                { TetrominoType.Z, new int[3,3]
                    {
                        { 0, 0, 0},
                        { 1, 1, 0},
                        { 0, 1, 1},
                    }
                },
            };

        private bool checkCollision(Tile[,] gameField, Tile[,] TetroTiles)
        { 
            for (int y = 0; y < TetroTiles.GetLength(1); y++)
            {
                for (int x = 0; x < TetroTiles.GetLength(0); x++)
                {
                    Tile currentTile = TetroTiles[x, y];
                    if (currentTile != Tile.EMPTY)
                    {
                        // Check if in bounds
                        if ((x + XPos < 0) || (x + XPos > gameField.GetLength(0)))
                        {
                            return true;
                        }


                        Tile currentTileInGameField = gameField[x + XPos, y + YPos];
                        if (currentTileInGameField != Tile.EMPTY)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }




        public int YPos;
        public int XPos;
        public Tile[,] Tiles;
        public TetrominoType TetroType;
        public int Rotation;

        public bool waiting = false;

        public Tetromino()
        {
            Array values = Enum.GetValues(typeof(TetrominoType));
            Random random = new Random();

            TetrominoType randomTetrominoType = TetrominoType.I; //(TetrominoType)values.GetValue(random.Next(values.Length));


            //Convert TetrominoType Enum into a Tile Enum
            Tile conversion = (Tile)randomTetrominoType;
            int[,] template = tilesTemplates[randomTetrominoType];
            Tiles = new Tile[template.GetLength(1), template.GetLength(0)];

            for (int y = 0; y < template.GetLength(1); y++)
            {
                for (int x = 0; x < template.GetLength(0); x++)
                {
                    if (template[y , x] == 1) // Y AND X ARE INVERTED! Because 2d arrays :)
                    {
                        Tiles[x, y] = conversion; // This is very bad code. :)
                    }
                    else
                    {
                        Tiles[x, y] = Tile.EMPTY;
                    }
                }
            }

            XPos = (10 / 2) - 1;
            YPos = 4;
        }

        public void Rotate(Tile[,] gameField)
        {
            int flip(int index, int dimension)
            {
                return Tiles.GetLength(dimension) - index - 1;
            }


            // Rotating a tile, just means flipping.
            Tile[,] rotatedTiles = new Tile[Tiles.GetLength(0), Tiles.GetLength(1)];
            if (Rotation % 2 == 0)
            {
                // Flip X
                for (int y = 0; y < Tiles.GetLength(0); y++)
                {
                    for (int x = 0; x < Tiles.GetLength(1); x++)
                    {
                        rotatedTiles[x, flip(y, 0)] = Tiles[y, x];
                        //rotated[flip(x, 1), y] = this.Tiles[y, x]; Counter clockwise
                    }
                }
            }
            else
            {
                // Flip Y
                for (int y = 0; y < this.Tiles.GetLength(0); y++)
                {
                    for (int x = 0; x < this.Tiles.GetLength(1); x++)
                    {
                        rotatedTiles[x, flip(y, 0)] = this.Tiles[y, x];
                        //rotated[flip(x, 1), y] = this.Tiles[y, x]; Counter clockwise
                    }
                }
            }

            bool IsConflict = checkCollision(gameField, rotatedTiles);
            if (IsConflict == true)
            {
                return;
            }


            Tiles = rotatedTiles;
            Rotation += 1;
        }

        public void GoLeft(Tile[,] gameField)
        {
            XPos -= 1;

            bool IsConflict = checkCollision(gameField, Tiles);
            if (IsConflict)
            {
                XPos += 1;
            }
        }

        public void GoRight(Tile[,] gameField)
        {
            XPos += 1;

            bool IsConflict = checkCollision(gameField, Tiles);
            if (IsConflict)
            {
                XPos -= 1;
            }
        }

        public bool Advance(Tile[,] gameField)
        {
            YPos += 1;
            bool IsConflict = checkCollision(gameField, Tiles);
            if (IsConflict)
            {
                YPos -= 1;
            }


            return IsConflict;
        }
    }
}
