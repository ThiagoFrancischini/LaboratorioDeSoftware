using LaboratorioDeSoftware.Core.Entities.Enums;

namespace LaboratorioDeSoftware.DTOs
{
    public class EventoPendenteDTO
    {
        public Guid EquipamentoId { get; set; }
        public string NomeEquipamento { get; set; }
        public long PatrimonioEquipamento { get; set; }
        public string NomeLaboratorio { get; set; }
        public DateTime ProximoEventoPrevisto { get; set; }
        public Enums.enTipoEvento TipoEvento { get; set; }
        public int DiasFaltantes { get; set; }
    }
}
