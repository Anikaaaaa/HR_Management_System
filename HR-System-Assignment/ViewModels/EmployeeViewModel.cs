using System.ComponentModel.DataAnnotations;

namespace HR_System_Assignment.ViewModels
{
    public class EmployeeViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int? DeptId { get; set; }
        [Required]
        public string? Deptartment { get; set; }
        public List<string>? Departments { get; set; }
        public IFormFile? Photo { get; set; }
        public string? PhotoLink { get; set; }
        [Required]
        public string JoiningDate { get; set; }
        public string? ManagerName { get; set; }
        public bool Status { get; set; }
    }
}
