using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Mdl = CourseProject.Model.Model;
using CmdP = CourseProject.Controller.CommandClasses.PasteGraph_C;


namespace CourseProject.Controller
{
    public class ClipRectangle
    {
        private Rectangle Rect;
        
        public ClipRectangle()
        {
            Rect = new Rectangle(0, 0, 230, 460);
        }

        public CmdP CheckVertex(CmdP cmd)
        {
            List<Mdl.Vertex> v = new List<Mdl.Vertex>(cmd.V);
            v.Reverse();
            
            List<Mdl.Edge> e = new List<Mdl.Edge>(cmd.E);
            e.Reverse();

            List<Mdl.Edge> edg = new List<Mdl.Edge>();
            
            foreach (var x in cmd.E)
            {
                edg.Add(new Mdl.Edge(x.V1, x.V2));
            }

            List<int> delete = new List<int>();
            
            
            
            foreach (var a in v)
            {
                if (Rect.Contains(a.X, a.Y) == false)
                {
                    int i = cmd.V.IndexOf(a);
                    cmd.V.Remove(a);

                    foreach (var b in e)
                    {
                        if (b.V1 == i || b.V2 == i)
                        {
                            delete.Add(cmd.E.IndexOf(b));
                        }
                        else
                        {
                            if (b.V1 > i)
                            {
                                edg[cmd.E.IndexOf(b)].V1--;
                            }

                            if (b.V2 > i)
                            {
                                edg[cmd.E.IndexOf(b)].V2--;
                            }
                        }
                    }
                }
            }


            delete = delete.Distinct().ToList();
            delete.Sort();
            
            for (int i = delete.Count - 1; i > -1; i--)
            {
                edg.RemoveAt(delete[i]);
            }
            
            CmdP newcmd = new CmdP(cmd, edg);
            
            return newcmd;
        }
    }
}