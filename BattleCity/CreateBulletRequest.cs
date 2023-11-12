namespace BattleCity
{
    class CreateBulletRequest
    {
        public Rect position { get; }
        public Direction direction { get; }
        public CreateBulletRequest(Rect pos, Direction direction)
        {
            position = pos;
            this.direction = direction;
        }
    }
}
