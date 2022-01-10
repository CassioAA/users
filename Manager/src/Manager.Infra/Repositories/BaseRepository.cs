using System.Linq;
using System.Threading.Tasks;
using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Manager.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Manager.Infra.Repositories {
    public class BaseRepository<T> : IBaseRepository<T> where T : Base {
        private readonly ManagerContext _context;

        public BaseRepository(ManagerContext context) {
            _context = context;
        }

        public virtual async Task<T> Create(T obj) {
            _context.Add(obj);
            await _context.SaveChangesAsync();

            /* 
                retorna o objeto com o seu ID recebido
                ao ter sido salvo no banco de dados
            */
            return obj;
        }

        public virtual async Task<T> Update(T obj) {
            _context.Entry(obj).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return obj;
        }

        public virtual async Task Remove(long id) {
            var obj = await Get(id);

            if(obj != null) {
                _context.Remove(obj);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<T> Get(long id) {
            var obj = await _context.Set<T>()
                .AsNoTracking().Where(x=>x.Id == id).ToListAsync();

            return obj.FirstOrDefault();
        }

        // todos os dados da tabela
        public virtual async Task<List<T>> Get(){
            return await _context.Set<T>()
                .AsNoTracking().ToListAsync();
        }


    }
}