using System;
using System.Data.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameOfLife
{
    class Grid
    {

        private int SizeX;
        private int SizeY;
        private Cell[,] cells;
        private Cell[,] nextGenerationCells;
        private static Random rnd;
        private Canvas drawCanvas;
        private Ellipse[,] cellsVisuals;

        
        public Grid(Canvas c)
        {
            drawCanvas = c;
            rnd = new Random();
            SizeX = (int) (c.Width / 5);
            SizeY = (int)(c.Height / 5);
            cells = new Cell[SizeX, SizeY];
            nextGenerationCells = new Cell[SizeX, SizeY];
            cellsVisuals = new Ellipse[SizeX, SizeY];
 
            for (int i = 0; i < SizeX; i++)
                for (int j = 0; j < SizeY; j++)
                {
                    cells[i, j] = new Cell(i, j, 0, false);
                }

            SetRandomPattern();
            InitCellsVisuals();
            
        }


        public void Clear()
        {
            for (int i = 0; i < SizeX; i++)
                for (int j = 0; j < SizeY; j++)
                {
                    cells[i, j] = new Cell(i, j, 0, false);
                    nextGenerationCells[i, j] = new Cell(i, j, 0, false);
                    cellsVisuals[i, j].Fill = Brushes.Gray;
                }
        }


        void MouseMove(object sender, MouseEventArgs e)
        {
            var cellVisual = sender as Ellipse;
            
            int i = (int) cellVisual.Margin.Left / 5;
            int j = (int) cellVisual.Margin.Top / 5;
            

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!cells[i, j].IsAlive)
                {
                    cells[i, j].IsAlive = true;
                    cells[i, j].Age = 0;
                    cellVisual.Fill = Brushes.White;
                }
            }
        }

        public void UpdateGraphics()
        {
            for (int i = 0; i < SizeX; i++)
                for (int j = 0; j < SizeY; j++)
                    cellsVisuals[i, j].Fill = cells[i, j].IsAlive
                                                  ? (cells[i, j].Age < 2 ? Brushes.White : Brushes.DarkGray)
                                                  : Brushes.Gray;
        }

        public void InitCellsVisuals()
        {
            for (int i = 0; i < SizeX; i++)
                for (int j = 0; j < SizeY; j++)
                {
                    cellsVisuals[i, j] = new Ellipse();
                    cellsVisuals[i, j].Width = cellsVisuals[i, j].Height = 5;
                    double left = cells[i, j].PositionX;
                    double top = cells[i, j].PositionY;
                    cellsVisuals[i, j].Margin = new Thickness(left, top, 0, 0);
                    cellsVisuals[i, j].Fill = Brushes.Gray;
                    drawCanvas.Children.Add(cellsVisuals[i, j]);

                    cellsVisuals[i, j].MouseMove += MouseMove;
                    cellsVisuals[i, j].MouseLeftButtonDown += MouseMove;
                 }
            UpdateGraphics();
                    
        }
        

        public static bool GetRandomBoolean()
        {
            return rnd.NextDouble() > 0.8;
        }

        public void SetRandomPattern()
        {
            for (int i = 0; i < SizeX; i++)
                for (int j = 0; j < SizeY; j++)
                    cells[i, j].IsAlive = GetRandomBoolean();
        }
        
        public void UpdateToNextGeneration()
        {
            for (int i = 0; i < SizeX; i++)
                for (int j = 0; j < SizeY; j++)
                {
                    cells[i, j].IsAlive = nextGenerationCells[i, j].IsAlive;
                    cells[i, j].Age = nextGenerationCells[i, j].Age;
                }

            UpdateGraphics();
        }
        

        public void Update()
        {
            bool alive = false;
            int age = 0;

            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    nextGenerationCells[i, j] = CalculateNextGeneration(i,j);          // UNOPTIMIZED change to OPTIMIZED
                    //CalculateNextGeneration(i, j, ref alive, ref age);   // UNOPTIMIZED
                    //nextGenerationCells[i, j].IsAlive = alive;   // UNOPTIMIZED
                    //nextGenerationCells[i, j].Age = age;   // UNOPTIMIZED
                }
            }
            UpdateToNextGeneration();
        }

        public Cell CalculateNextGeneration(int row, int column)    // OPTIMIZED
        {
            bool alive;
            int count;

            alive = cells[row, column].IsAlive;
            //age = cells[row, column].Age; not using here 
            count = CountNeighbors(row, column);

            if (alive && count < 2)
                return new Cell(row, column, 0, false);
            
            if (alive && (count == 2 || count == 3))
            {
                cells[row, column].Age++;
                return new Cell(row, column, cells[row, column].Age, true);
            }

            if (!alive && count == 3) // Wikipedia game info Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
                return new Cell(row, column, 0, true);
            
            return new Cell(row, column, 0, false);
        }

        public void CalculateNextGeneration(int row, int column, ref bool isAlive, ref int age)     // OPTIMIZED
        {
            isAlive = cells[row, column].IsAlive;
            age = cells[row, column].Age;

            int count = CountNeighbors(row, column);

            if (isAlive && (count != 2 || count != 3)) // Wikipedia game info Any live cell with two or three live neighbours lives on to the next generation. we look isAlive here
            {
                isAlive = false;
                age = 0;
            }

            if (isAlive && (count == 2 || count == 3))
            {
                cells[row, column].Age++;
                isAlive = true;
                age = cells[row, column].Age;
            }

            if (isAlive && count > 3)
            {
                isAlive = false;
                age = 0;
            }

            if (!isAlive && count == 3)
            {
                isAlive = true;
                age = 0;
            }
        }

        public int CountNeighbors(int i, int j)
        {
            int count = 0;

            //if (i != SizeX - 1 && cells[i + 1, j].IsAlive) count++;
            //if (i != SizeX - 1 && j != SizeY - 1 && cells[i + 1, j + 1].IsAlive) count++;
            //if (j != SizeY - 1 && cells[i, j + 1].IsAlive) count++;
            //if (i != 0 && j != SizeY - 1 && cells[i - 1, j + 1].IsAlive) count++;
            //if (i != 0 && cells[i - 1, j].IsAlive) count++;
            //if (i != 0 && j != 0 && cells[i - 1, j - 1].IsAlive) count++;
            //if (j != 0 && cells[i, j - 1].IsAlive) count++;
            //if (i != SizeX - 1 && j != 0 && cells[i + 1, j - 1].IsAlive) count++;

            for(int f = i - 1; f <= i + 1; f++) //Redundant Loops are fixed here
            {
                for (int k = j - 1; k <= j + 1; k++)
                {
                    if (f == i && k == j) continue;

                    if (f >= 0 && f < SizeX && k >= 0 && k < SizeY)
                    {
                        if (cells[f, k].IsAlive) count++;
                    }
                }
            }

            return count;
        }
    }
}