using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace BattleCity
{
    class BuildSquare : MyEntity, IKeyboardSupport, ISpawnPosition, IMovable, IWallCreator, IWallRemover
    {
        private List<MyKey> _keys = new List<MyKey> {
            new MyKey(Keys.Up), new MyKey(Keys.Down), new MyKey(Keys.Left),
            new MyKey(Keys.Right), new MyKey(Keys.W),new MyKey(Keys.R)
        };

        public List<MyKey> keys { get => _keys; }
        public Rect spawnPosition => new Rect(40, 40, 0, 0);

        public Rect desiredNewPosition => calculateNewPosition();

        public List<CreateWallRequest> createWallRequests => updateCreateWallRequest();

        public List<RemoveWallRequest> removeWallRequests => updateRemoveWallRequest();

        public BuildSquare(int id) : base(id, EntityType.BuildSquare)
        {
            position = new Rect(20, 20, 20, 20);
            pictureBox = new PictureBox();
            color = Color.FromArgb(255, 200, 200, 200);
            pictureBox.BackColor = color;
            pictureBox.Left = position.left;
            pictureBox.Top = position.top;
            pictureBox.Width = position.width;
            pictureBox.Height = position.height;
        }

        private bool hasMovedThisKeypress = false;

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
                            hasMovedThisKeypress = false;
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
                return newPos;
            }

            if (_keys.First(k => k.keyCode == Keys.Down).isPressed() && !hasMovedThisKeypress)
            {
                newPos.bottom += this.position.height;
                return newPos;
            }

            if (_keys.First(k => k.keyCode == Keys.Left).isPressed() && !hasMovedThisKeypress)
            {
                newPos.left -= this.position.width;
                return newPos;
            }

            if (_keys.First(k => k.keyCode == Keys.Right).isPressed() && !hasMovedThisKeypress)
            {
                newPos.left += this.position.width;
                return newPos;
            }

            return newPos;
        }

        public List<CreateWallRequest> updateCreateWallRequest()
        {
            var requests = new List<CreateWallRequest>();

            if (_keys.First(k => k.keyCode == Keys.W).isPressed())
            {
                requests.Add(new CreateWallRequest(new Rect(this.position.x, this.position.y, this.position.width, this.position.height)));
            }

            return requests;
        }

        public List<RemoveWallRequest> updateRemoveWallRequest()
        {
            var requests = new List<RemoveWallRequest>();

            if (_keys.First(k => k.keyCode == Keys.R).isPressed())
            {
                requests.Add(new RemoveWallRequest(new Rect(this.position.x, this.position.y, this.position.width, this.position.height)));
                System.Console.WriteLine("build square wants to remove wall");
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
