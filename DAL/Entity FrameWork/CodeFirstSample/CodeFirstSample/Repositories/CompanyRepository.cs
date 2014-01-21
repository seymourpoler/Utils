using System;
using CodeFirstSample.Entities;

namespace CodeFirstSample.Repositories
{
    public class CompanyRepository : BaseRepository
    {
        public CompanyRepository(DataBaseContext db)
            : base(db)
        { }

        public Guid Insert(Company company)
        {
            _db.Companies.Add(company);
            _db.SaveChanges();
            return company.IdCompany;
        }

        public Company GetById(Guid idCompany)
        {
            return _db.Companies.Find(idCompany);
        }

        public void Update(Company company)
        {
            _db.Entry<Company>(company).State = System.Data.EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(Guid idCompany)
        {
            var company = _db.Companies.Find(idCompany);
            if (company != null) 
            {
                _db.Companies.Remove(company);    
            }            
        }
    }
}
