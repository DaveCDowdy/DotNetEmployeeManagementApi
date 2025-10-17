namespace EmployeeManagement.Application.Interfaces
{
    public interface IEmployeeCommand
    {
        string FirstName { get; init; }
        string LastName { get; init; }
        string Email { get; init; }
        string Phone { get; init; }
        string Position { get; init; }
    }
}