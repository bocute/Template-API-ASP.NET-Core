using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RBTemplate.Domain.Business.Example;
using RBTemplate.Domain.Business.Example.Repository;
using RBTemplate.Domain.Entities.Exemplo.Business;
using RBTemplate.Domain.Interfaces;
using RBTemplate.Services.Api.ViewModels;

namespace RBTemplate.Services.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ExampleController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IExampleBusiness _exampleBusiness;
        private readonly IExampleRepository _exampleRepository;
        public ExampleController(INotificationHandler notifications, 
                                 IMapper mapper,
                                 IUser user,
                                 IExampleBusiness exampleBusiness,
                                 IExampleRepository exampleRepository) : base(notifications, user)
        {
            _mapper = mapper;
            _exampleBusiness = exampleBusiness;
            _exampleRepository = exampleRepository;
        }

        /// <summary>
        /// Buscar todos os itens.
        /// </summary>
        /// <param name="pageNumber">Número da página</param>   
        /// <param name="pageSize">Tamanho da página</param>   
        [HttpGet]
        public async Task<IEnumerable<ExampleViewModel>> Get(int? pageNumber, int pageSize)
        {
            return _mapper.Map<IEnumerable<ExampleViewModel>>(await _exampleRepository.GetAllAsync(pageNumber, pageSize));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ExampleViewModel model)
        {
            if (!await ValidModelState())
            {
                return await ResponseAsync();
            }

            var example = await _exampleBusiness.AddExample(_mapper.Map<Example>(model));

            return await ResponseAsync(_mapper.Map<ExampleViewModel>(example));

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody]ExampleViewModel model)
        {
            if (!await ValidModelState())
            {
                return await ResponseAsync();
            }

            var example = await _exampleBusiness.UpdateExample(id, _mapper.Map<Example>(model));

            return await ResponseAsync(_mapper.Map<ExampleViewModel>(example));

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _exampleBusiness.DeleteExample(id);

            return await ResponseAsync();
        }
    }
}