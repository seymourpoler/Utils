using System;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeFirstSample.Repositories;
using CodeFirstSample.Entities;

namespace CodeFirstSample.Test
{
    [TestClass]
    public class GroupRepositoryTest : BaseRepositoryTest
    {
        private GroupRepository _groupRepository;
        private UserRepository _userRepository;
        private Group _simpleGroup;
        private User _simpleUserOne;
        private User _simpleUserTwo;
        private User _simpleUserThree;


        public GroupRepositoryTest() : base() { }

        [TestInitialize]
        public void SetUp()
        {
            _groupRepository = new GroupRepository(_db);
            _userRepository = new UserRepository(_db);
            _simpleGroup = new Group() { Name = "Name" };
            _simpleGroup.IdGroup = _groupRepository.Insert(_simpleGroup);

            _simpleUserOne = new User() { Name = "SimpleUserOne", Login = "CodeOne" };
            _simpleUserOne.IdUser = _userRepository.Insert(_simpleUserOne);
            _simpleUserTwo = new User() { Name = "SimpleUserTwo", Login  = "CodeTwo" };
            _simpleUserTwo.IdUser = _userRepository.Insert(_simpleUserTwo);
            _simpleUserThree = new User() { Name = "SimpleUserThree", Login = "CodeThree"};
            _simpleUserThree.IdUser = _userRepository.Insert(_simpleUserThree);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _groupRepository.Delete(_simpleGroup.IdGroup);
            _userRepository.Delete(_simpleUserOne.IdUser);
            _userRepository.Delete(_simpleUserTwo.IdUser);
            _userRepository.Delete(_simpleUserThree.IdUser);
        }

        [TestMethod]
        public void Should_save_simple_group()
        {
            var groupExpected = _groupRepository.GetById(_simpleGroup.IdGroup);
            Assert.AreEqual<Group>(_simpleGroup, groupExpected);
        }

        [TestMethod]
        public void Should_save_group_with_users()
        {
            _simpleGroup.Users.Add(_simpleUserOne);
            _simpleGroup.Users.Add(_simpleUserTwo);
            _simpleGroup.Users.Add(_simpleUserThree);

            _groupRepository.Update(_simpleGroup);

            var groupModified = _groupRepository.GetById(_simpleGroup.IdGroup);

            Assert.AreEqual<Group>(_simpleGroup, groupModified);
            Assert.AreEqual<int>(_simpleGroup.Users.Count, groupModified.Users.Count);
            Assert.AreEqual<User>(_simpleGroup.Users[0], groupModified.Users[0]);
            Assert.AreEqual<User>(_simpleGroup.Users[1], groupModified.Users[1]);
        }

        [TestMethod]
        public void Should_update_simple_group()
        {
            var groupModified = _groupRepository.GetById(_simpleGroup.IdGroup);
            groupModified.Name = "CustomName";
            var groupExpected = _groupRepository.GetById(_simpleGroup.IdGroup);

            Assert.AreEqual<string>(groupModified.Name, groupExpected.Name);
        }

        [TestMethod]
        public void Should_update_group_with_users()
        {
            _groupRepository.AddUserToGroup(_simpleGroup, _simpleUserOne);
            _groupRepository.AddUserToGroup(_simpleGroup, _simpleUserTwo);
            _groupRepository.AddUserToGroup(_simpleGroup, _simpleUserThree);

            var groupModified = _groupRepository.GetById(_simpleGroup.IdGroup);
            groupModified.Name = "CustomName";
            _groupRepository.Update(groupModified);
            var groupExpected = _groupRepository.GetById(_simpleGroup.IdGroup);

            Assert.AreEqual<string>(groupModified.Name, groupExpected.Name);
        }
    }
}
