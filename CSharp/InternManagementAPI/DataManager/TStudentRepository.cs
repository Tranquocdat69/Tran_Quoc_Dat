using InternManagementAPI.Models;
using InternManagementAPI.Repository;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        public void AddStudent(TStudents entity)
        {
            _db.TStudents.Add(entity);
            _db.SaveChanges();
        }

        public void DeleteStudent(TStudents entity)
        {
            _db.TStudents.Remove(entity);
            _db.SaveChanges();
        }

        public List<TStudents> GetAllStudents()
        {
            return _db.TStudents.ToList();
        }

        public TStudents GetStudentById(int id)
        {
            return _db.TStudents.Find(id);
        }

        public void UpdateStudent(TStudents entity)
        {
            _db.TStudents.Update(entity);
            _db.SaveChanges();
        }
    }
}
