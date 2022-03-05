using System;
using System.Windows.Forms;
using Cntr = CourseProject.Controller.Controller;
using Mdl = CourseProject.Model.Model;

namespace CourseProject.View
{
    public partial class Form1 : Form
    {
        Cntr controller;
        Mdl model;
        
        public Form1()
        {
            InitializeComponent();
            model = new Mdl(sheet);
            controller = new Cntr(model);
            
        }

        //кнопка - выбрать вершину
        private void selectButton_Click(object sender, EventArgs e)
        {
            selectButton.Enabled = false;
            drawVertexButton.Enabled = true;
            добавитьВершинуToolStripMenuItem.Enabled = true;
            drawEdgeButton.Enabled = true;
            добавитьРеброToolStripMenuItem.Enabled = true;
            controller.SelectVert_C(sheet);
        }

        //кнопка - рисовать вершину
        private void drawVertexButton_Click(object sender, EventArgs e)
        {
            drawVertexButton.Enabled = false;
            добавитьВершинуToolStripMenuItem.Enabled = false;
            selectButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            добавитьРеброToolStripMenuItem.Enabled = true;
            controller.DrawVertex_C(sheet);
        }

        //кнопка - рисовать ребро
        private void drawEdgeButton_Click(object sender, EventArgs e)
        {
            drawEdgeButton.Enabled = false;
            добавитьРеброToolStripMenuItem.Enabled = false;
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            добавитьВершинуToolStripMenuItem.Enabled = true;
            controller.DrawEdge_C(sheet);
        }

        private void sheet_MouseClick(object sender, MouseEventArgs e)
        {
            //нажата кнопка "выбрать вершину", ищем степень вершины
            if (selectButton.Enabled == false)
            {
                controller.PressedSelect_C(listBoxMatrix, sheet, e);
            }
            //нажата кнопка "рисовать вершину"
            if (drawVertexButton.Enabled == false)
            {
                controller.PressedVertex_C(sheet, e, Paste, Copy);
            }
            //нажата кнопка "рисовать ребро"
            if (drawEdgeButton.Enabled == false)
            {
                controller.PressedEdge_C(sheet, e, Paste, Copy);
            }
        }

        private void pictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sheet.Image != null)
            {
                controller.SavePicture(sheet);
            }
        }

        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.SaveText(listBoxMatrix);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            controller.ChangeFont(label1, trackBar1, listBoxMatrix);
        }

        private void перезапуститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void выйтиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            controller.inv.Undo(sheet,
                drawEdgeButton,
                добавитьРеброToolStripMenuItem,
                selectButton,
                drawVertexButton,
                добавитьВершинуToolStripMenuItem,
                Paste,
                Copy);
        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            controller.inv.Redo(sheet,
                drawEdgeButton,
                добавитьРеброToolStripMenuItem,
                selectButton,
                drawVertexButton,
                добавитьВершинуToolStripMenuItem,
                Paste,
                Copy);
        }
        
        private void добавитьВершинуToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            drawVertexButton.Enabled = false;
            добавитьВершинуToolStripMenuItem.Enabled = false;
            selectButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            добавитьРеброToolStripMenuItem.Enabled = true;
            controller.DrawVertex_C(sheet);
        }
        private void добавитьРеброToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawEdgeButton.Enabled = false;
            добавитьРеброToolStripMenuItem.Enabled = false;
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            добавитьВершинуToolStripMenuItem.Enabled = true;
            controller.DrawEdge_C(sheet);
        }

        private void Paste_Click(object sender, EventArgs e)
        {
            drawEdgeButton.Enabled = false;
            добавитьРеброToolStripMenuItem.Enabled = false;
            selectButton.Enabled = false;
            drawVertexButton.Enabled = false;
            добавитьВершинуToolStripMenuItem.Enabled = false;
            Paste.Enabled = false;
            Copy.Enabled = false;
            controller.PasteGraph_C(sheet);
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            controller.CopyGraph();
        }

        private void buttonAdj_Click_1(object sender, EventArgs e)
        {
            controller.CreateAdjAndOutC(listBoxMatrix);
        }

        private void buttonInc_Click_1(object sender, EventArgs e)
        {
            controller.CreateIncAndOutC(listBoxMatrix);
        }
    }
}
