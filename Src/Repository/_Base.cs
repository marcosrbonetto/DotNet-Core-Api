using System;
using System.Linq;
using ApiDebts.Src.DAO;
using ApiDebts.Src.Model;

namespace ApiDebts.Src.Repository
{
    public class Base<T> where T : Model.Entity.Base
    {
        public LoggedUser LoggedUser { get; }
        public DebtsContext Context { get; }

        public Base(DebtsContext context, LoggedUser loggedUser)
        {
            Context = context;
            LoggedUser = loggedUser;
        }

        public void Error(string message)
        {
            throw new Exception(message);
        }

        public T GetById(int id, bool includeDeleted = false)
        {
            return new BaseGenerics<T>(Context, LoggedUser).GetById(id, includeDeleted);
        }

        public Entity GetById<Entity>(int id, bool includeDeleted = false) where Entity : Model.Entity.Base
        {
            return new BaseGenerics<Entity>(Context, LoggedUser).GetById(id, includeDeleted);
        }

        public bool Exists(int id, bool includeDeleted = false)
        {
            return new BaseGenerics<T>(Context, LoggedUser).Exists(id, includeDeleted);
        }

        public bool Exists<Entity>(int id, bool includeDeleted = false) where Entity : Model.Entity.Base
        {
            return new BaseGenerics<Entity>(Context, LoggedUser).Exists(id, includeDeleted);
        }

        public T Insert(T entity)
        {
            return new BaseGenerics<T>(Context, LoggedUser).Insert(entity);
        }

        public Entity Insert<Entity>(Entity entity) where Entity : Model.Entity.Base
        {
            return new BaseGenerics<Entity>(Context, LoggedUser).Insert(entity);
        }

        public void Delete(int id)
        {
            new BaseGenerics<T>(Context, LoggedUser).Delete(id);
        }

        public void Delete(T entity)
        {
            new BaseGenerics<T>(Context, LoggedUser).Delete(entity);
        }

        public void Delete<Entity>(int id) where Entity : Model.Entity.Base
        {
            new BaseGenerics<Entity>(Context, LoggedUser).Delete(id);
        }

        public void Delete<Entity>(Entity entity) where Entity : Model.Entity.Base
        {
            new BaseGenerics<Entity>(Context, LoggedUser).Delete(entity);
        }

        public T Update(T entity)
        {
            return new BaseGenerics<T>(Context, LoggedUser).Update(entity);
        }


        public Entity Update<Entity>(Entity entity) where Entity : Model.Entity.Base
        {
            return new BaseGenerics<Entity>(Context, LoggedUser).Update(entity);
        }
    }

    public class BaseGenerics<T> where T : Model.Entity.Base
    {

        public LoggedUser LoggedUser { get; }
        public DebtsContext Context { get; }

        public BaseGenerics(DebtsContext context, LoggedUser loggedUser)
        {
            Context = context;
            LoggedUser = loggedUser;
        }


        public T GetById(int id, bool includeDeleted = false)
        {
            T entity = null;
            if (includeDeleted)
            {
                entity = Context.Set<T>().SingleOrDefault(x => x.Id == id);
            }
            else
            {
                entity = Context.Set<T>().SingleOrDefault(x => x.Id == id && !x.DeleteDate.HasValue);
            }
            return entity;
        }

        public bool Exists(int id, bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return Context.Set<T>().Any(x => x.Id == id);
            }
            else
            {
                return Context.Set<T>().Any(x => x.Id == id && !x.DeleteDate.HasValue);
            }
        }

        public T Insert(T entity)
        {
            entity.CreationDate = DateTime.Now;
            entity.CreationUserId = LoggedUser.Id;

            Context.Add<T>(entity);
            Context.SaveChanges();

            return entity;
        }

        public void Delete(int id)
        {
            var entity = Context.Find<T>(id);
            this.Delete(entity);
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new Exception("The entity does not exists");
            }

            if (entity.DeleteDate.HasValue)
            {
                return;
            }

            entity.DeleteDate = DateTime.Now;
            entity.DeleteUserId = LoggedUser.Id;
            Context.SaveChanges();
        }

        public T Update(T entity)
        {
            if (entity == null || entity.DeleteDate.HasValue)
            {
                throw new Exception("The entity does not exists");
            }

            entity.ModificationDate = DateTime.Now;
            entity.ModificationUserId = LoggedUser.Id;
            Context.SaveChanges();
            return entity;
        }

    }
}