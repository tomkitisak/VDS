using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vds;

namespace vds.Services.Log
{
    interface IRepository
    {
        Task<Models.Log> Create(Models.Log log);

        Task<Models.Log> InsertCreate(Models.Log log);

        Task<Models.Log> InsertRead(Models.Log log);

        Task<Models.Log> InsertUpdate(Models.Log log);

        Task<Models.Log> InsertDelete(Models.Log log);

        Task<Models.Log> Read(Int64 logId);

        Task<List<Models.Log>> Reads();
    }
}
