using System.ComponentModel.DataAnnotations;

namespace HR_System_Assignment.ViewModels
{
    public class DepartmentViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int EmployeeCount { get; set; }
        public string? Description { get; set; }

    }
}
