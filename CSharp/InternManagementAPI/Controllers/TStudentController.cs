using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using InternManagementAPI.Models;
using InternManagementAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace InternManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TStudentController : ControllerBase
    {
        private readonly ITStudentRepository _repositoryStudent;
        public TStudentController(ITStudentRepository repositoryStudent)
        {
            _repositoryStudent = repositoryStudent;
        }

        [HttpGet]
        [Route("students")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _repositoryStudent.GetAllStudents();
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("studentDetail/{id}")]
        public IActionResult GetById(int id)
        {
            var result = _repositoryStudent.GetStudentById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("updateStudent/{id}")]
        public IActionResult Update(int id, [FromBody]TStudents student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != student.AStudentId)
            {
                return BadRequest("Student is null");
            }
            try
            {
                _repositoryStudent.UpdateStudent(student);
            }
            catch (DBConcurrencyException)
            {
                if (_repositoryStudent.GetStudentById(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }  
            }
            return NoContent();
        }

        [HttpPost]
        [Route("addStudent")]
        public IActionResult Add([FromBody]TStudents student)
        {
            if (student is null)
            {
                return BadRequest("Student is null");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _repositoryStudent.AddStudent(student);
            return NoContent();
        }

        [HttpDelete]
        [Route("deleteStudent/{id}")]
        public IActionResult Delete(int id)
        {
            var studentDelete = _repositoryStudent.GetStudentById(id);
            if (studentDelete == null)
            {
                return NotFound("Student couldn't be found");
            }
            _repositoryStudent.DeleteStudent(studentDelete);
            return NoContent();
        }

    }
}