using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeFirstSample.Entities;
using CodeFirstSample.Repositories;

namespace CodeFirstSample.Test
{
    [TestClass]
    public class TaskRepositoryTest : BaseRepositoryTest
    {
        private TaskRepository _taskRepository;
        private StatusRepository _statusRepository;
        private Task _simpleTask;
        private Status _newStatus;

        public TaskRepositoryTest()
        {
            _taskRepository = new TaskRepository(_db);
            _statusRepository = new StatusRepository(_db);
            _simpleTask = new Task() { Name = "SimpleTask", Priority = 12 };
            _newStatus = new Status() { 
                IdStatus = new Guid("00000000-0000-0000-0000-000000000001"),
                Name = "Started"
            };
            _statusRepository.Insert(_newStatus);
        }

        [TestInitialize]
        public void SetUp()
        {
            _simpleTask.IdTask = _taskRepository.Insert(_simpleTask);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _taskRepository.Delete(_simpleTask.IdTask);
            _statusRepository.Delete(_newStatus.IdStatus);
        }

        [TestMethod]
        public void should_save_simple_task()
        {
            var expectedTask = _taskRepository.GetById(_simpleTask.IdTask);

            Assert.AreEqual<Task>(expectedTask, _simpleTask);
        }

        [TestMethod]
        public void should_update_simple_task()
        {
            var expectedTask = _taskRepository.GetById(_simpleTask.IdTask);
            expectedTask.Minutes = 123;
            expectedTask.Name = "Simple Custom Task";
            expectedTask.Priority = 987;

            expectedTask.Status = _newStatus;
            _taskRepository.Update(expectedTask);

            var modifiedTask = _taskRepository.GetById(expectedTask.IdTask);

            Assert.AreEqual<Task>(expectedTask, modifiedTask);
        }
    }
}
