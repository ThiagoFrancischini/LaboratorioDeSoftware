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
            usuario.Id = Guid.NewGuid();
            usuario.Validar();          
            return await userRepository.Registrar(usuario);
        }

        public async Task<Usuario> Autenticar(string login, string senha)
        {            
            if(string.IsNullOrEmpty(login) || !login.Contains('@'))
            {
                throw new ApplicationException("Informe um Email válido!");
            }

            if(string.IsNullOrEmpty(senha))
            {
                throw new ApplicationException("Informe uma senha válida!");
            }   

            senha = Convert.ToBase64String(Encoding.UTF8.GetBytes(senha));

            var usuario = await userRepository.Autenticar(login, senha);

            if(usuario == null)
            {
                throw new ApplicationException("Usuário ou senha inválidos!");
            }

            return usuario;            
        }
    }
}
