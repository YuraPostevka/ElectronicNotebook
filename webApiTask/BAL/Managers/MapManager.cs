using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.Interfaces;
using DAL.Interfaces;
using Models.DTO;

namespace BAL.Managers
{
    public class MapManager : BaseManager, IMapManager
    {
        public MapManager(IUnitOfWork uOW) : base(uOW)
        {
        }

        public void Insert(GMapsDTO model)
        {
            var list = uOW.ToDoListRepo.GetByID(model.ListId);
            var point = ConvertLatLonToDbGeography(model.Longitude, model.Latitude);

            list.Position = point;
            uOW.Save();
        }

        public DbGeography ConvertLatLonToDbGeography(double longitude, double latitude)
        {
            var point = string.Format("POINT({1} {0})", latitude, longitude);
            return DbGeography.FromText(point);
        }

        public List<GMapsDTO> GetPoints()
        {
            var res = new List<GMapsDTO>();
            var lists = uOW.ToDoListRepo.All.ToList();

            foreach (var list in lists)
            {
                var gMaps = new GMapsDTO();
                if (list.Position != null)
                {
                    gMaps.ListId = list.Id;
                    gMaps.Latitude = list.Position.Latitude.Value;
                    gMaps.Longitude = list.Position.Longitude.Value;
                }
                res.Add(gMaps);
            }

            return res;
        }

        public GMapsDTO GetById(int id)
        {
            var list = uOW.ToDoListRepo.All.FirstOrDefault(i => i.Id == id);
            var gMaps = new GMapsDTO();
            if (list.Position == null) return gMaps;
            gMaps.ListId = list.Id;
            gMaps.Latitude = list.Position.Latitude.Value;
            gMaps.Longitude = list.Position.Longitude.Value;

            return gMaps;
        }
    }
}
