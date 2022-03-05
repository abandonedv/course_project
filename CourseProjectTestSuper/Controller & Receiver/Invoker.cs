using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CourseProject.Controller.CommandClasses;
using Cntr = CourseProject.Controller.Controller;
using Mdl = CourseProject.Model.Model;
using Cmd = CourseProject.Controller.CommandClasses.Command;
using CmdE = CourseProject.Controller.CommandClasses.PressedEdge_C;
using CmdV = CourseProject.Controller.CommandClasses.PressedVertex_C;
using CmdP = CourseProject.Controller.CommandClasses.PasteGraph_C;

namespace CourseProject.Controller
{
    public class Invoker
    {
        public List<Cmd> m_HistoryList = new List<Cmd>();
        public int m_NextUndo = -1;
        private Cntr cntr;


        public Invoker(Cntr cntr_r)
        {
            cntr = cntr_r;
        }
        
        public void ExecuteCommand(Cmd command, PictureBox sheet, MouseEventArgs e)
        {
            if (cntr.flag == 1)
            {
                command.Execute(sheet, e);
                if (cntr.model.check(e))
                {
                    m_HistoryList.Add(command);
                    cntr.flag = 0;
                    m_NextUndo = m_HistoryList.Count - 1;;
                }
            }
            
            else
            {
                if (command.GetType() == typeof(CmdE))
                {
                    cntr.flag = 1;
                    command.Execute(sheet, e);
                }
            }

            if (command.GetType() == typeof(PressedVertex_C))
            {
                command.Execute(sheet, e);
                m_HistoryList.Add(command);
                m_NextUndo = m_HistoryList.Count - 1;
            }

            if (command.GetType() == typeof(PressedVertex_C))
                cntr.model = new Mdl((PressedVertex_C)cntr.cmd);
            if (command.GetType() == typeof(PressedEdge_C))
                cntr.model = new Mdl((PressedEdge_C)cntr.cmd);
        }
        
        public void ExecuteCommand(Cmd command, PictureBox sheet)
        {
            
            if (command.GetType() == typeof(PasteGraph_C))
            {
                command.Execute(sheet);
                m_HistoryList.Add(command);
                m_NextUndo++;
            }

            if (command.GetType() == typeof(PasteGraph_C))
                cntr.model = new Mdl((PasteGraph_C)cntr.cmd);
        }

        public void Undo(PictureBox sheet, Button b1, ToolStripMenuItem b2, Button b3, Button b4, ToolStripMenuItem b5, Button b6, Button b7)
        {
            if (m_NextUndo <= 0) return;

            if (m_HistoryList[m_HistoryList.Count - 1].GetType() == typeof(PasteGraph_C))
            {
                if (m_HistoryList.Count != m_NextUndo)
                {
                    b1.Enabled = false;
                    b2.Enabled = false;
                    b3.Enabled = false;
                    b4.Enabled = false;
                    b5.Enabled = false;
                    b6.Enabled = false;
                    b7.Enabled = false;
                }
                else
                {
                    b1.Enabled = true;
                    b2.Enabled = true;
                    b3.Enabled = true;
                    b4.Enabled = true;
                    b5.Enabled = true;
                    b6.Enabled = true;
                    b7.Enabled = true;
                }
            }

            if (cntr.copy != null && m_HistoryList.Count - 2 != m_NextUndo)
            {
                b6.Enabled = false;
                b7.Enabled = false;
            }

            Cmd command = m_HistoryList[m_NextUndo - 1];
            
            command.Undo(sheet);
            
            m_NextUndo--;
        }

        public void Redo(PictureBox sheet,  Button b1, ToolStripMenuItem b2, Button b3, Button b4, ToolStripMenuItem b5, Button b6, Button b7)
        {
            if (m_NextUndo == m_HistoryList.Count - 1) return;
            
            if (m_HistoryList[m_HistoryList.Count - 1].GetType() == typeof(PasteGraph_C))
            {
                if (m_HistoryList.Count - 2 != m_NextUndo )
                {
                    b1.Enabled = false;
                    b2.Enabled = false;
                    b3.Enabled = false;
                    b4.Enabled = false;
                    b5.Enabled = false;
                    b6.Enabled = false;
                    b7.Enabled = false;
                }
                else
                {
                    b1.Enabled = true;
                    b2.Enabled = true;
                    b3.Enabled = true;
                    b4.Enabled = true;
                    b5.Enabled = true;
                    b6.Enabled = true;
                    b7.Enabled = true;
                }
            }

            int itemToRedo = m_NextUndo + 1;
            Cmd command = m_HistoryList[itemToRedo];
            
            command.Execute(sheet);
            
            m_NextUndo++;
        }

        public void TrimHistoryListAll()
        {
            for (int i =  m_HistoryList.Count - 2; i > -1; i--)
            {
                m_HistoryList.RemoveAt(i);
            }

            m_NextUndo = 0;
        }

        public Mdl DrawEdge_I(Mdl model)
        {
            try
            {
                if (m_NextUndo != m_HistoryList.Count - 1)
                {
                    if (m_HistoryList[m_NextUndo].GetType() == typeof(PressedEdge_C))
                    {
                        return new Mdl((PressedEdge_C) m_HistoryList[m_NextUndo]);
                    }
                    else if (m_HistoryList[m_NextUndo].GetType() == typeof(PressedVertex_C))
                    {
                        return new Mdl((PressedVertex_C) m_HistoryList[m_NextUndo]);
                    }
                }

                if (m_HistoryList.Count != 0)
                {
                    if (m_HistoryList[m_NextUndo].GetType() == typeof(PasteGraph_C))
                    {
                        TrimHistoryListAll();
                        return new Mdl((PasteGraph_C) m_HistoryList[m_NextUndo]);
                    }
                }

                return model;
            }
            catch (Exception e)
            {
                Application.Restart();
                return null;
            }
        }

        public Mdl DrawVertex_I(Mdl model)
        {
            if (m_NextUndo != m_HistoryList.Count - 1)
            {
                if (m_HistoryList[m_NextUndo].GetType() == typeof(PressedEdge_C))
                {
                    return new Mdl((PressedEdge_C) m_HistoryList[m_NextUndo]);
                }
                else if (m_HistoryList[m_NextUndo].GetType() == typeof(PressedVertex_C))
                {
                    return new Mdl((PressedVertex_C) m_HistoryList[m_NextUndo]);
                }
            }

            if (m_HistoryList.Count != 0)
            {
                if (m_HistoryList[m_NextUndo].GetType() == typeof(PasteGraph_C))
                {
                    TrimHistoryListAll();
                    return new Mdl((PasteGraph_C) m_HistoryList[m_NextUndo]);
                }
            }
            return model;
        }
    }
}