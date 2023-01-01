using System.Net;
using DemoWebApi.Common;
using DemoWebApi_BAL.Services;
using DemoWebApi_DAL.Model;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApi.Controllers
{
    [ApiController]
    [Route("api/v1/Projects/[controller]")]
    public class TasksController : Controller
    {
        private readonly ServiceTrackerManager _manager;
        public TasksController(ServiceTrackerManager manager)
        {
            _manager = manager;
        }


        /// <summary>
        ///Get all tasks without Projects.
        /// </summary>
        [HttpGet]
        
        public async Task<ActionResult> GetAllTask()
        {
            try
            {
                var task = await _manager.AllTasks();
                if (task is null)
                {
                    return NotFound(new Response
                        { Status = HttpStatusCode.NotFound, Message = "Doesn't have any tasks yet" });
                }
                return Ok(new Response { Status = HttpStatusCode.OK, Message = "Success", Data = task });
            }
            catch (Exception e)
            {
                return BadRequest(new Response { Status = HttpStatusCode.BadRequest, Message = e.Message });
            }

        }
        /// <summary>
        /// Get specific Task with bind Project.
        /// </summary>
        ///  <param name="id">Task Id</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                if (await _manager.AllTasks() is null)
                {
                    return NotFound(new Response
                        { Status = HttpStatusCode.NotFound, Message = "Doesn't have any tasks yet" });
                }
                var task = await _manager.TaskById(id);
                if (task == null)
                {
                    return NotFound(new Response
                        { Status = HttpStatusCode.NotFound, Message = $"Doesn't have task with Id = {id}" });
                }

                if (task.Project.Tasks is not null)
                {
                    task.Project.Tasks = null;
                }
                return Ok(new Response { Status = HttpStatusCode.OK, Message = "Success", Data = task });

            }
            catch (Exception e)
            {
                
                return BadRequest(new Response { Status = HttpStatusCode.BadRequest, Message = e.Message });

            }
        }
        /// <summary>
        /// Deletes a specific Task.
        /// </summary>
        /// <param name="id">Task Id</param>
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var task = await _manager.TaskById(id);

                if (task is null)
                {
                    return NotFound(new Response
                        { Status = HttpStatusCode.NotFound, Message = $"Doesn't have task with Id = {id}" });
                }
                _manager.DeleteTask(task);
                return Ok(new Response { Status = HttpStatusCode.OK, Message = "Success", Data =task });
            }
            catch (Exception e)
            {
                return BadRequest(new Response { Status = HttpStatusCode.BadRequest, Message = e.Message });
            }

        }
        /// <summary>
        /// Create a new Task.
        /// </summary>
        [HttpPost]

        public async Task<ActionResult> CreateTask(int projectId,[FromBody] TaskRequestArg arg )
        {
            try
            {
                var IsExist = await _manager.ProjectById(projectId);
                if (IsExist is null)
                {
                    return NotFound(new Response
                        { Status = HttpStatusCode.NotFound, Message = $"Doesn't have project with Id = {projectId}" });
                }
                EntityTask task = new EntityTask
                    {   
                        Name = arg.Name,
                        Description = arg.Description,
                        Priority = (int)arg.Priority,
                        ProjectID = projectId,
                        Status = (StatusTask)arg.Status
                    };
                
                await _manager.AddTask(task);
                return Created(new Uri(Request.GetEncodedUrl() + "/" + task.ID), new Response
                {
                    Status = HttpStatusCode.Created,
                    Message = "Success",
                    Data = task
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Response { Status = HttpStatusCode.BadRequest, Message = e.Message });
            }
        }
        /// <summary>
        /// Edit a specific Task.
        /// </summary>
        /// <param name="id">Task Id</param>
        [HttpPut("Edit/{id}")]

        public async Task<IActionResult> EditTask(int id, [FromBody] TaskRequestArg args)
        {
            try

            {
                var task = await _manager.TaskById(id);
                if (task is null)
                {
                    return NotFound(new Response
                        { Status = HttpStatusCode.NotFound, Message = $"Doesn't have task with Id = {id}" });
                }
                task.Priority = args.Priority ?? task.Priority;
                task.Description=args.Description ?? task.Description;
                task.Name = args.Name ?? task.Name;
                task.Status = args.Status ?? task.Status;

                await _manager.UpdateTask(task);
                return Ok(new Response { Status = HttpStatusCode.OK, Message = "Success", Data = task });
            }
            catch (Exception e)
            {
               

                return BadRequest(new Response { Status = HttpStatusCode.BadRequest, Message = e.Message });

            }

            
        }
        /// <summary>
        /// Assign Task to new Project.
        /// </summary>
        /// <param name="id">Task Id</param>
        /// <param name="projectId">Target Project Id</param>

        [HttpPut("Edit/dir/{id}")]

        public async Task<IActionResult> ChangeTaskDir(int id, int projectId)
        {
            try

            {
                var task = await _manager.MoveTask(id, projectId);
                if (task is null)
                {
                    return NotFound(new Response
                        { Status = HttpStatusCode.NotFound, Message = $"Doesn't have task with Id = {id}" });
                }
                return Ok(new Response { Status = HttpStatusCode.OK, Message = "Success", Data = task });
            }
            catch (Exception e)
            {


                return BadRequest(new Response { Status = HttpStatusCode.BadRequest, Message = e.Message });

            }


        }


    }
}
