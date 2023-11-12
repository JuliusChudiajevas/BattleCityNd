using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    class MyEntity : IComparable<MyEntity>, IEquatable<MyEntity>, IFormattable
    {
        public int id { get; }
        public Color color { get; set; }
        public EntityType type { get; }
        public Rect position { get; set; }
        public PictureBox pictureBox = new PictureBox();
        public MyEntity(int id, EntityType type)
        {
            this.id = id;
            this.type = type;
        }
        protected void updatePictureBox()
        {
            this.pictureBox.Left = this.position.left;
            this.pictureBox.Top = this.position.top;
        }

        public int CompareTo(MyEntity other)
        {
            return id.CompareTo(other.id);
        }

        public bool Equals(MyEntity other)
        {
            return this.id == other.id;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.id + this.type + "x:" + this.position.x + "y:" + this.position.y
                + "width:" + this.position.width + "height:" + this.position.height;
        }
    }
}
