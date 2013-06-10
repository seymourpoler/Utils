using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Demo.AnonymousTypes.Domain.Entities;
using Demo.AnonymousTypes.Domain.Persistence.Queries;
using Demo.AnonymousTypes.Domain.Persistence.Commands;
using System.Collections.Generic;

namespace Demo.AnonymousTypes.Tests
{ 
    [TestClass()]
    public class UsersTest
    {
        private TestContext testContextInstance;
        private const string _connectionString = "Server=127.0.0.1;Database=AppTestWeb;User Id=sa;Password=temporal;";

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            CleanUsers();
        }
        //
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            CleanUsers();
        }
        //
        #endregion

        private User CreateNewUser()
        { 
            return new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@doe.com"
            };
        }

        private void CleanUsers()
        {
            var db = new DeleteAllUsersCommand(_connectionString);
            db.Execute();
        }

        [TestMethod()]
        public void Should_Return_All_Users_In_The_Table()
        {
            var newUser = CreateNewUser();
            var dbCreate = new CreateUserCommand(_connectionString, newUser);
            dbCreate.Execute();

            var dbGet = new  GetAllUsersQuery(_connectionString);
            IEnumerable<User>  users = dbGet.Execute();
            Assert.IsTrue(users.ToList().Count > 0);
        }

        [TestMethod]
        public void Should_Create_A_User_In_The_Table()
        {
            var newUser = CreateNewUser();

            var db = new CreateUserCommand(_connectionString, newUser);
            db.Execute();

            var users = new GetAllUsersQuery(_connectionString).Execute();

            Assert.IsTrue(users.ToList().Count > 0);
        }

        [TestMethod]
        public void Sholud_Update_A_Create_User()
        {
            var newUser = CreateNewUser();

            new CreateUserCommand(_connectionString, newUser).Execute();
            newUser.Email = "email@email.com";

            var userUpdated = new GetAllUsersQuery(_connectionString).Execute().ToList();
            var usrUpdate = userUpdated[0];

            usrUpdate.Email = "email@email.com";

            new UpdateUserCommand(_connectionString, usrUpdate).Execute();

            var userListExpected = new GetAllUsersQuery(_connectionString).Execute().ToList();
            var userExpected = userListExpected[0];

            Assert.AreEqual(newUser.FirstName, userExpected.FirstName);
            Assert.AreEqual(newUser.LastName, userExpected.LastName);
            Assert.AreEqual(newUser.Email, userExpected.Email);
        }
    }
}
