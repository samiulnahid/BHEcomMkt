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
    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;

        public PageService(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public async Task<IEnumerable<Page>> GetAllPagesAsync()
        {
            return await _pageRepository.GetAllAsync();
        }

        public async Task<Page> GetPageByIdAsync(Guid id)
        {
            return await _pageRepository.GetByIdAsync(id);
        }

        public async Task AddPageAsync(Page page)
        {
            await _pageRepository.AddAsync(page);
        }

        public async Task UpdatePageAsync(Page page)
        {
            await _pageRepository.UpdateAsync(page);
        }

        public async Task DeletePageAsync(Guid id)
        {
            await _pageRepository.DeleteAsync(id);
        }
    }

}
