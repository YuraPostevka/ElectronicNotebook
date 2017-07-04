using BAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using DAL.Interfaces;
using System.Data.SqlClient;
using System.Configuration;
using Models.DTO;

namespace BAL.Managers
{
    public class TagManager : BaseManager, ITagManager
    {
        public TagManager(IUnitOfWork uOW) : base(uOW)
        {
        }

        public void Delete(string name, int listId)
        {
            if (name == null) return;
            var tagId = GetByName(name).Id;

            if (tagId == 0) return;

            var taglistdb = uOW.ToDoListsTagsRepo.All.Where(i => i.TagId == tagId && i.ToDoListId == listId).FirstOrDefault();
            if (taglistdb == null) return;
            uOW.ToDoListsTagsRepo.Delete(taglistdb);

            uOW.Save();
        }

        public List<Tag> GetAll()
        {
            return uOW.TagRepo.All.ToList();
        }

        public Tag Insert(Tag tag, int toDoListId)
        {
            //check if name is exist in db
            var tagDb = uOW.TagRepo.All.Where(n => n.Name == tag.Name).FirstOrDefault();

            if (tagDb != null)
            {
                return tagDb;
            }
            else
            {
                //insert new tag
                uOW.TagRepo.Insert(tag);
                uOW.Save();
            }

            return tag;
        }

        public void AttachToList(string tag, int toDoListId)
        {
            var Tag = new Tag()
            {
                Name = tag
            };

            // inserted/returned tag
            var tagDb = Insert(Tag, toDoListId);
            //check if relationship is exist
            var tagsLists = uOW.ToDoListsTagsRepo.All.FirstOrDefault(i => i.TagId == tagDb.Id && i.ToDoListId == toDoListId);
            //if true, return
            if (tagsLists != null) return;

            //else, insert new relationship
            else
            {
                uOW.ToDoListsTagsRepo.Insert(new ToDoListsTags() { TagId = tagDb.Id, ToDoListId = toDoListId });
                uOW.Save();
            }

        }

        public Tag GetByName(string name)
        {
            if (name == null) return null;
            var tag = uOW.TagRepo.All.FirstOrDefault(n => n.Name == name);
            return tag;
        }
        public List<TopTags> GetTop10()
        {
            var result = new List<TopTags>();
            using (SqlConnection conn = new SqlConnection(@"Data Source=CH976\SQLEXPRESS;Initial Catalog=Schedules;User ID=yourdrevm;Password=qwerty"))
            {
                conn.Open();
                var query = @"select top 10 ToDoListsTags.TagId, COUNT(*) as CountOf, Tags.Name
                            from ToDoListsTags

                             INNER JOIN Tags
                              on ToDoListsTags.TagId = Tags.Id


                            group by TagId, Tags.Name
                            order by COUNT(*) DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var tag = new TopTags();

                        tag.Count = reader.GetInt32(1);
                        tag.Name = reader.GetString(2);

                        result.Add(tag);
                    }
                }
            }
            return result;
        }
    }
}
