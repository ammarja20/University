using Core.DTOs;
using Core.Exceptions;
using Core.Forms;
using Core.Validators;
using Data.Entities;
using Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repo;
        private readonly ILogger<CourseService> _logger;

        public CourseService(ICourseRepository repo, ILogger<CourseService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<List<CourseDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all courses...");
            var list = await _repo.GetAllAsync();
            return list.Select(c => new CourseDto { Id = c.Id, Name = c.Name, Weight = c.Weight }).ToList();
        }

        public async Task<CourseDto> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching course {Id}", id);
            var c = await _repo.GetByIdAsync(id);
            if (c == null) throw new NotFoundException($"Course with ID {id} not found.");
            return new CourseDto { Id = c.Id, Name = c.Name, Weight = c.Weight };
        }

        public async Task<CourseDto> CreateAsync(CreateCourseForm form)
        {
            FormValidator.Validate(form);
            _logger.LogInformation("Creating course {Name}", form.Name);

            var errors = new List<string>();
            var exists = (await _repo.GetAllAsync()).Any(x => x.Name == form.Name);
            if (exists) errors.Add("Course name already exists.");

            if (errors.Any()) throw new BusinessException(errors);

            var course = new Course { Name = form.Name, Weight = form.Weight };
            await _repo.AddAsync(course);

            return new CourseDto { Id = course.Id, Name = course.Name, Weight = course.Weight };
        }

        public async Task UpdateAsync(int id, UpdateCourseForm form)
        {
            FormValidator.Validate(form);
            _logger.LogInformation("Updating course {Id}", id);

            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new NotFoundException($"Course with ID {id} not found.");

            var errors = new List<string>();
            var exists = (await _repo.GetAllAsync()).Any(x => x.Name == form.Name && x.Id != id);
            if (exists) errors.Add("Course name already exists.");
            if (errors.Any()) throw new BusinessException(errors);

            entity.Name = form.Name;
            entity.Weight = form.Weight;
            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting course {Id}", id);
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new NotFoundException($"Course with ID {id} not found.");
            await _repo.DeleteAsync(entity);
        }
    }
}
