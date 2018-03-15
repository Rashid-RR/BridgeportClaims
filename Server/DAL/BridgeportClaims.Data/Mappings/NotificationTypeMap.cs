using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class NotificationTypeMap : ClassMap<NotificationType>
    {
        public NotificationTypeMap()
        {
            Table("NotificationType");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.NotificationTypeId).GeneratedBy.Assigned().Column("NotificationTypeID");
            Map(x => x.TypeName).Column("TypeName").Not.Nullable().Length(255);
            Map(x => x.Code).Column("Code").Not.Nullable().Unique().Length(50);
            Map(x => x.NotificationConfigDescription).Column("NotificationConfigDescription").Length(1000);
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
            HasMany(x => x.Notification).KeyColumn("NotificationTypeID");
            HasMany(x => x.NotificationConfig).KeyColumn("NotificationTypeID");
        }
    }
}