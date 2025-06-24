using System.ComponentModel.DataAnnotations;

namespace LaboratorioDeSoftware.Core.Entities;
public class ConfiguracaoSistema
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Dias de antecedência para avisos")]
    public int DiasDeAntecedenciaAvisos { get; set; }
}