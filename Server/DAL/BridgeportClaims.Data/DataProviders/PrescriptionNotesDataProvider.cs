using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;
using NHibernate;
using NHibernate.Linq;

namespace BridgeportClaims.Data.DataProviders
{
    public class PrescriptionNotesDataProvider : BaseRepository
    {
        public PrescriptionNotesDataProvider(ISession session) : base(session) { }

        public dynamic GetPrescriptionNotes()
        {
            return Session.Query<Prescription>().ToFuture();
        }
    }
}
