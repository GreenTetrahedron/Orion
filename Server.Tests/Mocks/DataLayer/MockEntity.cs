using Orion.Server.DataLayer.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.Tests.Mocks.DataLayer
{
    public class MockEntity<T> : IEntity<T> where T : class
    {
        public IDictionary<Guid, T> _idToRecord;

        public MockEntity()
        {
            _idToRecord = new ConcurrentDictionary<Guid, T>();
        }

        public async Task<IQueryable<T>> GetAllRecords()
        {
            return _idToRecord.Values.AsQueryable();
        }

        public async Task<T> GetRecordById(Guid id)
        {
            return _idToRecord[id];
        }

        public async Task<bool> DeleteRecordById(Guid id)
        {
            return _idToRecord.Remove(id);
        }

        public async Task<bool> UpdateRecord(Guid id, T record)
        {
            _idToRecord[id] = record;
            return true;
        }

        public async Task<bool> AddRecord(Guid id, T record)
        {
            return _idToRecord.TryAdd(id, record);
        }
    }
}
