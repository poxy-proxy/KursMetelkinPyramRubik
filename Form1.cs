using System;
using System.Drawing;
using System.Windows.Forms;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;
using System.Drawing;
using System.Drawing.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Numerics;

namespace KursMetelkinPyramRubik
{
    public partial class Form1 : Form
    {
        float global_time = 0; // массив с параметрами установки камеры
       

        // экземпляра класса Explosion
        private Explosion BOOOOM_1 = new Explosion(1, 10, 1, 300, 500);

        private float angleX = 0.0f;
        private float angleY = -180.0f;
        private float angleXPer = 0.0f;
        private float angleYPer = -180.0f;
        private float scale = 1.0f;
        private float scalePer = 1.0f;

        // Переменные для хранения идентификаторов текстурных объектов
        private int wallTextureID;
        private int floorTextureID;
        private int ceilingTextureID;

        private float rot_1, rot_2;

        private double[,] GeometricArray = new double[64, 3];
        private double[,,] ResaultGeometric = new double[64, 64, 3];

        private int n = 30;
        private double a = -0.5;
        private double b = 8;
        private double h;
        private double Angle = 2 * Math.PI / 64;
        private int Iter = 64;
        private int count_elements;
        const int N = 5;
        double[] X = new double[N];
        double[] Y = new double[N];
        private int IterAnim = 0;

        public static double LagrangeInterpolation(double[] x, double[] y, double xval)
        {
            double yval = 0.0;
            double Products = y[0];
            for (int i = 0; i < x.Length; i++)
            {
                Products = y[i];
                for (int j = 0; j < x.Length; j++)
                {
                    if (i != j)
                    {
                        Products *= (xval - x[j]) / (x[i] - x[j]);
                    }
                }
                yval += Products;
            }
            return yval;
        }

