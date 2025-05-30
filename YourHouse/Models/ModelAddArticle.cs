using System.ComponentModel.DataAnnotations;

namespace YourHouse.Models
{
    public class ModelAddArticle
    {
        [Required(ErrorMessage = "Yêu cầu nhập tiêu đề.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập chi tiết.")]
        public string Desc { get; set; }
        public int? City { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập chi tiết địa chỉ.")]
        public string Address { get; set; }
        public int? District { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập điện tích.")]
        [Range(10.00, double.MaxValue, ErrorMessage = "Giá trị phải lớn hơn 10")]
        public double? S { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập giá.")]
        [Range(500000, double.MaxValue, ErrorMessage = "Giá trị phải lớn hơn 500000")]
        public double? Price { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tiền cọc.")]
        public double? TienCoc { get; set; }
        public string Type { get; set; }
        public int? Status { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập số phòng ngủ.")]
        [Range(1, int.MaxValue, ErrorMessage = "Giá trị phải lớn hơn 0")]
        public int? BedRoom { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập số phòng vệ sinh.")]
        [Range(1, int.MaxValue, ErrorMessage = "Giá trị phải lớn hơn 0")]
        public int? BathRoom { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập số tầng.")]
        [Range(1, int.MaxValue, ErrorMessage = "Giá trị phải lớn hơn 0")]
        public int? Floors { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tâng của phòng")]
        [Range(1, int.MaxValue, ErrorMessage = "Giá trị phải lớn hơn 0")]
        public int? Floor { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập hướng cửa chính.")]
        public int? DoorDrt { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập số người tối đa.")]
        [Range(1, int.MaxValue, ErrorMessage = "Giá trị phải lớn hơn 0")]
        public int? MaxPerson { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập giá nước.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá trị phải lớn hơn 0")]
        public double? WaterPrice { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập giá điện.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá trị phải lớn hơn 0")]
        public double? ElectricPrice { get; set; }

        [Required(ErrorMessage = "Yêu cầu thêm ảnh.")]
        public List<IFormFile> Images { get; set; }
    }
}
