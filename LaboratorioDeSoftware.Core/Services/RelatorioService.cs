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
        private readonly UsuarioRepository _userRepository;

        public RelatorioService(AppDbContext context)
        {
            _context = context;
            _relatorioRepository = new RelatorioRepository(context);
            _userRepository = new UsuarioRepository(context);
        }

        public async Task<RelatorioDTO> BuscarRelatorio (RelatorioFiltroDTO filtro)
        {
            if (filtro.UserId != null && filtro.UserId != Guid.Empty)
            {
                var user = await _userRepository.ProcurarPorId(filtro.UserId ?? Guid.Empty);

                if (user.TipoUsuario != Entities.Enums.Enums.enTipoUsuario.Administrador)
                {
                    filtro.LaboratorioId = user.LaboratorioId;
                }
            }

            return await _relatorioRepository.BuscarRelatorio(filtro);
        }
    }
}
