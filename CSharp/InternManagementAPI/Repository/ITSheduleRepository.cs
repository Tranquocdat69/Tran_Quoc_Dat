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
        List<TSchedule> GetScheduleByUsername(string username);
        bool UpsertSchedule(TSchedule entity);
        bool UpdateSchedule(int id, TSchedule entity);
        void DeleteSchedule(int id);
    }
}
