using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    class Wall : MyEntity, IMovable
    {
        public Wall(int id) : base(id, EntityType.Wall)
        {
            position = new Rect(20, 20, 20, 20);
            pictureBox = new PictureBox();
            color = Color.FromArgb(255, 210, 200, 100);
            pictureBox.BackColor = color;
            pictureBox.Left = position.left;
            pictureBox.Top = position.top;
            pictureBox.Width = position.width;
            pictureBox.Height = position.height;
        }

        public Rect desiredNewPosition => new Rect(this.position.x, this.position.y, this.position.width, this.position.height);

        public Rect getPosition()
        {
            var pos = new Rect(this.position.x, this.position.y, this.position.width, this.position.height);
            return pos;
        }

        public void setPosition(Rect newPosition)
        {
            this.position.x = newPosition.x;
            this.position.y = newPosition.y;
            updatePictureBox();
        }
    }
}
