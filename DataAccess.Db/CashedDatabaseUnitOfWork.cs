using Cashed.DataAccess.Contract;
using Cashed.DataAccess.Model.Basic;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Cashed.DataAccess.Db
{
    public class CashedDatabaseUnitOfWork : IUnitOfWork
    {
        private CashedDbContext _context;
        private Dictionary<Type, object> _queryRepositories;
        private Dictionary<Type, object> _commandRepositories;

        public CashedDatabaseUnitOfWork(CashedDbContext context)
        {
            _context = context;
            _queryRepositories = new Dictionary<Type, object>();
            _commandRepositories = new Dictionary<Type, object>();
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public ICommandRepository<T> GetCommandRepository<T>() where T : class, IHasName
        {
            _commandRepositories.TryGetValue(typeof(T), out object repo);
            if (repo != null) return repo as ICommandRepository<T>;
            var props = _context.GetType().GetProperties();
            foreach (var p in props)
            {
                var pType = p.PropertyType;
                var gArgs = pType.GenericTypeArguments;
                if (gArgs.Length > 0 && gArgs[0] == typeof(T))
                {
                    var pValue = p.GetValue(_context, new object[0]);
                    var qr = new CommandRepository<T>(pValue as DbSet<T>);
                    _commandRepositories.Add(typeof(T), qr);
                    return qr;
                }
            }
            return null;
        }

        public IQueryRepository<T> GetQueryRepository<T>() where T : class, IHasName
        {
            _queryRepositories.TryGetValue(typeof(T), out object repo);
            if (repo != null) return repo as IQueryRepository<T>;
            var props = _context.GetType().GetProperties();
            foreach (var p in props)
            {
                var pType = p.PropertyType;
                var gArgs = pType.GenericTypeArguments;
                if (gArgs.Length > 0 && gArgs[0] == typeof(T))
                {
                    var pValue = p.GetValue(_context, new object[0]);
                    var qr = new QueryRepository<T>(pValue as DbSet<T>);
                    _queryRepositories.Add(typeof(T), qr);
                    return qr;
                }
            }
            return null;
        }
    }
}
