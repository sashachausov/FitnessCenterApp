using FitnessCenter.Application.Other;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Validators
{
    public class EmployeeValidator : AbstractValidator<EmployeeDto>
    {
        public EmployeeValidator()
        {
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Фамилия обязательна")
                .MaximumLength(25).WithMessage("Фамилия не должна превышать 25 символов");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Имя обязательно")
                .MaximumLength(25).WithMessage("Имя не должно превышать 25 символов");

            RuleFor(x => x.MiddleName)
                .MaximumLength(25).WithMessage("Отчество не должно превышать 25 символов");

            // Phone validation aligned with DB CHECK and client validator
            RuleFor(x => x.PhoneNumber)
                .Must(BeValidPhoneNumber)
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
                .WithMessage("Неверный формат телефона.\n\n" +
                             "Разрешённые форматы:\n" +
                             "• +7 (XXX) XXX-XX-XX\n" +
                             "• 8 XXX XXX-XX-XX\n" +
                             "• +7XXXXXXXXXX\n" +
                             "• 8XXXXXXXXXX (11 цифр)");
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Неверный формат email")
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .MaximumLength(100);

            RuleFor(x => x.RoleId)
                .NotNull().WithMessage("Должность (роль) обязательна");
        }

        private static bool BeValidPhoneNumber(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return true;

            // Allow formats:
            // +7 (XXX) XXX-XX-XX
            // 8 XXX XXX-XX-XX
            // +7XXXXXXXXXX
            // 8XXXXXXXXXX
            var allowedPattern = @"^(\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}|8 \d{3} \d{3}-\d{2}-\d{2}|\+7\d{10}|8\d{10})$";
            return Regex.IsMatch(phone.Trim(), allowedPattern);
        }
    }
}
