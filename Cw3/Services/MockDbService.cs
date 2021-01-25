using System.Collections.Generic;
using System.Data;
using System.Linq;
using Cw3.Models;

namespace Cw3.Services
{
    public class MockStudentDbService : IDbService<Student>
    {
        private static readonly ICollection<Student> Students = new List<Student>
        {
            new Student {IdStudent = 1, FirstName = "Jan", LastName = "Kowalski"},
            new Student {IdStudent = 2, FirstName = "Anna", LastName = "Malewski"},
            new Student {IdStudent = 3, FirstName = "Andrzej", LastName = "Andrzejewicz"}
        };

        public IEnumerable<Student> GetEntries()
        {
            return Students;
        }

        public Student GetEntry(int id)
        {
            var studentsWithId = Students.Where(student => student.IdStudent == id).ToList();
            if (studentsWithId.Count == 0) return null;
            if (studentsWithId.Count > 1)
                throw new DataException($" {studentsWithId.Count} students with same ID.");
            return studentsWithId.First();
        }

        public int NextId()
        {
            var ids = Students.Select(student => student.IdStudent).ToList();
            if (ids.Count == 0) return 1;
            ids.Sort();
            return ids.Last() + 1;
        }

        public void AddEntry(Student entryToAdd)
        {
            if (Students.Count(student => student.IdStudent == entryToAdd.IdStudent) > 0)
                throw new DataException($"Student ID : {entryToAdd.IdStudent} already exist");
            Students.Add(entryToAdd);
        }

        public void RemoveEntry(Student entryToRemove)
        {
            Students.Remove(entryToRemove);
        }
    }
}