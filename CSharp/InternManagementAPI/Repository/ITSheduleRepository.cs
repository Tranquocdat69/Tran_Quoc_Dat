using InternManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternManagementAPI.Repository
{
    public interface ITSheduleRepository
    {
        List<TSchedule> GetAllSchedules();
        TSchedule GetScheduleById(int id);
        void AddSchedule(TSchedule entity);
        void UpdateSchedule(TSchedule entity);
        void DeleteSchedule(TSchedule entity);
    }
}
