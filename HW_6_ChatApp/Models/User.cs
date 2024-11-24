using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW_6_ChatApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
       
       
        public virtual List<Message>? ToMessage { get; set; }
        public virtual List<Message>? FromMessages { get; set; }
    }
}
