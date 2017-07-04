using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface ITagManager
    {
        List<Tag> GetAll();
        void AttachToList(string tag, int toDoListId);
        Tag Insert(Tag tag, int toDoListId);
        void Delete(string name, int listId);
        Tag GetByName(string name);
        List<TopTags> GetTop10();
    }
}
