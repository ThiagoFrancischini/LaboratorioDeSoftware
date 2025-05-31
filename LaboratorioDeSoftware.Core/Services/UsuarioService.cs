using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using LaboratorioDeSoftware.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratorioDeSoftware.Core.Services
{
    public class UsuarioService
    {
        private AppDbContext _context;
        private UsuarioRepository userRepository;
        public UsuarioService(AppDbContext context)
        {
            _context = context;
            userRepository = new UsuarioRepository(context);
        }

        public async Task<Usuario> Registrar(Usuario usuario)
        {
            if (await userRepository.ExisteEmail(usuario.Email))
            {
                throw new ApplicationException("Já existe um usuário com este email");
            }

            usuario.Id = Guid.NewGuid();
            usuario.Validar();
            await userRepository.Registrar(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> Autenticar(string login, string senha)
        {
            if (string.IsNullOrEmpty(login) || !login.Contains('@'))
            {
                throw new ApplicationException("Informe um Email válido!");
            }

            if (string.IsNullOrEmpty(senha))
            {
                throw new ApplicationException("Informe uma senha válida!");
            }

            senha = Convert.ToBase64String(Encoding.UTF8.GetBytes(senha));

            var usuario = await userRepository.Autenticar(login, senha);

            if (usuario == null)
            {
                throw new ApplicationException("Usuário ou senha inválidos!");
            }

            return usuario;
        }

        public async Task<List<Usuario>> ProcurarTodos()
        {
            return await userRepository.ProcurarTodos();
        }

        public async Task<Usuario> ProcurarPorId(Guid id)
        {
            return await userRepository.ProcurarPorId(id);
        }

        public async Task<Usuario> AtualizarUsuario(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            if (await userRepository.ExisteEmail(usuario.Email, usuario.Id))
                throw new ApplicationException("Este e-mail já está em uso por outro usuário!");

            var user = await userRepository.AtualizarUsuario(usuario);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task RemoverUsuario(Guid id)
        {
            var usuario = await userRepository.ProcurarPorId(id);

            if (usuario == null)
            {
                throw new ApplicationException("Usuário não encontrado!");
            }

            userRepository.Remover(usuario);

            await _context.SaveChangesAsync();
        }
    }
}
