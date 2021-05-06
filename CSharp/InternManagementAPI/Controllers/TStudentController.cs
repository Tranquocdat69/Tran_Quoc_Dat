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
using Microsoft.EntityFrameworkCore;
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
        [Route("studentFilter")]
        public IActionResult GetStudentsFilter(string username = null,string email = null, string fullname = null)
        {
            var result = _repositoryStudent.GetStudentsByUserNameOrEmailOrFullName(username, email, fullname);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPatch]
        [Route("updateStudent/{id}")]
        public IActionResult PartialUpdate(int id, /*[FromBody]JsonPatchDocument<TStudents> patchStudent*/ [FromBody]TStudents student)
        {
            var entity = _repositoryStudent.GetStudentById(id);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (entity == null)
            {
                return BadRequest("Student is null");
            }
            //patchStudent.ApplyTo(entity, ModelState);
            _repositoryStudent.UpdateStudent(id, student);
            return Ok(student);
        }

        [HttpPost]
        [Route("upsertStudent")]
        public IActionResult Upsert([FromBody]TStudents student)
        {
            if (student is null)
            {
                return BadRequest("Student is null");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _repositoryStudent.UpsertStudent(student);
            return Ok(student);
        }

        [HttpDelete]
        [Route("deleteStudent/{id}")]
        public IActionResult Delete(int id)
        {
            _repositoryStudent.DeleteStudent(id);
            return NoContent();
        }

    }
}