using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratorioDeSoftware.Core.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Cargo { get; set; }
        public Entities.Enums.Enums.enTipoUsuario TipoUsuario { get; set; }
        private string senha;
        public string Senha
        {
            get
            {
                if (string.IsNullOrEmpty(senha))
                {
                    return senha;
                }

                return Convert.ToBase64String(Encoding.UTF8.GetBytes(senha));
            }
            set
            {
                senha = value;
            }
        }
        public void Validar()
        {
            if(Id == Guid.Empty)
            {
                throw new ApplicationException("O id precisa ser informado!");
            }

            if (string.IsNullOrEmpty(Email))
            {
                throw new ApplicationException("O Email precisa ser informado!");
            }

            if (string.IsNullOrEmpty(Senha))
            {
                throw new ApplicationException("O Email precisa ser informado!");
            }
        }
    }
}
