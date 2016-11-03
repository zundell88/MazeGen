using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGeneratorAndSolver
{
    // create Maze in this class
    public class GenerateMaze 
    {
        private Maze _maze;
        public GenerateMaze(Maze maze)
        {
            _maze = maze;
        }

        public void Generate(int value)
        {
            // create maze in this method using depth first search
            //Init(value);
            _maze.MazePen.Dispose();
            _maze.MazePen = _maze.X < 5 ? new Pen(Brushes.WhiteSmoke, 1) : new Pen(Brushes.WhiteSmoke, 3);
            SearchAlgorithm algo = new SearchAlgorithm(_maze);
            algo.DepthFirstSearch();
        }
    }
}
