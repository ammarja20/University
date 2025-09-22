using Core.DTOs;
using Core.Forms;

namespace Core.Services
{
    public interface IStudentService
    {
        Task<List<StudentDto>> GetAllAsync();
        Task<StudentDto?> GetByIdAsync(int id);
        Task<StudentDto> CreateAsync(CreateStudentForm form);
        Task<StudentDto?> UpdateAsync(int id, UpdateStudentForm form);
        Task<bool> DeleteAsync(int id);
    }
}
