using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DTO;

namespace BAL.Interfaces
{
    public interface IMapManager
    {
        void Insert(GMapsDTO model);
        List<GMapsDTO> GetPoints();
        GMapsDTO GetById(int id);
    }
}
