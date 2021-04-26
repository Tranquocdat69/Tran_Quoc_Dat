using InternManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternManagementAPI.Repository
{
    public interface ITStudentRepository
    {
        List<TStudents> GetAllStudents();
        TStudents GetStudentById(int id);
        void AddStudent(TStudents entity);
        void UpdateStudent(TStudents entity);
        void DeleteStudent(TStudents entity);
    }

}
