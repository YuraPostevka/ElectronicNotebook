using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ToDoListsTags
    {
        [Key, Column(Order = 0)]
        [ForeignKey("Tag")]
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("ToDoList")]
        public int ToDoListId { get; set; }
        public virtual ToDoList ToDoList { get; set; }
    }
}
