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
        List<TStudents> GetStudentsByUserNameOrEmailOrFullName(string username, string email, string fullname);
        TStudents GetStudentById(int id);
        void UpsertStudent(TStudents entity);
        void UpdateStudent(int id, TStudents entity);
        void DeleteStudent(int id);
    }

}
