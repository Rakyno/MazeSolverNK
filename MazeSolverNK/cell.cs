using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolverNK
{
    public class Cell
    {
        public const int TOP = 0;
        public const int RIGHT = 3;
        public const int BOTTOM = 2;
        public const int LEFT = 1;

        public int X { get; set; }
        public int Y { get; set; }
        public bool[] Walls = { true, true, true, true }; // Top, Right, Bottom, Left
        public bool Visited { get; set; } = false;
        public bool Selected { get; set; } = false;
        public bool PartOfPath { get; set; } = false; // Used for highlighting the solution path
        public Cell Previous { get; set; } = null;
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        // Get unvisited neighbors
        public List<Cell> GetNeighbors(Cell[,] grid, int rows, int cols)
        {
            var neighbors = new List<Cell>();

            // Top
            if (X > 0 && !grid[X - 1, Y].Visited) neighbors.Add(grid[X - 1, Y]);
            // Right
            if (Y < cols - 1 && !grid[X, Y + 1].Visited) neighbors.Add(grid[X, Y + 1]);
            // Bottom
            if (X < rows - 1 && !grid[X + 1, Y].Visited) neighbors.Add(grid[X + 1, Y]);
            // Left
            if (Y > 0 && !grid[X, Y - 1].Visited) neighbors.Add(grid[X, Y - 1]);

            return neighbors;
        }

        // Get unvisited accessable neighbors
        public List<Cell> GetUnvisitedOpenNeighbors(Cell[,] grid, int rows, int cols)
        {
            var neighbors = new List<Cell>();

            // Top
            if (X > 0 && !grid[X - 1, Y].Visited)
                neighbors.Add(grid[X - 1, Y]);
            // Right
            if (Y < cols - 1 && !grid[X, Y + 1].Visited)
                neighbors.Add(grid[X, Y + 1]);
            // Bottom
            if (X < rows - 1 && !grid[X + 1, Y].Visited)
                neighbors.Add(grid[X + 1, Y]);
            // Left 
            if (Y > 0 && !grid[X, Y - 1].Visited)
                neighbors.Add(grid[X, Y - 1]);

            return neighbors;
        }
        

        // Remove the wall between this cell and the given neighbor
        public void RemoveWall(Cell next)
        {
            int dx = next.X - X;
            int dy = next.Y - Y;
     
            if (dy == 1) { Walls[2] = false; next.Walls[0] = false; } // Bottom
            else if (dy == -1) { Walls[0] = false; next.Walls[2] = false; } // Top
            else if (dx == 1) { Walls[1] = false; next.Walls[3] = false; } // Right
            else if (dx == -1) { Walls[3] = false; next.Walls[1] = false; } // Left
        }
    }
}
