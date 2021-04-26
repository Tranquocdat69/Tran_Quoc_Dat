using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using InternManagementAPI.Models;
using InternManagementAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TScheduleController : ControllerBase
    {
        private readonly ITSheduleRepository _repositorySchedule;
        public TScheduleController(ITSheduleRepository repositorySchedule)
        {
            _repositorySchedule = repositorySchedule;
        }
        
        [HttpGet]
        [Route("schedules")]
        public IActionResult GetAll()
        {
            var result = _repositorySchedule.GetAllSchedules();
            return Ok(result);
        }

        //[HttpGet("{id}", Name = "GetScheduleDetail")]
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
        [HttpPost]
        [Route("addSchedule")]
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
            _repositorySchedule.AddSchedule(schedule);

            return CreatedAtAction("GetById", new { id = schedule.AScheduleId }, schedule);
            //return CreatedAtRoute("GetScheduleDetail", new { id = schedule.AScheduleId }, schedule);
            //return NoContent();
        }
        [HttpPut]
        [Route("updateSchedule/{id}")]
        public IActionResult Update(int id, [FromBody]TSchedule schedule)
        {
            if (id != schedule.AScheduleId)
            {
                return BadRequest("Schedule is null");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                _repositorySchedule.UpdateSchedule(schedule);
            }
            catch (DBConcurrencyException)
            {
                if (_repositorySchedule.GetScheduleById(id) == null)
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }

        [HttpDelete]
        [Route("deleteSchedule/{id}")]
        public IActionResult Delete(int id)
        {
            var scheduleDelete = _repositorySchedule.GetScheduleById(id);
            if (scheduleDelete == null)
            {
                return NotFound();
            }
            _repositorySchedule.DeleteSchedule(scheduleDelete);
            return NoContent();
        }
    }
}