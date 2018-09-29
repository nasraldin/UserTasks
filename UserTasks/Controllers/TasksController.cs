using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using UserTasks.Core.Entities;
using UserTasks.Core.Uow.Interfaces;
using UserTasks.ViewModels;

namespace UserTasks.Controllers
{
    [Route("api/[controller]")]
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
        [HttpGet]
        public IActionResult Get()
        {
            var allTasks = _unitOfWork.TaskItems.GetAll();
            return Ok(Mapper.Map<IEnumerable<TaskItemViewModel>>(allTasks));
        }

        // GET api/tasks/5
        [HttpGet("{id}")]
        public TaskItem Get(int id)
        {
            var getTask = _unitOfWork.TaskItems.GetById(id);
            return getTask;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var getTaskItem = _unitOfWork.TaskItems.GetById(id);
            if (getTaskItem != null)
            {
                _unitOfWork.TaskItems.DeleteTask(getTaskItem.Id);
            }
        }
    }
}