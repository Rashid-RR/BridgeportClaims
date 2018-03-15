using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Data.Mappings
{
    public class NotificationMap : ClassMap<Notification>
    {
        public NotificationMap()
        {
            Table("Notification");
            Schema("dbo");
            DynamicUpdate();
            SchemaAction.None();
            LazyLoad();
            Id(x => x.NotificationId).GeneratedBy.Identity().Column("NotificationID");
            References(x => x.DismissedByUser).Column("DismissedByUserID");
            References(x => x.NotificationType).Column("NotificationTypeID");
            Map(x => x.MessageText).Column("MessageText").Not.Nullable().Length(4000);
            Map(x => x.GeneratedDate).Column("GeneratedDate").Not.Nullable();
            Map(x => x.IsDismissed).Column("IsDismissed").Not.Nullable();
            Map(x => x.DismissedDate).Column("DismissedDate");
            Map(x => x.CreatedOnUtc).Column("CreatedOnUTC").Not.Nullable();
            Map(x => x.UpdatedOnUtc).Column("UpdatedOnUTC").Not.Nullable();
        }
    }
}