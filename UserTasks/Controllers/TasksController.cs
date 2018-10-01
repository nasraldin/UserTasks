using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using UserTasks.Core.Entities;
using UserTasks.Core.Uow.Interfaces;
using UserTasks.ViewModels;

namespace UserTasks.Controllers
{
    //[Route("api/[controller]")]
    //[Authorize]
    public class TasksController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TasksController> _logger;

        public TasksController(IUnitOfWork unitOfWork, ILogger<TasksController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: api/tasks/get
        [HttpGet("[action]")]
        [Route("api/Tasks/Get")]
        public IActionResult Get()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskItems = _unitOfWork.TaskItems.GetAll();

            if (taskItems == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<IEnumerable<TaskItemViewModel>>(taskItems));
        }

        //// GET: api/TaskItems/5
        //[HttpGet("{id}")]
        //public IActionResult Get([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var taskItem = _unitOfWork.TaskItems.GetTask(id);

        //    if (taskItem == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(Mapper.Map<TaskItemViewModel>(taskItem));
        //}

        // POST: api/TaskItems
        [HttpGet("[action]")]
        [Route("api/Tasks/Post")]
        public IActionResult Post([FromBody] TaskItem taskItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.TaskItems.CreateTask(taskItem);
            _unitOfWork.SaveChanges();
            _logger.LogInformation("User Add New Task: " + taskItem.Task);
            return Ok();
        }


        // PUT: api/TaskItems/5
        [HttpPut("[action]")]
        [Route("api/Tasks/Put")]
        public IActionResult Put([FromRoute] int id, [FromBody] TaskItem taskItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != taskItem.Id)
            {
                return BadRequest();
            }

            _unitOfWork.TaskItems.UpdateTask(taskItem);

            try
            {
                _unitOfWork.SaveChanges();
                _logger.LogInformation("User Update Task: " + taskItem.Task);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskItemExists(id))
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

        [HttpPut("[action]")]
        [Route("api/Tasks/AssignTask")]
        public IActionResult AssignTask([FromBody] string[] prams)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (prams == null || prams.Length == 0)
            {
                return NotFound();
            }

            var id = Convert.ToInt32(prams[0]);
            var userId = Convert.ToInt32(prams[1]);

            _unitOfWork.TaskItems.AssignTask(id, userId);

            try
            {
                _unitOfWork.SaveChanges();
                _logger.LogInformation("Assign task to new user: " + userId);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskItemExists(id))
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

        [HttpPut("[action]")]
        [Route("api/Tasks/TaskDone")]
        public IActionResult TaskDone([FromBody] int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var chickTaskDone = _unitOfWork.TaskItems.GetById(id.Value);

            if (!chickTaskDone.IsDone)
            {
                _unitOfWork.TaskItems.TaskDone(id.Value);
                _unitOfWork.SaveChanges();
                _logger.LogInformation("User Done Task: " + id.Value);
            }
            else
            {
                _logger.LogInformation("Task Already Done: " + id.Value);
            }

            return NoContent();
        }

        // DELETE: api/TaskItems/5
        [HttpDelete]
        [Route("api/Tasks/Delete/{id}")]
        public IActionResult Delete([FromRoute] int? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!id.HasValue)
            {
                return NotFound();
            }

            var taskItem = _unitOfWork.TaskItems.GetById(id.Value);
            if (taskItem == null)
            {
                return NotFound();
            }

            _unitOfWork.TaskItems.DeleteTask(taskItem.Id);
            _unitOfWork.SaveChanges();
            _logger.LogInformation("User Delete Task: " + taskItem.Task);
            return Ok(taskItem);
        }

        private bool TaskItemExists(int id)
        {
            return _unitOfWork.TaskItems.Any(id);
        }
    }
}