using EasyCaching.Models;

namespace EasyCaching.Interfaces
{
    public interface IStudentService
    {
        Task<List<Student>> GetStudents();
    }
}
