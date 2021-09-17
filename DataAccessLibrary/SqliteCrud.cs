using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLibrary.Models;

namespace DataAccessLibrary
{
    public class SqliteCrud
    {
        private readonly string _connectionString;
        private SqliteDataAccess db = new SqliteDataAccess();

        public SqliteCrud(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public List<BasicPersonModel> GetAllPeople()
        {
            string sql = @"SELECT Id, FirstName, LastName 
                            FROM People;";
            
            return db.LoadData<BasicPersonModel, dynamic>(
                sql,
                new { }, 
                _connectionString);
        }

        public FullPersonModel GetFullPersonById(int id)
        {
            var output = new FullPersonModel();

            // Get person name and id
            string sql = @"SELECT Id, FirstName, LastName
                            FROM People 
                            WHERE Id = @Id;";
            output.Person = db.LoadData<BasicPersonModel, dynamic>(
                sql,
                new {Id = id},
                _connectionString).FirstOrDefault();

            // Get addresses as list.
            sql = @"SELECT a.*
                    FROM Addresses a
                    INNER JOIN PeopleAddresses pa ON pa.AddressId = a.Id
                    WHERE pa.PersonId = @Id;";
            output.Addresses = db.LoadData<AddressModel, dynamic>(
                sql,
                new {Id = id},
                _connectionString);
            
            // Get employers as list
            sql = @"SELECT e.*
                    FROM Employers e
                    INNER JOIN PeopleEmployers pe ON pe.EmployerId = e.Id
                    WHERE pe.PersonId = @Id;";
            output.Employers = db.LoadData<EmployerModel, dynamic>(
                sql,
                new {Id = id},
                _connectionString);
            
            return output;
        }

        public void CreatePerson(FullPersonModel newPerson)
        {
            // Save basic person in Person table
            string sql = @"INSERT INTO People (FirstName, LastName)
                            VALUES (@FirstName, @LastName);";
            db.SaveData(
                    sql,
                    new {newPerson.Person.FirstName, newPerson.Person.LastName},
                    _connectionString);
            // Get new person's Id 
            sql = @"SELECT Id FROM People
                    WHERE FirstName = @FirstName
                    AND LastName = @LastName;";
            int personId = db.LoadData<int, dynamic>(
                sql,
                new {newPerson.Person.FirstName, newPerson.Person.LastName},
                _connectionString).First();

            // Search Addresses for new person's address
            foreach (var address in newPerson.Addresses)
            {
                int addressId = 0;
                sql = @"SELECT Id FROM Addresses
                    WHERE Street = @Street
                    AND City = @City
                    AND State = @State
                    AND ZIP = @ZIP;";
                addressId = db.LoadData<int, dynamic>(
                    sql,
                    new {address.Street, address.City, address.State, address.ZIP},
                    _connectionString).FirstOrDefault();
            
                // create new address if address is not in table
                if (addressId == 0)
                {
                    sql = @"INSERT INTO Addresses (Street, City, State, ZIP)
                    VALUES (@Street, @City, @State, @ZIP);";
                    db.SaveData(
                        sql,
                        new {address.Street, address.City, address.State, address.ZIP},
                        _connectionString);

                    sql = @"SELECT Id FROM Addresses
                    WHERE Street = @Street
                    AND City = @City
                    AND State = @State
                    AND ZIP = @ZIP;";
                    addressId = db.LoadData<int, dynamic>(
                        sql,
                        new {address.Street, address.City, address.State, address.ZIP},
                        _connectionString).First();
                }

                // Link person and address in PeopleAddress table
                sql = @"INSERT INTO PeopleAddresses (PersonId, AddressId) VALUES (@PersonId, @AddressId);";
                db.SaveData(
                    sql,
                    new {PersonId = personId, AddressId = addressId},
                    _connectionString);
            }
             
            // Search Employers for new person's employer
            foreach (var newEmployer in newPerson.Employers)
            {
                int employerId = 0;
                sql = @"SELECT Id FROM Employers 
                    WHERE Employer = @Employer;";
                employerId = db.LoadData<int, dynamic>(
                    sql, 
                    new { Employer = newEmployer.Employer}, 
                    _connectionString).FirstOrDefault();

                // create new employer if employer is not in table
                if (employerId == 0)
                {
                    sql = @"INSERT INTO Employers (Employer)
                            VALUES (@Employer);";
                    db.SaveData(
                        sql,
                        new {newEmployer.Employer},
                        _connectionString);

                    sql = @"SELECT Id FROM Employers
                            WHERE Employer = @Employer;";
                    employerId = db.LoadData<int, dynamic>(
                        sql,
                        new {newEmployer.Employer},
                        _connectionString).First();
                }

                // if employer is present, link
                // else create employer and link
                sql = @"INSERT INTO PeopleEmployers (PersonId, EmployerId) VALUES (@PersonId, @EmployerId);";
                db.SaveData(
                    sql,
                    new {PersonId = personId, EmployerId = employerId},
                    _connectionString);
            }
        }

        public void DeletePerson(int id)
        {
            string sql = @"DELETE FROM People
                            WHERE Id = @Id;";
            db.SaveData(
                sql,
                new {Id = id},
                _connectionString);
        }
    }
}