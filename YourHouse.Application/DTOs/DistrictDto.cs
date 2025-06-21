using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourHouse.Application.DTOs
{
    public class DistrictDto
    {
        public int CityId { get; set; }

        public int DistrictId { get; set; }

        public string DistrictName { get; set; } = null!;
    }
}
