using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using CourseProject.Controller.CommandClasses;
using Mdl = CourseProject.Model.Model;
using Cmd = CourseProject.Controller.CommandClasses.Command;
using CL = CourseProject.Controller.ClipRectangle;

namespace CourseProject.Controller
{
    public class Controller
    {
        public Mdl model;
        public Cmd cmd;
        public int flag = 0;
        public Cmd copy = null;
        private CL ClipRect = new CL();
        public Invoker inv;
        public Controller(Mdl m)
        {
            model = m;
            inv = new Invoker(this);
        }

        public void SelectVert_C(PictureBox sheet)
        {
            sheet.Image = model.SelectVert_M();
        }

        public void CopyGraph()
        {
            copy = ClipRect.CheckVertex(new PasteGraph_C(model));
        }
        
        public void PasteGraph_C(PictureBox sheet)
        {
            if (copy != null)
            {
                cmd = copy;
                copy = null;
                inv.ExecuteCommand(cmd, sheet);
            }
        }

        public void DrawEdge_C(PictureBox sheet)
        {
            model = inv.DrawEdge_I(model);
            sheet.Image = model.DrawEdge_M();
        }

        public void DrawVertex_C(PictureBox sheet)
        {
            model = inv.DrawVertex_I(model);
            sheet.Image = model.DrawVertex_M();
        }

        //создание матрицы смежности и вывод в листбокс
        public void CreateAdjAndOutC(ListBox listBoxMatrix)
        {
            model.CreateAdjAndOutM(listBoxMatrix);
        }
        
        //создание матрицы инцидентности и вывод в листбокс
        public void CreateIncAndOutC(ListBox listBoxMatrix)
        {
            model.CreateIncAndOutM(listBoxMatrix);
        }

        public void PressedSelect_C(ListBox listBoxMatrix, PictureBox sheet, MouseEventArgs e)
        { 
            sheet.Image = model.PressedSelect_M(listBoxMatrix, e);
        }

        public void PressedVertex_C(PictureBox sheet, MouseEventArgs e, Button b1, Button b2)
        {
            b1.Enabled = true;
            b2.Enabled = true;
            cmd = new PressedVertex_C(model);
            inv.ExecuteCommand(cmd, sheet, e);
        }
        
        public void PressedEdge_C(PictureBox sheet, MouseEventArgs e, Button b1, Button b2)
        {
            b1.Enabled = true;
            b2.Enabled = true;
            cmd = new PressedEdge_C(model);
            inv.ExecuteCommand(cmd, sheet, e);
        }

        public void SavePicture(PictureBox sheet)
        {
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Title = "Сохранить картинку как...";
            savedialog.OverwritePrompt = true;
            savedialog.CheckPathExists = true;
            savedialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
            savedialog.ShowHelp = true;
            if (savedialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    sheet.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                catch
                {
                    MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void SaveText(ListBox listBoxMatrix)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "Сохранить текст как...";
            saveFile.OverwritePrompt = true;
            saveFile.CheckPathExists = true;
            saveFile.Filter = "Text (*.txt)|*.txt";
            saveFile.ShowHelp = true;
            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    using (var sw = new StreamWriter(saveFile.FileName, false))
                        foreach (var item in listBoxMatrix.Items)
                            sw.Write(item.ToString() + Environment.NewLine);
                }
                catch
                {
                    MessageBox.Show("Невозможно сохранить текст", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
        }

        public void ChangeFont(Label label1, TrackBar trackBar1, ListBox listBoxMatrix)
        {
            label1.Text = "Font size is " + trackBar1.Value;
            listBoxMatrix.Font = new Font(listBoxMatrix.Font.FontFamily, trackBar1.Value);
        }
    }
}