using Manager.Domain.Entities;
using Manager.Infra.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Manager.Infra.Context {
   public class ManagerContext : DbContext{

      public ManagerContext()
      { }

      public ManagerContext(DbContextOptions<ManagerContext> options) : base(options) 
      {}

      // a tabela
      public virtual DbSet<User> Users { get; set; }

      protected override void OnModelCreating(ModelBuilder builder) {
         
         /* 
            aplicando as configurações
            definidas no mapeamento
         */
         builder.ApplyConfiguration(new UserMap());
      }

   }
}