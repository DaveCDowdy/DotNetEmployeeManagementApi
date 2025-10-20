using FluentValidation;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Validation; // Assuming IEmployeeCommand is now implemented by the DTO/Command

namespace EmployeeManagement.Application.Validation
{
    // This validates the DTO/Command used by the POST endpoint
    public class CreateEmployeeRequestValidator : EmployeeBaseValidator<CreateEmployeeRequest>
    {
        public CreateEmployeeRequestValidator()
        {
            // Apply all common rules from the base class
            // This includes FirstName, LastName, Email, Phone, and Position validation
            ApplyCommonRules();
            
            // Note: Since this is a creation request, no ID check is needed.
        }
    }
}