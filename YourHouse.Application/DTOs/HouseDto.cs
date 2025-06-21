using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourHouse.Application.DTOs
{
    public class HouseDto
    {
        public int ArticleId { get; set; }
        public int BedRoom { get; set; }

        public int BathRoom { get; set; }

        public int? Floors { get; set; }
    }
}
