using BAL.Interfaces;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.DTO;

namespace BAL.Managers
{
    public class ToDoItemManager : BaseManager, IToDoItemManager
    {
        public ToDoItemManager(IUnitOfWork uOW)
            : base(uOW)
        { }

        /// <summary>
        /// Delete todo item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int? id)
        {
            if (id == null) return;
            var item = new ToDoItem() { Id = id.Value };
            uOW.ToDoItemRepo.Delete(item);
            uOW.Save();
        }

        /// <summary>
        /// Get all todo items
        /// </summary>
        /// <returns></returns>
        public List<ToDoItem> GetAll()
        {
            return uOW.ToDoItemRepo.All.ToList();
        }

        /// <summary>
        /// Get todo item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ToDoItem GetById(int? id)
        {
            if (id == null) return null;
            return uOW.ToDoItemRepo.GetByID(id);
        }

        /// <summary>
        /// Get not completed todo items
        /// </summary>
        /// <returns></returns>
        public List<ToDoItem> GetNotCompletedItems()
        {
            return uOW.ToDoItemRepo.All.Where(n => n.IsCompleted == false).ToList();
        }

        /// <summary>
        /// Insert todo item
        /// </summary>
        /// <param name="item"></param>
        public ToDoItem Insert(ToDoItem item)
        {
            uOW.ToDoItemRepo.Insert(item);
            uOW.Save();
            return item;
        }

        /// <summary>
        /// Mark/Unmark todoItem as completed
        /// </summary>
        /// <param name="item"></param>
        public void ChangeCompletedItem(int id, bool isCompleted)
        {
            try
            {
                var dbItem = uOW.ToDoItemRepo.GetByID(id);

                dbItem.IsCompleted = isCompleted;

                uOW.Save();
            }
            catch { }
        }

        /// <summary>
        /// Update todo item
        /// </summary>
        /// <param name="item"></param>
        public void ChangeItemText(int id, string text)
        {
            try
            {
                var dbItem = uOW.ToDoItemRepo.GetByID(id);
                dbItem.Text = text;


                uOW.Save();
            }
            catch { }

        }

        public void SubmitNotify(SetNotificationDTO model)
        {
            var dbItem = uOW.ToDoItemRepo.GetByID(model.ItemId);

            dbItem.IsNotify = true;
            dbItem.NotifyTime = model.Date;

            uOW.Save();
        }

        public void DismissNotify(int itemId)
        {
            var itemDb = uOW.ToDoItemRepo.GetByID(itemId);
            itemDb.IsNotify = false;

            uOW.Save();
        }
    }
}
