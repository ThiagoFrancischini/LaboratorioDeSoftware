using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LaboratorioDeSoftware.Core.Services
{
    public class ConfiguracaoService
    {
        private readonly AppDbContext _context;
        private const int CONFIG_ID = 1;

        public ConfiguracaoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ConfiguracaoSistema> GetConfiguracoesAsync()
        {
            var config = await _context.ConfiguracoesSistema.FindAsync(CONFIG_ID);

            if (config == null)
            {
                config = new ConfiguracaoSistema
                {
                    Id = CONFIG_ID,
                    DiasDeAntecedenciaAvisos = 15
                };
                _context.ConfiguracoesSistema.Add(config);
                await _context.SaveChangesAsync();
            }

            return config;
        }

        public async Task SalvarConfiguracoesAsync(ConfiguracaoSistema model)
        {
            model.Id = CONFIG_ID;

            _context.ConfiguracoesSistema.Update(model);
            await _context.SaveChangesAsync();
        }
    }
}