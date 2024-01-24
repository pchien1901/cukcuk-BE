using AutoMapper;
using core.Entities;
using MISA.CUKCUK.Core.DTOs.ImportDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.AutoMapper
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CustomerImport, Customer>();
        }
    }
}
