using InternManagementAPI.Models;
using InternManagementAPI.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternManagementAPI.DataManager
{
    public class TScheduleRepository : ITSheduleRepository
    {
        private readonly TrainingContext _db;
        public TScheduleRepository(TrainingContext db)
        {
            _db = db;
        }
        public bool UpsertSchedule(TSchedule entity)
        {
            bool check = _db.TSchedules.Where(s => s.AStudentId == entity.AStudentId).Any(sch => sch.AAttendedDate == entity.AAttendedDate);
            if (check == false)
            {
                int insertCheck = _db.Database.ExecuteSqlRaw("exec spScheduleUpdate {0},{1},{2}", entity.AStudentId, entity.AAttendedDate, entity.ASession);
                if (insertCheck < 0)
                {
                    return true;
                }
            }
            return false;
        }

        public void DeleteSchedule(int id)
        {
            _db.Database.ExecuteSqlRaw("exec spScheduleDelete {0}", id);
        }

        public List<TSchedule> GetAllSchedules()
        {
            return _db.TSchedules.ToList();
        }

        public TSchedule GetScheduleById(int id)
        {
            return _db.TSchedules.Find(id);
        }

        public bool UpdateSchedule(int id, TSchedule entity)
        {
            TSchedule CurrentSchedule = _db.TSchedules.Find(id);
            bool check = _db.TSchedules.Where(s => s.AStudentId == entity.AStudentId & s.AAttendedDate != CurrentSchedule.AAttendedDate).Any(sch => sch.AAttendedDate == entity.AAttendedDate);
            if (check == false)
            {
                if (CurrentSchedule != null)
                {
                    CurrentSchedule.ASession = entity.ASession;
                    CurrentSchedule.AAttendedDate = entity.AAttendedDate;
                    _db.SaveChanges();
                    return true;
                }
                return false;
            }
            return false;
        }

        public List<TSchedule> GetScheduleByUsername(string username)
        {
            List<TSchedule> list = new List<TSchedule>();
            var result = _db.TSchedules.FromSqlRaw("exec spScheduleSelect {0}", username).ToList();

            foreach (var item in result)
            {
                TSchedule schedule = new TSchedule();
                schedule.AScheduleId = item.AScheduleId;
                schedule.AStudentId = item.AStudentId;
                schedule.ACreatedDate = item.ACreatedDate;
                schedule.AUpdateDate = item.AUpdateDate;
                schedule.AAttendedDate = item.AAttendedDate;
                schedule.ASession = item.ASession;
                TStudents student = _db.TStudents.Find(item.AStudentId);
                schedule.AStudent = student;
                list.Add(schedule);
            }

            return list;
        }
    }
}
