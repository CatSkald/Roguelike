using System.Drawing;

namespace CatSkald.Roguelike.Core.Cells
{
    public sealed class Door : Cell
    {
        public Door(Point location) : base(location, XType.Door)
        {
        }

        public bool IsOpened { get; private set; }

        public override Appearance Appearance => 
            new Appearance(IsOpened ? '\'' : '+', Color.Orange, true, true, !IsOpened);

        public void Open()
        {
            IsOpened = true;
        }
    }
}
