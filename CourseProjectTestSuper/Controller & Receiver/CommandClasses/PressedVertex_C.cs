using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Mdl = CourseProject.Model.Model;

namespace CourseProject.Controller.CommandClasses
{
    public class PressedVertex_C : Command
    {
        public Mdl.DrawGraph G;
        public List<Mdl.Vertex> V;
        public List<Mdl.Edge> E;
        public int[,] AMatrix; //смежности

        public int Selected1; //выбранные вершины
        public int Selected2;

        public PressedVertex_C(Mdl model)
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
        
        public Bitmap PVertex_C(MouseEventArgs e)
        {
            V.Add(new Mdl.Vertex(e.X, e.Y));
            G.DrawVertex(e.X, e.Y, V.Count.ToString());
            AMatrix = new int[V.Count, V.Count];
            G.FillAdjacencyMatrix(V.Count, E, AMatrix);
            return G.GetBitmap();
        }

        public override void Execute(PictureBox sheet, MouseEventArgs e)
        {
            sheet.Image = PVertex_C(e);
        }

        public override void Execute(PictureBox sheet)
        {
            sheet.Image = ShowMe();
        }

        public override void Undo(PictureBox sheet)
        {
            sheet.Image = ShowMe();
        }
        
    }
}