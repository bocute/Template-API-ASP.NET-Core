using RBTemplate.Domain.Business.Example.Repository;
using RBTemplate.Domain.Entities.Exemplo.Business;
using RBTemplate.Domain.Interfaces;
using RBTemplate.Domain.Models;
using RBTemplate.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RBTemplate.Domain.Business.Example
{
    public class ExampleBusiness : Business<Example>, IExampleBusiness
    {
        private readonly IExampleRepository _exampleRepository;
        private readonly INotificationHandler _notifications;
        public ExampleBusiness(IUnitOfWork uow, 
            INotificationHandler notifications,
            IExampleRepository exampleRepository) : base(uow, notifications)
        {
            _exampleRepository = exampleRepository;
            _notifications = notifications;
        }
        public async Task<Example> AddExample(Example request)
        {
            var newExample = Example.ExampleFactory.AddExample(request.Descricao);

            if (!await IsValidAsync(newExample))
                return await Task.FromResult(request);

            _exampleRepository.Add(newExample);

            if (await Commit())
            {
                return await Task.FromResult(newExample);
            }

            return request;
        }

        public async Task<Example> UpdateExample(Guid id, Example request)
        {
            var oldExample = await _exampleRepository.GetByIdAsync(id);

            if (oldExample == null)
            {
                await _notifications.AddNotificationAsync(new DomainNotification("ExampleBusiness", "Example não encontrado."));
                return await Task.FromResult(request);
            }

            var newExample = Example.ExampleFactory.UpdateExample(oldExample, request.Descricao);

            if (!await IsValidAsync(newExample))
                return await Task.FromResult(request);

            _exampleRepository.Update(newExample);


            if (await Commit())
            {
                return await Task.FromResult(newExample);
            }

            return request;
        }

        public async Task<bool> DeleteExample(Guid id)
        {
            if (await ExistsAsync(id, null, "ExampleBusiness"))
            {
                _exampleRepository.Remove(id);

                if (await Commit())
                {
                    return true;
                }
            }

            return false;
        }

        protected override async Task<bool> ExistsAsync(Guid? id, string usuarioId, string messageType)
        {
            var oldExample = await _exampleRepository.GetByIdAsync(id.Value);

            if (oldExample == null)
            {
                await _notifications.AddNotificationAsync(new DomainNotification(messageType, "Example não encontrado."));
                return false;

            }

            return true;
        }
    }
}
