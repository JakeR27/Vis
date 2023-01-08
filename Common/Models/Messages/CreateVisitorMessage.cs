namespace Vis.Common.Models.Messages
{
    [Serializable]
    public class CreateVisitorMessage : BaseMessage
    {
        public Visitor Visitor = new();
    }
}
