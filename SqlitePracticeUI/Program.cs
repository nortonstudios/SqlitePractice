using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;


namespace SqliteHomework
{
    class Program
    {
        static void Main(string[] args)
        {
            SqliteCrud sql = new SqliteCrud(GetConnectionString());
            FullPersonModel blackWidow = new FullPersonModel()
            {
                Person = new BasicPersonModel()
                {
                    FirstName = "Natasha",
                    LastName = "Romanov"
                },
                Addresses = new List<AddressModel>()
                {
                    new AddressModel()
                    {
                        Street = "12 Avengers Tower",
                        City = "New York",
                        State = "New York",
                        ZIP = "10023"
                    }
                },
                Employers = new List<EmployerModel>()
                {
                    new EmployerModel()
                    {
                        Employer = "Avengers"
                    },
                    new EmployerModel()
                    {
                        Employer = "Russia I guess"
                    }
                }
            };
            FullPersonModel spiderMan = new FullPersonModel()
            {
                Person = new BasicPersonModel()
                {
                    FirstName = "Peter",
                    LastName = "Parker"
                },
                Addresses = new List<AddressModel>()
                {
                    new AddressModel()
                    {
                        Street = "12 Avengers Tower",
                        City = "New York",
                        State = "New York",
                        ZIP = "10023"
                    }, 
                    new AddressModel()
                    {
                        Street = "283 Queens Blvd.",
                        City = "New York",
                        State = "New York",
                        ZIP = "10099"
                    }
                },
                Employers = new List<EmployerModel>()
                {
                    new EmployerModel()
                    {
                        Employer = "Avengers"
                    }, 
                    new EmployerModel()
                    {
                        Employer = "Daily Bugle"
                    },
                    new EmployerModel()
                    {
                        Employer = "Fantastic Four"
                    }
                }
            };
            
            //ReadAllPeople(sql);

            //CreatePerson(sql, blackWidow);
            //CreatePerson(sql, spiderMan);
            
            
            //GetFullPersonById(sql, 6);
            //DeletePersonById(sql, 6);
            ReadAllPeople(sql);

            //Console.WriteLine(GetConnectionString());
            
            Console.WriteLine("\nI'm finished.");
            Console.ReadLine();
        }

        private static string GetConnectionString(string connectionStringName = "Default")
        {
            string output = "";

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            output = config.GetConnectionString(connectionStringName);

            return output;
        }

        private static void ReadAllPeople(SqliteCrud sql)
        {
            var rows = sql.GetAllPeople();

            foreach (var row in rows)
            {
                Console.WriteLine($"{row.Id}: {row.FirstName} {row.LastName}");
            }
        }

        private static void GetFullPersonById(SqliteCrud sql, int id)
        {
            FullPersonModel person = sql.GetFullPersonById(id);
            Console.WriteLine($"{person.Person.Id}: {person.Person.FirstName} {person.Person.LastName}");
            foreach (var address in person.Addresses)
            {
                Console.WriteLine($"{address.Street}\t{address.City}\t{address.State}\t{address.ZIP}");
            }
            foreach (var employer in person.Employers)
            {
                Console.WriteLine($"{employer.Employer}");
            }
        }

        private static void CreatePerson(SqliteCrud sql, FullPersonModel person)
        {
            sql.CreatePerson(person);
        }

        private static void DeletePersonById(SqliteCrud sql, int id)
        {
            sql.DeletePerson(id);
        }
    }
}