using BAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using DAL.Interfaces;
using Models.DTO;
using AutoMapper;



namespace BAL.Managers
{
    public class ToDoListManager : BaseManager, IToDoListManager
    {
        public ToDoListManager(IUnitOfWork uOW)
            : base(uOW) { }

        /// <summary>
        /// Delete todo list
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int? id)
        {
            if (id == null) return;
            try
            {
                var item = new ToDoList() { Id = id.Value };
                uOW.ToDoListRepo.Delete(item);
                uOW.Save();
            }
            catch { }
        }

        /// <summary>
        /// Get all todo lists
        /// </summary>
        /// <returns></returns>
        public List<ListTagDTO> GetAll()
        {
            var result = new List<ListTagDTO>();
            List<ToDoList> toDoLists = uOW.ToDoListRepo.All.ToList();


            foreach (var list in toDoLists)
            {
                var listTag = Mapper.Map<ListTagDTO>(list);

                //get ids of tags with list
                var ids = uOW.ToDoListsTagsRepo.All.Where(i => i.ToDoListId == list.Id).Select(i => i.TagId).ToList();
                //get tags with list
                var tags = uOW.TagRepo.All.Where(m => ids.Contains(m.Id)).ToList();
                //get items by list id

                var items = uOW.ToDoItemRepo.All.Where(i => i.ToDoList_Id == list.Id).OrderBy(c => c.IsCompleted).ToList();

                listTag.Items.AddRange(items);
                listTag.Tags.AddRange(tags);

                result.Add(listTag);
            }
            return result;
        }

        /// <summary>
        /// Get todo list by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ToDoList GetById(int? id)
        {
            if (id == null) return null;
            return uOW.ToDoListRepo.GetByID(id);
        }

        /// <summary>
        /// Insert todo list
        /// </summary>
        /// <param name="item"></param>
        public ListTagDTO Insert(ToDoList item)
        {

            if (item == null) return null;

            uOW.ToDoListRepo.Insert(item);
            uOW.Save();

            var mappedItem = Mapper.Map<ListTagDTO>(item);
            return mappedItem;
        }

        /// <summary>
        /// Update todo list
        /// </summary>
        /// <param name="item"></param>
        public void ChangeName(int id, string name)
        {
            var dbItem = uOW.ToDoListRepo.GetByID(id);
            dbItem.Name = name;

            uOW.Save();
        }
        public List<ListTagDTO> GetListsByTagName(string tagName)
        {
            var result = new List<ListTagDTO>();

            var tagId = uOW.TagRepo.All.Where(n => n.Name == tagName).Select(i => i.Id).FirstOrDefault();

            var listsIds = uOW.ToDoListsTagsRepo.All.Where(i => i.TagId == tagId).Select(i => i.ToDoListId).ToList();

            foreach (var listId in listsIds)
            {
                var list = uOW.ToDoListRepo.Get(includeProperties: "Items").Where(i => i.Id == listId).FirstOrDefault();
                var listTagDTO = Mapper.Map<ListTagDTO>(list);

                var ids = uOW.ToDoListsTagsRepo.All.Where(i => i.ToDoListId == list.Id).Select(i => i.TagId).ToList();
                var tags = uOW.TagRepo.All.Where(m => ids.Contains(m.Id)).ToList();

                listTagDTO.Tags = tags;

                result.Add(listTagDTO);
            }
            return result;
        }
    }
}

