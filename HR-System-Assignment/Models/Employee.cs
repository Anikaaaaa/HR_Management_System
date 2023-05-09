using System.ComponentModel.DataAnnotations;

namespace HR_System_Assignment.Models
{
    public class Employee
    {
        [Key]
        public int emp_id { get; set; }
        [Required]
        public string emp_name { get; set; }
        public int? dept_id { get; set; }
        public string? emp_photo { get; set; }
        [Required]
        public string emp_joining_date { get; set; } 
        public string? manager_name { get; set; }
        public string emp_status { get; set; }
    }
}
