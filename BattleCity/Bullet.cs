using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    class Bullet : MyEntity, IMovable, IDirection
    {

        private Direction _direction = Direction.Left;

        public Rect desiredNewPosition => calculateNewPosition();


        public Direction direction { get => this._direction; set => this._direction = value; }

        public Bullet(int id) : base(id, EntityType.Bullet)
        {
            position = new Rect(20, 20, 10, 10);
            pictureBox = new PictureBox();
            color = Color.FromArgb(255, 255, 255, 255);
            pictureBox.BackColor = color;
            pictureBox.Left = position.left;
            pictureBox.Top = position.top;
            pictureBox.Width = position.width;
            pictureBox.Height = position.height;
        }

        private Rect calculateNewPosition()
        {
            var newPos = new Rect(this.position.x, this.position.y, this.position.width, this.position.height);
            if (_direction == Direction.Up)
            {
                newPos.y -= 1;
            }
            if (_direction == Direction.Down)
            {
                newPos.y += 1;
            }
            if (_direction == Direction.Left)
            {
                newPos.x -= 1;
            }
            if (_direction == Direction.Right)
            {
                newPos.x += 1;
            }

            return newPos;
        }


        public void setPosition(Rect newPosition)
        {
            this.position.x = newPosition.x;
            this.position.y = newPosition.y;
            updatePictureBox();
        }

        public Rect getPosition()
        {
            var pos = new Rect(this.position.x, this.position.y, this.position.width, this.position.height);
            return pos;
        }
    }
}
