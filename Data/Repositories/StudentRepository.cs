using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly UniversityDbContext _db;
        public StudentRepository(UniversityDbContext db) => _db = db;

        public async Task<List<Student>> GetAllAsync() => await _db.Students.ToListAsync();
        public async Task<Student?> GetByIdAsync(int id) => await _db.Students.FindAsync(id);
        public async Task AddAsync(Student student)
        {
            _db.Students.Add(student);
            await _db.SaveChangesAsync();
        }
        public async Task UpdateAsync(Student student)
        {
            _db.Students.Update(student);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteAsync(Student student)
        {
            _db.Students.Remove(student);
            await _db.SaveChangesAsync();
        }
    }
}
