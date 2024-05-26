using Backend.Dtos;
using Backend.Models;

namespace Backend.Data
{
    public interface IDataRepository<T> where T : class
    {
        //Default DataRepository Methods
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<Users>> GetAllDevsAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> Save();

        // User specific Methods
        Task<IEnumerable<Users>> GetAllUnassignedDevs();
        Task<IEnumerable<T>> GetAllAssignedDevsAsync();


        //Task Specific Data Repository Methods
        Task<IEnumerable<Tasks>> GetAllTasksAsync(int projectId);
        IAsyncEnumerable<Users?> GetAllDevTasksAsync(int id);
        Task<AssignedTasks?> GetByIdAsync(int userId, int taskId);

        // Attachments Data Repository Methods
        Task<IEnumerable<AssignedTasks>> FileGetter();


        //Comment Specific Data Repository Methods
        Task<IEnumerable<Comments>> GetAllCommentsAsync(int taskId);


        //Project Specific Data Repository Methods
        Task<ProjectsStatus?> GetProjectByIdAsync(int userId, int projectId);
        Task<IEnumerable<ProjectsStatus>> GetStatusProjects(int userId, string status);
    }
}
