using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public interface IAgentRepository
    {

        Task<Guid> AddAsync(Agent agent);
        Task<Agent> GetByIdAsync(Guid id);
        Task<IEnumerable<Agent>> GetAllAsync();
        Task<bool> UpdateAsync(Agent agent);
        Task DeleteAsync(Guid id);


    }

}
