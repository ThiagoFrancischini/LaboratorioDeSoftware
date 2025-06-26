using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratorioDeSoftware.Core.DTOs.Filtros
{
    public class EquipamentoFiltroDTO
    {
        public Guid? LaboratorioId { get; set; }
        public bool? Disponivel { get; set; }
    }
}
