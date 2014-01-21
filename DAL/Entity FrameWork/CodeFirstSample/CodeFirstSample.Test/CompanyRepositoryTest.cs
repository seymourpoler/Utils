using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeFirstSample.Repositories;
using CodeFirstSample.Entities;

namespace CodeFirstSample.Test
{
    [TestClass]
    public class CompanyRepositoryTest : BaseRepositoryTest
    {
        private CompanyRepository _companyRepository;
        private GroupRepository _groupRepository;
        private Company _simpleCompany;
        private Company _newCompany;
        private Group _simpleGroup;
        private Group _groupIT;
        private Group _groupMarketing;
        private Group _groupManagement;

        [TestInitialize]
        public void SetUp()
        {
            _companyRepository = new CompanyRepository(_db);
            _groupRepository = new GroupRepository(_db);
            _simpleCompany = new Company() { Name = "New Co" };
            _simpleGroup = new Group() { Name = "Administrators" };

            _newCompany = new Company() { Name = "New Company" };
            _companyRepository.Insert(_newCompany);

            _groupIT = new Group() { Name = "IT" };
            _groupRepository.Insert(_groupIT);
            _groupMarketing = new Group() { Name = "Marketing" };
            _groupRepository.Insert(_groupMarketing);
            _groupManagement = new Group() { Name = "Management" };
            _groupRepository.Insert(_groupManagement);

        }

        [TestCleanup]
        public void CleanUp()
        {
            _companyRepository.Delete(_simpleCompany.IdCompany);
            _companyRepository.Delete(_newCompany.IdCompany);

            _groupRepository.Delete(_groupIT.IdGroup);
            _groupRepository.Delete(_groupMarketing.IdGroup);
            _groupRepository.Delete(_groupManagement.IdGroup);
        }

        [TestMethod]
        public void Should_save_simple_Company()
        {
            var idCompany = _companyRepository.Insert(_simpleCompany);
            var expected = _companyRepository.GetById(idCompany);
            Assert.AreEqual<Company>(_simpleCompany, expected);
        }

        [TestMethod]
        public void Should_update_simple_Company()
        {
            var idCompany = _companyRepository.Insert(_simpleCompany);
            var modified = _companyRepository.GetById(idCompany);

            modified.Name = "New Co, custom";
            _companyRepository.Update(modified);
            var expected = _companyRepository.GetById(modified.IdCompany);

            Assert.AreEqual<Company>(modified, expected);
        }

        [TestMethod]
        public void Should_save_Company_with_groups()
        {
            _newCompany.Groups.Add(_groupMarketing);
            _companyRepository.Update(_newCompany);
            var expected = _companyRepository.GetById(_newCompany.IdCompany);

            Assert.AreEqual<Company>(_newCompany, expected);
            Assert.AreEqual(_newCompany.Groups.Count, expected.Groups.Count);
            Assert.AreEqual<Group>(_newCompany.Groups[0], expected.Groups[0]);
        }

        [TestMethod]
        public void Should_update_Company_with_groups()
        {
            _newCompany.Groups.Add(_groupIT);
            _newCompany.Groups.Add(_groupMarketing);
            _companyRepository.Update(_newCompany);
            var modified = _companyRepository.GetById(_newCompany.IdCompany);

            modified.Name = "Custom New Co.";
            _companyRepository.Update(modified);
            var expected = _companyRepository.GetById(_newCompany.IdCompany);

            Assert.AreEqual<Company>(modified, expected);
            Assert.AreEqual(modified.Groups.Count, expected.Groups.Count);
            Assert.AreEqual<Group>(modified.Groups[0], expected.Groups[0]);
        }

        [TestMethod]
        public void Should_update_groups_from_a_company()
        {
            _newCompany.Groups.Add(_groupIT);
            _newCompany.Groups.Add(_groupMarketing);
            _newCompany.Groups.Add(_groupManagement);

            _companyRepository.Update(_newCompany);

            _newCompany.Groups.Remove(_groupIT);
            _newCompany.Groups.Remove(_groupMarketing);
            _companyRepository.Update(_newCompany);

            var expected = _companyRepository.GetById(_newCompany.IdCompany);

            Assert.AreEqual<Company>(_newCompany, expected);
            Assert.AreEqual(_newCompany.Groups.Count, expected.Groups.Count);
            Assert.AreEqual<Group>(_newCompany.Groups[0], expected.Groups[0]);
        }
    }
}
