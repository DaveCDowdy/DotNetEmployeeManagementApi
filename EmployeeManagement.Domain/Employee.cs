namespace EmployeeManagement.Domain;

public record Employee(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string Position
);