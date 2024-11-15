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
    public class AgentService : IAgentService
    {
        private readonly IAgentRepository _agentRepository;

        public AgentService(IAgentRepository agentRepository)
        {
            _agentRepository = agentRepository;
        }

        public async Task<IEnumerable<Agent>> GetAllAgentsAsync()
        {
            return await _agentRepository.GetAllAsync();
        }

        public async Task<Agent> GetAgentByIdAsync(Guid id)
        {
            return await _agentRepository.GetByIdAsync(id);
        }

        public async Task<Guid> AddAgentAsync(Agent agent)
        {
            return await _agentRepository.AddAsync(agent);
             //await _storeRepository.AddAsync(store);
            //return agent.AgentID;
        }

        public async Task<bool> UpdateAgentAsync(Agent agent)
        {
            return await _agentRepository.UpdateAsync(agent);
        }

        public async Task DeleteAgentAsync(Guid id)
        {
            await _agentRepository.DeleteAsync(id);
        }
    }
}
