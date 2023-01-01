using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using DemoWebApi_BAL.Extension;
using DemoWebApi_DAL.Interface;
using DemoWebApi_DAL.Model;
using DemoWebApi_DAL.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApi_BAL.Services
{
    public class ServiceTrackerManager
    {
        private readonly IRepository<EntityTask> _taskRepository;
        private readonly IRepository<EntityProject> _projectRepository;

        public ServiceTrackerManager(IRepository<EntityTask> taskRepository, IRepository<EntityProject> projectRepository)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
        }

        public enum SortParam 
        {
            ById,
            ByName,
            ByStatus,
            ByPriority,
            ByStartDate
            
        }
        public async Task<List<EntityProject>?>AllProjects(StatusProject? status, SortParam? sortParam, bool? isDescending, int? priorityFilter, bool? isGreater)
        {
            var projects = _projectRepository
                .GetAll()
                .WhereIf(status is not null, x => x.Status == status)
                .WhereIf(priorityFilter is not null && isGreater is true, x => x.Priority>= priorityFilter)
                .WhereIf(priorityFilter is not null && isGreater is false, x => x.Priority <= priorityFilter);

            //I don't like this filtering implementation. It seems to me that there should be a more smooth solution that would not break the queue.
            //I was thinking about building a lambda expression tree. I thought that sophisticated for this case. Anyway feel free to contact me and share you thoughts at @tg0vk on tg/vk   

            projects = sortParam switch
            {
                SortParam.ById =>
                    projects.OrderByIf(isDescending, x => x.ID),
                SortParam.ByName =>
                    projects.OrderByIf(isDescending, x => x.Name),
                SortParam.ByStatus =>
                    projects.OrderByIf(isDescending, x => x.Status),
                SortParam.ByPriority =>
                    projects.OrderByIf(isDescending, x => x.Priority),
                SortParam.ByStartDate =>
                    projects.OrderByIf(isDescending, x => x.StartDate),
                _ => projects
            }; 
            
            return await projects.ToListAsync();
        }
        public async Task<List<EntityTask>?> AllTasks()
        {
            try
            {
                return await _taskRepository
                    .GetAll()
                    .ToListAsync();
            }
            //At this application level, need to do implement handling BL error
            catch (Exception e)
            {
                
                throw;
            }
        }

        public async Task<EntityTask?> AddTask(EntityTask task)
        {

            try
            {
                await _taskRepository.Create(task);

                if (task.Project.Tasks is not null)
                {
                    task.Project.Tasks = null;
                }
                return task;

            }
            catch (Exception e)
            {
                throw;
            }


        }
        public async Task<EntityProject?> AddProject(EntityProject project)
        {
            
            try
            {
                return await _projectRepository.Create(project);
            }
            catch (Exception e)
            {
                throw;
            }
           

        }
        

        public async Task<EntityProject?> ProjectById(int id)
        {
            try
            { 
                
                return await _projectRepository.GetById(id); 

            }
            catch (Exception e)
            {
                
                throw;
            }
        }
        public async Task<EntityTask?> TaskById(int id)
        {
            try
            { var task = await _taskRepository.GetById(id);

               
                return task;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<EntityProject?> UpdateProject( EntityProject project)
        {
            try
            {
                
                project.CompletionDate = project.Status switch
                {
                    StatusProject.Completed => DateTime.Now,
                    > StatusProject.Completed or < StatusProject.NotStarted => throw new Exception("Invalid Project Status"),
                    _ => project.CompletionDate
                };

                await _projectRepository.Update(project);
                return project;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<EntityTask?> UpdateTask(EntityTask task)
        {
            try
            {
                
                if (task.Status < StatusTask.ToDo || task.Status > StatusTask.Done)
                {
                    throw new Exception("Invalid Project Status");
                }

                await _taskRepository.Update(task);
                if (task.Project.Tasks is not null)
                {
                    task.Project.Tasks = null;
                }
                return task;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        //At this application level, need to do implement handling BL error
        public async Task<EntityTask?> MoveTask(int id, int projectId)
        {
            try
            {
                var projectIsExist = await ProjectById(projectId);
                if (projectIsExist is null)
                { throw new Exception("Target project doesn't exist");
                }

                var task = await TaskById(id);
                if (task is null)
                {
                    return null;
                } 
                task.ProjectID = projectId;
                await UpdateTask(task);
                return task;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public void DeleteProject(EntityProject project)
        {
            try
            {
                 _projectRepository.Delete(project);
            }
            catch (Exception e)
            {
                
                throw;
            }
        }

        public void DeleteTask(EntityTask task)
        {
            try
            {
                _taskRepository.Delete(task);
                if (task.Project.Tasks is not null)
                {
                    task.Project.Tasks = null;
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
