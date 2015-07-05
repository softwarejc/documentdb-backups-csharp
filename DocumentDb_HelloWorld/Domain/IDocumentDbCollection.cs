using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DocumentDb_HelloWorld.Domain
{
    public interface IDocumentDbCollection<T> : IDisposable
        where T : Document
    {
        Task<T> CreateAsync(T item);

        T Get(Expression<Func<T, bool>> predicate);

        T Get(string id);

        IEnumerable<T> Where(Expression<Func<T, bool>> predicate = null);

        IEnumerable<T> Documents { get; }

        Task<T> ReplaceAsync(string id, T item);

        Task DeleteAsync(string id);

        Task DeleteAsync(T item);
    }
}