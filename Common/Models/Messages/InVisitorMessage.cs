namespace Vis.Common.Models.Messages
{
    [Serializable]
    public class InVisitorMessage : BaseMessage
    {
        public Guid VisitorId;
        public DateTime Time;
    }
}