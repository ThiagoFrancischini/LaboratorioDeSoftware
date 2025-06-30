using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratorioDeSoftware.Core.DTOs.Filtros
{    
    public enum DisponibilidadeFiltro
    {
        Todos = 0,
        Sim = 1,
        Nao = 2
    }

    public class EquipamentoFiltroDTO
    {
        public Guid? LaboratorioId { get; set; }
        public DisponibilidadeFiltro Disponivel { get; set; } = DisponibilidadeFiltro.Todos; 

        public string? NomeEquipamento { get; set; }
        public Guid? CategoriaId { get; set; }
        public Guid? UserId { get;set; }
    }    
}
