using System;
using System.Linq;
using System.Linq.Expressions;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;

namespace BridgeportClaims.Data.Services.Payors
{
    public class PayorService : IPayorService
    {
        private readonly IRepository<Payor> _payorRepository;

        public PayorService(IRepository<Payor> payorRepository)
        {
            _payorRepository = payorRepository;
        }

        public Payor GetPayorById(int id) => _payorRepository.Get(id);

        public IQueryable<Payor> GetManyPayors(Expression<Func<Payor, bool>> predicate) =>
            _payorRepository.GetMany(predicate);

        public IQueryable<Payor> GetAllPayors()
            => _payorRepository.GetAll();

        public void InsertPayor(Payor payor)
            => _payorRepository.Save(payor);

        public void UpdatePayor(Payor payor)
            => _payorRepository.Update(payor);

        public void DeletePayor(Payor payor)
            => _payorRepository.Delete(payor);

        public void DeletePayorByPayorId(int id)
            => _payorRepository.Delete(GetPayorById(id));
    }
}