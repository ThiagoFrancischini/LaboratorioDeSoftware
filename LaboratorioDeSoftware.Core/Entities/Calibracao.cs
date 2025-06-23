using static LaboratorioDeSoftware.Core.Entities.Enums.Enums;

namespace LaboratorioDeSoftware.Core.Entities
{
    public class Calibracao
    {
        public Guid Id { get; set; }
        public Guid EquipamentoId { get; set; }
        public Equipamento Equipamento { get; set; }
        public string GrandezaParametro { get; set; }
        public DateTime DataCalibracao { get; set; }
        public DateTime DataAcompanhamento { get; set; }
        public double Erro { get; set; }
        public double Incerteza { get; set; }
        public enStatusCalibracoes Status { get; set; }
        public string Observacoes { get; set; }
        public bool CS { get; set; }
        public long NumeroSolicitacao { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public int Custo { get; set; }
    }
}