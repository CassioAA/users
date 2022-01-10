using Manager.Domain.Entities;
using Manager.Infra.Mappings;
using Microsoft.EntityFrameworkCore;


namespace Manager.Infra.Context {
 public class ManagerContext : DbContext{

    public ManagerContext()
    { }

    public ManagerContext(DbContextOptions<ManagerContext> options) : base(options) 
    {}

   protected override void OnConfiguring(DbContextOptionsBuilder optionsbuilder) {

      optionsbuilder.UseSqlServer(@"Data Source=DESKTOP-9KN6GCL\SQLEXPRESS;Initial Catalog=USERSMANAGER;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
   }

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