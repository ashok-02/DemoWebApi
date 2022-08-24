using System.ComponentModel.DataAnnotations;

namespace DemoWebApi.ViewModel
{
    public class DeptInfo_VM
    {
        [Key]
        public string Name { get; set; }
        public int? Count { get; set; }
        public int? Total_Salary { get; set; }
    }
}
