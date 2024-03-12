using Domain.Entities;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace DataAccess.EntityMappings;

public class OwnerMapping : ClassMapping<Owner>
{
    public OwnerMapping()
    {
        Id(o => o.Id, map =>
        {
            map.Generator(Generators.Guid);
            map.Type(NHibernateUtil.Guid);
            map.Column("Id");
        });

        Property(o => o.FirstName, map =>
        {
            map.Type(NHibernateUtil.String);
            map.Length(25);
            map.NotNullable(true);
            map.Column("FirstName");
        });

        Property(o => o.LastName, map =>
        {
            map.Type(NHibernateUtil.String);
            map.Length(25);
            map.NotNullable(true);
            map.Column("LastName");
        });

        Property(o => o.Age, map =>
        {
            map.Type(NHibernateUtil.Byte);
            map.NotNullable(true);
            map.Column("Age");
        });

        Property(o => o.Email, map =>
        {
            map.Type(NHibernateUtil.String);
            map.Length(50);
            map.NotNullable(true);
            map.Column("Email");
        });

        Property(o => o.PhoneNumber, map =>
        {
            map.Type(NHibernateUtil.String);
            map.Length(20);
            map.NotNullable(true);
            map.Column("PhoneNumber");
        });

        Bag(o => o.Animals, map =>
        {
            map.Key(k => k.Column("OwnerId"));
            map.Cascade(Cascade.All);
            map.Inverse(true);
        }, rel => rel.OneToMany());

        Table("Owners");
    }
}
