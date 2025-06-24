using LaboratorioDeSoftware.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratorioDeSoftware.Core.Entities
{
    public class ManutencaoCorretiva
    {
        public Guid Id { get; set; }
        public Guid EquipamentoId { get; set; }
        public Equipamento Equipamento { get; set; }
        public DateTime DataProblemaApresentado { get; set; }
        public string ProblemaApresentado { get; set; }
        public DateTime DataRetorno { get; set; }
        public string EstadoRetorno { get;set; }
        public Enums.Enums.enStatusManutencoes Status { get; set; }
        public decimal Valor {  get; set; }
    }
}
