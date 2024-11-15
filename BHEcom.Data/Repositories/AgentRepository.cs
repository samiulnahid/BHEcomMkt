using BHEcom.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public class AgentRepository : IAgentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AgentRepository> _logger;

        public AgentRepository(ApplicationDbContext context, ILogger<AgentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Guid> AddAsync(Agent agent)
        {
            try
            {
                await _context.Agents.AddAsync(agent);
                await _context.SaveChangesAsync();
                return agent.AgentID; // Assuming AgentId is a Guid
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding an agent.");
                return Guid.Empty; // Return null in case of failure
            }
        }


        public async Task<Agent> GetByIdAsync(Guid id)
        {
            var result = await (from agent in _context.Agents
                                join user in _context.Users
                                on agent.UserID equals user.UserId into addressUserGroup
                                from user in addressUserGroup.DefaultIfEmpty() // Left join to handle missing users
                                where agent.UserID == id
                                select new Agent
                                {
                                    AgentID = agent.AgentID,
                                    UserID = agent.UserID,
                                    AgencyName = agent.AgencyName,
                                    ContactPerson = agent.ContactPerson,
                                    ContactEmail = agent.ContactEmail,
                                    ContactPhone = agent.ContactPhone,
                                    UserName = user != null ? user.UserName : "Unknown User", 
                                }).FirstOrDefaultAsync();

            return result;
            //return await _context.Agents.FindAsync(id);
        }

        public async Task<IEnumerable<Agent>> GetAllAsync()
        {
            var result = await (from agent in _context.Agents
                                join user in _context.Users
                                on agent.UserID equals user.UserId 
                                select new Agent
                                {
                                    AgentID = agent.AgentID,
                                    UserID = agent.UserID,
                                    AgencyName = agent.AgencyName,
                                    ContactPerson = agent.ContactPerson,
                                    ContactEmail = agent.ContactEmail,
                                    ContactPhone = agent.ContactPhone,
                                    UserName = user != null ? user.UserName : "Unknown User",
                                }).ToListAsync();

            return result;
            //return await _context.Agents.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Agent agent)
        {
            try
            {
                // Check if the agent exists in the database
                var existingAgent = await _context.Agents
                                                  .AsNoTracking() // Ensures EF doesn't track the original entity
                                                  .FirstOrDefaultAsync(a => a.AgentID == agent.AgentID);

                if (existingAgent == null)
                {
                    _logger.LogError($"Agent with ID {agent.AgentID} not found.");
                    return false; // Return false if the agent does not exist
                }

                // Update the agent
                _context.Agents.Update(agent);
                int affectedRows = await _context.SaveChangesAsync();

                return true; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the agent.");
                return false; // Return false in case of an exception
            }
        }


        public async Task DeleteAsync(Guid id)
        {
            var agent = await _context.Agents.FindAsync(id);
            if (agent != null)
            {
                _context.Agents.Remove(agent);
                await _context.SaveChangesAsync();
            }
        }
    }

}
