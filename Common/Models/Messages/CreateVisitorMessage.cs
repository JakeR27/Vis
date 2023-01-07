using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.Common.Models;

namespace Common.Models.Messages
{
    public class CreateVisitorMessage : BaseMessage
    {
        public Visitor Visitor = new();
    }
}
