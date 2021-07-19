using CoreBLOG.CORE.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CoreBLOG.CORE.Service
{
    public interface ICoreService<T> where T: CoreEntity
    {
        bool Add(T item);
        bool Add(List<T> item);
        bool Update(T item);
        bool Remove(T item);
        bool Remove(Guid id);
        bool Any(Expression<Func<T, bool>> exp);
        bool RemoveAll(Expression<Func<T, bool>> exp); // Verilen linq ifadesine uygun olarak işlem yapar
        T GetByID(Guid id);
        T GetByDefault(Expression<Func<T, bool>> exp); // Verilen linq ifadesindeki koşullara göre sonuç döndürür
        List<T> GetActive();
        List<T> GetDefault(Expression<Func<T, bool>> exp);
        List<T> GetAll();
        bool Activate(Guid id); //Aktif edilecek id
        int Save(); // int olmasının sebebi db.SaveChanges metodu int sonuç döndürür.
       



    }
}
