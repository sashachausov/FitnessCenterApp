using FitnessCenter.Application.Other;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Validators
{
    public class ClientValidator : AbstractValidator<ClientDto>
    {
        public ClientValidator()
        {
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Фамилия обязательная")
                .MaximumLength(25).WithMessage("Фамилия не должена превышать 25 символов");
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Имя обязательно")
                .MaximumLength(25).WithMessage("Имя не должно превышать 25 символов.");
            RuleFor(x => x.MiddleName)
                .MaximumLength(25).WithMessage("Отчество не должена превышать 25 символов");
            RuleFor(x => x.PhoneNumber)
                .Must(BeValidRussianPhoneNumber)
                .Matches(@"^(\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}|\+7\d{10}|8 \d{3} \d{3}-\d{2}-\d{2}|8\d{10})$")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
                .WithMessage("Неверный формат телефона. Допустимые форматы:\n" +
                             "+7 (XXX) XXX-XX-XX\n" +
                             "8 XXX XXX-XX-XX\n" +
                             "+7XXXXXXXXXX\n" +
                             "8XXXXXXXXXX");
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Неверный формат email-адреса")
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .MaximumLength(100).WithMessage("Email не должен превышать 100 символов");
        }

        private static bool BeValidRussianPhoneNumber(string? phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return true;

            // Correct cleaning of non-digit characters
            string cleaned = phoneNumber.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "");

            if (cleaned.StartsWith("+7") && cleaned.Length == 12 && long.TryParse(cleaned.Substring(2), out _))
                return true;
            if (cleaned.StartsWith("8") && cleaned.Length == 11 && long.TryParse(cleaned.Substring(1), out _))
                return true;
            return false;
        }
    }
}
