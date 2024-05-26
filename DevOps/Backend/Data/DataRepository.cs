using Microsoft.EntityFrameworkCore;
using Backend.Models;
using System.Collections;
using Microsoft.IdentityModel.Tokens;
using Backend.DTOs;

namespace Backend.Data
{
    public class DataRepository<T> : IDataRepository<T> where T : class
    {
        private readonly DatabaseContext _dbContext;
        private readonly DbSet<T> table;

        public DataRepository(DatabaseContext db)
        {
            _dbContext = db;
            table = _dbContext.Set<T>();
        }

        //Default DataRepository Methods
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await table.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await table.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await table.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            table.Remove(entity);
        }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        //User Specific Data Repositories Methods
        public async Task<IEnumerable<Users>> GetAllDevsAsync()
        {
            return await _dbContext.Users.Where(x => x.RoleId == 1).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAssignedDevsAsync()
        {
            return await table.Include("AssignedTasks").ToListAsync();
        }

        public async Task<IEnumerable<Users>> GetAllUnassignedDevs()
        {
            return await _dbContext.Users.Include(x => x.AssignedProjects)
            .ThenInclude(x => x.Projects)
            .ThenInclude(x => x.Tasks)
            .ToListAsync();
        }


        //Task Specific Data Repositories Methods
        public async Task<IEnumerable<Tasks>> GetAllTasksAsync(int projectId)
        {
            return await _dbContext.Tasks.Where(x => x.ProjectId == projectId).ToListAsync();
        }

        //Get the assigened tasks for a specific user
        public async IAsyncEnumerable<Users?> GetAllDevTasksAsync(int id)
        {
            yield return await _dbContext.Users.Include(x => x.AssignedTasks).Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<AssignedTasks?> GetByIdAsync(int userId, int taskId)
        {
            return await _dbContext.AssignedTasks.Where(x => x.UserId == userId && x.TaskId == taskId).FirstOrDefaultAsync();
        }
        //Attachments
        public async Task<IEnumerable<AssignedTasks>> FileGetter()
        {
            return await _dbContext.AssignedTasks.Include(x => x.Users).Include(x => x.Tasks).Where(x => x.Attachments != "").ToListAsync();
        }


        //Projects Specific Data Repositories Methods
        //Get users were the they haven't rejected a porject
        public async Task<IEnumerable<ProjectsStatus>> GetStatusProjects(int userId, string status)
        {
            //1.pending 2.accepted 3.rejected
            return await _dbContext.ProjectsStatus.Include(x => x.Projects).Where(x => x.UserId == userId && x.Status == status).ToListAsync();
        }
        public async Task<ProjectsStatus?> GetProjectByIdAsync(int userId, int projectId)
        {
            return await _dbContext.ProjectsStatus.Where(x => x.UserId == userId && x.ProjectId == projectId).FirstOrDefaultAsync();
        }


        //Comments Specifies Data Repositories Methods
        public async Task<IEnumerable<Comments>> GetAllCommentsAsync(int taskId)
        {
            return await _dbContext.Comments.Include(x => x.Users).Where(x => x.TaskId == taskId).ToListAsync();
        }
    }
}
