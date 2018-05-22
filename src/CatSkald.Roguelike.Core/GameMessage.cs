namespace CatSkald.Roguelike.Core
{
    public struct GameMessage
    {
        public GameMessage(MessageType type) : this(type, null)
        {
        }

        public GameMessage(MessageType type, params string[] args)
        {
            Type = type;
            Args = args;
        }

        public MessageType Type { get; }
        public string[] Args { get; }
    }
}
