namespace Vis.Common.Models.Messages
{
    [Serializable]
    public class OutVisitorMessage : BaseMessage
    {
        public Guid VisitorId;
        public DateTime Time;
    }
}