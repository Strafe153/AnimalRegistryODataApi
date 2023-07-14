using Core.Entities;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace DataAccess.EntityMappings;

public class AnimalMapping : ClassMapping<Animal>
{
    public AnimalMapping()
    {
        Id(a => a.Id, map =>
        {
            map.Generator(Generators.Guid);
            map.Type(NHibernateUtil.Guid);
            map.Column("Id");
        });

        Property(a => a.PetName, map =>
        {
            map.Type(NHibernateUtil.String);
            map.Length(25);
            map.NotNullable(true);
            map.Column("PetName");
        });

        Property(a => a.Age, map =>
        {
            map.Type(NHibernateUtil.Byte);
            map.NotNullable(true);
            map.Column("Age");
        });

        Property(a => a.Kind, map =>
        {
            map.Type(NHibernateUtil.String);
            map.Length(50);
            map.NotNullable(true);
            map.Column("Kind");
        });

        ManyToOne(a => a.Owner, map => map.Column("OwnerId"));

        Table("Animals");
    }
}