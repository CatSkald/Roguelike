using System.Drawing;

namespace CatSkald.Roguelike.Core.Cells
{
    public sealed class Door : Cell
    {
        public Door(Point location) : base(location, XType.Door)
        {
        }

        public bool IsOpened { get; private set; }
        public bool IsLocked { get; private set; }

        public override Appearance GetAppearance() => 
            new Appearance("Door", (IsOpened ? "Opened" : "Closed") + " wooden door.",
                IsOpened ? '\'' : '+', Color.Orange, isVisible: true, isSolid: true, isObstacle: IsLocked);


        public void Unlock()
        {
            IsLocked = false;
        }

        public bool Open()
        {
            if (!IsLocked)
            {
                IsOpened = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void OpenWithLock()
        {
            IsLocked = false;
            IsOpened = true;
        }

        public void Lock()
        {
            IsLocked = true;
        }
    }
}
