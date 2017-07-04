using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface IToDoListTagManager
    {
        void Delete(int? listId, int? tagId);
        void Insert(int listId, int tagId);
    }
}
