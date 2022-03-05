using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Mdl = CourseProject.Model.Model;

namespace CourseProject.Controller.CommandClasses
{
    public class PressedEdge_C : Command
    {
        public Mdl.DrawGraph G;
        public List<Mdl.Vertex> V;
        public List<Mdl.Edge> E;

        public int Selected1; //выбранные вершины
        public int Selected2;

        public PressedEdge_C(Mdl model)
        {
            V = new List<Mdl.Vertex>(model.V);
            E = new List<Mdl.Edge>(model.E);
            G = model.G;
            Selected1 = model.Selected1;
            Selected2 = model.Selected2;
        }

        public Bitmap ShowMe()
        {
            G.ClearSheet();
            G.DrawAllGraph(V, E);
            return G.GetBitmap();
        }

        public  Bitmap PEdge_C(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                for (int i = 0; i < V.Count; i++)
                {
                    if (Math.Pow(V[i].X - e.X, 2) + Math.Pow(V[i].Y - e.Y, 2) <= G.R * G.R)
                    {
                        if (Selected1 == -1)
                        {
                            G.DrawSelectedVertex(V[i].X, V[i].Y);
                            Selected1 = i;
                            return G.GetBitmap();
                        }

                        if (Selected2 == -1)
                        {
                            G.DrawSelectedVertex(V[i].X, V[i].Y);
                            Selected2 = i;
                            E.Add(new Mdl.Edge(Selected1, Selected2));
                            G.DrawEdge(V[Selected1], V[Selected2], E[E.Count - 1], E.Count - 1);
                            Selected1 = -1;
                            Selected2 = -1;
                            return G.GetBitmap();
                        }
                    }
                }
            }

            if (e.Button == MouseButtons.Right)
            {
                if ((Selected1 != -1) &&
                    (Math.Pow((V[Selected1].X - e.X), 2) + Math.Pow((V[Selected1].Y - e.Y), 2) <= G.R * G.R))
                {
                    G.DrawVertex(V[Selected1].X, V[Selected1].Y, (Selected1 + 1).ToString());
                    Selected1 = -1;
                    return G.GetBitmap();
                }
            }

            return G.GetBitmap();
        }

        public override void Execute(PictureBox sheet, MouseEventArgs e)
        {
            sheet.Image = PEdge_C(e);
        }

        public override void Undo(PictureBox sheet)
        {
            sheet.Image = ShowMe();
        }

        public override void Execute(PictureBox sheet)
        {
            sheet.Image = ShowMe();
        }
    }
}