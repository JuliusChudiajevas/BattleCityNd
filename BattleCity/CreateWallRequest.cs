namespace BattleCity
{
    class CreateWallRequest
    {
        public Rect position { get; }
        public CreateWallRequest(Rect pos)
        {
            position = pos;
        }
    }
}
