using Backend.Models;
using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Backend.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controller
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProjectController(IDataRepository<Projects> projectRepo, IDataRepository<ProjectsStatus> assignedProjectsRepo) : ControllerBase
    {
        private readonly IDataRepository<Projects> _projectRepo = projectRepo;
        private readonly IDataRepository<ProjectsStatus> _assignedProjectsRepo = assignedProjectsRepo;


        //Dev APIs
        [HttpGet]
        public async Task<JsonResult> GetPendingProjects(int userId)
        {
            return new JsonResult(Ok(await _projectRepo.GetStatusProjects(userId, "pending")));
        }

        [HttpGet]
        public async Task<JsonResult> GetAcceptedProjects(int userId)
        {
            return new JsonResult(Ok(await _projectRepo.GetStatusProjects(userId, "accepted")));
        }
        [HttpGet]
        public async Task<JsonResult> GetRejectedProjects(int userId)
        {
            return new JsonResult(Ok(await _projectRepo.GetStatusProjects(userId, "rejected")));
        }


        //Team Leader APIs
        [HttpPost]
        public async Task<IActionResult> Create(ProjectDto project)
        {
            Projects projectToAdd = new()
            {
                Title = project.Title
            };

            await _projectRepo.AddAsync(projectToAdd);
            await _projectRepo.Save();
            return Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<Projects>> GetAll()
        {
            var projects = await _projectRepo.GetAllAsync();
            return projects;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProject(int id, ProjectDto updatedProject)
        {
            if (updatedProject == null)
                return BadRequest();

            var projectToUpdate = await _projectRepo.GetByIdAsync(id);

            if (projectToUpdate == null)
                return NotFound();

            projectToUpdate.Title = updatedProject.Title;

            _projectRepo.Update(projectToUpdate);
            await _projectRepo.Save();

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var projectToDelete = await _projectRepo.GetByIdAsync(id);
            if (projectToDelete == null)
                return NotFound();
            _projectRepo.Delete(projectToDelete);
            await _projectRepo.Save();

            return Ok();
        }

        //Assigned Projects
        [HttpPost]
        public async Task<IActionResult> AssignProject(AssignedProjectDto projectToAssign)
        {
            ProjectsStatus assignedProject = new()
            {
                UserId = projectToAssign.UserId,
                ProjectId = projectToAssign.ProjectId,
                Status = projectToAssign.Status,
            };

            await _assignedProjectsRepo.AddAsync(assignedProject);
            try
            {

                await _assignedProjectsRepo.Save();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException) //To check if the user is already assigned to the task
            {
                return BadRequest("User Already Assigned");
            }
            return Ok();
        }

        [HttpGet]
        public async Task<JsonResult> GetAllAssignedProjects()
        {
            return new JsonResult(Ok(await _assignedProjectsRepo.GetAllAsync()));
        }

        //Dev API
        [HttpPut]
        public async Task<IActionResult> UpdateAssignedProject(AssignedProjectDto updateAssignedProject)
        {
            if (updateAssignedProject == null)
                return BadRequest();

            var AssignedProjectToUpdate = await _assignedProjectsRepo.GetProjectByIdAsync(updateAssignedProject.UserId, updateAssignedProject.ProjectId);

            if (AssignedProjectToUpdate == null)
                return NotFound();

            // Update the properties of the AssignedProject
            AssignedProjectToUpdate.Status = updateAssignedProject.Status;

            _assignedProjectsRepo.Update(AssignedProjectToUpdate);
            await _assignedProjectsRepo.Save();

            return Ok();

        }
        ////////////////////////////////////////////////////////////////
        [HttpDelete]
        public async Task<IActionResult> DeleteAssignedProject(int userId, int projectId)
        {
            var AssignedProjectToDelete = await _assignedProjectsRepo.GetProjectByIdAsync(userId, projectId);

            if (AssignedProjectToDelete == null)
                return NotFound();

            _assignedProjectsRepo.Delete(AssignedProjectToDelete);
            await _assignedProjectsRepo.Save();

            return Ok();

        }
    }
}