using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Mdl = CourseProject.Model.Model;

namespace CourseProject.Controller.CommandClasses
{
    public class PasteGraph_C : Command
    {
        public Mdl.DrawGraph G;
        public List<Mdl.Vertex> V;
        public List<Mdl.Edge> E;

        public int Selected1; //выбранные вершины
        public int Selected2;

        public PasteGraph_C(PasteGraph_C cmd, List<Mdl.Edge> Er)
        {
            V = new List<Mdl.Vertex>(cmd.V);
            E = new List<Mdl.Edge>(Er);
            G = cmd.G;
            Selected1 = cmd.Selected1;
            Selected2 = cmd.Selected2;
        }
        
        public PasteGraph_C(Mdl model)
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
        
        public override void Execute(PictureBox sheet, MouseEventArgs e)
        {
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
