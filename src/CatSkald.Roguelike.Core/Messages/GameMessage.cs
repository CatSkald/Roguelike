namespace CatSkald.Roguelike.Core.Messages
{
    public struct GameMessage
    {
        public GameMessage(MessageType type) : this(type, null)
        {
        }

        public GameMessage(MessageType type, params object[] args)
        {
            Type = type;
            Args = args;
        }

        public MessageType Type { get; }
        public object[] Args { get; }
    }
}
