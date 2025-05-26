using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LeaderboardService
    {
        private readonly FrameForgeDbContext _dbContext;
        public LeaderboardService(FrameForgeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Student> GetAllSorted()
        {
            List<Student> all = _dbContext.Students.ToList();
            all.Sort((x, y) => y.StarsAmount.CompareTo(x.StarsAmount));
            return all;

        }

        public int GetStudentsPlace(Student student)
        {
            var allStudents = _dbContext.Students.ToList();
            allStudents.Sort((x, y) => y.StarsAmount.CompareTo(x.StarsAmount));
            
            var indexOfStudent = allStudents.IndexOf(student);
            
            return indexOfStudent;
        }

    }
}
