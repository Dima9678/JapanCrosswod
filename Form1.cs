using System;
using System.Drawing;
using System.Windows.Forms;

namespace MyWinFormsApp
{
    public partial class Form1 : Form
    {
        //Размер поля
        const int gridSize = 10;
        //Размер клетки в пикселях
        const int cellSize = 80;

        TaskCompletionSource<bool> keyWaiter;
        
        void OnKeyDown(object sender, KeyEventArgs e)
        {
            keyWaiter?.TrySetResult(true);
        }

        //модель данных
        int[,] cells = new int[gridSize, gridSize];



        //Конструктор формы
        public Form1()
        {
            InitializeComponent(); //Создание окна
            this.DoubleBuffered = true; //Убирает мерцание при перерисовке

            this.KeyPreview = true;
            this.KeyDown += OnKeyDown;
            // пример: закрасить верхнюю строку при запуске
            Main();
        }

        //Бэкенд
        async void Main()
        {   //заполнение полных горизонтальных линий
            int cycle = 0;
            int[][] fillHorisontal =
            {
[2],
[2,3],
[2,3],
[2,3],
[7],
[6],
[2,3],
[2,3],
[2,4],
[2,3],
            };
            int[][] fillVertical = 
            {
[9],
[9],
[2],
[2],
[3],
[5],
[3,3],
[3,3],
[3,2],
[2,2],
};

            


            while (true)
            {
                //Если количество пустых клеток совпадает с цифрой, заполняются все белые клетки
                //Заполнение полных горизонтальных линий
                for (int i = 0; i < fillHorisontal.Length; i++)
                {
                    if (fillHorisontal[i].Length > 1)
                    {
                        continue;
                    }
                    int whitePix = 0;
                    for (int j = 0; j < gridSize; j++)
                    {
                        if (cells[i,j] == 0 | cells[i, j] == 1)
                        {
                            whitePix++;
                        }
                    }
                    if(fillHorisontal[i][0] == whitePix)
                    {
                        for (int j = 0; j < gridSize; j++)
                        {
                            if (cells[i,j] == 0)
                            {
                                cells[i,j] = 1;
                            }
                        }
                    }
                }
                //заполнение полных ветрикальных линий
                for (int i = 0; i < fillVertical.Length; i++)
                {
                    //Короче, если в столбике будет 2 группы клеток, то скип
                    if (fillVertical[i].Length > 1)
                    {
                        continue;
                    }

                    int whitePix = 0;
                    for (int j = 0; j < gridSize; j++)
                    {
                        if (cells[j,i] == 0 | cells[j,i] == 1)
                        {
                            whitePix++;
                        }
                    }
                    if(fillVertical[i][0] == whitePix)
                    {
                        for (int j = 0; j < gridSize; j++)
                        {
                            if (cells[j,i] == 0)
                            {
                                cells[j,i] = 1;
                            }
                        }
                    }
                }


                //Заполнение поля серыми клеточками
                for (int i = 0; i < fillHorisontal.Length; i++)
                {
                    int blackPix = 0;
                    for (int j = 0; j < gridSize; j++)
                    {
                        if (cells[i,j] == 1)
                        {
                            blackPix++;
                        }
                    }
                    if (blackPix == fillHorisontal[i][0])
                    {
                        for (int j = 0; j < gridSize; j++)
                        {
                            if (cells[i,j] == 0)
                            {
                                cells[i,j] = 2;
                            }
                        }
                    }
                }
                for (int i = 0; i < fillVertical.Length; i++)
                {
                    int blackPix = 0;
                    for (int j = 0; j < gridSize; j++)
                    {
                        if (cells[j,i] == 1)
                        {
                            blackPix++;
                        }
                    }
                    if (blackPix == fillVertical[i][0])
                    {
                        for (int j = 0; j < gridSize; j++)
                        {
                            if (cells[j,i] == 0)
                            {
                                cells[j,i] = 2;
                            }
                        }
                    }
                }
                

                
                //если осталось три пустые клетки, и нужно поставить две единицы
                for (int i = 0; i < fillHorisontal.Length; i++)
                {
                    if (fillHorisontal[i].Length >= 2)
                    {
                        //поиск количества пустых клеток
                        int emptyPix = 0;
                        for (int j = 0; j < gridSize; j++)
                        {
                            if (cells[i,j] == 0)
                            {
                                emptyPix++;
                            }
                        }
                        //поиск суммы клеток
                        int sum = 0;
                        for (int j = 0; j < fillHorisontal[i].Length; j++)
                        {
                            sum += fillHorisontal[i][j];
                        }
                        sum = sum + fillHorisontal[i].Length - 1;
                        //если количество клето + пробелы совпадают с оставшимися клетками, доставить
                        if (emptyPix == sum)
                        {
                            //поиск индекса первой пустой клетки
                            int j = 0;
                            for (; j < gridSize; j++)
                            {
                                //когда нашлась первая свободная клетка
                                if (cells[i,j] == 0)
                                {
                                    break;
                                }
                            }

                            //берется первый элемент из строчки [1 2 3]
                            for (int k = 0; k < fillHorisontal[i].Length; k++)
                            {
                                //начинается заполнение элементов
                                //j свигает курсор
                                for (int l = 0; l < fillHorisontal[i][k]; l++)
                                {
                                    cells[i,j] = 1;
                                    j++;
                                }
                                cells[i,j] = 2;
                                j++;
                            }
                        }
                    }
                }



                //если осталось три пустые клетки, и нужно поставить две единицы
                for (int i = 0; i < fillVertical.Length; i++)
                {
                    if (fillVertical[i].Length >= 2)
                    {
                        //поиск количества пустых клеток
                        int emptyPix = 0;
                        for (int j = 0; j < gridSize; j++)
                        {
                            if (cells[j,i] == 0)
                            {
                                emptyPix++;
                            }
                        }
                        //поиск суммы клеток
                        int sum = 0;
                        for (int j = 0; j < fillVertical[i].Length; j++)
                        {
                            sum += fillVertical[i][j];
                        }
                        sum = sum + fillVertical[i].Length - 1;
                        //если количество клето + пробелы совпадают с оставшимися клетками, доставить
                        if (emptyPix == sum)
                        {
                            //поиск индекса первой пустой клетки
                            int j = 0;
                            for (; j < gridSize; j++)
                            {
                                //когда нашлась первая свободная клетка
                                if (cells[j,i] == 0)
                                {
                                    break;
                                }
                            }

                            //берется первый элемент из строчки [1 2 3]
                            for (int k = 0; k < fillVertical[i].Length; k++)
                            {
                                //начинается заполнение элементов
                                //j свигает курсор
                                for (int l = 0; l < fillVertical[i][k]-1; l++)
                                {
                                    cells[j,i] = 1;
                                    j++;
                                }
                                cells[j,i] = 2;
                                j++;
                            }
                        }
                    }
                }


                //Если количество клеток для заполнения = 9, а размер поля 10, заполняются 8 в центре(строки)
                for (int i = 0; i < fillHorisontal.Length; i++)
                {
                    if (fillHorisontal[i].Length > 1)
                    {
                        continue;
                    }
                    int whitePix = 0;
                    for (int j = 0; j < gridSize; j++)
                    {
                        if (cells[i,j] == 0 | cells[i, j] == 1)
                        {
                            whitePix++;
                        }
                    }

                    if(fillHorisontal[i][0]*2 > whitePix)
                    {
                        //отнимаем 10-9=1 с каждой стороны
                        //Размер отступа с каждой стороны
                        int indent = whitePix - fillHorisontal[i][0];
                        // количество черных клеток, которые нужно заполнить
                        //В данном примере будет 8
                        int blackPix = gridSize - indent - indent;
                        //счетчик отвечает за количество пропущенных белых клеток
                        //в даном примере будет одна
                        int skipPixCounter = 0;
                        int filledPixCounter = 0;
                        for (int j = 0; j < gridSize; j++)
                        {
                            if (cells[i,j] == 0)
                            {
                                if (skipPixCounter < indent)
                                {
                                    skipPixCounter++;
                                }
                                else
                                {
                                    if (filledPixCounter < blackPix)
                                    {
                                        cells[i,j] = 1;
                                        filledPixCounter++;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                        
                                }
                            }
                        }
                    }
                }

                
                //Если количество клеток для заполнения = 9, а размер поля 10, заполняются 8 в центре(столбцы))
                for (int i = 0; i < fillVertical.Length; i++)
                {
                    if (fillVertical[i].Length > 1)
                    {
                        continue;
                    }
                    int whitePix = 0;
                    for (int j = 0; j < gridSize; j++)
                    {
                        if (cells[j,i] == 0 | cells[j,i] == 1)
                        {
                            whitePix++;
                        }
                    }

                    if (fillVertical[i][0] * 2 > whitePix)
                    {
                        //отнимаем 10-9=1 с каждой стороны
                        //Размер отступа с каждой стороны
                        int indent = whitePix - fillVertical[i][0];
                        // количество черных клеток, которые нужно заполнить
                        //В данном примере будет 8
                        int blackPix = gridSize - indent - indent;
                        //счетчик отвечает за количество пропущенных белых клеток
                        //в даном примере будет одна
                        int skipPixCounter = 0;
                        int filledPixCounter = 0;
                        for (int j = 0; j < gridSize; j++)
                        {
                            if (cells[j,i] == 0)
                            {
                                if (skipPixCounter < indent)
                                {
                                    skipPixCounter++;
                                }
                                else
                                {
                                    if (filledPixCounter < blackPix)
                                    {
                                        cells[j,i] = 1;
                                        filledPixCounter++;
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                }
                            }
                        }
                    }
                }

                //Соединение разрывов, между большими группами клеток(строки)
                for (int i = 0; i < fillHorisontal.Length; i++)
                {
                    if (fillHorisontal[i].Length > 1)
                    {
                        continue;
                    }
                    int firstBlackPixIndex = 0;
                    int lastBlackPixIndex = 0;
                    bool neetToConnect = false;
                    //Ищем первый черный пиксель
                    for (int j = 0; j < fillHorisontal[i].Length; j++)
                    {
                        if (cells[i,j] == 1)
                        {
                            firstBlackPixIndex = j;
                            neetToConnect = true;
                            break;
                        }
                    }
                    //Ищем последний черный пиксель
                    for (int j = gridSize - 1; j >= 0 ; j--)
                    {
                        if (cells[i, j] == 1)
                        {
                            lastBlackPixIndex = j;
                            neetToConnect = true;
                            break;
                        }
                    }
                    //заполнение от первого до последнего черного пикселя
                    for (int j = 0; j < gridSize; j++)
                    {
                        if(j >= firstBlackPixIndex & j <= lastBlackPixIndex & neetToConnect == true)
                        {
                            cells[i,j] = 1;
                        }
                    }
                }

                
                //Соединение разрывов, между большими группами клеток(столбцы)
                for (int i = 0; i < fillVertical.Length; i++)
                {
                    if (fillVertical[i].Length > 1)
                    {
                        continue;
                    }
                    int firstBlackPixIndex = 0;
                    int lastBlackPixIndex = 0;
                    bool neetToConnect = false;
                    //Ищем первый черный пиксель
                    for (int j = 0; j < fillVertical[i].Length; j++)
                    {
                        if (cells[j,i] == 1)
                        {
                            firstBlackPixIndex = j;
                            neetToConnect = true;
                            break;
                        }
                    }
                    //Ищем последний черный пиксель
                    for (int j = gridSize - 1; j >= 0 ; j--)
                    {
                        if (cells[j,i] == 1)
                        {
                            lastBlackPixIndex = j;
                            neetToConnect = true;
                            break;
                        }
                    }
                    //заполнение от первого до последнего черного пикселя
                    for (int j = 0; j < gridSize; j++)
                    {
                        if(j >= firstBlackPixIndex & j <= lastBlackPixIndex & neetToConnect == true)
                        {
                            cells[j,i] = 1;
                        }
                    }
                }

                cycle++;
                this.Invalidate(); // перерисовать

                keyWaiter = new TaskCompletionSource<bool>();
                await keyWaiter.Task; // ждать клавишу
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    Rectangle rect = new Rectangle(j * cellSize,i * cellSize,cellSize,cellSize);

                    if (cells[i, j] == 1)
                    {
                        g.FillRectangle(Brushes.Black, rect);
                    }
                    else if (cells[i, j] == 2)
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(192, 192, 192)), rect); 
                    }
                    else
                    {
                        g.FillRectangle(Brushes.White, rect);
                    }
                    g.DrawRectangle(Pens.Gray, rect);

                }
            }
        }
    }
}