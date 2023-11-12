using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace BattleCity
{
    class PlayerSquare : MyEntity, IKeyboardSupport, ISpawnPosition, IMovable, IBulletCreator, IDirection
    {
        private List<MyKey> _keys = new List<MyKey> {
            new MyKey(Keys.Up), new MyKey(Keys.Down), new MyKey(Keys.Left),
            new MyKey(Keys.Right), new MyKey(Keys.W)
        };

        public List<MyKey> keys { get => _keys; }
        public Rect spawnPosition => new Rect(200, 600, 0, 0);
        private Direction _direction = Direction.Left;

        public Rect desiredNewPosition => calculateNewPosition();

        public List<CreateBulletRequest> createBulletRequests => updateCreateBulletRequest();

        public Direction direction { get => this._direction; set => throw new System.NotImplementedException(); }

        public PlayerSquare(int id) : base(id, EntityType.PlayerSquare)
        {
            position = new Rect(20, 20, 20, 20);
            pictureBox = new PictureBox();
            color = Color.FromArgb(255, 200, 200, 255);
            pictureBox.BackColor = color;
            pictureBox.Left = position.left;
            pictureBox.Top = position.top;
            pictureBox.Width = position.width;
            pictureBox.Height = position.height;
        }

        private bool hasMovedThisKeypress = false;
        private bool hasShotThisKeypress = false;

        public void relayKeys(List<MyKey> newKeys)
        {
            if (newKeys != null)
                foreach (var newKey in newKeys)
                {
                    var maybeKey = _keys.FirstOrDefault(k => k.keyCode == newKey.keyCode);
                    if (maybeKey != null)
                    {
                        if (maybeKey.currentState != newKey.currentState)
                        {
                            maybeKey.currentState = newKey.currentState;

                            if (maybeKey.keyCode != Keys.W)
                                hasMovedThisKeypress = false;

                            if (maybeKey.keyCode == Keys.W)
                                hasShotThisKeypress = false;
                        }
                    }
                }
        }

        private Rect calculateNewPosition()
        {
            var newPos = new Rect(this.position.x, this.position.y, this.position.width, this.position.height);
            if (_keys.First(k => k.keyCode == Keys.Up).isPressed() && !hasMovedThisKeypress)
            {
                newPos.bottom -= this.position.height;
                this._direction = Direction.Up;
                return newPos;
            }

            if (_keys.First(k => k.keyCode == Keys.Down).isPressed() && !hasMovedThisKeypress)
            {
                newPos.bottom += this.position.height;
                this._direction = Direction.Down;
                return newPos;
            }

            if (_keys.First(k => k.keyCode == Keys.Left).isPressed() && !hasMovedThisKeypress)
            {
                newPos.left -= this.position.width;
                this._direction = Direction.Left;
                return newPos;
            }

            if (_keys.First(k => k.keyCode == Keys.Right).isPressed() && !hasMovedThisKeypress)
            {
                newPos.left += this.position.width;
                this._direction = Direction.Right;
                return newPos;
            }

            return newPos;
        }

        public List<CreateBulletRequest> updateCreateBulletRequest()
        {
            var requests = new List<CreateBulletRequest>();
            if (_keys.First(k => k.keyCode == Keys.W).isPressed() && !hasShotThisKeypress)
            {
                Rect bulletOutputPosition = new Rect(-300, -300, -300, -300);
                if (this._direction == Direction.Up) bulletOutputPosition = new Rect(this.position.x - 5 + (this.position.width / 2), this.position.top - 10);
                else if (this._direction == Direction.Down) bulletOutputPosition = new Rect(this.position.x - 5 + (this.position.width / 2), this.position.bottom + 10);
                else if (this._direction == Direction.Left) bulletOutputPosition = new Rect(this.position.left - 10, this.position.top - 5 + this.position.height / 2);
                else if (this._direction == Direction.Right) bulletOutputPosition = new Rect(this.position.right + 1, this.position.top - 5 + this.position.height / 2);

                requests.Add(new CreateBulletRequest(bulletOutputPosition, this._direction));
                hasShotThisKeypress = true;
            }

            return requests;
        }

        public void setPosition(Rect newPosition)
        {
            this.position.x = newPosition.x;
            this.position.y = newPosition.y;
            updatePictureBox();
            hasMovedThisKeypress = true;
        }

        public Rect getPosition()
        {
            var pos = new Rect(this.position.x, this.position.y, this.position.width, this.position.height);
            return pos;
        }
    }
}
