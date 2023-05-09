using System.ComponentModel.DataAnnotations;

namespace HR_System_Assignment.Models
{
    public class Department
    {
        [Key]
        public int dept_id { get; set; }
        [Required]
        public string dept_name { get; set; }
        public string? dept_description { get; set; }
    }
}
