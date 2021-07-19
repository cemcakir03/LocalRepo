using CoreBLOG.CORE.Entity;
using CoreBLOG.CORE.Entity.Enums;
using CoreBLOG.CORE.Service;
using CoreBLOG.MODEL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Transactions;

namespace CoreBLOG.SERVICE.Base
{
    public class BaseService<T> : ICoreService<T> where T : CoreEntity
    {
        private readonly BlogContext context;
        public BaseService(BlogContext _context)
        {
            this.context = _context;
        }
        public bool Activate(Guid id)
        {
            T activated = GetByID(id);
            activated.Status = Status.Active;
            return Update(activated);
        }

        public bool Add(T item)
        {
            try
            {
                context.Set<T>().Add(item);
                return Save() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Add(List<T> item)
        {
            try
            {
                // using => kod bloğunda bulunan işlemler using scope'larından çıktıktan sonra ram bellekten silinir.

                // TransactionScope => Birbirini izleyen işlemlerin herhangi birinde hata olması durumunda yapılan tüm işlemlerin geri alınmasını sağlar.

                using (TransactionScope scope = new TransactionScope())
                {
                    context.Set<T>().AddRange(item);

                    scope.Complete(); // scope'lar içinde işlemlerin hepsinin doğru bi şekilde gerçekleştiğini kontrol eder.

                    return Save() > 0;


                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Any(Expression<Func<T, bool>> exp)
        {
            return context.Set<T>().Any(exp);
        }

        public List<T> GetActive()
        {
            return context.Set<T>().Where(x => x.Status != Status.Deleted).ToList();
        }

        public List<T> GetAll()
        {
            return context.Set<T>().ToList();
        }

        public T GetByDefault(Expression<Func<T, bool>> exp)
        {
            return context.Set<T>().FirstOrDefault(exp);
        }

        public T GetByID(Guid id)
        {
            // o anda kullanılan entity tipine göre Set<T>() metodu kullanıldı
            return context.Set<T>().Find(id);
        }

        public List<T> GetDefault(Expression<Func<T, bool>> exp)
        {
            return context.Set<T>().Where(exp).ToList();
        }

        public bool Remove(T item)
        {
            item.Status = Status.Deleted;
            return Update(item);
        }

        public bool Remove(Guid id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    T item = GetByID(id);
                    item.Status = Status.Deleted;
                    scope.Complete();
                    return Update(item);
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool RemoveAll(Expression<Func<T, bool>> exp)
        {
            // verilen linq ifadesine göre silinmek istenen listedeki verilerin tek tek doğru bi şekilde silinmesinin kontrolü
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var list = GetDefault(exp);
                    int count = 0;

                    foreach (var item in list)
                    {
                        item.Status = Status.Deleted;
                        bool updated = Update(item);
                        if (updated) count++;
                    }
                    if (list.Count == count)
                    {
                        scope.Complete();
                    }
                    else return false;

                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        public bool Update(T item)
        {
            try
            {
                //Gelen entity önce save metoduna uğrar, save metodu 0'dan büyük döndürürse Update işlemini uygular.

                context.Set<T>().Update(item);
                return Save() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public void DetachedEntity(T item)
        {
            // Giriş yapılan entry hangisiyse garbage collector tarafından toplanmasına izin verilir, performans arttırır.

            context.Entry<T>(item).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        }
    }
}
