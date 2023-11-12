namespace BattleCity
{
    class Rect
    {
        public int x, y, width, height;
        public int left { get => x; set => x = value; }
        public int right { get => x + width; set => x = value - width; }
        public int top { get => y; set => y = value; }
        public int bottom { get => y + height; set => y = value - height; }
        public Rect(int x, int y, int width = 0, int height = 0)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
    }
}
