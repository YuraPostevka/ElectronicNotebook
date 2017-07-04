using AutoMapper;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<ToDoList, ListTagDTO>();
        }
    }
}
