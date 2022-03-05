using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Cntr = CourseProject.Controller.Controller;
using Cmd = CourseProject.Controller.CommandClasses.Command;
using CmdE = CourseProject.Controller.CommandClasses.PressedEdge_C;
using CmdV = CourseProject.Controller.CommandClasses.PressedVertex_C;
using CmdP = CourseProject.Controller.CommandClasses.PasteGraph_C;

namespace CourseProject.Model
{
    public class Model
    {
        public Cntr Controller = null;
        public DrawGraph G;
        public List<Vertex> V;
        public List<Edge> E;
        public int[,] AMatrix; //смежности
        public int[,] Matrix; //инцидентности

        public int Selected1; //выбранные вершины
        public int Selected2;


        public Model(PictureBox sheet)
        {
            V = new List<Vertex>();
            G = new DrawGraph(sheet.Width, sheet.Height);
            E = new List<Edge>();
            sheet.Image = G.GetBitmap();
        }

        public Model(Model other)
        {
            V = new List<Vertex>(other.V);
            E = new List<Edge>(other.E);
            G = other.G;
            Selected1 = other.Selected1;
            Selected2 = other.Selected2;
        }
        public Model(CmdE other)
        {
            V = new List<Vertex>(other.V);
            E = new List<Edge>(other.E);
            G = other.G;
            Selected1 = other.Selected1;
            Selected2 = other.Selected2;
        }

        public Model(CmdV other)
        {
            V = new List<Vertex>(other.V);
            E = new List<Edge>(other.E);
            G = other.G;
            Selected1 = other.Selected1;
            Selected2 = other.Selected2;
        }
        
        public Model(CmdP other)
        {
            V = new List<Vertex>(other.V);
            E = new List<Edge>(other.E);
            G = other.G;
            Selected1 = other.Selected1;
            Selected2 = other.Selected2;
        }

        public class Vertex
        {
            public int X, Y;

