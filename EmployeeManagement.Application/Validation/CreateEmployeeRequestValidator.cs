using EmployeeManagement.Application.DTOs;

namespace EmployeeManagement.Application.Validation
{
    public class CreateEmployeeRequestValidator : EmployeeBaseValidator<CreateEmployeeRequest>
    {
        public CreateEmployeeRequestValidator()
        {
            ApplyCommonRules();
        }
    }
}