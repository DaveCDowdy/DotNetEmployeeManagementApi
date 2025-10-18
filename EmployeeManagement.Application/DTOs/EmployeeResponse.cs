namespace EmployeeManagement.Application.DTOs

{
    public record EmployeeResponse(
        int Id,
        string FirstName,
        string LastName,
        string Email,
        string Phone,
        string Position
    );
}