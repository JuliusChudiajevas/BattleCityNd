using System.Drawing;
using System.Windows.Forms;


namespace BattleCity
{
    class EnemySpawn : MyEntity, IMovable
    {
        public EnemySpawn(int id) : base(id, EntityType.EnemySpawn)
        {
            position = new Rect(20, 20, 20, 20);
            pictureBox = new PictureBox();
            color = Color.FromArgb(255, 255, 50, 50);
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
