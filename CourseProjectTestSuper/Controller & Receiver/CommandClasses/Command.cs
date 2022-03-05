using System.Windows.Forms;

namespace CourseProject.Controller.CommandClasses
{
    public abstract class Command
    {
        public abstract void Execute(PictureBox sheet, MouseEventArgs e);

        public abstract void Execute(PictureBox sheet);

        public abstract void Undo(PictureBox sheet);
    }
}
