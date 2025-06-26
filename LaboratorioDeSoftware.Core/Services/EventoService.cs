using LaboratorioDeSoftware.Core.DTOs;
using LaboratorioDeSoftware.Core.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaboratorioDeSoftware.Core.Entities;

namespace LaboratorioDeSoftware.Core.Services
{
    public class EventoService
    {
        private readonly AppDbContext _context;
        private readonly ConfiguracaoService _configuracaoService;
        private readonly UsuarioService _usuarioService;
        public EventoService(AppDbContext context)
        {
            _context = context;
            _usuarioService = new UsuarioService(context);
            _configuracaoService = new ConfiguracaoService(context);
        }

        public async Task<List<EventoPendenteDTO>> GetAvisosDeCalibracaoAsync(Guid userId)
        {
            var config = await _configuracaoService.GetConfiguracoesAsync();
            var diasAntecedenciaAviso = config.DiasDeAntecedenciaAvisos;

            var avisos = new List<EventoPendenteDTO>();

            var user = await _usuarioService.ProcurarPorId(userId);

            IQueryable<Equipamento> equipamentosQuery = _context.Equipamentos
                .Include(e => e.Laboratorio)
                .Include(e => e.Calibracoes);

            //SE FOR ADMIN BUSCA TODOS, SE NÃO SÓ OS DO SEU LABORATORIO
            if(user.TipoUsuario != Entities.Enums.Enums.enTipoUsuario.Administrador)
            {
                equipamentosQuery = equipamentosQuery.Where(e => e.LaboratorioId == user.LaboratorioId); 
            }

            var equipamentos = await equipamentosQuery.ToListAsync();

            foreach (var equipamento in equipamentos)
            {
                var ultimaCalibracao = equipamento.Calibracoes
                    .OrderByDescending(c => c.DataCalibracao)
                    .FirstOrDefault();

                DateTime dataBase = ultimaCalibracao?.DataCalibracao ?? equipamento.DataColocacaoUso;

                DateTime proximaCalibracao = dataBase.AddDays(equipamento.PeriodicidadeCalibracao);
                int diasFaltantes = (proximaCalibracao.Date - DateTime.Today).Days;

                if (diasFaltantes <= diasAntecedenciaAviso)
                {
                    avisos.Add(new EventoPendenteDTO
                    {
                        EquipamentoId = equipamento.Id,
                        NomeEquipamento = equipamento.Nome,
                        PatrimonioEquipamento = equipamento.Num_Patrimonio,
                        NomeLaboratorio = equipamento.Laboratorio?.Nome ?? "N/D",
                        ProximoEventoPrevisto = proximaCalibracao,
                        TipoEvento = Entities.Enums.Enums.enTipoEvento.Calibracao,
                        DiasFaltantes = diasFaltantes
                    });
                }
            }

            return avisos.OrderBy(a => a.DiasFaltantes).ToList();
        }
    }
}
