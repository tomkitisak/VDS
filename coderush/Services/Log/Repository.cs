using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vds;
using vds.Data;
using Microsoft.EntityFrameworkCore;


namespace vds.Services.Log
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _context;

        public Repository(
            ApplicationDbContext context
            )
        {
            _context = context;
        }

        public async Task<Models.Log> Create(Models.Log log)
        {
            try
            {
                await _context.Log.AddAsync(log);
                await _context.SaveChangesAsync();

                return log;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<Models.Log> InsertCreate(Models.Log log)
        {
            try
            {
                log.OperationType = OperationType.CREATE;

                await _context.Log.AddAsync(log);
                await _context.SaveChangesAsync();

                return log;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<Models.Log> InsertRead(Models.Log log)
        {
            try
            {
                log.OperationType = OperationType.READ;

                await _context.Log.AddAsync(log);
                await _context.SaveChangesAsync();

                return log;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<Models.Log> InsertUpdate(Models.Log log)
        {
            try
            {
                log.OperationType = OperationType.UPDATE;

                await _context.Log.AddAsync(log);
                await _context.SaveChangesAsync();

                return log;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<Models.Log> InsertDelete(Models.Log log)
        {
            try
            {
                log.OperationType = OperationType.DELETE;

                await _context.Log.AddAsync(log);
                await _context.SaveChangesAsync();

                return log;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<Models.Log> Read(Int64 logId)
        {
            try
            {
                Models.Log log = new Models.Log();
                log = await _context.Log.Where(x => x.LogId.Equals(logId)).FirstOrDefaultAsync();

                return log;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Models.Log>> Reads()
        {
            try
            {
                List<Models.Log> logs = new List<Models.Log>();
                logs = await _context.Log.ToListAsync();
                return logs;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
