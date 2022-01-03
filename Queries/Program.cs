using System;
using System.Linq;

namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new PlutoContext();

            // LINQ syntax

            var query =
                from c in context.Courses
                where c.Name.Contains("c#")
                orderby c.Name
                select c;

            foreach (var course in query)
            {
                Console.WriteLine(course.Name);
            }

            // Get all courses in Level 1

            var query1 =
                from c in context.Courses
                where c.Level == 1
                select c;

            // Ordering

            var query2 =
                from c in context.Courses
                where c.Author.Id == 1
                orderby c.Level descending, c.Name
                select c;

            // Projection

            var query3 =
                from c in context.Courses
                where c.Author.Id == 1
                orderby c.Level descending, c.Name
                select new { Name = c.Name, Author = c.Author.Name };

            // Grouping

            var query4 =
                from c in context.Courses
                group c by c.Level into g
                select g;

            foreach (var group in query4)
            {
                //System.Console.WriteLine("{0} ({1})", group.Key, group.Count());

                Console.WriteLine(group.Key);

                foreach (var course in group)
                {
                    Console.WriteLine("\t{0}", course.Name);
                }
            }

            // Joining

            // Inner Join
            var query5 =
                from c in context.Courses
                join a in context.Authors on c.AuthorId equals a.Id
                select new { CourseName = c.Name, AuthorName = a.Name };

            // Group Join
            var query6 =
                from a in context.Authors
                join c in context.Courses on a.Id equals c.AuthorId into g
                select new { AuthorName = a.Name, Courses = g.Count() };

            foreach(var x in query6)
            {
                Console.WriteLine("{0} ({1})", x.AuthorName, x.Courses);
            }

            // Cross Join
            var query7 =
                from a in context.Authors
                from c in context.Courses
                select new { AuthorName = a.Name, CourseName = c.Name };

            foreach(var x in query7)
            {
                Console.WriteLine("{0} - {1}", x.AuthorName, x.CourseName);
            }

            // Extension methods

            var courses = context.Courses
                .Where(c => c.Name.Contains("c#"))
                .OrderBy(c => c.Name);

            foreach(var course in courses)
            {
                Console.WriteLine(course.Name);
            }

            // Restriction

            var courses1 = context.Courses
                .Where(c => c.Level == 1);

            // Ordering

            var courses2 = context.Courses
                .Where(c => c.Level == 1)
                .OrderByDescending(c => c.Name)
                .ThenByDescending(c => c.Level);

            // Projection

            var courses3 = context.Courses
                .Where(c => c.Level == 1)
                .OrderByDescending(c => c.Name)
                .ThenByDescending(c => c.Level)
                .Select(c => new { CourseName = c.Name, AuthorName = c.Author.Name });

            var tags = context.Courses
                .Where(c => c.Level == 1)
                .OrderByDescending(c => c.Name)
                .ThenByDescending(c => c.Level)
                .SelectMany(c => c.Tags);

            foreach(var t in tags)
            {
                Console.WriteLine(t.Name);
            }

            // Set Operations

            var tags1 = context.Courses
                .Where(c => c.Level == 1)
                .OrderByDescending(c => c.Name)
                .ThenByDescending(c => c.Level)
                .SelectMany(c => c.Tags)
                .Distinct();

            foreach (var t in tags1)
            {
                Console.WriteLine(t.Name);
            }

            // Grouping

            var groups = context.Courses.GroupBy(c => c.Level);

            foreach(var group in groups)
            {
                Console.WriteLine("Key: " + group.Key);

                foreach(var course in group)
                {
                    Console.WriteLine("\t" + course.Name);
                }
            }

            // Joining

            // Inner Join
            context.Courses.Join(context.Authors, 
                c => c.AuthorId, 
                a => a.Id, 
                (course, author) => new
                    {
                        CourseName = course.Name,
                        AuthorName = author.Name
                    });

            // Group Join
            context.Authors.GroupJoin(context.Courses, a => a.Id, c => c.AuthorId, (author, course) => new
            {
                AuthorName = author.Name,
                Courses = course
            });

            // Cross Join
            context.Authors.SelectMany(a => context.Courses, (author, course) => new
            {
                AuthorName = author.Name,
                CourseName = course.Name
            });

            // Partitioning

            var course4 = context.Courses.Skip(10).Take(10);

            // Element Operators

            context.Courses.OrderBy(c => c.Level).FirstOrDefault(c => c.FullPrice > 100);
            context.Courses.SingleOrDefault(c => c.Id == 1);

            // Quantifying

            var allAbove10Dollars = context.Courses.All(c => c.FullPrice > 10);
            context.Courses.Any(c => c.Level == 1);

            // Aggregating

            var count = context.Courses.Where(c => c.Level == 1).Count();
            context.Courses.Max(c => c.FullPrice);
            context.Courses.Max(c => c.FullPrice);
            context.Courses.Average(c => c.FullPrice);

        }
    }
}
