using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class PrescriberMap : ClassMap<Prescriber>
    {
        public PrescriberMap()
        {
            Table("Prescriber");
            Schema("dbo");
            SchemaAction.None();
            DynamicUpdate();
            LazyLoad();
            Id(x => x.PrescriberNpi).GeneratedBy.Assigned().Column("PrescriberNPI");
            References(x => x.StateId).Column("StateID");
            Map(x => x.ExpDate).Column("ExpDate");
            Map(x => x.PrescriberName).Column("PrescriberName").Length(255);
            Map(x => x.Addr1).Column("Addr1").Length(255);
            Map(x => x.Addr2).Column("Addr2").Length(255);
            Map(x => x.City).Column("City").Length(255);
            Map(x => x.Zip).Column("Zip").Length(255);
            Map(x => x.Fldohnum).Column("FLDOHNUM").Length(255);
            Map(x => x.Phone).Column("Phone").Length(255);
            Map(x => x.Fax).Column("Fax").Length(255);
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
        }
    }
}