            public Vertex(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        public class Edge
        {
            public int V1, V2;

            public Edge(int v1, int v2)
            {
                V1 = v1;
                V2 = v2;
            }
        }

        public Bitmap DrawEdge_M()
        {
            G.ClearSheet();
            G.DrawAllGraph(V, E);
            Selected1 = -1;
            Selected2 = -1;
            return G.GetBitmap();
        }

        public Bitmap DrawVertex_M()
        {
            G.ClearSheet();
            G.DrawAllGraph(V, E);
            return G.GetBitmap();
        }

        public Bitmap SelectVert_M()
        {
            G.ClearSheet();
            G.DrawAllGraph(V, E);
            Selected1 = -1;
            return G.GetBitmap();

        }

        public bool check(MouseEventArgs e)
        {
            for (int i = 0; i < V.Count; i++)
            {
                if (Math.Pow(V[i].X - e.X, 2) + Math.Pow(V[i].Y - e.Y, 2) <= G.R * G.R)
                    return true;
            }

            return false;
        }
        
        public Bitmap PressedSelect_M(ListBox listBoxMatrix, MouseEventArgs e)
        {
            for (int i = 0; i < V.Count; i++)
            {
                if (Math.Pow(V[i].X - e.X, 2) + Math.Pow(V[i].Y - e.Y, 2) <= G.R * G.R)
                {
                    if (Selected1 != -1)
                    {
                        Selected1 = -1;
                        G.ClearSheet();
                        G.DrawAllGraph(V, E);
                        return G.GetBitmap();
                    }

                    if (Selected1 == -1)
                    {
                        G.DrawSelectedVertex(V[i].X, V[i].Y);
                        Selected1 = i;
                        CreateAdjAndOutM(listBoxMatrix);
                        listBoxMatrix.Items.Clear();
                        int degree = 0;
                        for (int j = 0; j < V.Count; j++)
                            degree += AMatrix[Selected1, j];
                        listBoxMatrix.Items.Add("Степень вершины №" + (Selected1 + 1) + " равна " + degree);
                        return G.GetBitmap();
                    }
                }
            }

            return G.GetBitmap();
        }

        public void CreateAdjAndOutM(ListBox listBoxMatrix)
        {
            AMatrix = new int[V.Count, V.Count];
            G.FillAdjacencyMatrix(V.Count, E, AMatrix);
            listBoxMatrix.Items.Clear();
            string sOut = "   ";
            for (int i = 0; i < V.Count; i++)
                sOut += (i + 1) + " ";
            listBoxMatrix.Items.Add(sOut);
            for (int i = 0; i < V.Count; i++)
            {
                sOut = (i + 1) + "  ";
                for (int j = 0; j < V.Count; j++)
                    sOut += AMatrix[i, j] + " ";
                listBoxMatrix.Items.Add(sOut);
            }
        }

        public void CreateIncAndOutM(ListBox listBoxMatrix)
        {
            if (E.Count > 0)
            {
                Matrix = new int[V.Count, E.Count];
                G.FillIncidenceMatrix(V.Count, E, Matrix);
                listBoxMatrix.Items.Clear();
                string sOut = "    ";
                for (int i = 0; i < E.Count; i++)
                    sOut += (char) ('a' + i) + " ";
                listBoxMatrix.Items.Add(sOut);
                for (int i = 0; i < V.Count; i++)
                {
                    sOut = (i + 1) + "   ";
                    for (int j = 0; j < E.Count; j++)
                        sOut += Matrix[i, j] + " ";
                    listBoxMatrix.Items.Add(sOut);
                }
            }
            else
                listBoxMatrix.Items.Clear();
        }

        public class DrawGraph
        {
            Bitmap bitmap;
            Pen blackPen;
            Pen brownPen;
            Pen mediumVioletRedPen;
            Graphics gr;
            Font fo;
            Brush br;
            PointF point;
            public int R = 20; //радиус окружности вершины

            public DrawGraph(int width, int height)
            {
                bitmap = new Bitmap(width, height);
                gr = Graphics.FromImage(bitmap);
                ClearSheet();
                blackPen = new Pen(Color.Black);
                blackPen.Width = 3;
                brownPen = new Pen(Color.Brown);
                brownPen.Width = 3;
                mediumVioletRedPen = new Pen(Color.MediumVioletRed);
                mediumVioletRedPen.Width = 3;
                fo = new Font("Consolas", 10);
                br = Brushes.Black;
            }

            public Bitmap GetBitmap()
            {
                return bitmap;
            }

            public void ClearSheet()
            {
                gr.Clear(Color.White);
            }

            public void DrawVertex(int x, int y, string number)
            {
                gr.FillEllipse(Brushes.White, (x - R), (y - R), 2 * R, 2 * R);
                gr.DrawEllipse(blackPen, (x - R), (y - R), 2 * R, 2 * R);
                point = new PointF(x - 5, y - 7);
                gr.DrawString(number, fo, br, point);
            }

            public void DrawSelectedVertex(int x, int y)
            {
                gr.DrawEllipse(brownPen, (x - R), (y - R), 2 * R, 2 * R);
            }

            public void DrawEdge(Vertex v1, Vertex v2, Edge e, int numberE)
            {
                if (e.V1 == e.V2)
                {
                    gr.DrawArc(mediumVioletRedPen, (v1.X - 2 * R), (v1.Y - 2 * R), 2 * R, 2 * R, 90, 270);
                    point = new PointF(v1.X - (int) (2.75 * R), v1.Y - (int) (2.75 * R));
                    gr.DrawString(((char) ('a' + numberE)).ToString(), fo, br, point);
                    DrawVertex(v1.X, v1.Y, (e.V1 + 1).ToString());
                }
                else
                {
                    gr.DrawLine(mediumVioletRedPen, v1.X, v1.Y, v2.X, v2.Y);
                    point = new PointF((v1.X + v2.X) / 2, (v2.Y + v1.Y) / 2);
                    gr.DrawString(((char) ('a' + numberE)).ToString(), fo, br, point);
                    DrawVertex(v1.X, v1.Y, (e.V1 + 1).ToString());
                    DrawVertex(v2.X, v2.Y, (e.V2 + 1).ToString());
                }
            }

            public void DrawAllGraph(List<Vertex> v, List<Edge> e)
            {
                //рисуем ребра
                for (int i = 0; i < e.Count; i++)
                {
                    if (e[i].V1 == e[i].V2)
                    {
                        gr.DrawArc(mediumVioletRedPen, (v[e[i].V1].X - 2 * R), (v[e[i].V1].Y - 2 * R), 2 * R, 2 * R,
                            90, 270);
                        point = new PointF(v[e[i].V1].X - (int) (2.75 * R), v[e[i].V1].Y - (int) (2.75 * R));
                        gr.DrawString(((char) ('a' + i)).ToString(), fo, br, point);
                    }
                    else
                    {
                        gr.DrawLine(mediumVioletRedPen, v[e[i].V1].X, v[e[i].V1].Y, v[e[i].V2].X, v[e[i].V2].Y);
                        point = new PointF((v[e[i].V1].X + v[e[i].V2].X) / 2, (v[e[i].V1].Y + v[e[i].V2].Y) / 2);
                        gr.DrawString(((char) ('a' + i)).ToString(), fo, br, point);
                    }
                }

                //рисуем вершины
                for (int i = 0; i < v.Count; i++)
                {
                    DrawVertex(v[i].X, v[i].Y, (i + 1).ToString());
                }
            }

            //заполняет матрицу смежности
            public void FillAdjacencyMatrix(int numberV, List<Edge> e, int[,] matrix)
            {
                for (int i = 0; i < numberV; i++)
                for (int j = 0; j < numberV; j++)
                    matrix[i, j] = 0;
                for (int i = 0; i < e.Count; i++)
                {
                    matrix[e[i].V1, e[i].V2] = 1;
                    matrix[e[i].V2, e[i].V1] = 1;
                }
            }

            //заполняет матрицу инцидентности
            public void FillIncidenceMatrix(int numberV, List<Edge> e, int[,] matrix)
            {
                for (int i = 0; i < numberV; i++)
                for (int j = 0; j < e.Count; j++)
                    matrix[i, j] = 0;
                for (int i = 0; i < e.Count; i++)
                {
                    matrix[e[i].V1, i] = 1;
                    matrix[e[i].V2, i] = 1;
                }
            }

        }
    }
}