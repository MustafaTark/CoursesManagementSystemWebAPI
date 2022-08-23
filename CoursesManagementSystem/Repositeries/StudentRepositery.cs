using CoursesManagementSystem.Data;
using CoursesManagementSystem.Models;
using CoursesManagementSystem.Repo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Concurrent;

namespace CoursesManagementSystem.Repositeries
{
    public class StudentRepositery : IServiceRepositery<Student>
    {
        private ConcurrentDictionary<int, Student> studentsCash ;
        private readonly ApplicationDb db;
        public StudentRepositery( ApplicationDb db)
        {
            this.db = db;
            if (studentsCash == null)
            {
                studentsCash = new ConcurrentDictionary<int, Student>(db.Students!.Include(s=>s.Courses).Include(s=>s.Grades).ToDictionary(s=>s.Id));
            }

        }

        public async Task<Student> CreateAsync(Student student)
        {
            await db.Students!.AddAsync(student);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                return studentsCash.AddOrUpdate(student.Id, student, UpdateCash!);
            }
            else
            {
                return null!;
            }
        }

        public async Task<bool?> DeleteAsync(int id)
        {
            Student student = db.Students!.Find(id)!;
            db.Students!.Remove(student);
            int affected =await db.SaveChangesAsync();
            if (affected==1)
            {
                return studentsCash.TryRemove(id, out student!);
            }
            return null;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await Task.Run<IEnumerable<Student>>(() => studentsCash.Values); 
        }

        public Task<Student> GetByIdAsync(int id)
        {
            return Task.Run(() =>
            {
                studentsCash.TryGetValue(id,out Student? student);
                return student!;
            });
        }

        public async Task<Student> UpadteAsync(int id, Student? student)
        {
            // update in database
            db.Students!.Update(student!);
            
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                // update in cache
                return UpdateCash(id, student!)!;
            }
            return null!;
        }
        private Student UpdateCash(int id,Student student)
        {
            Student? old;
            if (studentsCash!.TryGetValue(id, out old))
            {
                if (studentsCash.TryUpdate(id, student, old))
                {
                    return student;
                }
            }
            return null!;
        }

        
    }
}
