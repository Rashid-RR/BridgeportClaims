using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class NotificationConfigMap : ClassMap<NotificationConfig>
    {
        public NotificationConfigMap()
        {
            Table("NotificationConfig");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.NotificationConfigId).GeneratedBy.Identity().Column("NotificationConfigID");
            References(x => x.NotificationType).Column("NotificationTypeID");
            Map(x => x.NotificationValue).Column("NotificationValue").Not.Nullable();
            Map(x => x.SqlVariantDataType).Column("SQLVariantDataType").Not.Nullable().Length(30);
            Map(x => x.EffectiveDate).Column("EffectiveDate").Not.Nullable();
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
        }
    }
}