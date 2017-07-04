using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface IToDoListManager
    {
        List<ListTagDTO> GetAll();
        ToDoList GetById(int? id);
        ListTagDTO Insert(ToDoList item);
        void Delete(int? id);
        void ChangeName(int id, string name);
        List<ListTagDTO> GetListsByTagName(string tagName);
    }
}
