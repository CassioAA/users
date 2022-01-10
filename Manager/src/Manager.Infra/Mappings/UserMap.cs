using Manager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manager.Infra.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User> {

        public void Configure(EntityTypeBuilder<User> builder) {
            
            /* 
                configuração do banco de dados
                usando o pacote EF Design
            */
            builder.ToTable("User");
            // a chave primária
            builder.HasKey(x => x.Id);
            // configuração da chave primária
            builder.Property(x => x.Id)
                // autoincremento
                .UseIdentityColumn()
                // no SQL é um tipo equivalente à long
                .HasColumnType("BIGINT");
            
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(70)
                .HasColumnName("name")
                // no SQL é um tipo equivalente à string
                .HasColumnType("VARCHAR(70)");
            
            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnName("password")
                .HasColumnType("VARCHAR(30)");

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(180)
                .HasColumnName("email")
                .HasColumnType("VARCHAR(180)");
        }
    }
}