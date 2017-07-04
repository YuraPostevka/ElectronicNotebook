using BAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;

namespace BAL.Managers
{
    public class ToDoListTagManager : BaseManager, IToDoListTagManager
    {
        public ToDoListTagManager(IUnitOfWork uOW) : base(uOW)
        {
        }

        public void Delete(int? listId, int? tagId)
        {
            throw new NotImplementedException();
        }

        public void Insert(int listId, int tagId)
        {
            throw new NotImplementedException();
        }
    }
}
