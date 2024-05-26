using Backend.DTOs;
using Backend.Models;
using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Backend.DTOs.Sent;
using backend.Helpers;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controller
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TaskController(IDataRepository<Tasks> taskRepo, IDataRepository<AssignedTasks> AssignedTaskRepo, IDataRepository<Users> userRepo) : ControllerBase
    {
        private readonly IDataRepository<Tasks> _taskRepo = taskRepo;
        private readonly IDataRepository<AssignedTasks> _assignedTasksRepo = AssignedTaskRepo;
        private readonly IDataRepository<Users> _usersRepo = userRepo;


        //CRETE
        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskDto task)
        {
            if (task == null)
                return BadRequest("A Task is required");

            Tasks tasks = new()
            {
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                ProjectId = task.ProjectId,
            };

            await _taskRepo.AddAsync(tasks);
            await _taskRepo.Save();

            return Ok();
        }

        //READ
        [HttpGet]
        public async Task<JsonResult> GetTasks(int userId, int role, int projectId)
        {
            var tasks = await _taskRepo.GetAllTasksAsync(projectId);
            if (tasks == null)
                return new JsonResult(NotFound());

            List<TaskDto> taskToSent = [];
            foreach (var task in tasks)
            {
                taskToSent.Add(new()
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Status = task.Status,
                    Editable = role == 2
                });
            }
            if (role == 2) //Team Leader Tasks
                return new JsonResult(Ok(taskToSent));

            else if (role == 1) //Developer Tasks
            {
                var userTasks = _usersRepo.GetAllDevTasksAsync(userId);
                await foreach (var userTask in userTasks) //Loop over the assigned users
                {
                    foreach (var taskAssignedToUser in userTask?.AssignedTasks ?? []) //get the assigned tasks data corresponding to a the user
                    {
                        foreach (var task in taskToSent) //Check the task details
                        {
                            if (taskAssignedToUser.TaskId == task.Id) //checks if the task is assigned to the user
                            {
                                task.Editable = true; //To let the corresponding user edit the tasks that is assigned to him only
                            }
                        }
                    }
                }
                return new JsonResult(Ok(taskToSent));
            }

            return new JsonResult(BadRequest());
        }

        //UPDATE
        [HttpPut]
        public async Task<IActionResult> UpdateTask(CreateTaskDto updatedTask)
        {
            if (updatedTask == null)
                return BadRequest(); // Return 400 Bad Request if the request body is null or if the IDs don't match

            var taskToUpdate = await _taskRepo.GetByIdAsync(updatedTask.Id ?? 0); //null-coalescing operator to provide a default value

            if (taskToUpdate == null)
                return NotFound(); // Return 404 if project with given ID is not found

            // Update the properties of the Task
            taskToUpdate.Title = updatedTask.Title;
            taskToUpdate.Status = updatedTask.Status;
            taskToUpdate.Description = updatedTask.Description;
            taskToUpdate.ProjectId = updatedTask.ProjectId;

            _taskRepo.Update(taskToUpdate);
            await _taskRepo.Save();

            return Ok();
        }

        //DELETE
        [HttpDelete]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var taskToDelete = await _taskRepo.GetByIdAsync(id);

            if (taskToDelete == null)
            {
                return NotFound();
            }

            _taskRepo.Delete(taskToDelete);
            await _taskRepo.Save();

            return Ok();
        }



        // Assigned Tasks
        [HttpPost]
        public async Task<IActionResult> AssignTask(AssignedTaskDto taskDetails)
        {
            AssignedTasks assignedTask = new()
            {
                UserId = taskDetails.UserId,
                TaskId = taskDetails.TaskId,

                Attachments = taskDetails.Attachments
            };

            await _assignedTasksRepo.AddAsync(assignedTask);
            try
            {

                await _assignedTasksRepo.Save();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException) //To check if the user is already assigned to the task
            {
                return BadRequest("User Already Assigned");
            }

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAssignedTask(int userId, int taskId)
        {
            var taskToDelete = await _assignedTasksRepo.GetByIdAsync(userId, taskId);

            if (taskToDelete == null)
                return NotFound();

            _assignedTasksRepo.Delete(taskToDelete);
            await _assignedTasksRepo.Save();

            return Ok();

        }


        //Assigned Tasks with users
        [HttpGet]
        public async Task<JsonResult> GetAssignedDevs(int taskId)
        {
            var assignedUsers = await _usersRepo.GetAllAssignedDevsAsync();

            List<UserDto> userToSent = [];

            foreach (var user in assignedUsers)
            {
                if (user.AssignedTasks.Any(x => x.TaskId == taskId))
                {

                    userToSent.Add(new()
                    {
                        Id = user.Id,
                        UserName = user.FirstName + " " + user.LastName
                    });
                }
            }

            return new JsonResult(Ok(userToSent));
        }

        [HttpGet]
        public async Task<JsonResult> GetUnassignedDevs(int taskId)
        {
            var unAssignedUsers = await _usersRepo.GetAllUnassignedDevs();
            List<UserDto> userToSent = [];

            foreach (var user in unAssignedUsers)
            {
                if (await _assignedTasksRepo.GetByIdAsync(user.Id, taskId) == null)
                {

                    if (user.AssignedProjects.Any(x => x.Status == "accepted" && x.Projects.Tasks.Any(y => y.Id == taskId)))
                    {
                        userToSent.Add(new()
                        {
                            Id = user.Id,
                            UserName = user.FirstName + " " + user.LastName
                        });
                    }
                }

            }

            return new JsonResult(Ok(userToSent));
        }


        // Attachments Specific APIs
        [HttpPost]
        public async Task<IActionResult> AttachFile([FromForm] AssignAttachmentDto TaskDto)
        {
            if (TaskDto == null)
            {
                return BadRequest(TaskDto);
            }


            var newTask = await _assignedTasksRepo.GetByIdAsync(TaskDto.UserId, TaskDto.TaskId);

            if (TaskDto.File != null)
            {
                var result = UploadHandler.Upload(TaskDto.File, "AssignedTasks");
                if (!string.IsNullOrEmpty(result.ErrorMessage))
                {
                    return BadRequest(new
                    {
                        message = result.ErrorMessage
                    });
                }

                newTask.Attachments = result.FileName;


                _assignedTasksRepo.Update(newTask);
                await _assignedTasksRepo.Save();

                return Ok(new
                {
                    message = "Task is created"
                });

            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<JsonResult> GetAllAttachedTasks()
        {
            var baseUrl = "http://localhost:5164/";

            var tasks = await _assignedTasksRepo.FileGetter();
            foreach (var task in tasks)
            {
                if (!task.Attachments.IsNullOrEmpty())
                    task.Attachments = baseUrl + task.Attachments;
            }
            return new JsonResult(Ok(tasks));
        }

    }
}