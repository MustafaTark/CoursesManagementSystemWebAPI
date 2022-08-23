using CoursesManagementSystem.Data;
using CoursesManagementSystem.Models;
using CoursesManagementSystem.Repo;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace CoursesManagementSystem.Repositeries
{
    public class CourseRepositery:IServiceRepositery<Course>
    {
        private readonly ConcurrentDictionary<int, Course> courseCash;
        private readonly ApplicationDb db;
        public CourseRepositery(ApplicationDb db)
        {
            this.db = db;
            if (courseCash == null)
            {
                courseCash = new ConcurrentDictionary<int, Course>(db.Courses!.Include(c=>c.Students).Include(i=>i.Instructor).ToDictionary(s => s.Id));
            }

        }

        public async Task<Course> CreateAsync(Course course)
        {
            await db.Courses!.AddAsync(course);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                return courseCash.AddOrUpdate(course.Id, course, UpdateCash!);
            }
            else
            {
                return null!;
            }
        }

        public async Task<bool?> DeleteAsync(int id)
        {
            Course course = db.Courses!.Find(id)!;
            db.Courses!.Remove(course);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                return courseCash.TryRemove(id, out course!);
            }
            return null;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await Task.Run<IEnumerable<Course>>(() => courseCash.Values);
        }

        public Task<Course> GetByIdAsync(int id)
        {
            return Task.Run(() =>
            {
                courseCash.TryGetValue(id, out Course? course);
                return course!;
            });
        }

        public async Task<Course> UpadteAsync(int id, Course course)
        {
            // update in database
            db.Courses!.Update(course!);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                // update in cache
                return UpdateCash(id, course!)!;
            }
            return null!;
        }
        private Course UpdateCash(int id, Course course)
        {
            Course? old;
            if (courseCash!.TryGetValue(id, out old))
            {
                if (courseCash.TryUpdate(id, course, old))
                {
                    return course;
                }
            }
            return null!;
        }
    }
}
