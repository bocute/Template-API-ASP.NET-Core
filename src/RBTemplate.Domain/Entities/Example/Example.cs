using FluentValidation;
using RBTemplate.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RBTemplate.Domain.Business.Example
{
    public class Example : Entity<Example>
    {
        public Guid Id { get; private set; }
        public string Descricao { get; private set; }

        public static class ExampleFactory
        {
            public static Example AddExample(string descricao)
            {
                var example = new Example()
                {
                    Id = Guid.NewGuid(),
                    Descricao = descricao
                };

                return example;
            }

            public static Example UpdateExample(Example example, string descricao)
            {
                example.Descricao = descricao;

                return example;
            }

        }
        public override bool IsValid()
        {
            ValidateExample();

            ValidationResult = Validate(this);

            return ValidationResult.IsValid;
        }

        private void ValidateExample()
        {
            RuleFor(e => e.Descricao)
                .NotEmpty().WithMessage("A descrição deve ser informada.")
                .MaximumLength(150).WithMessage("A descrição deve ter no máximo 150 caracteres");

        }
    }
}
