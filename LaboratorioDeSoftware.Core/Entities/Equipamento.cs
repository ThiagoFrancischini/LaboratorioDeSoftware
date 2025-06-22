namespace LaboratorioDeSoftware.Core.Entities
{
    public class Equipamento
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public long Numero_Serie { get; set; }
        public long Num_Patrimonio { get; set; }
        public string CACalibracao { get; set; }
        public string CAVerificacao { get; set; }
        public string CapacidadeMedicao { get; set; }
        public int PeriodicidadeCalibracao { get; set; } //DIAS
        public int PeriodicidadeVerificacaoIntermediaria { get; set; } //DIAS
        public string ResolucaoDivisaoEscala { get; set; }
        public Guid ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public Guid LaboratorioId { get; set; }
        public Laboratorio Laboratorio { get; set; }
        public DateTime DataColocacaoUso { get; set; }
        public long NumeroCertificadoCalibracao { get; set; }
        public bool Disponivel { get; set; }
        public virtual ICollection<TagEquipamento> Tags { get; set; } = new List<TagEquipamento>();
        public virtual ICollection<Calibracao> Calibracoes { get; set; } = new List<Calibracao>();
    }   
}