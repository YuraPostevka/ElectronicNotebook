using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ToDoList : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<ToDoItem> Items { get; set; }

        [ForeignKey("User_Id")]
        public virtual User User { get; set; }
        public int User_Id { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Created { get; set; }

        public virtual System.Data.Entity.Spatial.DbGeography Position { get; set; }

    }
}
