namespace BattleCity
{
    class RemoveWallRequest
    {
        public Rect position { get; }
        public RemoveWallRequest(Rect pos)
        {
            position = pos;
        }
    }
}
