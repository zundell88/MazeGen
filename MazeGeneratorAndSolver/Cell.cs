using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGeneratorAndSolver
{
    public class Cell
    {
        public Cell(Point location, Point position)
        {
            Position = position;
            Location = location; // Don't mind location, it's for the UI
            IsVisited = false;
            PreviousCell = null;
            this.Path = Paths.None;
        }

        /*
         * this array defines cell walls 
         * true= Wall exist, false= Wall dosen't exist.
         * Sequence of these is Left, Top, Right, Bottom
         */
        private bool[] _cellWalls = new bool[] { true, true, true, true };

        public bool[] CellWalls
        {
            get { return _cellWalls; }
            set { _cellWalls = value; }
        }

        /*
         * you may use Point datatype instead of int[,] array if you like to draw proper UI using Pen
         * but if you want to create UI like displayed in problem document 
         * then int[,] array will helpful.
         */
        public Point Location { get; set; }
        /*
         * you may use Point datatype instead of int[,] array if you like to draw proper UI using Pen
         * but if you want to create UI like displayed in problem document 
         * then int[,] array will helpful.
         */
        public Point Position { get; set; }
        public bool IsVisited { get; set; }
        public Cell PreviousCell { get; set; }
        public enum Paths
        {
            Up, Down, Right, Left, None
        }

        public Paths Path;
        public void Draw(Graphics g, Pen pen, Size size)
        {
            // Draws every wall, if it is intact
            if (this.CellWalls[0])
            {
                g.DrawLine(pen,
                    this.Location,
                    new Point(this.Location.X, this.Location.Y + size.Height));
            }
            if (this.CellWalls[2])
            {
                g.DrawLine(pen,
                    new Point(this.Location.X + size.Width, this.Location.Y),
                    new Point(this.Location.X + size.Width, this.Location.Y + size.Height));
            }
            if (this.CellWalls[1])
            {
                g.DrawLine(pen,
                    this.Location,
                    new Point(this.Location.X + size.Width, this.Location.Y));
            }
            if (this.CellWalls[3])
            {
                g.DrawLine(pen,
                    new Point(this.Location.X, this.Location.Y + size.Height),
                    new Point(this.Location.X + size.Width, this.Location.Y + size.Height));
            }
        }
    }
}
