// File: EmployeeManagement.Domain/Employee.cs

namespace EmployeeManagement.Domain
{
    // CHANGE: Convert from a positional record to a standard class
    public class Employee
    {
        // Parameterless constructor required by EF Core and AutoMapper for creation
        public Employee() { } 

        // CRITICAL: Constructor used for seeding/loading existing data
        public Employee(int id, string firstName, string lastName, string email, string phone, string position)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Position = position;
        }

        // Properties (EF Core requires a public setter/init for tracking changes)
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
    }
}