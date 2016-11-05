using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeGeneratorAndSolver
{
    public class SearchAlgorithm
    {
        Random r = new Random();
        private Maze _maze;
        private Stack<Cell> cellStack = new Stack<Cell>(); 
        public SearchAlgorithm(Maze maze)
        {
            _maze = maze;
        }

        #region Build Maze with Depth-First Search
        public void DepthFirstSearch()
        {
            Cell startCell = _maze.MazeArray[0,r.Next(_maze.Height)];
            _maze.Begin = startCell;
            startCell.IsVisited = true;
            cellStack.Push(startCell);

            while (cellStack.Count > 0)
            {
                Cell currentCell = cellStack.Pop();
                var unvisitedNeighbours = GetCurrentCellNeighbours(currentCell);
               
                if (unvisitedNeighbours.Count > 0)
                {
                    var tempPos = unvisitedNeighbours[r.Next(unvisitedNeighbours.Count)];
                    Cell nextCell = _maze.MazeArray[tempPos.X, tempPos.Y];
                    RemoveWall(currentCell,nextCell);
                    cellStack.Push(currentCell);
                    cellStack.Push(nextCell);
                }
            }
            MakeMazeBeginEnd();
        }

        private void MakeMazeBeginEnd()
        {
            Point temp = new Point();
            Random random = new Random();
            temp.Y = random.Next(_maze.Height);
            temp.X = 0;
            _maze.MazeArray[temp.X, temp.Y].CellWalls[0] = false;
            _maze.Begin = _maze.MazeArray[temp.X, temp.Y];

            temp.Y = random.Next(_maze.Height);
            temp.X = _maze.Width - 1;
            _maze.MazeArray[temp.X, temp.Y].CellWalls[2] = false;
            _maze.End = _maze.MazeArray[temp.X, temp.Y];
        }

        private void RemoveWall(Cell current, Cell next)
        {
            // Next is down 
            if (current.Position.X == next.Position.X && current.Position.Y > next.Position.Y)
            {
                _maze.MazeArray[current.Position.X, current.Position.Y].CellWalls[1] = false;
                _maze.MazeArray[next.Position.X, next.Position.Y].CellWalls[3] = false;
            }
            // the next is up
            else if (current.Position.X == next.Position.X)
            {
                _maze.MazeArray[current.Position.X, current.Position.Y].CellWalls[3] = false;
                _maze.MazeArray[next.Position.X, next.Position.Y].CellWalls[1] = false;
            }
            // the next is right
            else if (current.Position.X > next.Position.X)
            {
                _maze.MazeArray[current.Position.X, current.Position.Y].CellWalls[0] = false;
                _maze.MazeArray[next.Position.X, next.Position.Y].CellWalls[2] = false;
            }
            // the next is left
            else
            {
                _maze.MazeArray[current.Position.X, current.Position.Y].CellWalls[2] = false;
                _maze.MazeArray[next.Position.X, next.Position.Y].CellWalls[0] = false;
            }
        }

        private List<Point> GetCurrentCellNeighbours(Cell current)
        {
            List<Point> neighbours = new List<Point>();

            Point tempPos = current.Position;
            // Check left neigbour cell 
            tempPos.X = current.Position.X - 1;
            if (tempPos.X >= 0 && AllWallsIntact(_maze.MazeArray[tempPos.X, tempPos.Y]))
            {
                neighbours.Add(tempPos);
            }

            // Check right neigbour cell 
            tempPos.X = current.Position.X + 1;
            if (tempPos.X < _maze.Width && AllWallsIntact(_maze.MazeArray[tempPos.X, tempPos.Y]))
            {
                neighbours.Add(tempPos);
            }

            // Check Upper neigbour cell 
            tempPos.X = current.Position.X;
            tempPos.Y = current.Position.Y - 1;
            if (tempPos.Y >= 0 && AllWallsIntact(_maze.MazeArray[tempPos.X, tempPos.Y]))
            {
                neighbours.Add(tempPos);
            }

            // Check Lower neigbour cell 
            tempPos.Y = current.Position.Y + 1;
            if (tempPos.Y < _maze.Height && AllWallsIntact(_maze.MazeArray[tempPos.X, tempPos.Y]))
            {
                neighbours.Add(tempPos);
            }

            return neighbours;
        }

        private bool AllWallsIntact(Cell cell)
        {
            for (int i = 0; i < 4; i++)
            {
                if (!_maze.MazeArray[cell.Position.X, cell.Position.Y].CellWalls[i])
                {
                    return false;
                }
            }
            return true;
        }

        List<Cell> PointsToCells(List<Point> points)
        {
            return points.Select(p => _maze.MazeArray[p.X, p.Y]).ToList();
        }

        Cell GetRandom(List<Cell> cells)
        {
            return cells[r.Next(cells.Count)];
        }


        #endregion Depth-First Search

        #region Solve maze with Breadth-First Search

        public bool BreadthFirstSearch()
        {
            // Implement Breadth First Search here

            Cell current = _maze.Begin;
            
            // Display where we are
            _maze.CurrentSolvePosition = current.Position;

            //mark as visited to avoid infinite loops and mark it green
            current.IsVisited = true;
            while (current.Position != _maze.End.Position)
            {
                //move one step to the right
                if (current.Position.X + 1 < _maze.Width && !current.CellWalls[2])
                {
                    Cell next = _maze.MazeArray[current.Position.X + 1, current.Position.Y];
                    next.PreviousCell = current;
                    current = next;
                }
                // Display where we are
                _maze.CurrentSolvePosition = current.Position;

                //mark as visited to avoid infinite loops and mark it green
                current.IsVisited = true;

                Thread.Sleep(50);
            }

            Thread.Sleep(400);
            ShowFoundPath(current);
            return true;

        }

        private void ShowFoundPath(Cell cell)
        {
            var path = new List<Cell>();
            while (cell != null)
            {
                path.Add(cell);
                cell = cell.PreviousCell;
            }
            _maze.FoundPath = path;
        }

        #endregion Breadth-First Search

    }
}
