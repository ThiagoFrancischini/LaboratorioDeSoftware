using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using LaboratorioDeSoftware.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratorioDeSoftware.Core.Services
{
public class LaboratorioService
{
    private AppDbContext _context;
    private LaboratorioRepository labRepository;
    private UsuarioRepository userRepository;
    public LaboratorioService(AppDbContext context)
    {
        _context = context;
        labRepository = new LaboratorioRepository(context);
        userRepository = new UsuarioRepository(context);
    }

    public async Task<List<Laboratorio>> ProcurarTodos()
    {
        return await labRepository.ProcurarTodos();
    }

    public async Task<Laboratorio> ProcurarPorId(Guid id)
    {
        return await labRepository.ProcurarPorId(id);
    }

    public async Task<Laboratorio> Inserir(Laboratorio laboratorio)
    {
        laboratorio.Id = Guid.NewGuid();

        laboratorio.Validar();

        if (laboratorio.Responsavel == null && laboratorio.ResponsavelId != Guid.Empty)
        {
            laboratorio.Responsavel = await userRepository.ProcurarPorId(laboratorio.ResponsavelId);
        }

        await labRepository.Inserir(laboratorio);

        await _context.SaveChangesAsync();

        return laboratorio;
    }

    public async Task<Laboratorio> Alterar(Laboratorio laboratorio)
    {
        await labRepository.Alterar(laboratorio);

        return laboratorio;
    }

    public async Task Remover(Laboratorio lab)
    {
        _context.Laboratorios.Remove(lab);

        await _context.SaveChangesAsync();
    }
}
}