        public Form1()
        {
            InitializeComponent();
            simpleOpenGlControl1.InitializeContexts();
            Glut.glutInit();
            simpleOpenGlControl1.KeyDown += MainForm_KeyDown;
            LoadTextures();


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Gl.glClearColor(1.0f, 1.0f, 1.0f, 0.0f); // Изменено на белый цвет (R=1, G=1, B=1)

            Gl.glViewport(0, 0, simpleOpenGlControl1.Width, simpleOpenGlControl1.Height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            if (radioButton5.Checked)
            {
                Gl.glOrtho(-2, 2, -2, 2, -10, 10); // Ортогональная проекция
            }
            else if (radioButton4.Checked)
            {
                Glu.gluPerspective(45, (float)simpleOpenGlControl1.Width / (float)simpleOpenGlControl1.Height, 0.1, 100); // Перспективная проекция
            }
            else
            {
                Glu.gluPerspective(45, (float)simpleOpenGlControl1.Width / (float)simpleOpenGlControl1.Height, 0.1, 100);
            }

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glEnable(Gl.GL_DEPTH_TEST);

            // количество элементов последовательности геометрии, на основе которых будет строится тело вращения
            count_elements = 32;

            h = (b - a) / n;
            // непосредственное заполнение точек
            // после изменения данной геометрии мы сразу получим новое тело вращения
            GeometricArray[0, 0] = 0;
            GeometricArray[0, 1] = 0;
            GeometricArray[0, 2] = 0;

            GeometricArray[1, 0] = 0.7;
            GeometricArray[1, 1] = 0;
            GeometricArray[1, 2] = 1;

            GeometricArray[2, 0] = 1.3;
            GeometricArray[2, 1] = 0;
            GeometricArray[2, 2] = 2;

            GeometricArray[3, 0] = 1.0;
            GeometricArray[3, 1] = 0;
            GeometricArray[3, 2] = 3;

            GeometricArray[4, 0] = 0.5;
            GeometricArray[4, 1] = 0;
            GeometricArray[4, 2] = 4;

            GeometricArray[5, 0] = 3;
            GeometricArray[5, 1] = 0;
            GeometricArray[5, 2] = 6;

            GeometricArray[6, 0] = 1;
            GeometricArray[6, 1] = 0;
            GeometricArray[6, 2] = 7;

            GeometricArray[7, 0] = 0;
            GeometricArray[7, 1] = 0;
            GeometricArray[7, 2] = 7.2f;

            GeometricArray[8, 0] = 0;
            GeometricArray[8, 1] = 0;
            GeometricArray[8, 2] = 8;

            GeometricArray[9, 0] = 0.7;
            GeometricArray[9, 1] = 0;
            GeometricArray[9, 2] = 9;

            GeometricArray[10, 0] = 1.3;
            GeometricArray[10, 1] = 0;
            GeometricArray[10, 2] = 10;

            GeometricArray[11, 0] = 1.0;
            GeometricArray[11, 1] = 0;
            GeometricArray[11, 2] = 11;

            GeometricArray[12, 0] = 0.5;
            GeometricArray[12, 1] = 0;
            GeometricArray[12, 2] = 12;

            GeometricArray[13, 0] = 3;
            GeometricArray[13, 1] = 0;
            GeometricArray[13, 2] = 13;

            GeometricArray[14, 0] = 1;
            GeometricArray[14, 1] = 0;
            GeometricArray[14, 2] = 14;

            GeometricArray[15, 0] = 0;
            GeometricArray[15, 1] = 0;
            GeometricArray[15, 2] = 15.2f;

            GeometricArray[16, 0] = 0;
            GeometricArray[16, 1] = 0;
            GeometricArray[16, 2] = 16;

            GeometricArray[17, 0] = 0.7;
            GeometricArray[17, 1] = 0;
            GeometricArray[17, 2] = 17;

            GeometricArray[18, 0] = 1.3;
            GeometricArray[18, 1] = 0;
            GeometricArray[18, 2] = 18;

            GeometricArray[19, 0] = 1.0;
            GeometricArray[19, 1] = 0;
            GeometricArray[19, 2] = 19;

            GeometricArray[20, 0] = 0.5;
            GeometricArray[20, 1] = 0;
            GeometricArray[20, 2] = 20;

            GeometricArray[21, 0] = 3;
            GeometricArray[21, 1] = 0;
            GeometricArray[21, 2] = 21;

            GeometricArray[22, 0] = 1;
            GeometricArray[22, 1] = 0;
            GeometricArray[22, 2] = 22;

            GeometricArray[23, 0] = 0;
            GeometricArray[23, 1] = 0;
            GeometricArray[23, 2] = 23.2f;

            GeometricArray[24, 0] = 0;
            GeometricArray[24, 1] = 0;
            GeometricArray[24, 2] = 24;

            GeometricArray[25, 0] = 0.7;
            GeometricArray[25, 1] = 0;
            GeometricArray[25, 2] = 25;

            GeometricArray[26, 0] = 1.3;
            GeometricArray[26, 1] = 0;
            GeometricArray[26, 2] = 26;

            GeometricArray[27, 0] = 1.0;
            GeometricArray[27, 1] = 0;
            GeometricArray[27, 2] = 27;

            GeometricArray[28, 0] = 0.5;
            GeometricArray[28, 1] = 0;
            GeometricArray[28, 2] = 28;

            GeometricArray[29, 0] = 3;
            GeometricArray[29, 1] = 0;
            GeometricArray[29, 2] = 29.2f;

            GeometricArray[30, 0] = 1;
            GeometricArray[30, 1] = 0;
            GeometricArray[30, 2] = 30;

            GeometricArray[31, 0] = 0;
            GeometricArray[31, 1] = 0;
            GeometricArray[31, 2] = 31.2f;

            GeometricArray[32, 0] = 0;
            GeometricArray[32, 1] = 0;
            GeometricArray[32, 2] = 32;


            // Points to interpolate
            X[0] = 1.0; Y[0] = 22.0;
            X[1] = 20.0; Y[1] = 29.5;
            X[2] = 30.0; Y[2] = 26.4;
            X[3] = 70.0; Y[3] = 199;
            X[4] = 150.0; Y[4] = 291.0;
            // по умолчанию мы будем отрисовывать фигуру в режиме GL_POINTS
          //  comboBox1.SelectedIndex = 0;

            // цикл по последовательности точек кривой, на основе которой будет построено тело вращения
            for (int ax = 0; ax < n; ax++)
            {

                // цикл по меридианам объекта, заранее определенным в программе
                for (int bx = 0; bx < Iter; bx++)
                {

                    // для всех (bx > 0) элементов алгоритма используются предыдушая построенная последовательность
                    // для ее поворота на установленный угол
                    if (bx > 0)
                    {

                        double new_x = ResaultGeometric[ax, bx - 1, 0] * Math.Cos(Angle) - ResaultGeometric[ax, bx - 1, 1] * Math.Sin(Angle);
                        double new_y = ResaultGeometric[ax, bx - 1, 0] * Math.Sin(Angle) + ResaultGeometric[ax, bx - 1, 1] * Math.Cos(Angle);
                        ResaultGeometric[ax, bx, 0] = new_x;
                        ResaultGeometric[ax, bx, 1] = new_y;
                        ResaultGeometric[ax, bx, 2] = GeometricArray[ax, 2];

                    }
                    else // для построения первого меридиана мы используем начальную кривую, описывая ее нулевым значением угла поворота
                    {

                        double new_x = ax;
                        double new_y = LagrangeInterpolation(X, Y, new_x);
                        ResaultGeometric[ax, bx, 0] = new_x;
                        ResaultGeometric[ax, bx, 1] = new_y;
                        ResaultGeometric[ax, bx, 2] = GeometricArray[ax, 2];

                    }

                }

            }

            // активация таймера
           // RenderTimer.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            // вызываем функцию, отвечающую за отрисовку сцены
          //  Draw();

        }
        // функция отрисовки сцены
        private void Draw()
        {
            Gl.glColor3f(0.0f, 0.0f, 1.0f);

            // два параметра, которые мы будем использовать для непрерывного вращения сцены вокруг двух координатных осей
            rot_1++;
            rot_2++; // очистка буфера цвета и буфера глубины
           // Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
          //  Gl.glClearColor(255, 255, 255, 1);
            // очищение текущей матрицы
          //  Gl.glLoadIdentity();

            // установка положения камеры (наблюдателя). Как видно из кода
            // дополнительно на положение наблюдателя по оси Z влияет значение,
            // установленное в ползунке, доступном для пользователя

            // таким образом, при перемещении ползунка наблюдатель будет отдалятся или приближатся к объекту наблюдения
            Gl.glTranslated(0, 0, -7);//- trackBar1.Value);
            // 2 поворота (углы rot_1 и rot_2)
            Gl.glRotated(rot_1, 1, 0, 0);
            Gl.glRotated(rot_2, 0, 1, 0);

            // устанавливаем размер точек равный 5
            Gl.glPointSize(5.0f);

            // условие switch определяет установленный режим отображения на основе выбранного пункта
            // элемента comboBox, установленного в форме программы
            switch (0)
            {

                case 0: // отображение в виде точек
                    {

                        // режим вывода геометрии - точки
                        Gl.glBegin(Gl.GL_POINTS);

                        // выводим всю ранее просчитанную геометрию объекта
                        for (int ax = 0; ax < n; ax++)
                        {

                            for (int bx = 0; bx < Iter; bx++)
                            {

                                // отрисовка точки
                                Gl.glVertex3d(ResaultGeometric[ax, bx, 0]/100, ResaultGeometric[ax, bx, 1]/100, ResaultGeometric[ax, bx, 2]/100);

                            }

                        }
                        // завершаем режим рисования
                        Gl.glEnd();

                        break;

                    }
                case 1: // отображение объекта в сеточном режиме, используя режим GL_LINES_STRIP
                    {

                        // устанавливаем режим отрисвки линиями (последовательность линий)
                        Gl.glBegin(Gl.GL_LINE_STRIP);
                        for (int ax = 0; ax < n; ax++)
                        {

                            for (int bx = 0; bx < Iter; bx++)
                            {


                                Gl.glVertex3d(ResaultGeometric[ax, bx, 0], ResaultGeometric[ax, bx, 1], ResaultGeometric[ax, bx, 2]);
                                Gl.glVertex3d(ResaultGeometric[ax + 1, bx, 0], ResaultGeometric[ax + 1, bx, 1], ResaultGeometric[ax + 1, bx, 2]);

                                if (bx + 1 < Iter - 1)
                                {

                                    Gl.glVertex3d(ResaultGeometric[ax + 1, bx + 1, 0], ResaultGeometric[ax + 1, bx + 1, 1], ResaultGeometric[ax + 1, bx + 1, 2]);

                                }
                                else
                                {

                                    Gl.glVertex3d(ResaultGeometric[ax + 1, 0, 0], ResaultGeometric[ax + 1, 0, 1], ResaultGeometric[ax + 1, 0, 2]);

                                }

                            }

                        }
                        Gl.glEnd();
                        break;

                    }
                case 2: // отрисовка оболочки с расчетом нормалей для корректного затенения граней объекта
                    {

                        Gl.glBegin(Gl.GL_QUADS); // режим отрисовки полигонов, состоящих из 4 вершин
                        for (int ax = 0; ax < n; ax++)
                        {

                            for (int bx = 0; bx < Iter; bx++)
                            {

                                // вспомогательные переменные для более наглядного использования кода при расчете нормалей
                                double x1 = 0, x2 = 0, x3 = 0, x4 = 0, y1 = 0, y2 = 0, y3 = 0, y4 = 0, z1 = 0, z2 = 0, z3 = 0, z4 = 0;

                                // первая вершина
                                x1 = ResaultGeometric[ax, bx, 0];
                                y1 = ResaultGeometric[ax, bx, 1];
                                z1 = ResaultGeometric[ax, bx, 2];

                                if (ax + 1 < n) // если текущий ax не последний
                                {

                                    // берем следующую точку последовательности
                                    x2 = ResaultGeometric[ax + 1, bx, 0];
                                    y2 = ResaultGeometric[ax + 1, bx, 1];
                                    z2 = ResaultGeometric[ax + 1, bx, 2];

                                    if (bx + 1 < Iter - 1) // если текущий bx не последний
                                    {

                                        // берем следующую точку последовательности и следующий меридиан
                                        x3 = ResaultGeometric[ax + 1, bx + 1, 0];
                                        y3 = ResaultGeometric[ax + 1, bx + 1, 1];
                                        z3 = ResaultGeometric[ax + 1, bx + 1, 2];

                                        // точка, соотвествующуя по номеру только на соседнем меридиане
                                        x4 = ResaultGeometric[ax, bx + 1, 0];
                                        y4 = ResaultGeometric[ax, bx + 1, 1];
                                        z4 = ResaultGeometric[ax, bx + 1, 2];

                                    }
                                    else
                                    {

                                        // если это последний меридиан, то в качестве следующего мы берем начальный (замыкаем геометрию фигуры)
                                        x3 = ResaultGeometric[ax + 1, 0, 0];
                                        y3 = ResaultGeometric[ax + 1, 0, 1];
                                        z3 = ResaultGeometric[ax + 1, 0, 2];

                                        x4 = ResaultGeometric[ax, 0, 0];
                                        y4 = ResaultGeometric[ax, 0, 1];
                                        z4 = ResaultGeometric[ax, 0, 2];

                                    }

                                }
                                else // данный элемент ax последний, следовательно мы будем использовать начальный (нулевой) вместо данного ax
                                {

                                    // слудуещей точкой будет нулевая ax
                                    x2 = ResaultGeometric[0, bx, 0];
                                    y2 = ResaultGeometric[0, bx, 1];
                                    z2 = ResaultGeometric[0, bx, 2];


                                    if (bx + 1 < Iter - 1)
                                    {

                                        x3 = ResaultGeometric[0, bx + 1, 0];
                                        y3 = ResaultGeometric[0, bx + 1, 1];
                                        z3 = ResaultGeometric[0, bx + 1, 2];

                                        x4 = ResaultGeometric[ax, bx + 1, 0];
                                        y4 = ResaultGeometric[ax, bx + 1, 1];
                                        z4 = ResaultGeometric[ax, bx + 1, 2];

                                    }
                                    else
                                    {

                                        x3 = ResaultGeometric[0, 0, 0];
                                        y3 = ResaultGeometric[0, 0, 1];
                                        z3 = ResaultGeometric[0, 0, 2];

                                        x4 = ResaultGeometric[ax, 0, 0];
                                        y4 = ResaultGeometric[ax, 0, 1];
                                        z4 = ResaultGeometric[ax, 0, 2];

                                    }

                                }


                                // переменные для расчета нормали
                                double n1 = 0, n2 = 0, n3 = 0;

                                // нормаль будем расчитывать как векторное произведение граней полигона
                                // для нулевого элемента нормаль мы будем считать немного по-другому

                                // на самом деле разница в расчете нормали актуальна только для последнего и первого полигона на меридиане

                                if (ax == 0) // при расчете нормали для ax мы будем использовать точки 1,2,3
                                {

                                    n1 = (y2 - y1) * (z3 - z1) - (y3 - y1) * (z2 - z1);
                                    n2 = (z2 - z1) * (x3 - x1) - (z3 - z1) * (x2 - x1);
                                    n3 = (x2 - x1) * (y3 - y1) - (x3 - x1) * (y2 - y1);

                                }
                                else // для остальных - 1,3,4
                                {

                                    n1 = (y4 - y3) * (z1 - z3) - (y1 - y3) * (z4 - z3);
                                    n2 = (z4 - z3) * (x1 - x3) - (z1 - z3) * (x4 - x3);
                                    n3 = (x4 - x3) * (y1 - y3) - (x1 - x3) * (y4 - y3);

                                }


                                // если не включен режим GL_NORMILIZE, то мы должны в обязательном порядке
                                // произвести нормализацию вектора нормали, перед тем как передать информацию о нормали
                                double n5 = (double)Math.Sqrt(n1 * n1 + n2 * n2 + n3 * n3);
                                n1 /= (n5 + 0.01);
                                n2 /= (n5 + 0.01);
                                n3 /= (n5 + 0.01);

                                // передаем информацию о нормали
                                Gl.glNormal3d(-n1, -n2, -n3);

                                // передаем 4 вершины для отрисовки полигона
                                Gl.glVertex3d(x1, y1, z1);
                                Gl.glVertex3d(x2, y2, z2);
                                Gl.glVertex3d(x3, y3, z3);
                                Gl.glVertex3d(x4, y4, z4);

                            }

                        }

                        // завершаем выбранный режим рисования полигонов
                        Gl.glEnd();
                        break;

                    }

            }

            // возвращаем сохраненную матрицу
            Gl.glPopMatrix();

            // завершаем рисование
            Gl.glFlush();

            // обновляем элемент AnT
            simpleOpenGlControl1.Invalidate();

        }

        private void simpleOpenGlControl_Paint(object sender, PaintEventArgs e)
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glLoadIdentity();
            Gl.glTranslatef(0.0f, 0.0f, -6.0f);
            Gl.glScalef(scale, scale, scale); // Увеличение/уменьшение пирамиды
            Gl.glRotatef(angleX, 1.0f, 0.0f, 0.0f);
            Gl.glRotatef(angleY, 0.0f, 1.0f, 0.0f);
          
            DrawRubiksPyramid();
          
            if (radioButton5.Checked)
            {
                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glLoadIdentity();
                  Gl.glOrtho(-2, 2, -2, 2, -10, 10); // Ортогональная проекция
            //    Gl.glOrtho(-0.99, 1, -0.99, 1, -1, 1);

                Gl.glMatrixMode(Gl.GL_MODELVIEW);
               // simpleOpenGlControl1.Refresh();
            }
            else if (radioButton4.Checked)
            {
                Glu.gluPerspective(45, (float)simpleOpenGlControl1.Width / (float)simpleOpenGlControl1.Height, 0.1, 100); // Перспективная проекция
            }
            else
            {
                Glu.gluPerspective(45, (float)simpleOpenGlControl1.Width / (float)simpleOpenGlControl1.Height, 0.1, 100);
            }

            if (checkBox4.Checked)
            {
                // Включаем фильтр тиснения
                Gl.glEnable(Gl.GL_TEXTURE_2D);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
                Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_COMBINE);
                Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_SRC0_RGB, Gl.GL_PREVIOUS);
                Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_COMBINE_RGB, Gl.GL_ADD);
                Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_SRC1_RGB, Gl.GL_TEXTURE);
                Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_OPERAND1_RGB, Gl.GL_SRC_COLOR);
            }
            else
            {
                Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_MODULATE);

                // Отключаем фильтр тиснения
                Gl.glDisable(Gl.GL_TEXTURE_2D);
            }
            //Gl.glColor3f(1.0f, 1.0f, 1.0f);
             Gl.glEnable(Gl.GL_TEXTURE_2D);
            // Включаем фильтр тиснения
            // Применяем смещение к объекту
            Gl.glTranslatef(-1f, -1f, -1F);
            DrawRoom(); // Рисуем комнату
            Gl.glDisable(Gl.GL_TEXTURE_2D);
            simpleOpenGlControl1.Invalidate();
         //   DrawAnimationBall();
            Draw();

        }

        private void DrawRubiksPyramid()
        {
           

            if (checkBox6.Checked)
            {
                global_time += 0.01f;
                //// выполняем просчет взрыва
                BOOOOM_1.Calculate(global_time);
            }
            else
            {
                global_time = 0;
                if (checkBox1.Checked == false)
                {
                    if (radioButton1.Checked)
                    {
                        float size = 1.5f;
                        float height = size * (float)Math.Sqrt(3) / 2f; // Высота правильного треугольника

                        float halfBase = size / 2f; // Половина основания

                        Gl.glPushMatrix();

                        //// Перемещаем пирамиду в указанный центр
                        //Gl.glTranslatef(center[0], center[1] - height / 2f, center[2]);

                        Gl.glBegin(Gl.GL_TRIANGLES);

                        // Front face (красный)
                        Gl.glColor3f(1.0f, 0.0f, 0.0f); // Красный
                        Gl.glVertex3f(0.0f, height / 2f, 0.0f);
                        Gl.glVertex3f(-halfBase, -height / 2f, halfBase);
                        Gl.glVertex3f(halfBase, -height / 2f, halfBase);

                        // Right face (зеленый)
                        Gl.glColor3f(0.0f, 1.0f, 0.0f); // Зеленый
                        Gl.glVertex3f(0.0f, height / 2f, 0.0f);
                        Gl.glVertex3f(halfBase, -height / 2f, halfBase);
                        Gl.glVertex3f(0.0f, -height / 2f, -halfBase);

                        // Left face (синий)
                        Gl.glColor3f(0.0f, 0.0f, 1.0f); // Синий
                        Gl.glVertex3f(0.0f, height / 2f, 0.0f);
                        Gl.glVertex3f(0.0f, -height / 2f, -halfBase);
                        Gl.glVertex3f(-halfBase, -height / 2f, halfBase);

                        // Bottom face (желтый)
                        Gl.glBegin(Gl.GL_TRIANGLES);
                        Gl.glColor3f(1.0f, 1.0f, 0.0f); // Желтый

                        Gl.glVertex3f(-halfBase, -height / 2f, halfBase);
                        Gl.glVertex3f(halfBase, -height / 2f, halfBase);
                        Gl.glVertex3f(0.0f, -height / 2f, -halfBase);

                        Gl.glEnd();
                        // Glut.glutWireTetrahedron();

                        Gl.glPopMatrix();

                    }
                    else
                    {
                        DrawAnimation();
                    }
                }
                else
                {
                    DrawSierpinskiPyramid(4);
                }
            }
            //    DrawSphere(10f); // Предполагая, что радиус сферы 10f
            if (checkBox2.Checked)
            {
                DrawAnimationBall();
            }
        }

        private void DrawAnimation()
        {
            if (radioButton3.Checked)
            {
                IncrIter();
            }
            float size = 1.5f;
            float height = size * (float)Math.Sqrt(3) / 2f; // Высота правильного треугольника

            float halfBase = size / 2f; // Половина основания
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glPushMatrix();

            //// Перемещаем пирамиду в указанный центр
            //Gl.glTranslatef(center[0], center[1] - height / 2f, center[2]);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, rubikId[IncrIter()]);
            Gl.glBegin(Gl.GL_TRIANGLES);

            // Front face (красный)
            //  Gl.glColor3f(1.0f, 1.0f, 0.0f); // Красный
            Gl.glColor3f(0.9f, 0.8f, 0.9f);
            Gl.glTexCoord3f(0.0f, height / 2f, 0.0f);
            Gl.glVertex3f(0.0f, height / 2f, 0.0f);
            // Gl.glEnd();

            //  Gl.glBindTexture(Gl.GL_TEXTURE_2D, rubikId[0]);
            // Gl.glBegin(Gl.GL_TRIANGLES);
            Gl.glTexCoord3f(-halfBase, -height / 2f, halfBase);
            //    Gl.glTexCoord3f(2.0f, 2.0f, 2.0f);
            // Gl.glColor3f(0.0f, 1.0f, 0.0f);

            Gl.glVertex3f(-halfBase, -height / 2f, halfBase);
            //  Gl.glEnd();

            // Gl.glBindTexture(Gl.GL_TEXTURE_2D, rubikId[0]);
            //   Gl.glBegin(Gl.GL_TRIANGLES);
            Gl.glTexCoord3f(halfBase, -height / 2f, halfBase);
            //Gl.glTexCoord3f(2.0f, 2.0f, 2.0f);
            //   Gl.glColor3f(0.0f, 0.0f, 1.0f);
            Gl.glVertex3f(halfBase, -height / 2f, halfBase);
            Gl.glEnd();
            //Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(-halfRoomSize, 0.0f, -halfRoomSize);
            //Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f(halfRoomSize, 0.0f, -halfRoomSize);
            //Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f(halfRoomSize, 0.0f, halfRoomSize);
            //Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(-halfRoomSize, 0.0f, halfRoomSize);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, rubikId[IncrIter()]); // привязываем вторую текстуру
            Gl.glBegin(Gl.GL_TRIANGLES);
            // Right face (зеленый)
          //  Gl.glColor3f(1.0f, 1.0f, 1.0f); // Зеленый
                                            // Gl.glBindTexture(Gl.GL_TEXTURE_2D, rubikId[1]);
            Gl.glTexCoord3f(0.0f, height / 2f, 0.0f);
            Gl.glVertex3f(0.0f, height / 2f, 0.0f);
            Gl.glTexCoord3f(halfBase, -height / 2f, halfBase);
            Gl.glVertex3f(halfBase, -height / 2f, halfBase);
            Gl.glTexCoord3f(0.0f, -height / 2f, -halfBase);
            Gl.glVertex3f(0.0f, -height / 2f, -halfBase);
            Gl.glEnd();

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, rubikId[IncrIter()]);
            Gl.glBegin(Gl.GL_TRIANGLES);
            // Left face (синий)
          //  Gl.glColor3f(1.0f, 1.0f, 1.0f); // Синий
            Gl.glTexCoord3f(0.0f, height / 2f, 0.0f);
            Gl.glVertex3f(0.0f, height / 2f, 0.0f);
            Gl.glTexCoord3f(0.0f, -height / 2f, -halfBase);
            Gl.glVertex3f(0.0f, -height / 2f, -halfBase);
            Gl.glTexCoord3f(-halfBase, -height / 2f, halfBase);
            Gl.glVertex3f(-halfBase, -height / 2f, halfBase);
            Gl.glEnd();
            // Bottom face (желтый)
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, rubikId[IncrIter()]);
            Gl.glBegin(Gl.GL_TRIANGLES);
         //   Gl.glColor3f(1.0f, 1.0f, 1.0f); // Желтый

            Gl.glTexCoord3f(-halfBase, -height / 2f, halfBase);
            Gl.glVertex3f(-halfBase, -height / 2f, halfBase);
            Gl.glTexCoord3f(halfBase, -height / 2f, halfBase);
            Gl.glVertex3f(halfBase, -height / 2f, halfBase);
            Gl.glTexCoord3f(0.0f, -height / 2f, -halfBase);
            Gl.glVertex3f(0.0f, -height / 2f, -halfBase);

            Gl.glEnd();
            // Glut.glutWireTetrahedron();

            Gl.glPopMatrix();
            Gl.glDisable(Gl.GL_TEXTURE_2D);
        }

        private int IncrIter()
        {
            if (IterAnim == 3)
            {
                IterAnim = 0;
            }
            else
            {
                IterAnim++;
            }
            return IterAnim;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    angleY -= 5.0f;
                    break;
                case Keys.D:
                    angleY += 5.0f;
                    break;
                case Keys.S:
                    angleX -= 5.0f;
                    break;
                case Keys.W:
                    angleX += 5.0f;
                    break;
                case Keys.Q:
                    scale -= 0.1f; // Уменьшить масштаб
                    break;
                case Keys.E:
                    scale += 0.1f; // Увеличить масштаб
                    break;
            }
            simpleOpenGlControl1.Refresh();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                // Очистим текущее содержимое OpenGL контрола
                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

                // Уменьшим глубину отображения, чтобы не уйти в бесконечность
                int depth = 4;

                // Активируем OpenGL контекст
                //  simpleOpenGlControl1.MakeCurrent();

                // Вызовем метод для отрисовки пирамиды Серпинского
                DrawSierpinskiPyramid(depth);

                // Обновим содержимое OpenGL контрола
                simpleOpenGlControl1.Refresh();
            }
        }



        private void DrawSierpinskiPyramid(int depth)
        {
            // Нарисуем пирамиду Серпинского с заданной глубиной
            DrawSierpinskiPyramidRecursively(depth, new float[] { 0f, 0f, 0f }, 1f);
        }

        private void DrawSierpinskiPyramidRecursively(int depth, float[] center, float size)
        {
            if (depth == 0)
            {
                DrawPyramid(center, size); // Нарисуем обычную пирамиду
            }
            else
            {
                // Разделим каждую грань на более мелкие пирамиды
                float newSize = size / 2f;

                float[] top = { center[0], center[1] + size, center[2] };
                float[] front = { center[0], center[1] + newSize, center[2] + newSize };
                float[] back = { center[0], center[1] + newSize, center[2] - newSize };
                float[] left = { center[0] - newSize, center[1] + newSize, center[2] };
                float[] right = { center[0] + newSize, center[1] + newSize, center[2] };

                DrawSierpinskiPyramidRecursively(depth - 1, top, newSize);
                DrawSierpinskiPyramidRecursively(depth - 1, front, newSize);
                DrawSierpinskiPyramidRecursively(depth - 1, back, newSize);
                DrawSierpinskiPyramidRecursively(depth - 1, left, newSize);
                DrawSierpinskiPyramidRecursively(depth - 1, right, newSize);
            }
        }

        private void DrawPyramid(float[] center, float size)
        {
            float halfSize = size / 2f;

            Gl.glPushMatrix();

            // Перемещаем пирамиду в указанный центр
            Gl.glTranslatef(center[0], center[1], center[2]);
            float height = size * (float)Math.Sqrt(3) / 2f; // Высота правильного треугольника

            float halfBase = size / 2f; // Половина основания
            Gl.glBegin(Gl.GL_TRIANGLES);

            // Front face (красный)
            Gl.glColor3f(1.0f, 0.0f, 0.0f); // Красный
            Gl.glVertex3f(0.0f, height / 2f, 0.0f);
            Gl.glVertex3f(-halfBase, -height / 2f, halfBase);
            Gl.glVertex3f(halfBase, -height / 2f, halfBase);

            // Right face (зеленый)
            Gl.glColor3f(0.0f, 1.0f, 0.0f); // Зеленый
            Gl.glVertex3f(0.0f, height / 2f, 0.0f);
            Gl.glVertex3f(halfBase, -height / 2f, halfBase);
            Gl.glVertex3f(0.0f, -height / 2f, -halfBase);

            // Left face (синий)
            Gl.glColor3f(0.0f, 0.0f, 1.0f); // Синий
            Gl.glVertex3f(0.0f, height / 2f, 0.0f);
            Gl.glVertex3f(0.0f, -height / 2f, -halfBase);
            Gl.glVertex3f(-halfBase, -height / 2f, halfBase);

            // Bottom face (желтый)
            Gl.glBegin(Gl.GL_TRIANGLES);
            Gl.glColor3f(1.0f, 1.0f, 0.0f); // Желтый

            Gl.glVertex3f(-halfBase, -height / 2f, halfBase);
            Gl.glVertex3f(halfBase, -height / 2f, halfBase);
            Gl.glVertex3f(0.0f, -height / 2f, -halfBase);

            Gl.glEnd();

            Gl.glPopMatrix();
        }



      

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                scalePer = scale;
                angleXPer = angleX;
                angleYPer = angleY;
                angleX = 335F;
                angleY = -345F;
                scale = 2F;
                //Gl.glMatrixMode(Gl.GL_PROJECTION);
                //Gl.glLoadIdentity();
                //Gl.glOrtho(-2, 2, -2, 2, -10, 10); // Ортогональная проекция
                //Gl.glMatrixMode(Gl.GL_MODELVIEW);
                simpleOpenGlControl1.Refresh();
            }
            else
            {
                angleX = angleXPer;
                angleY = angleYPer;
                scale = scalePer;
            }
        }

      

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                // Включаем фильтр тиснения
                Gl.glEnable(Gl.GL_TEXTURE_2D);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
                Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_COMBINE);
                Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_SRC0_RGB, Gl.GL_PREVIOUS);
                Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_COMBINE_RGB, Gl.GL_ADD);
                Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_SRC1_RGB, Gl.GL_TEXTURE);
                Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_OPERAND1_RGB, Gl.GL_SRC_COLOR);
            }
            else
            {
                // Отключаем фильтр тиснения
                Gl.glDisable(Gl.GL_TEXTURE_2D);
            }

            simpleOpenGlControl1.Refresh();
        }

        int[] rubikId = new int[4];
        // Метод для загрузки текстур из файлов изображений
        private void LoadTextures()
        {
            // Загрузка текстуры для стен
            Bitmap wallTexture = new Bitmap("wall_texture.jpg");
            wallTextureID = LoadTexture(wallTexture);

            // Загрузка текстуры для пола
            Bitmap floorTexture = new Bitmap("floor_texture.jpg");
            floorTextureID = LoadTexture(floorTexture);

            // Загрузка текстуры для потолка
            Bitmap ceilingTexture = new Bitmap("ceiling_texture.jpg");
            ceilingTextureID = LoadTexture(ceilingTexture);

            Bitmap rub1Texture = new Bitmap("blue.png");
            rubikId[0]= LoadTexture(rub1Texture);

            Bitmap rub2Texture = new Bitmap("red.png");
            rubikId[1] = LoadTexture(rub2Texture);

            Bitmap rub3Texture = new Bitmap("green.png");
            rubikId[2] = LoadTexture(rub3Texture);

            Bitmap rub4Texture = new Bitmap("yellow.png");
            rubikId[3] = LoadTexture(rub4Texture);


            // Загружаем первую текстуру
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, rubikId[0]);
            // Здесь загрузка первой текстуры rubikIds[0]

            // Загружаем вторую текстуру
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, rubikId[1]);
            // Здесь загрузка второй текстуры rubikIds[1]

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, rubikId[2]);

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, rubikId[3]);
            // Сбрасываем привязку текстуры
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
        }

        // Метод для загрузки текстуры и возврата ее идентификатора
        private int LoadTexture(Bitmap bitmap)
        {
            int textureID;
            Gl.glGenTextures(1, out textureID);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, textureID);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                               ImageLockMode.ReadOnly,
                                               System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, bitmap.Width, bitmap.Height, 0,
                            Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, data.Scan0);
            bitmap.UnlockBits(data);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            return textureID;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                Random rnd = new Random();
                // устанавливаем новые координаты взрыва
                BOOOOM_1.SetNewPosition(rnd.Next(1, 1), rnd.Next(1, 1), rnd.Next(1, 1));
                // случайную силу
                BOOOOM_1.SetNewPower(rnd.Next(20, 80));
                // и активируем сам взрыв
                BOOOOM_1.Boooom(global_time);
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                angleX = angleXPer;
                angleY = angleYPer;
                scale = scalePer;
                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glLoadIdentity();
                Glu.gluPerspective(45, (float)simpleOpenGlControl1.Width / (float)simpleOpenGlControl1.Height, 0.1, 100); // Перспективная проекция
                Gl.glMatrixMode(Gl.GL_MODELVIEW);
                simpleOpenGlControl1.Refresh();
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                scalePer = scale;
                angleXPer = angleX;
                angleYPer = angleY;
                angleX = 420F;
                angleY = 160F;
                scale = 1.8F;
                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glLoadIdentity();
                Gl.glOrtho(-2, 2, -2, 2, -10, 10); // Ортогональная проекция
                Gl.glMatrixMode(Gl.GL_MODELVIEW);
                simpleOpenGlControl1.Refresh();
            }
            else
            {
                angleX = angleXPer;
                angleY = angleYPer;
                scale = scalePer;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            angleX += 5.0f;
            simpleOpenGlControl1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            angleY -= 5.0f;
            simpleOpenGlControl1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            angleX -= 5.0f;
            simpleOpenGlControl1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            angleY += 5.0f;
            simpleOpenGlControl1.Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            scale -= 0.1f;
            simpleOpenGlControl1.Refresh();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            scale += 0.1F;
            simpleOpenGlControl1.Refresh();
        }

        private void выбратьФайлДляЗагрузкиToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
       "Программа, реализующая различные эффекты с пирамидкой Рубика и поверхность вращения на основе полинома Лагранжа. Управление " +
       "с клавиатуры: W-вверх, S-вниз, A-влево, D-вправо, Q-дальше, E-ближе.",
       "О программе",
       MessageBoxButtons.OK,
       MessageBoxIcon.Information,
       MessageBoxDefaultButton.Button1,
       MessageBoxOptions.DefaultDesktopOnly);
        }

        //private void button7_Click(object sender, EventArgs e)
        //{
        //    DrawAnimationBall();
        //}

        private float ballPositionX = 0.0f;
        private float ballPositionY = 0.0f;
        private float ballPositionZ = 0.0f;
        private float ballVelocityX = 0.01f; // начальная скорость шарика по оси X
        private float ballVelocityY = 0.01f; // начальная скорость шарика по оси Y
        private float ballVelocityZ = 0.01f; // начальная скорость шарика по оси Z
        private float ballRadius = 1f; // радиус шарика
        private Vector3 velocity = new Vector3(1, 1, 1);
        private bool flagBall=true;
        private void DrawAnimationBall()
        {
            if (flagBall)
            {
                // Обновляем позицию шарика
                ballPositionX += ballVelocityX;
                ballPositionY += ballVelocityY;
                ballPositionZ += ballVelocityZ;
            }
            else
            {
                ballPositionX -= ballVelocityX;
                ballPositionY -= ballVelocityY;
                ballPositionZ -= ballVelocityZ;
            }

            if (ballPositionX >= 10)
            {
                flagBall = false;
            }

            if (ballPositionX <= -10)
            {
                flagBall = true;
            }

            // Проверяем столкновение с пирамидой
            if (CheckCollisionWithPyramid())
            {
                // Меняем форму шарика (если необходимо)
                ChangeBallShape();
                Vector3[] pyramidNormals = new Vector3[]
{
    new Vector3(0, 1, 1),   // Нормаль для передней грани
    new Vector3(1, 1, 0),   // Нормаль для правой грани
    new Vector3(-1, 1, 0),  // Нормаль для левой грани
    new Vector3(0, 1, -1),  // Нормаль для задней грани
    // Для нижней грани нормаль будет направлена вверх, например: new Vector3(0, 1, 0)
};
                // Отбиваем шарик в случайном направлении
                ReflectBall(pyramidNormals[0]);
            }

            // Рисуем шарик
            DrawBall();
           
        }

        private void DrawSphere(float radius)
        {
            Glut.glutSolidSphere(radius, 20, 20);
        }

        private void DrawBall()
        {
          
            //Gl.glPushMatrix();
            //Gl.glMatrixMode(Gl.GL_MODELVIEW);
            //Gl.glLoadIdentity();
          //  Gl.glTranslatef(0, 0, -1);
            Gl.glColor3f(1.0f, 0.0f, 0.0f); // Красный
            //Gl.glEnable(Gl.GL_DEPTH_TEST);
            //Gl.glEnable(Gl.GL_CULL_FACE);
            //Gl.glEnable(Gl.GL_LIGHTING);
            //Gl.glEnable(Gl.GL_LIGHT0);

            Gl.glTranslatef(-ballPositionX/35, -ballPositionY/35, -ballPositionZ/35);
           // Glut.glutSolidSphere(1f, 20, 20);
            //Gl.glFinish();
            //Gl.glPopMatrix();
          //  simpleOpenGlControl1.Invalidate();
        }

      
      

        private bool CheckCollisionWithPyramid()
        {
            // Параметры пирамиды
            float pyramidSize = 1.5f; // Размер пирамиды
            float pyramidHeight = pyramidSize * (float)Math.Sqrt(3) / 2f; // Высота пирамиды
            float halfBase = pyramidSize / 2f; // Половина основания пирамиды

            // Координаты центра пирамиды
            float pyramidCenterX = 0.0f;
            float pyramidCenterY = 0.0f;
            float pyramidCenterZ = 0.0f;

            // Вычисляем ближайшую точку на поверхности пирамиды к центру шарика
            float closestPointX = Math.Max(-halfBase, Math.Min(ballPositionX, halfBase));
            float closestPointY = Math.Max(-pyramidHeight / 2f, Math.Min(ballPositionY, pyramidHeight / 2f));
            float closestPointZ = Math.Max(-halfBase, Math.Min(ballPositionZ, halfBase));

            // Вычисляем расстояние между центром шарика и ближайшей точкой на поверхности пирамиды
            float distanceX = ballPositionX - closestPointX;
            float distanceY = ballPositionY - closestPointY;
            float distanceZ = ballPositionZ - closestPointZ;
            float distanceSquared = distanceX * distanceX + distanceY * distanceY + distanceZ * distanceZ;

            // Проверяем, произошло ли столкновение
            return distanceSquared < ballRadius * ballRadius;
        }

        private void ChangeBallShape()
        {
            // Реализация изменения формы шарика после столкновения с пирамидой
            // Здесь вы можете использовать любой алгоритм для изменения формы шарика,
            // например, изменение его радиуса, цвета или текстуры.

            // В этом примере просто изменяем радиус шарика
            ballRadius *= 0.8f; // Уменьшаем радиус шарика на 20%
        }

        private void ReflectBall(Vector3 normal)
        {
            // Нормализуем вектор нормали для обеспечения корректной отраженной скорости
          //  normal.Normalize();

            // Рассчитываем отраженное направление скорости шарика
            Vector3 newVelocity = velocity - 2 * Vector3.Dot(velocity, normal) * normal;

            // Присваиваем новую скорость шарику
            velocity = newVelocity;
        }


        // Метод для отрисовки стен, пола и потолка с текстурами
        private void DrawRoom()
        {
            float roomSize = 10.0f; // Размер комнаты
            float halfRoomSize = roomSize / 2.0f; // Половина размера комнаты


            Gl.glColor3f(0.4f, 0.4f, 0f);
            // Рисуем пол
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, floorTextureID);
            Gl.glBegin(Gl.GL_QUADS);
          

            Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(-halfRoomSize, 0.0f, -halfRoomSize);
            Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f(halfRoomSize, 0.0f, -halfRoomSize);
            Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f(halfRoomSize, 0.0f, halfRoomSize);
            Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(-halfRoomSize, 0.0f, halfRoomSize);
            Gl.glEnd();

            // Рисуем стены
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, wallTextureID);
            Gl.glBegin(Gl.GL_QUADS);
            // Стена 1 (передняя)
            Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(-halfRoomSize, 0.0f, halfRoomSize);
            Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f(halfRoomSize, 0.0f, halfRoomSize);
            Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f(halfRoomSize, roomSize, halfRoomSize);
            Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(-halfRoomSize, roomSize, halfRoomSize);
            // Стена 2 (задняя)
            Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(-halfRoomSize, 0.0f, -halfRoomSize);
            Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f(halfRoomSize, 0.0f, -halfRoomSize);
            Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f(halfRoomSize, roomSize, -halfRoomSize);
            Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(-halfRoomSize, roomSize, -halfRoomSize);
            // Стена 3 (левая)
            Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(-halfRoomSize, 0.0f, -halfRoomSize);
            Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f(-halfRoomSize, 0.0f, halfRoomSize);
            Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f(-halfRoomSize, roomSize, halfRoomSize);
            Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(-halfRoomSize, roomSize, -halfRoomSize);
            // Стена 4 (правая)
            Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(halfRoomSize, 0.0f, -halfRoomSize);
            Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f(halfRoomSize, 0.0f, halfRoomSize);
            Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f(halfRoomSize, roomSize, halfRoomSize);
            Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(halfRoomSize, roomSize, -halfRoomSize);
            Gl.glEnd();

            // Рисуем потолок
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, ceilingTextureID);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(-halfRoomSize, roomSize, halfRoomSize);
            Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f(halfRoomSize, roomSize, halfRoomSize);
            Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f(halfRoomSize, roomSize, -halfRoomSize);
            Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(-halfRoomSize, roomSize, -halfRoomSize);
            Gl.glEnd();
        }
    }


    class Partilce
    {
        // позиция частицы
        private float[] position = new float[3];
        // размер
        private float _size;
        // время жизни
        private float _lifeTime;

        // вектор гравитации
        private float[] Grav = new float[3];
        // ускорение частицы
        private float[] power = new float[3];
        // коэфицент затухания силы
        private float attenuation;

        // набранная скорость
        private float[] speed = new float[3];

        // временной интервал активации частицы
        private float LastTime = 0;

        // конструктор класса
        public Partilce(float x, float y, float z, float size, float lifeTime, float start_time)
        {
            // записываем все начальные настройки частицы, устанавливаем начальный коэфицент затухания
            // и обнуляем скорость и силу, приложенную к частице
            _size = size;
            _lifeTime = lifeTime;

            position[0] = x;
            position[1] = y;
            position[1] = z;

            speed[0] = 0;
            speed[1] = 0;
            speed[2] = 0;

            Grav[0] = 0;
            Grav[1] = -9.8f;
            Grav[2] = 0;

            attenuation = 3.33f;

            power[0] = 0;
            power[0] = 0;
            power[0] = 0;

            LastTime = start_time;

        }

        // функция установка ускорения, действующего на частицу
        public void SetPower(float x, float y, float z)
        {
            power[0] = x;
            power[1] = y;
            power[2] = z;
        }

        // инвертирование скорости частицы по заданной оси с указанным затуханием
        // удобно использовать для простой демонстрации столкновений, например с землей
        public void InvertSpeed(int os, float attenuation)
        {
            speed[os] *= -1 * attenuation;
        }

        // получение размера частицы
        public float GetSize()
        {
            return _size;
        }

        // установка нового значения затухания
        public void setAttenuation(float new_value)
        {
            attenuation = new_value;
        }

        // обновление позиции частицы
        public void UpdatePosition(float timeNow)
        {
            // орпределяем разницу во времени, прошедшую с последнего обновления
            // позиции частицы (ведь таймер может быть не фиксированный)
            float dTime = timeNow - LastTime;
            _lifeTime -= dTime;

            // обновляем последнюю отметку временного интервала
            LastTime = timeNow;

            // перерасчитываем ускорение, движущее частицу, с учетом затухания
            for (int a = 0; a < 3; a++)
            {
                if (power[a] > 0)
                {
                    power[a] -= attenuation * dTime;

                    if (power[a] <= 0)
                        power[a] = 0;
                }

                // перерасчитываем позицию частицы с учетом гравитации, вектора ускорения и прощедшего промежутка времени
                position[a] += (speed[a] * dTime + (Grav[a] + power[a]) * dTime * dTime);

                // обновляем скорость частицы
                speed[a] += (Grav[a] + power[a]) * dTime;
            }
        }

        // проверка, не закончилось ли время жизни частицы
        public bool isLife()
        {
            if (_lifeTime > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // получение координат частицы
        public float GetPositionX()
        {
            return position[0];
        }
        public float GetPositionY()
        {
            return position[1];
        }
        public float GetPositionZ()
        {
            return position[2];
        }

    }

    class Explosion
    {
        // позиция взрыва
        private float[] position = new float[3];
        // мощность
        private float _power;
        // максимальное количество частиц
        private int MAX_PARTICLES = 1000;
        // текущее установленное количество частиц
        private int _particles_now;

        // активирован
        private bool isStart = false;

        // массив частиц на основе созданного ранее класса
        private Partilce[] PartilceArray;

        // дисплейный список для рисования частицы создан
        private bool isDisplayList = false;
        // номер дисплейного списка для отрисовки
        private int DisplayListNom = 0;

        private bool isChange = false;
        // конструктор класса; в него передаются координаты, где должен произойти взрыв, мощность и количество чатиц
        public Explosion(float x, float y, float z, float power, int particle_count)
        {
            position[0] = x;
            position[1] = y;
            position[2] = z;

            _particles_now = particle_count;
            _power = power;

            // если число частиц превышает максимально разрешенное
            if (particle_count > MAX_PARTICLES)
            {
                particle_count = MAX_PARTICLES;
            }

            // создаем массив частиц необходимого размера
            PartilceArray = new Partilce[particle_count];
        }

        // функция обновления позиции взрыва
        public void SetNewPosition(float x, float y, float z)
        {
            position[0] = x;
            position[1] = y;
            position[2] = z;
        }

        // установка нового значения мощности взрыва
        public void SetNewPower(float new_power)
        {
            _power = new_power;
        }

        // создания дисплейного списка для отрисовки частицы (т.к. отрисовывать даже небольшой полигон такое количество раз очень накладно)
        private void CreateDisplayList(bool change) //добавления переменной change для разворота частицы в пространстве
        {
            // генерация дисплейного списка
            DisplayListNom = Gl.glGenLists(1);

            // начало создания списка
            Gl.glNewList(DisplayListNom, Gl.GL_COMPILE);

            // режим отрисовки треугольника
            Gl.glBegin(Gl.GL_TRIANGLES);

            // задаем форму частицы
            Gl.glVertex3d(0, 0, 0);
            Gl.glVertex3d(0.02f, 0.02f, 0);
            if (!change)
            {
                Gl.glVertex3d(0.02f, 0, -0.02f);
            }
            else
            {
                Gl.glVertex3d(-0.02f, 0, 0.02f);
            }

            Gl.glEnd();

            // завершаем отрисовку частицы
            Gl.glEndList();

            // флаг - дисплейный список создан
            isDisplayList = true;
        }

        // функция, реализующая взрыв
        public void Boooom(float time_start)
        {
            // инициализируем экземпляр класса Random
            Random rnd = new Random();

            // если дисплейный список не создан, надо его создать
            if (!isDisplayList)
            {
                CreateDisplayList(isChange);
            }

            // по всем частицам
            for (int ax = 0; ax < _particles_now; ax++)
            {
                // создаем частицу
                PartilceArray[ax] = new Partilce(position[0], position[1], position[2], 5.0f, 10, time_start);

                // случайным образом генериуем ориентацию вектора ускорения для данной частицы
                int direction_x = rnd.Next(1, 3);
                int direction_y = rnd.Next(1, 3);
                int direction_z = rnd.Next(1, 3);

                // если сгенерированно число 2 - то мы заменяем его на -1.
                if (direction_x == 2)
                    direction_x = -1;


                if (direction_y == 2)
                    direction_y = -1;

                if (direction_z == 2)
                    direction_z = -1;

                // задаем мощность в промежутке от 5 до 100% от указанной (чтобы частицы имели разное ускорение)
                float _power_rnd = rnd.Next((int)_power / 20, (int)_power);
                // устанавливаем затухание, равное 50% от мощности
                PartilceArray[ax].setAttenuation(_power / 2.0f);
                // устанавливаем ускорение частицы, еще раз генерируя случайное число
                // таким образом мощность определится от 10 - до 100% полученной
                // Здесь же применяем ориентацию для векторов ускорения
                PartilceArray[ax].SetPower(_power_rnd * ((float)rnd.Next(100, 1000) / 1000.0f) * direction_x, _power_rnd * ((float)rnd.Next(100, 1000) / 1000.0f) * direction_y, _power_rnd * ((float)rnd.Next(100, 1000) / 1000.0f) * direction_z);
            }

            // взрыв активирован
            isStart = true;
        }

        // калькуляция текущего взрыва
        public void Calculate(float time)
        {
            // только в том случае, если взрыв уже активирован
            if (isStart)
            {
                CreateDisplayList(isChange);
                // проходим циклом по всем частицам
                for (int ax = 0; ax < _particles_now; ax++)
                {
                    // если время жизни частицы еще не вышло
                    if (PartilceArray[ax].isLife())
                    {
                        // обновляем позицию частицы
                        PartilceArray[ax].UpdatePosition(time);

                        // сохраняем текущую матрицу
                        Gl.glPushMatrix();
                        // получаем размер частицы
                        float size = PartilceArray[ax].GetSize();

                        // выполняем перемещение частицы в необходимую позицию
                        Gl.glTranslated(PartilceArray[ax].GetPositionX(), PartilceArray[ax].GetPositionY(), PartilceArray[ax].GetPositionZ());
                        // масштабируем ее в соотвествии с ее размером
                        Gl.glScalef(size, size, size);

                        // вызываем дисплейный список для отрисовки частицы из кеша видеоадаптера
                        Gl.glCallList(DisplayListNom);

                        //   Gl.glRotatef(90, 1, 0, 0);
                        //  Gl.glTranslatef(-5, -10, 5);
                        // Gl.glRotated(30, 1, 0, 0);

                        // возвращаем матрицу
                        Gl.glPopMatrix();

                        // отражение от "земли"
                        // если координата Y стала меньше нуля (удар о землю)
                        if (PartilceArray[ax].GetPositionY() < 0)
                        {
                            // инвертируем проекцию скорости на ось Y, как будто частица ударилась и отскочила от земли
                            // причем скорость затухает на 40%
                            PartilceArray[ax].InvertSpeed(1, 0.6f);
                        }

                        //  Gl.glRotated(30, 0, 1, 0);
                    }

                }
                isChange = !isChange;
            }
        }
    }
}
