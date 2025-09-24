using Core.DTOs;
using Core.Forms;
using Data.Entities;
using Data.Repositories;

namespace Core.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repo;
        public StudentService(IStudentRepository repo) => _repo = repo;

        public async Task<List<StudentDto>> GetAllAsync()
            => (await _repo.GetAllAsync())
                .Select(s => new StudentDto { Id = s.Id, Name = s.Name, Email = s.Email })
                .ToList();

        public async Task<StudentDto?> GetByIdAsync(int id)
        {
            var student = await _repo.GetByIdAsync(id);
            return student == null ? null : new StudentDto { Id = student.Id, Name = student.Name, Email = student.Email };
        }

        public async Task<StudentDto> CreateAsync(CreateStudentForm form)
        {
            var student = new Student { Name = form.Name, Email = form.Email };
            await _repo.AddAsync(student);
            return new StudentDto { Id = student.Id, Name = student.Name, Email = student.Email };
        }

        public async Task<StudentDto?> UpdateAsync(int id, UpdateStudentForm form)
        {
            var student = await _repo.GetByIdAsync(id);
            if (student == null) return null;
            student.Name = form.Name;
            student.Email = form.Email;
            await _repo.UpdateAsync(student);
            return new StudentDto { Id = student.Id, Name = student.Name, Email = student.Email };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _repo.GetByIdAsync(id);
            if (student == null) return false;
            await _repo.DeleteAsync(student);
            return true;
        }
    }
}
