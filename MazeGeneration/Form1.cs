using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MazeGeneratorAndSolver;
namespace MazeGeneration
{
    public partial class Form1 : Form
    {
        private GenerateMaze _mazeGenerator;
        private Maze _maze;
        private BackgroundWorker _backGroundWorker;
        private bool hasSolution;
        public Form1()
        {
            InitializeComponent();
            timer1.Enabled = false;

            _backGroundWorker = new BackgroundWorker();
            _backGroundWorker.DoWork +=_backGroundWorker_DoWork;
            _backGroundWorker.RunWorkerCompleted += _backGroundWorker_RunWorkerCompleted;
        }

        private void GenerateBtn_Click(object sender, EventArgs e)
        {
            // generate Maze here using GenerateMaze class
            GenerateBtn.Enabled = false;
            SolveBtn.Enabled = false;
            this.timer1.Enabled = true;
            this.hasSolution = false;
            this._backGroundWorker.RunWorkerAsync(new object[] { 30, false });
        }

        private void SolveBtn_Click(object sender, EventArgs e)
        {
            // solve maze here using MazeSolver class
            GenerateBtn.Enabled = false;
            SolveBtn.Enabled = false;
            this.timer1.Enabled = true;
            if (_maze != null)
            {
                _maze.Solving = true;
                this.hasSolution = true;
                this._backGroundWorker.RunWorkerAsync(new object[] { 30, true });
            }
        }

        private void mazePicBox_Paint(object sender, PaintEventArgs e)
        {
            if (this._maze != null)
            {
                this._maze.Draw(e.Graphics);
                if (this.hasSolution)
                    this._maze.DrawPath(e.Graphics);
            }
        }

        private void _backGroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] args = e.Argument as object[];

            int value = 25; // Cell size
            bool solving = (bool)args[1];
            if (!solving)
            {
                //this._maze.Generate(this.pictureBoxDraw.Width / value,
                //    (this.pictureBoxDraw.Height - value) / value,
                //    (int)args[2]);
                _maze = new Maze(mazePicBox.Width, mazePicBox.Height, 25);
                _mazeGenerator = new GenerateMaze(_maze);
                _mazeGenerator.Generate(value);
            }
            else
            {
                var solver = new MazeSolver(_maze);
                solver.Solve();
                this.hasSolution = true;
            }
            this.mazePicBox.Invalidate();
        }

        private void _backGroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GenerateBtn.Enabled = true;
            SolveBtn.Enabled = true;
            this.timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            mazePicBox.Invalidate();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this._maze = new Maze(this.mazePicBox.Width, this.mazePicBox.Height, 25);
           
          
            // re-draw the picture
            //this.mazePicBox.Invalidate();
        }
    }
}
