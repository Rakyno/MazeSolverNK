using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MazeSolverNK
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Maze maze;
        private const int CellSize = 20;

        public Form1()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            InitializeComponent();
            maze = new Maze(20, 20); // Create a 20x20 maze
            maze.GenerateMaze();
            this.Paint += Form1_Paint; 
        }

        private void DrawMaze(Graphics g)
        {
            foreach (var cell in maze.cells)
            {
                int x = cell.X * CellSize + 25;
                int y = cell.Y * CellSize + 50;

                // Draw the walls of the cell
                if (cell.Walls[0]) g.DrawLine(Pens.Black, x, y, x + CellSize, y); // Top
                if (cell.Walls[1]) g.DrawLine(Pens.Black, x + CellSize, y, x + CellSize, y + CellSize); // Right
                if (cell.Walls[2]) g.DrawLine(Pens.Black, x, y + CellSize, x + CellSize, y + CellSize); // Bottom
                if (cell.Walls[3]) g.DrawLine(Pens.Black, x, y, x, y + CellSize); // Left

                // Draw a small circle if the cell is part of the solution path
                if (cell.Selected)
                {
                    
                    int circleDiameter = CellSize / 4;
                    int circleX = x + (CellSize - circleDiameter) / 2;
                    int circleY = y + (CellSize - circleDiameter) / 2;

                    g.FillEllipse(Brushes.Red, circleX, circleY, circleDiameter, circleDiameter);
                }
            }
            Update();
        }

        private async Task AnimateSolutionPath(Graphics g, List<Cell> solutionPath)
        {
            foreach (var cell in solutionPath)
            {
                // Calculate position for the circle
                int x = cell.X * CellSize +25;
                int y = cell.Y * CellSize + 50;
                int circleDiameter = CellSize / 4;
                int circleX = x + (CellSize - circleDiameter) / 2;
                int circleY = y + (CellSize - circleDiameter) / 2;

                // Draw the circle
                g.FillEllipse(Brushes.Red, circleX, circleY, circleDiameter, circleDiameter);

                // Small delay for animation
                await Task.Delay(50); // Adjust delay (in milliseconds) for speed of animation
            }
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawMaze(e.Graphics);
        }


        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            maze = new Maze(20, 20);

            maze.GenerateMaze();

            this.Invalidate();
        }

 

        private async void barButtonItem2_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (Graphics g = this.CreateGraphics())
            {
                // Solve the maze and get the solution path
                List<Cell> solutionPath = maze.SolveDFS(); // Or SolveBFS()

                // Animate the solution path
                await AnimateSolutionPath(g, solutionPath);
            }

        }

        private void barButtonItem3_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            List<Cell> path = maze.SolveBFS(maze);

            using (Graphics g = this.CreateGraphics())
            {
                foreach (var cell in path)
                {
                    int x = cell.X * CellSize + 25;
                    int y = cell.Y * CellSize + 50;
                    g.FillEllipse(Brushes.Green, x + CellSize / 4, y + CellSize / 4, CellSize / 2, CellSize / 2);
                    System.Threading.Thread.Sleep(50); // Add a slight delay for animation effect
                }
            }

        }
    }
}
