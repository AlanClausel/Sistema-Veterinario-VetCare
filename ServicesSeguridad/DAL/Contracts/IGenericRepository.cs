using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesSecurity.DAL.Contracts
{
    public interface IGenericRepository<T> where T : class, new()
    {
        void Add(T obj);

        void Delete(Guid id);

        void Update(T obj);

        //#nullable enable
        IEnumerable<T> SelectAll(string sFilterExpression);
        IEnumerable<T> SelectAll();

        T SelectOne(Guid id);
        T SelectOneByName(String sName);
        T GetOneByName(String sName);

    }
}
