using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.DataLayer.Entities
{
    public interface IEntity<T> where T : class
    {
        public Task<IQueryable<T>> GetAllRecords();

        public Task<T> GetRecordById(Guid id);

        public Task<bool> DeleteRecordById(Guid id);

        public Task<bool> UpdateRecord(Guid id, T record);

        public Task<bool> AddRecord(Guid id, T record);
    }
}
