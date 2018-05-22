namespace CatSkald.Roguelike.Core.Cells
{
    public sealed class Door : Cell
    {
        public Door()
        {
            Type = XType.DoorClosed;
        }

        public bool IsOpened => Type == XType.DoorOpened;

        public void Open()
        {
            Type = XType.DoorOpened;
        }
    }
}
