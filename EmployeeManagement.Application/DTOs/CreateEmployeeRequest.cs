using EmployeeManagement.Application.Interfaces;

namespace EmployeeManagement.Application.DTOs

{
    public record CreateEmployeeRequest : IEmployeeCommand
    {
        public required string FirstName { get; init; } 
        public required string LastName { get; init; }
        public required string Email { get; init; }
        public required string Phone { get; init; }
        public required string Position { get; init; }
        public CreateEmployeeRequest() { }
    }
}