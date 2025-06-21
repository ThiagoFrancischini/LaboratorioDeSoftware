using System.ComponentModel.DataAnnotations;

namespace LaboratorioDeSoftware.Core.Entities
{
    public class TagEquipamento
    {
        public Guid Id { get; set; }
        public string Tag { get; set; }
        public Guid EquipamentoId { get; set; }
        public virtual Equipamento Equipamento { get; set; }
    }
}