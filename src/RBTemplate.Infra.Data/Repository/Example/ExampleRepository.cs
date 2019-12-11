using RBTemplate.Domain.Business.Example;
using RBTemplate.Domain.Business.Example.Repository;
using RBTemplate.Infra.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace RBTemplate.Infra.Data.Repository
{
    public class ExampleRepository : Repository<Example>, IExampleRepository
    {
        public ExampleRepository(ExampleContext context) : base(context)
        {
        }
    }
}
