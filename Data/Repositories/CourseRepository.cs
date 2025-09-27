using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly UniversityDbContext _db;
        public CourseRepository(UniversityDbContext db) => _db = db;

        public Task<List<Course>> GetAllAsync() => _db.Courses.ToListAsync();
        public Task<Course?> GetByIdAsync(int id) => _db.Courses.FindAsync(id).AsTask();

        public async Task AddAsync(Course course)
        {
            _db.Courses.Add(course);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Course course)
        {
            _db.Courses.Update(course);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Course course)
        {
            _db.Courses.Remove(course);
            await _db.SaveChangesAsync();
        }
    }
}
