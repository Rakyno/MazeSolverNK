using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolverNK
{
    public class Maze
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public Cell[,] cells { get; set; }

        public Maze(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            cells = new Cell[rows, cols];

            // Initialize all cells
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    cells[x, y] = new Cell(x, y);
                }
            }
        }

        

        // Solve the maze using Depth-First Search (DFS)
        public List<Cell> SolveDFS()
        {
            // Reset all cells to unvisited
            foreach (var cell in cells)
            {
                cell.Visited = false;
            }

            Stack<Cell> stack = new Stack<Cell>();
            Dictionary<Cell, Cell> cameFrom = new Dictionary<Cell, Cell>();

            Cell startCell = cells[0, 0];
            Cell endCell = cells[Rows - 1, Cols - 1];

            stack.Push(startCell);
            startCell.Visited = true;
            cameFrom[startCell] = null;

            while (stack.Count > 0)
            {
                Cell current = stack.Pop();

                // Check if we've reached the end
                if (current == endCell)
                {
                    return ReconstructPath(cameFrom, startCell, endCell); // Return only the correct path
                }

                // Get valid unvisited neighbors
                List<Cell> unvisitedNeighbors = GetUnvisitedOpenNeighbors(current);

                foreach (var neighbor in unvisitedNeighbors)
                {
                    if (!neighbor.Visited)
                    {
                        stack.Push(neighbor);
                        neighbor.Visited = true;
                        cameFrom[neighbor] = current;
                    }
                }
            }
            return new List<Cell>();
        }

        public List<Cell> SolveBFS(Maze maze)
        {
            // Reset all cells to unvisited
            foreach (var cell in maze.cells)
            {
                cell.Visited = false;
                cell.Previous = null; // To reconstruct the path
            }

            Queue<Cell> queue = new Queue<Cell>();
            List<Cell> path = new List<Cell>();

            Cell startCell = maze.cells[0, 0];
            Cell endCell = maze.cells[Rows - 1, Cols - 1];

            queue.Enqueue(startCell);
            startCell.Visited = true;

            while (queue.Count > 0)
            {
                Cell current = queue.Dequeue();

                // Check if we've reached the end
                if (current == endCell)
                {
                    // Reconstruct the path
                    while (current != null)
                    {
                        path.Add(current);
                        current = current.Previous;
                    }
                    path.Reverse();
                    return path;
                }

                // Get valid neighbors
                List<Cell> neighbors = GetUnvisitedOpenNeighbors(current);

                foreach (var neighbor in neighbors)
                {
                    if (!neighbor.Visited)
                    {
                        neighbor.Visited = true;
                        neighbor.Previous = current;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return path;
        }

        private List<Cell> ReconstructPath(Dictionary<Cell, Cell> cameFrom, Cell startCell, Cell endCell)
        {
            List<Cell> path = new List<Cell>();
            Cell current = endCell;

            while (current != null)
            {
                path.Add(current);
                current.Selected = true; // Mark the cell as part of the solution
                current = cameFrom[current]; // Move to the parent cell
            }

            path.Reverse(); // Reverse the path to get it from start to end
            return path;
        }


        // Get neighbors of a cell that are unvisited and accessible
        private List<Cell> GetUnvisitedOpenNeighbors(Cell current)
        {
            List<Cell> neighbors = new List<Cell>();

            // Top neighbor
            if (!current.Walls[0] && current.Y > 0 && !cells[current.X, current.Y - 1].Visited)
                neighbors.Add(cells[current.X, current.Y - 1]);

            // Right neighbor
            if (!current.Walls[1] && current.X < Cols - 1 && !cells[current.X + 1, current.Y].Visited)
                neighbors.Add(cells[current.X + 1, current.Y]);

            // Bottom neighbor
            if (!current.Walls[2] && current.Y < Rows - 1 && !cells[current.X, current.Y + 1].Visited)
                neighbors.Add(cells[current.X, current.Y + 1]);

            // Left neighbor
            if (!current.Walls[3] && current.X > 0 && !cells[current.X - 1, current.Y].Visited)
                neighbors.Add(cells[current.X - 1, current.Y]);

            return neighbors;
        }

        // Generate the maze using Recursive Backtracking
        public void GenerateMaze()
        {
           
            Stack<Cell> stack = new Stack<Cell>();
            var random = new Random();

            Cell current = cells[0, 0];


            while (true)
            {

                var neighbors = current.GetUnvisitedOpenNeighbors(cells, Rows, Cols);

                if (neighbors.Count > 0)
                {

                    var next = neighbors[random.Next(neighbors.Count)];
                    next.Visited = true;

                    // Remove the wall between current and next
                    current.RemoveWall(next);

                    // Push the current cell to the stack
                    stack.Push(current);

                    // Move to the next cell
                    current = next;
                }
                else if (stack.Count > 0)
                {
                    current = stack.Pop();
                }
                else
                {
                    break;
                }
            }

        }
    }
}
