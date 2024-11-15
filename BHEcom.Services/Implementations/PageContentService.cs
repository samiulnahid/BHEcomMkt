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
    public class PageContentService : IPageContentService
    {
        private readonly IPageContentRepository _pageContentRepository;

        public PageContentService(IPageContentRepository pageContentRepository)
        {
            _pageContentRepository = pageContentRepository;
        }

        public async Task<IEnumerable<PageContent>> GetAllPageContentsAsync()
        {
            return await _pageContentRepository.GetAllAsync();
        }

        public async Task<PageContent> GetPageContentByIdAsync(Guid id)
        {
            return await _pageContentRepository.GetByIdAsync(id);
        }

        public async Task AddPageContentAsync(PageContent pageContent)
        {
            await _pageContentRepository.AddAsync(pageContent);
        }

        public async Task UpdatePageContentAsync(PageContent pageContent)
        {
            await _pageContentRepository.UpdateAsync(pageContent);
        }

        public async Task DeletePageContentAsync(Guid id)
        {
            await _pageContentRepository.DeleteAsync(id);
        }
    }

}
