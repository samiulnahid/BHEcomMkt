using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Interfaces
{
    public interface IAgentService
    {
        Task<IEnumerable<Agent>> GetAllAgentsAsync();
        Task<Agent> GetAgentByIdAsync(Guid id);
        Task<Guid> AddAgentAsync(Agent agent);
        Task<bool> UpdateAgentAsync(Agent agent);
        Task<bool> DeleteAgentAsync(Guid id);
    }
}
