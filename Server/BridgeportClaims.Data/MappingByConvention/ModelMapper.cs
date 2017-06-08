using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BridgeportClaims.Entities.BaseEntity;
using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Data;
using NHibernate.Mapping.ByCode;

namespace BridgeportClaims.Data.MappingByConvention
{
    public class MyModelMapper : ConventionModelMapper
    {
        public MyModelMapper()
        {
            IsEntity((t, declared) =>
                typeof(Entity<>).IsAssignableFrom(t) &&
                typeof(Entity) != t);

            IsRootEntity((t, declared) =>
                t.BaseType == typeof(Entity));

            IsList((member, declared) =>
                member
                    .GetPropertyOrFieldType()
                    .IsGenericType &&
                member
                    .GetPropertyOrFieldType()
                    .GetGenericInterfaceTypeDefinitions()
                    .Contains(typeof(IList<>)));


            IsVersion((member, declared) =>
                member.Name == "Version" &&
                member.MemberType == MemberTypes.Property &&
                member.GetPropertyOrFieldType() == typeof(int));

            IsTablePerClassHierarchy((t, declared) =>
                typeof(Payor).IsAssignableFrom(t));

            BeforeMapSubclass += ConfigureDiscriminatorValue;
            BeforeMapClass += ConfigureDiscriminatorColumn;
            BeforeMapClass += ConfigurePoidGenerator;
            BeforeMapList += ConfigureListCascading;
        }


        private void ConfigureListCascading(
            IModelInspector modelInspector, PropertyPath member,
            IListPropertiesMapper propertyCustomizer)
        {
            propertyCustomizer.Cascade(Cascade.All |
                                       Cascade.DeleteOrphans);
        }

        private void ConfigurePoidGenerator(
            IModelInspector modelInspector, System.Type type,
            IClassAttributesMapper classCustomizer)
        {
            classCustomizer.Id(id =>
                id.Generator(Generators.GuidComb));
        }

        private static void ConfigureDiscriminatorColumn(
            IModelInspector modelInspector, System.Type type,
            IClassAttributesMapper classCustomizer)
        {
            if (modelInspector.IsTablePerClassHierarchy(type))
            {
                classCustomizer.Discriminator(x =>
                    x.Column(type.Name + "Type"));
            }
        }

        private void ConfigureDiscriminatorValue(
            IModelInspector modelInspector, System.Type type,
            ISubclassAttributesMapper subclassCustomizer)
        {
            subclassCustomizer.DiscriminatorValue(type.Name);
        }
    }
}