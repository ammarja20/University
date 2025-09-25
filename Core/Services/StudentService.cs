using Core.DTOs;
using Core.Exceptions;
using Core.Forms;
using Core.Validators;
using Data.Entities;
using Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ILogger<StudentService> _logger;

        public StudentService(IStudentRepository studentRepository, ILogger<StudentService> logger)
        {
            _studentRepository = studentRepository;
            _logger = logger;
        }

        public async Task<List<StudentDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all students...");
            var students = await _studentRepository.GetAllAsync();

            if (!students.Any())
            {
                _logger.LogWarning("No students found.");
                return new List<StudentDto>();
            }

            return students.Select(s => new StudentDto
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email
            }).ToList();
        }

        public async Task<StudentDto> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching student with ID {Id}", id);

            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                _logger.LogError("Student with ID {Id} not found", id);
                throw new NotFoundException($"Student with ID {id} not found.");
            }

            return new StudentDto { Id = student.Id, Name = student.Name, Email = student.Email };
        }

        public async Task<StudentDto> CreateAsync(CreateStudentForm form)
        {
            FormValidator.Validate(form);
            _logger.LogInformation("Creating new student {Email}", form.Email);

            var errors = new List<string>();

            var exists = (await _studentRepository.GetAllAsync())
                         .Any(s => s.Email == form.Email);
            if (exists)
            {
                _logger.LogWarning("Duplicate email {Email} detected", form.Email);
                errors.Add("Email already exists.");
            }

            if (errors.Any())
                throw new BusinessException(errors);

            var student = new Student { Name = form.Name, Email = form.Email };
            await _studentRepository.AddAsync(student);

            _logger.LogInformation("Student {Email} created successfully", form.Email);

            return new StudentDto { Id = student.Id, Name = student.Name, Email = student.Email };
        }

        public async Task UpdateAsync(int id, UpdateStudentForm form)
        {
            FormValidator.Validate(form);
            _logger.LogInformation("Updating student with ID {Id}", id);

            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                _logger.LogError("Student with ID {Id} not found for update", id);
                throw new NotFoundException($"Student with ID {id} not found.");
            }

            var errors = new List<string>();
            
            var exists = (await _studentRepository.GetAllAsync())
                         .Any(s => s.Email == form.Email && s.Id != id);
            if (exists)
                errors.Add("Email already exists.");

            if (errors.Any())
            {
                _logger.LogWarning("Validation failed for student update {Id}: {Errors}", id, string.Join(", ", errors));
                throw new BusinessException(errors);
            }

            student.Name = form.Name;
            student.Email = form.Email;

            await _studentRepository.UpdateAsync(student);

            _logger.LogInformation("Student with ID {Id} updated successfully", id);
        }


        public async Task DeleteAsync(int id)  //  غيرنا البوليان
        {
            _logger.LogInformation("Deleting student with ID {Id}", id);

            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                _logger.LogError("Student with ID {Id} not found for deletion", id);
                throw new NotFoundException($"Student with ID {id} not found.");
            }

            await _studentRepository.DeleteAsync(student);

            _logger.LogInformation("Student with ID {Id} deleted successfully", id);
        }
    }
}
