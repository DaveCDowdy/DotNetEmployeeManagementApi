using FluentValidation;
using EmployeeManagement.Application.Interfaces;

namespace EmployeeManagement.Application.Validation
{
    public abstract class EmployeeBaseValidator<T> : AbstractValidator<T>
        where T : class, IEmployeeCommand 
    {
        protected void ApplyCommonRules()
        {
            // --- Required Fields ---
            RuleFor(e => e.FirstName)
                .NotEmpty().WithMessage("First name is required.");
            
            RuleFor(e => e.LastName)
                .NotEmpty().WithMessage("Last name is required.");
            
            // --- Email Format Validation ---
            RuleFor(e => e.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("A valid email address is required.");
            
            // --- Phone Number Validation (Digits Only, Exact 10) ---
            RuleFor(e => e.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches("^[0-9]{10}$").WithMessage("Phone number must be exactly 10 digits and contain no symbols or spaces.");
            
            // --- Position Validation ---
            RuleFor(e => e.Position)
                .NotEmpty().WithMessage("Position is required.")
                .MaximumLength(100).WithMessage("Position cannot exceed 100 characters.");
        }
    }
}