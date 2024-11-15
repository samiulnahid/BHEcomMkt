using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Implementations
{
    public class SEOService : ISEOService
    {
        private readonly ISEORepository _seoRepository;

        public SEOService(ISEORepository seoRepository)
        {
            _seoRepository = seoRepository;
        }

        public async Task<IEnumerable<SEO>> GetAllSEOAsync()
        {
            return await _seoRepository.GetAllAsync();
        }

        public async Task<SEO> GetSEOByIdAsync(Guid id)
        {
            return await _seoRepository.GetByIdAsync(id);
        }

        public async Task AddSEOAsync(SEO seo)
        {
            await _seoRepository.AddAsync(seo);
        }

        public async Task UpdateSEOAsync(SEO seo)
        {
            await _seoRepository.UpdateAsync(seo);
        }

        public async Task DeleteSEOAsync(Guid id)
        {
            await _seoRepository.DeleteAsync(id);
        }
    }

}
