using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHEcom.Common.Models;

namespace BHEcom.Services.Implementations
{
    public class AttributeService : IAttributeService
    {
        private readonly IAttributeRepository _attributeRepository;

        public AttributeService(IAttributeRepository attributeRepository)
        {
            _attributeRepository = attributeRepository;
        }

        public async Task<IEnumerable<Attributes>> GetAllAttributesAsync()
        {
            return await _attributeRepository.GetAllAsync();
        }

        public async Task<Attributes> GetAttributeByIdAsync(Guid id)
        {
            return await _attributeRepository.GetByIdAsync(id);
        }

        public async Task AddAttributeAsync(Attributes attribute)
        {
            await _attributeRepository.AddAsync(attribute);
        }

        public async Task UpdateAttributeAsync(Attributes attribute)
        {
            await _attributeRepository.UpdateAsync(attribute);
        }

        public async Task DeleteAttributeAsync(Guid id)
        {
            await _attributeRepository.DeleteAsync(id);
        }
    }
}
