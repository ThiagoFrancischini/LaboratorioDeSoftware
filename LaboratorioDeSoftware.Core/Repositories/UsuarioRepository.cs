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

        public async Task<bool> ExisteEmail(string email, Guid userId)
        {
            return await _context.Usuarios.AnyAsync(u => u.Email == email && u.Id != userId);
        }

        public async Task<Usuario> ProcurarPorId(Guid id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario> AtualizarUsuario(Usuario usuarioAtualizado)
        {
            var usuarioExistente = await ProcurarPorId(usuarioAtualizado.Id);

            if (usuarioExistente == null)
                throw new ApplicationException("Usuário não encontrado!");

            usuarioExistente.Nome = usuarioAtualizado.Nome;
            usuarioExistente.Email = usuarioAtualizado.Email;
            usuarioExistente.Telefone = usuarioAtualizado.Telefone;
            usuarioExistente.Cargo = usuarioAtualizado.Cargo;
            usuarioExistente.TipoUsuario = usuarioAtualizado.TipoUsuario;

            if (!string.IsNullOrEmpty(usuarioAtualizado.Senha))
            {
                usuarioExistente.Senha = Convert.ToBase64String(Encoding.UTF8.GetBytes(usuarioAtualizado.Senha));
            }

            await _context.SaveChangesAsync();
            return usuarioExistente;
        }

        public void Remover(Usuario usuario)
        {                           
            _context.Usuarios.Remove(usuario);                        
        }
    }
}
