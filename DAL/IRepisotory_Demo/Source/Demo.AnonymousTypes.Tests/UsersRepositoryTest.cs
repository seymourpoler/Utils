using Demo.AnonymousTypes.Domain.Persistence;
using Demo.AnonymousTypes.Domain.Persistence.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Demo.AnonymousTypes.Domain.Entities;
using System.Collections.Generic;

namespace Demo.AnonymousTypes.Tests
{
    /// <summary>
    ///This is a test class for UsersRepositoryTest and is intended
    ///to contain all UsersRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UsersRepositoryTest
    {
        private TestContext testContextInstance;
        private UsersRepository _usersRepository;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
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

        public UsersRepositoryTest()
        {
            _usersRepository = new UsersRepository();
        }

        private User NewUser()
        {
            return new User()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email"
            };
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
            _usersRepository.DeleteAll();
        }
        //
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            _usersRepository.DeleteAll();
        }
        //
        #endregion


        /// <summary>
        ///A test for DeleteAllUsers
        ///</summary>
        [TestMethod()]
        public void DeleteAllUsersTest()
        {
            var newUser = NewUser();
            _usersRepository.Save(newUser);
            _usersRepository.Save(newUser);

            var users = _usersRepository.GetAll().ToList();
            Assert.AreEqual(2, users.Count);
            _usersRepository.DeleteAll();
            users = _usersRepository.GetAll().ToList();
            Assert.AreEqual(0, users.Count);
        }

        /// <summary>
        ///A test for DeleteUserById
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void DeleteUserByIdTest()
        {
            var newUser = NewUser();
            _usersRepository.Save(newUser);
            var users = _usersRepository.GetAll().ToList();
            int idUser = users[0].Id;
            _usersRepository.DeleteById(idUser);
            var user = _usersRepository.GetById(idUser);
            Assert.Fail("Exception expected");
        }

        /// <summary>
        ///A test for GetAllUsers
        ///</summary>
        [TestMethod()]
        public void GetAllUsersTest()
        {
            var newUser = NewUser();
            _usersRepository.Save(newUser);
            _usersRepository.Save(newUser);

            var users = _usersRepository.GetAll().ToList();
            Assert.IsTrue(users.Count > 0);
        }

        /// <summary>
        ///A test for GetUserById
        ///</summary>
        [TestMethod()]
        public void GetUserByIdTest()
        {
            var newUser = NewUser();
            _usersRepository.Save(newUser);

            var users = _usersRepository.GetAll().ToList();
            var user = users[0];
            var userExpected = _usersRepository.GetById(user.Id);
            Assert.AreEqual(user.Id, userExpected.Id);
            Assert.AreEqual(user.FirstName, userExpected.FirstName);
            Assert.AreEqual(user.LastName, userExpected.LastName);
            Assert.AreEqual(user.Email, userExpected.Email);
        }

        /// <summary>
        ///A test for UpdateUser
        ///</summary>
        [TestMethod()]
        public void UpdateUserTest()
        {
            User newUser = NewUser();
            _usersRepository.Save(newUser);
            
            var user = _usersRepository.GetAll().ToList()[0];

            user.Email = "email@email.com";
            _usersRepository.Update(user);

            var userExpected = _usersRepository.GetAll().ToList()[0];

            Assert.AreEqual(user.Id, userExpected.Id);
            Assert.AreEqual(user.FirstName, userExpected.FirstName);
            Assert.AreEqual(user.LastName, userExpected.LastName);
            Assert.AreEqual(user.Email, userExpected.Email);
        }
    }
}
