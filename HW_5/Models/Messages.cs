using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTest.Models
{
    public class Messages
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool Received { get; set; }
        public int? ToUserId { get; set; }
        public int? FromUserId { get; set; }
        public virtual Users? ToUser { get; set; }
        public virtual Users? FromUser { get; set; }
    }
}
