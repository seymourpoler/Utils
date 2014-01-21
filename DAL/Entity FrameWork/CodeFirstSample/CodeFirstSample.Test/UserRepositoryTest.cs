using System;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeFirstSample.Repositories;
using CodeFirstSample.Entities;

namespace CodeFirstSample.Test
{
    [TestClass]
    public class UserRepositoryTest : BaseRepositoryTest
    {
        private UserRepository _userRepository;
        private TaskRepository _taskRepository;
        private User _simpleUser;

        public UserRepositoryTest():base(){}

        [TestInitialize]
        public void SetUp()
        {
            _userRepository = new UserRepository(_db);
            _taskRepository = new TaskRepository(_db);
            _simpleUser = new User() { Name = "Name" };
            _simpleUser.IdUser = _userRepository.Insert(_simpleUser);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _userRepository.Delete(_simpleUser.IdUser);
        }

        [TestMethod]
        public void Should_save_simple_user()
        {
            var userExpected = _userRepository.GetById(_simpleUser.IdUser);
            Assert.AreEqual<User>(_simpleUser, userExpected);
        }

        [TestMethod]
        public void Should_update_simple_user()
        {
            var userModified = _userRepository.GetById(_simpleUser.IdUser);
            userModified.Name = "CustomName";
            userModified.Login = "CodeOne";
            _userRepository.Update(userModified);
            var userExpected = _userRepository.GetById(_simpleUser.IdUser);
            
            Assert.AreEqual<User>(userModified, userExpected);
        }

        [TestMethod]
        public void Should_update_simple_user_with_tasks()
        {
            var taskOne = new Task() { Name = "TaskOne", Minutes = 12, Priority = 5 };
            _taskRepository.Insert(taskOne);
            var taskTwo = new Task() { Name = "TaskTwo", Minutes = 1, Priority = 3 };
            _taskRepository.Insert(taskTwo);
            var taskThree = new Task() { Name = "TaskThree", Minutes = 120, Priority = 6 };
            _taskRepository.Insert(taskThree);

            _simpleUser.Tasks.Add(taskOne);
            _simpleUser.Tasks.Add(taskTwo);
            _simpleUser.Tasks.Add(taskThree);

            _userRepository.Update(_simpleUser);

            var userModified = _userRepository.GetById(_simpleUser.IdUser);

            Assert.AreEqual<User>(userModified, _simpleUser);
            Assert.AreEqual<int>(userModified.Tasks.Count, _simpleUser.Tasks.Count);
        }
    }
}
