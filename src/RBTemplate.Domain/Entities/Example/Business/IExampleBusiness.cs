using RBTemplate.Domain.Business.Example;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RBTemplate.Domain.Entities.Exemplo.Business
{
    public interface IExampleBusiness
    {
        Task<Example> AddExample(Example request);
        Task<Example> UpdateExample(Guid id, Example request);
        Task<bool> DeleteExample(Guid id);
    }
}
