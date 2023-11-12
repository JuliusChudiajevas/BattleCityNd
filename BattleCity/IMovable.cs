namespace BattleCity
{
    interface IMovable
    {
        Rect desiredNewPosition { get; }
        Rect getPosition();
        void setPosition(Rect newPosition);

    }
}
