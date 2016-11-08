using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGeneratorAndSolver
{
    public class Maze
    {
        public Maze(int width, int height, int cellSize)
        {
            _cellSize = cellSize;
            _width = width / cellSize;
            _height = height / cellSize;
            _mazeArray = new Cell[_width, _height];
            Initialize();
            Begin = _mazeArray[0, 0];
            End = _mazeArray[_width - 1, _height - 1];
        }
        private void Initialize()
        {
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    MazeArray[j, i] = new Cell(new Point(j * X, i * Y), new Point(j, i));
                }
            }
        }

        private int _cellSize;
        private int _width;
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private int _height;
        public int Height
        {
            get { return _height; }
        }
        // this array will contian Maze cells
        private Cell[,] _mazeArray;

        public Cell[,] MazeArray
        {
            get { return _mazeArray; }
        }
        
        public Cell Begin { get; set; } // contain Entrance cell of maze
        public Cell End { get; set; } // contains Exit cell of maze
        public int X => _cellSize;
        public int Y => _cellSize;
        public bool Working { get; set; }
        public bool Solving { get; set; }
        public bool Finished = false; //To brush Exit cell when maze solved.
        public Point CurrentGenerateCell { get; set; }
        public Point CurrentSolvePosition { get; set; }
        public Pen LocationPen = new Pen(Brushes.IndianRed, 5);
        public Pen MazePen = new Pen(Brushes.WhiteSmoke, 3);
        public List<Cell> FoundPath = new List<Cell>();
        private static void drawVerticalWall(Graphics g, ref Point location, int height, Pen pen)
        {
            g.DrawLine(pen,
                location,
                new Point(location.X, location.Y + height));
        }
        public void Draw(Graphics g)
        {
            g.Clear(Color.Black);

            // in case Generate() have not been called yet
            if (this._width == 0)
                return;

            // draws begin
            g.FillRectangle(Brushes.Red, new Rectangle(this.Begin.Location, new Size(this.X, this.Y)));

            // loap on every cell in the bounds
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    // Visited cell: fill green square
                    if (this.MazeArray[i, j].IsVisited)
                    {
                        g.FillRectangle(Brushes.Green, new Rectangle(this.MazeArray[i, j].Location, new Size(this.X, this.Y)));
                    }

                    // draws a red line indicates the current generation location
                    if (this.Working && this.CurrentGenerateCell.X == j && this.CurrentGenerateCell.Y == i)
                    {
                        Point target = new Point(j * X, i * Y);
                        drawVerticalWall(g, ref target, Y, this.LocationPen);
                    }

                    // fills the current square in the solving process
                    if (this.Solving && this.CurrentSolvePosition.X == i && this.CurrentSolvePosition.Y == j)
                    {
                        g.FillRectangle(Brushes.IndianRed, new Rectangle(this.MazeArray[i, j].Location, new Size(this.X, this.Y)));
                    }

                    // Draw the intact walls
                    this.MazeArray[i, j].Draw(g, MazePen, new Size(this.X, this.Y));

                    if (this.MazeArray[i, j].Path != Cell.Paths.None)
                    {
                        switch (this.MazeArray[i, j].Path)
                        {
                            case Cell.Paths.Up:
                                g.DrawLine(this.LocationPen,
                    new Point(this.MazeArray[i, j].Location.X + X / 2, this.MazeArray[i, j].Location.Y + Y / 2),
                    new Point(this.MazeArray[i - 1, j].Location.X + X / 2, this.MazeArray[i - 1, j].Location.Y + Y / 2));
                                break;
                            case Cell.Paths.Down:
                                g.DrawLine(this.LocationPen,
                    new Point(this.MazeArray[i, j].Location.X + X / 2, this.MazeArray[i, j].Location.Y + Y / 2),
                    new Point(this.MazeArray[i + 1, j].Location.X + X / 2, this.MazeArray[i + 1, j].Location.Y + Y / 2));
                                break;
                            case Cell.Paths.Right:
                                g.DrawLine(this.LocationPen,
                    new Point(this.MazeArray[i, j].Location.X + X / 2, this.MazeArray[i, j].Location.Y + Y / 2),
                    new Point(this.MazeArray[i, j + 1].Location.X + X / 2, this.MazeArray[i, j + 1].Location.Y + Y / 2));
                                break;
                            default:
                                g.DrawLine(this.LocationPen,
                    new Point(this.MazeArray[i, j].Location.X + X / 2, this.MazeArray[i, j].Location.Y + Y / 2),
                    new Point(this.MazeArray[i, j - 1].Location.X + X / 2, this.MazeArray[i, j - 1].Location.Y + Y / 2));
                                break;
                        }
                    }
                }
            }
        }

        public void DrawPath(Graphics g)
        {
            // maze-begin square
            g.FillRectangle(Brushes.Red, new Rectangle(this.Begin.Location, new Size(this.X, this.Y)));

            // loap on every item in the list
            for (int i = 1; i < this.FoundPath.Count; i++)
            {
                // draw a line between the (i item) and (i - 1 item)
                // line begins in the middle of both squares so we add unit / 2
                g.DrawLine(this.LocationPen,
                    new Point(this.FoundPath[i].Location.X + X / 2, this.FoundPath[i].Location.Y + Y / 2),
                    new Point(this.FoundPath[i - 1].Location.X + X / 2, this.FoundPath[i - 1].Location.Y + Y / 2));
                if (Finished)
                    g.FillRectangle(Brushes.Red, new Rectangle(this.End.Location, new Size(this.X, this.Y)));
            }
        }
    }
}