using System;
using BridgeportClaims.Entities.Domain;
using FluentNHibernate.Automapping;

namespace BridgeportClaims.Data.RepositoryUnitOfWork
{
    public class AutomappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type) => type.GetInterface(typeof(IEntity).FullName) != null;
    }
}
