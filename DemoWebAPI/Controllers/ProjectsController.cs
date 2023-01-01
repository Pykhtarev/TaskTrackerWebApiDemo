using System.Net;
using System.Threading.Tasks;
using DemoWebApi.Common;
using DemoWebApi_BAL.Services;
using DemoWebApi_DAL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static DemoWebApi_BAL.Services.ServiceTrackerManager;

namespace DemoWebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProjectsController : Controller
    {
        private readonly ServiceTrackerManager _manager;

        public ProjectsController(ServiceTrackerManager manager)
        {
            _manager = manager;
        }
        /// <summary>
        ///Return list all projects without tasks
        /// </summary>
        ///  <param name="status">Filter by status project where 0 is Not Started, 1 is Active, 2 is Completed</param>
        /// <param name="sortParam">Parameters for sorting by Project field where By Id = 0,ByName = 1,By Status = 2,By Priority = 3,By Start Date = 4</param>
        ///  <param name="isDescending">Flag indicate sort param is descending or not</param>
        ///  <param name="isGreater">Flag indicate condition param for filter by priority</param>
        ///  <param name="priorityFilter">Filter by priority</param>
        [HttpGet]
        public async Task<ActionResult> GetAllProject(StatusProject? status, SortParam? sortParam, bool isDescending,
            int? priorityFilter, bool? isGreater)
        {
            try
            {
                var project = await  _manager.AllProjects(status, sortParam, isDescending, priorityFilter, isGreater);
                if (project is null)
                {
                    return NotFound(new Response
                        { Status = HttpStatusCode.NotFound, Message = "Doesn't have any projects yet" });
                }
                return Ok(new Response { Status = HttpStatusCode.OK, Message = "Success", Data = project });
            }
            catch (Exception e)
            {
                return BadRequest(new Response { Status = HttpStatusCode.BadRequest, Message = e.Message });
            }

        }

        /// <summary>
        /// Return a specific Project with tasks.
        /// </summary>
        ///  <param name="id">Project Id</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            try
            {
                var project = await _manager.ProjectById(id);
                if (project == null)
                {
                    return NotFound(new Response
                        { Status = HttpStatusCode.NotFound, Message = $"Doesn't have project with Id = {id}" });
                }

                return Ok(new Response { Status = HttpStatusCode.OK, Message = "Success", Data = project });

            }
            catch (Exception e)
            {
                if (await _manager.AllProjects(null, null, null, null, null) is null)
                {
                    return NotFound(new Response
                        { Status = HttpStatusCode.NotFound, Message = "Doesn't have any projects yet" });
                }
                return BadRequest(new Response { Status = HttpStatusCode.BadRequest, Message = e.Message });

            }
        }



        /// <summary>
        /// Create new Project.
        /// </summary>
        [HttpPost]

        public async Task<ActionResult> CreateProject([FromBody] EntityProject project)
        {
            try
            {
                await _manager.AddProject(project);
                return Created(new Uri(Request.GetEncodedUrl() + "/" + project.ID), new Response
                {
                    Status = HttpStatusCode.Created, Message = "Success", Data = project
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Response { Status = HttpStatusCode.BadRequest, Message = e.Message });
            }
        }
        /// <summary>
        /// Edit specific Project.
        /// </summary>
        ///  <param name="id">Project Id</param>
        [HttpPut("Edit/{id}")]

        public async Task<IActionResult> EditProject(int id, [FromBody]  ProjectRequestArgs args)
        {
            try
            {
                var project = await _manager.ProjectById(id);

                if (project is null)
                {
                    return NotFound(new Response
                        { Status = HttpStatusCode.NotFound, Message = $"Doesn't have project with Id = {id}" });
                }
                project.Status= args.Status ?? project.Status ;
                project.Name = args.Name ?? project.Name;
                project.Priority = args.Priority ?? project.Priority;
                project.Description = args.Description?? project.Description;
                project.StartDate = args.StartDate ?? project.StartDate;

                await _manager.UpdateProject(project);
                project.Tasks = null;
                return Ok(new Response { Status = HttpStatusCode.OK, Message = "Success", Data = project });
            }
            catch (Exception e)
            {

                return BadRequest(new Response { Status = HttpStatusCode.BadRequest, Message = e.Message });

            }

            
            
        }
        /// <summary>
        /// Deletes a specific Project.
        /// </summary>
        ///  <param name="id">Project Id</param>
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var project = await _manager.ProjectById(id);

                if (project is null)
                {
                    return NotFound(new Response
                        { Status = HttpStatusCode.NotFound, Message = $"Doesn't have project with Id = {id}" });
                }
                _manager.DeleteProject(project);

            
                return Ok(new Response { Status = HttpStatusCode.OK, Message = "Success", Data = project });
            }
            catch (Exception e)
            {
               return BadRequest(new Response { Status = HttpStatusCode.BadRequest, Message = e.Message });
            }
           
        }
    }
}
