using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGeneratorAndSolver
{
    // this class will be used to solve Maze 
    public class MazeSolver
    {
        private Maze _maze;
        public MazeSolver(Maze maze)
        {
            _maze = maze;
        }

        public void Solve()
        {
            _maze.Solving = true;
            // use this method to solve Maze
            _maze.FoundPath.Clear();
            this.unvisitAll();
            SearchAlgorithm algo = new SearchAlgorithm(_maze);
            algo.BreadthFirstSearch();
            _maze.Solving = false;
        }

        private void unvisitAll()
        {
            for (int i = 0; i < this._maze.MazeArray.GetLength(0); i++)
            {
                for (int j = 0; j < this._maze.MazeArray.GetLength(1); j++)
                {
                    _maze.MazeArray[i, j].IsVisited = false;
                    _maze.MazeArray[i, j].Path = Cell.Paths.None;
                }
            }
        }
        
    }
}
