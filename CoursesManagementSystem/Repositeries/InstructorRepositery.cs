using CoursesManagementSystem.Data;
using CoursesManagementSystem.Models;
using CoursesManagementSystem.Repo;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace CoursesManagementSystem.Repositeries
{
    public class InstructorRepositery: IServiceRepositery<Instructor>
    {
        private readonly ConcurrentDictionary<int, Instructor> instructorCash;
        private readonly ApplicationDb db;
        public InstructorRepositery(ApplicationDb db)
        {
            this.db = db;
            if (instructorCash == null)
            {
                instructorCash = new ConcurrentDictionary<int, Instructor>
                    (db.Instructors!.Include(i=>i.Posts).Include(c=>c.Courses).ToDictionary(s => s.Id));
            }

        }

        public async Task<Instructor> CreateAsync(Instructor instructor)
        {
            await db.Instructors!.AddAsync(instructor);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                return instructorCash.AddOrUpdate(instructor.Id, instructor, UpdateCash!);
            }
            else
            {
                return null!;
            }
        }

        public async Task<bool?> DeleteAsync(int id)
        {
            Instructor instructor = db.Instructors!.Find(id)!;
            db.Instructors!.Remove(instructor);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                return instructorCash.TryRemove(id, out instructor!);
            }
            return null;
        }

        public async Task<IEnumerable<Instructor>> GetAllAsync()
        {
            return await Task.Run<IEnumerable<Instructor>>(() => instructorCash.Values);
        }

        public Task<Instructor> GetByIdAsync(int id)
        {
            return Task.Run(() =>
            {
                instructorCash.TryGetValue(id, out Instructor? instructor);
                return instructor!;
            });
        }

        public async Task<Instructor> UpadteAsync(int id, Instructor instructor)
        {
            // update in database
            db.Instructors!.Update(instructor!);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                // update in cache
                return UpdateCash(id, instructor!)!;
            }
            return null!;
        }
        private Instructor UpdateCash(int id, Instructor instructor)
        {
            Instructor? old;
            if (instructorCash!.TryGetValue(id, out old))
            {
                if (instructorCash.TryUpdate(id, instructor, old))
                {
                    return instructor;
                }
            }
            return null!;
        }
    }
}
