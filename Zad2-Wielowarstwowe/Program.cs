using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using Zad2_Wielowarstwowe.Data;
using Zad2_Wielowarstwowe.Models;
using static Zad2_Wielowarstwowe.Models.TeamMember;

namespace Zad2_Wielowarstwowe
{
    class Program
    {
        static void Main(string[] args)
        {
            // Baza danych
            DatabaseExample();

            //Lazy loading
            LazyLoadingExample();

            //n+1
            DatabaseProblemExample();
        }

        public static void DatabaseExample()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase(databaseName: "Test");

            var options = builder.Options;

            using (var context = new DataContext(options))
            {
                AddTeamMember(context);
                DeleteTeam(context, 1);
            }
        }

        public static void LazyLoadingExample()
        {
            var personalLoan = new PersonalLoanLazyClass("123456789");
            Console.WriteLine("\n\n.......................Press Enter " +
                    "to continue.......................\n\n");
            Console.Read();
            Console.WriteLine(personalLoan.LoanDetail.LoanAmount.ToString());
            Console.Read();
            Console.Read();
        }

        public static void AddTeamMember(DataContext context)
        {
            var teamMember = new TeamMember
            {
                FirstName = "Elizabeth",
                LastName = "Lincoln",
                MemberTitle = Title.Developer
            };

            context.TeamMembers.Add(teamMember);
            context.SaveChanges();
        }

        public static void DeleteTeam(DataContext context, int id)
        {
            var team = GetTeam(context, id);

            context.Remove(team);
            context.SaveChanges();
        }

        public static Team GetTeam(DataContext context, int id)
        {
            return context.Teams.FirstOrDefault(t => t.Id == id);
        }

        public static void DatabaseProblemExample()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase(databaseName: "Test");

            var options = builder.Options;

            using (var context = new DataContext(options))
            {
                var teams = context.TeamMembers
                    .Where(t => t.LastName.Contains("a"))
                    .Select(f => f.Team)
                    .ToList();

                foreach (var team in teams)
                {
                    Console.WriteLine(team);

                    // po wykryciu nulla Entity Framework wykonana każdorazowo zapytanie pobierające TeamMembers
                    Console.WriteLine(team.TeamMembers);
                }

                var teams2 = context.TeamMembers
                    .Where(t => t.LastName.Contains("a"))
                    .Include(tm => tm.Team.TeamMembers)
                    .Select(f => f.Team)
                    .ToList();

                foreach (var team in teams2)
                {
                    Console.WriteLine(team);

                    // aby uniknąć n+1 problemu należy użyć include dla TeamMembers
                    Console.WriteLine(team.TeamMembers);
                }
            }
        }

        public class PersonalLoanLazyClass
        {
            private readonly Lazy<Loan> lazyLoan;
            public string AccountNumber { get; set; }
            public Loan LoanDetail
            {
                get { return this.lazyLoan.Value; }
            }
            public PersonalLoanLazyClass(string accountNumber)
            {
                Console.WriteLine("PersonalLoanLazyClass call to costructor....");

                this.AccountNumber = accountNumber;
                this.lazyLoan = new Lazy<Loan>(() => new Loan(this.AccountNumber));

                Console.WriteLine("Completed initialization.......");
            }
        }

        public class PersonalLoan
        {
            private Loan _loanDetail;
            public string AccountNumber { get; set; }
            public Loan LoanDetail
            {
                get { return _loanDetail ?? (_loanDetail = new Loan(this.AccountNumber)); }
            }
        }
        public class Loan
        {
            public string AccountNumber { get; set; }
            public float LoanAmount { get; set; }
            public Loan(string accountNumber)
            {
                Console.WriteLine("loan class constructor");

                this.AccountNumber = accountNumber;
                this.LoanAmount = 1000;

                Console.WriteLine("loan object created");
            }
        }
    }
}
