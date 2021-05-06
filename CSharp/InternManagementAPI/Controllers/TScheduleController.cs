using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using InternManagementAPI.Models;
using InternManagementAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace InternManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TScheduleController : ControllerBase
    {
        private readonly ITSheduleRepository _repositorySchedule;
        private readonly TrainingContext _db;
        public TScheduleController(ITSheduleRepository repositorySchedule, TrainingContext db)
        {
            _repositorySchedule = repositorySchedule;
            _db = db;
        }

        [HttpGet]
        [Route("schedules")]
        public IActionResult GetAll()
        {
            var result = _repositorySchedule.GetAllSchedules();
            return Ok(result);
        }

        [HttpGet]
        [Route("scheduleDetail/{id}")]
        public IActionResult GetById(int id)
        {
            var result = _repositorySchedule.GetScheduleById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("scheduleUsername/{username}")]
        public IActionResult GetByUsername(string username)
        {
            var result = _repositorySchedule.GetScheduleByUsername(username);
            //var user = _db.TStudents.Single(s => s.AUsername == username);
            //List<TSchedule> result = _db.Entry(user).Collection(u => u.TSchedule).Query().ToList();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("upsertSchedule")]
        public IActionResult Add([FromBody]TSchedule schedule)
        {
            if (schedule is null)
            {
                return BadRequest("Schedule is null");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            bool checkUpsertSchedule = _repositorySchedule.UpsertSchedule(schedule);
            if (checkUpsertSchedule)
            {
                return CreatedAtAction("GetById", new { id = schedule.AScheduleId }, schedule);
            }
            else
            {
                return BadRequest();
            }
            //return CreatedAtRoute("GetScheduleDetail", new { id = schedule.AScheduleId }, schedule);
            //return NoContent();
        }
        [HttpPatch]
        [Route("updateSchedule/{id}")]
        public IActionResult Update(int id, /*[FromBody]JsonPatchDocument<TSchedule> patchSchedule*/ TSchedule schedule)
        {
            var entity = _repositorySchedule.GetScheduleById(id);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (entity == null)
            {
                return BadRequest("Schedule is null");
            }
            bool checkUpdateSchedule = _repositorySchedule.UpdateSchedule(id, schedule);
            if (checkUpdateSchedule)
            {
                return Ok(schedule);
            }
            return BadRequest();
            //patchSchedule.ApplyTo(entity, ModelState);
        }

        [HttpDelete]
        [Route("deleteSchedule/{id}")]
        public IActionResult Delete(int id)
        {
            _repositorySchedule.DeleteSchedule(id);
            return NoContent();
        }
    }
}