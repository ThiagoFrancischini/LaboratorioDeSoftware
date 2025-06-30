using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratorioDeSoftware.Core.Entities
{
    public class Laboratorio
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Localizacao { get; set; }
        public Guid ResponsavelId { get; set; }
        public Usuario Responsavel { get; set; }
        public string Observacao { get;set; }
        public ICollection<Equipamento> Equipamentos { get; set; } = new List<Equipamento>();
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public void Validar()
        {
            if (Id == Guid.Empty)
            {
                throw new ApplicationException("Informe um Id valido!");
            }

            if (string.IsNullOrEmpty(Nome))
            {
                throw new ApplicationException("Informe um nome valido!");
            }

            if (string.IsNullOrEmpty(Localizacao))
            {
                throw new ApplicationException("Informe uma localização valido!");
            }

            if (ResponsavelId == Guid.Empty)
            {
                throw new ApplicationException("Informe um responsavel valido!");
            }

            if(this.Observacao == null)
            {
                this.Observacao = "";
            }
        }
    }
}
