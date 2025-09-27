using Core.DTOs;
using Core.Forms;

namespace Core.Services
{
    public interface ICourseService
    {
        Task<List<CourseDto>> GetAllAsync();
        Task<CourseDto> GetByIdAsync(int id);
        Task<CourseDto> CreateAsync(CreateCourseForm form);
        Task UpdateAsync(int id, UpdateCourseForm form);
        Task DeleteAsync(int id);
    }
}
