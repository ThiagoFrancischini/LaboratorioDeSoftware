using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratorioDeSoftware.Core.DTOs.Filtros
{
    public class ProdutoFiltroDTO
    {
        public string? Nome { get; set; }        
        public string? MarcaFabricante { get; set; }
        public string? Modelo { get; set; }       
        public Guid? CategoriaId { get; set; }    
    }
}
