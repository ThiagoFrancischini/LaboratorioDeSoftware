using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.DTOs.Filtros;
using LaboratorioDeSoftware.Core.Entities;
using LaboratorioDeSoftware.Core.Repositories;
using static LaboratorioDeSoftware.Core.Entities.Enums.Enums;

namespace LaboratorioDeSoftware.Core.Services
{
    public class ManutencaoCorretivaService
    {
        private readonly AppDbContext _context;
        private readonly ManutencaoCorretivaRepository _manutencaoRepository;
        private readonly UsuarioRepository  _userRepository;
        public ManutencaoCorretivaService(AppDbContext context)
        {
            _context = context;
            _manutencaoRepository = new ManutencaoCorretivaRepository(context);
            _userRepository = new UsuarioRepository(context);
        }

        public async Task<List<ManutencaoCorretiva>> ProcurarTodos(EventoFiltroDTO filtro)
        {
            if (filtro.UsuarioId != null && filtro.UsuarioId != Guid.Empty)
            {
                var user = await _userRepository.ProcurarPorId(filtro.UsuarioId ?? Guid.Empty);

                if (user.TipoUsuario != Entities.Enums.Enums.enTipoUsuario.Administrador)
                {
                    filtro.LaboratorioId = user.LaboratorioId;
                }
            }

            return await _manutencaoRepository.ProcurarTodos(filtro);
        }

        public async Task<ManutencaoCorretiva> ProcurarPorId(Guid id)
        {
            return await _manutencaoRepository.ProcurarPorId(id);
        }

        public async Task<ManutencaoCorretiva> Inserir(ManutencaoCorretiva manutencao)
        {
            if (manutencao.EquipamentoId == Guid.Empty)
                throw new ApplicationException("Informe um equipamento válido!");

            manutencao.Id = Guid.NewGuid();

            await _manutencaoRepository.Inserir(manutencao);
            await _context.SaveChangesAsync();

            return manutencao;
        }

        public async Task<ManutencaoCorretiva> Alterar(ManutencaoCorretiva manutencao)
        {
            if (manutencao.EquipamentoId == Guid.Empty)
                throw new ApplicationException("Informe um equipamento válido!");

            return await _manutencaoRepository.Alterar(manutencao);
        }

        public async Task Remover(ManutencaoCorretiva manutencao)
        {
            _manutencaoRepository.Remover(manutencao);
            await _context.SaveChangesAsync();
        }
    }
}