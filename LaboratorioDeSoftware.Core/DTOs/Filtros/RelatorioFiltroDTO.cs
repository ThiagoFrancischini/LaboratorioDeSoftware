using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratorioDeSoftware.Core.DTOs.Filtros
{
    public class RelatorioFiltroDTO
    {
        public Guid? LaboratorioId { get; set; }
        public Guid? EquipamentoId { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}
