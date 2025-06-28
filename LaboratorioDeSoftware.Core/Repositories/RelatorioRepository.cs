using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.DTOs;
using LaboratorioDeSoftware.Core.DTOs.Filtros;
using LaboratorioDeSoftware.Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace LaboratorioDeSoftware.Core.Repositories
{
    public class RelatorioRepository (AppDbContext _context)
    {
        public async Task<RelatorioDTO> BuscarRelatorio (RelatorioFiltroDTO filtro)
        {
            IQueryable<Calibracao> queryCalibracoes = _context.Calibracoes
                .Include(x => x.Equipamento);
            
            IQueryable<ManutencaoCorretiva> queryManutencoes = _context.ManutencoesCorretivas
                .Include(x => x.Equipamento);

            if (filtro.LaboratorioId != null && filtro.LaboratorioId.HasValue)
            {
                queryCalibracoes = queryCalibracoes.Where(x => x.Equipamento.LaboratorioId == filtro.LaboratorioId);
                queryManutencoes = queryManutencoes.Where(x => x.Equipamento.LaboratorioId == filtro.LaboratorioId);
            }
            
            if (filtro.EquipamentoId != null && filtro.EquipamentoId.HasValue)
            {
                queryCalibracoes = queryCalibracoes.Where(x => x.Equipamento.Id == filtro.EquipamentoId);
                queryManutencoes = queryManutencoes.Where(x => x.Equipamento.Id == filtro.EquipamentoId);
            }

            if (filtro.DataInicio != null && filtro.DataInicio.HasValue)
            {
                queryCalibracoes = queryCalibracoes.Where(x => x.DataCalibracao >= filtro.DataInicio);
                queryManutencoes = queryManutencoes.Where(x => x.DataProblemaApresentado  >= filtro.DataInicio);
            }

            if (filtro.DataFim != null && filtro.DataFim.HasValue)
            {
                queryCalibracoes = queryCalibracoes.Where(x => x.DataCalibracao < filtro.DataFim);
                queryManutencoes = queryManutencoes.Where(x => x.DataProblemaApresentado < filtro.DataFim);
            }

            var custosCalibracao = queryCalibracoes.Sum(x => x.Custo);
            var custosManutencao = queryManutencoes.Sum(x => x.Valor);

            var relatorioDTO = new RelatorioDTO();

            relatorioDTO.CustoCalibracoes = custosCalibracao;
            relatorioDTO.CustoManutencoes = custosManutencao;
            relatorioDTO.CustoTotal = custosCalibracao + custosManutencao;

            return relatorioDTO;
        }
    }
}

