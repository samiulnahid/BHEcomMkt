﻿using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public interface IAddressRepository
    {
        Task<Guid> AddAsync(Address address);
        Task<Address> GetByIdAsync(Guid id);
        Task<Address?> GetByUserIdAsync(Guid id, string type);
        Task<IEnumerable<Address>> GetAllAsync();
        Task<IEnumerable<Address>> GetAllByUserIdAsync(Guid id);
        Task<bool> UpdateAsync(Address address);
        Task<bool> DeleteAsync(Guid id);
    }

}
