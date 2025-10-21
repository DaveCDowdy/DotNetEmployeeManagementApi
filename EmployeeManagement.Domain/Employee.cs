namespace EmployeeManagement.Domain
{
    public class Employee
    {
        public Employee() { } 
        
        public Employee(int id, string firstName, string lastName, string email, string phone, string position)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Position = position;
        }
        
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
    }
}