using DAL.Interfaces;
using DAL.Repositories;
using Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private MainContext context;

        #region Private Repositories

        private IGenericRepository<User> userRepo;
        private IGenericRepository<ToDoList> toDoListRepo;
        private IGenericRepository<ToDoItem> toDoItemRepo;
        private IGenericRepository<InviteUser> inviteUserRepo;
        private IGenericRepository<Tag> tagRepo;
        private IGenericRepository<ToDoListsTags> toDoListTagRepo;

        #endregion Private Repositories

        public UnitOfWork()
        {
            context = new MainContext();

            userRepo = new GenericRepository<User>(context);
            toDoListRepo = new GenericRepository<ToDoList>(context);
            toDoItemRepo = new GenericRepository<ToDoItem>(context);
            inviteUserRepo = new GenericRepository<InviteUser>(context);
            tagRepo = new GenericRepository<Tag>(context);
            toDoListTagRepo = new GenericRepository<ToDoListsTags>(context);
        }

        #region Repositories Getters

        public IGenericRepository<User> UserRepo
        {
            get
            {
                if (userRepo == null) userRepo = new GenericRepository<User>(context);
                return userRepo;
            }
        }

        public IGenericRepository<ToDoList> ToDoListRepo
        {
            get
            {
                if (toDoListRepo == null) toDoListRepo = new GenericRepository<ToDoList>(context);
                return toDoListRepo;
            }
        }

        public IGenericRepository<ToDoItem> ToDoItemRepo
        {
            get
            {
                if (toDoItemRepo == null) toDoItemRepo = new GenericRepository<ToDoItem>(context);
                return toDoItemRepo;
            }
        }
        public IGenericRepository<InviteUser> InviteUserRepo
        {
            get
            {
                if (inviteUserRepo == null) inviteUserRepo = new GenericRepository<InviteUser>(context);
                return inviteUserRepo;
            }
        }
        public IGenericRepository<Tag> TagRepo
        {
            get
            {
                if (tagRepo == null) tagRepo = new GenericRepository<Tag>(context);
                return tagRepo;
            }
        }
        public IGenericRepository<ToDoListsTags> ToDoListsTagsRepo
        {
            get
            {
                if (toDoListTagRepo == null) toDoListTagRepo = new GenericRepository<ToDoListsTags>(context);
                return toDoListTagRepo;
            }
        }

        #endregion Repositories Getters

        public void UpdateContext()
        {
            context = new MainContext();
        }
        public int Save()
        {
            try
            {
                UpdateTrackedEntities();
                return context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                return 0;
            }
        }

        private void UpdateTrackedEntities()
        {
            var entities = context.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);

            foreach (var ent in entities)
            {
                ((BaseEntity)ent.Entity).Modified = DateTime.UtcNow;
            }
        }

        #region Dispose

        // https://msdn.microsoft.com/ru-ru/library/system.idisposable(v=vs.110).aspx

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Dispose
    }
}