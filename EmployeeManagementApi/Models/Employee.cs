using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementApi.Models
{
    public class Employee
    {
        public int Id { get; set; }
     
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Phone number is required")]
        public string Phone { get; set; }
        
        [Required(ErrorMessage = "Address is required")]
        public string Position { get; set; }
    }
}