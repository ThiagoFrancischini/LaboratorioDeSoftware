using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using LaboratorioDeSoftware.Core.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratorioDeSoftware.Core.Repositories
{
    public class LaboratorioRepository(AppDbContext _context)
    {
        public async Task<List<Laboratorio>> ProcurarTodos(Usuario usuario)
        {
            if(usuario.TipoUsuario == Enums.enTipoUsuario.Administrador)
            {
                return await _context.Laboratorios.Include(x => x.Responsavel).OrderBy(x => x.Nome).ToListAsync();
            }

            return await _context.Laboratorios
                .Include(x => x.Responsavel)
                .Where(x => x.Id == usuario.LaboratorioId)
                .OrderBy(x => x.Nome)
                .ToListAsync();
        }

        public async Task<Laboratorio> ProcurarPorId(Guid id)
        {
            return await _context.Laboratorios.FindAsync(id);
        }

        public async Task<Laboratorio> Inserir(Laboratorio laboratorio)
        {
            await _context.Laboratorios.AddAsync(laboratorio);

            return laboratorio;
        }

        public async Task<Laboratorio> Alterar(Laboratorio laboratorio)
        {
            var labExistente = await ProcurarPorId(laboratorio.Id);

            if (labExistente == null)
                throw new ApplicationException("Laboratorio não encontrado!");

            labExistente.Nome = laboratorio.Nome;
            labExistente.Responsavel = laboratorio.Responsavel;
            labExistente.ResponsavelId = laboratorio.ResponsavelId;
            labExistente.Observacao = laboratorio.Observacao;
            labExistente.Localizacao = laboratorio.Localizacao;

            await _context.SaveChangesAsync();
            return labExistente;
        }

        public void Remover(Laboratorio lab)
        {
            _context.Laboratorios.Remove(lab);
        }
    }
}
