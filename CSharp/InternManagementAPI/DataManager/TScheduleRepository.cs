using InternManagementAPI.Models;
using InternManagementAPI.Repository;
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
        public void AddSchedule(TSchedule entity)
        {
            _db.TSchedules.Add(entity);
            _db.SaveChanges();
        }

        public void DeleteSchedule(TSchedule entity)
        {
            _db.TSchedules.Remove(entity);
            _db.SaveChanges();
        }

        public List<TSchedule> GetAllSchedules()
        {
            return _db.TSchedules.ToList();
        }

        public TSchedule GetScheduleById(int id)
        {
            return _db.TSchedules.Find(id);
        }

        public void UpdateSchedule(TSchedule entity)
        {
            _db.TSchedules.Update(entity);
            _db.SaveChanges();
        }
    }
}
