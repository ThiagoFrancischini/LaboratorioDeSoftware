using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using LaboratorioDeSoftware.Core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratorioDeSoftware.Core.Repositories
{
    public class UsuarioRepository(AppDbContext _context)
    {
        public async Task<Usuario> Registrar(Usuario user)
        {
            await _context.Usuarios.AddAsync(user);

            return user;
        }

        public async Task<Usuario> Autenticar(string email, string senha)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);
        }

        public async Task<List<Usuario>> ProcurarTodos()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<bool> ExisteEmail(string email)
        {
            return await _context.Usuarios.AnyAsync(u => u.Email == email);
        }
    }
}
