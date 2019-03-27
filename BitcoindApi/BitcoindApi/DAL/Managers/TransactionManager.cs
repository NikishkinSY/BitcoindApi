using System.Collections.Generic;
using System.Linq;
using BitcoindApi.DAL.Entities;

namespace BitcoindApi.DAL.Managers
{
    public class TransactionManager
    {
        private readonly DataContext _dataContext;

        public TransactionManager(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<Transaction> GetNewTransactions()
        {
            return _dataContext.Income.Where(x => x.Confirmations < 3);
        }
    }
}
