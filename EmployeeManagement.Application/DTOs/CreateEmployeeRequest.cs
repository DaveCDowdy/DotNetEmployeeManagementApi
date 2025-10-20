namespace EmployeeManagement.Application.DTOs

{
    public record CreateEmployeeRequest(
        string FirstName,
        string LastName,
        string Email,
        string Phone,
        string Position
    );
}