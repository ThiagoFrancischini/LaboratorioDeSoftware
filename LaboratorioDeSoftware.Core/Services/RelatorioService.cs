using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.DTOs;
using LaboratorioDeSoftware.Core.DTOs.Filtros;
using LaboratorioDeSoftware.Core.Entities;
using LaboratorioDeSoftware.Core.Repositories;
using ProdutoDeSoftware.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LaboratorioDeSoftware.Core.Services
{
    public class RelatorioService
    {
        private readonly AppDbContext _context;
        private readonly RelatorioRepository _relatorioRepository;

        public RelatorioService(AppDbContext context)
        {
            _context = context;
            _relatorioRepository = new RelatorioRepository(context);
        }

        public async Task<RelatorioDTO> BuscarRelatorio (RelatorioFiltroDTO filtro)
        {
            return await _relatorioRepository.BuscarRelatorio(filtro);
        }
    }
}
