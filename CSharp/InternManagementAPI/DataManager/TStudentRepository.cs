using InternManagementAPI.Models;
using InternManagementAPI.Repository;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternManagementAPI.DataManager
{
    public class TStudentRepository : ITStudentRepository
    {
        private readonly TrainingContext _db;
        public TStudentRepository(TrainingContext db)
        {
           _db = db;
        }
        public void UpsertStudent(TStudents entity)
        {
            _db.Database.ExecuteSqlRaw("spStudentUpdate {0},{1},{2}", entity.AUsername, entity.AFullName, entity.AEmail);
        }

        public void DeleteStudent(int id)
        {
            _db.Database.ExecuteSqlRaw("spStudentDelete {0}", id);
        }

        public List<TStudents> GetAllStudents()
        {
            return _db.TStudents.OrderByDescending(s=>s.ACreatedDate).ToList();
        }

        public List<TStudents> GetStudentsByUserNameOrEmailOrFullName(string username, string email, string fullname)
        {
            string StoredProc = " exec spStudentSelect "+ "@pUsername = '" + username + "'," + "@pEmail = '" + email + "'," + "@pFullName = N'" + fullname + "'";
            return _db.TStudents.FromSqlRaw(StoredProc).ToList();
        }

        public TStudents GetStudentById(int id)
        {
            return _db.TStudents.Find(id);
        }

        public void UpdateStudent(int id, TStudents entity)
        {
            //var local = _db.Set<TStudents>().Local.FirstOrDefault(entry => entry.AStudentId.Equals(id));
            //// check if local is not null 
            //if (local != null)
            //{
            //    // detach
            //    _db.Entry(local).State = EntityState.Detached;
            //}
            //_db.Entry(entity).State = EntityState.Modified;
            var updateEntity = _db.TStudents.Where(u=>u.AStudentId == id).FirstOrDefault<TStudents>();
            if (updateEntity != null)
            {
                updateEntity.AUsername = entity.AUsername;
                updateEntity.AFullName = entity.AFullName;
                updateEntity.AEmail = entity.AEmail;
                _db.SaveChanges();
            }
            //_db.TStudents.Update(updateEntity);
        }
    }
}
