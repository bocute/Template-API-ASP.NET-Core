using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RBTemplate.Domain.Models
{
    public abstract class Entity<T> : AbstractValidator<T> where T : Entity<T>
    {
        public ValidationResult ValidationResult { get; protected set; }
        public abstract bool IsValid();
        protected Entity()
        {
            ValidationResult = new ValidationResult();
        }
    }
}
