using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourHouse.Application.DTOs
{
    public class ChungCuDto
    {
        public int ArticleId { get; set; }
        public int? Floor { get; set; }

        public int BedRoom { get; set; }

        public int BathRoom { get; set; }

        public int MaxPerson { get; set; }

        public decimal? WaterPrice { get; set; }

        public decimal? ElectricPrice { get; set; }
    }
}
