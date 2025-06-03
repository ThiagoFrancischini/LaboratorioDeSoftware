using static LaboratorioDeSoftware.Core.Entities.Enums.Enums;

namespace LaboratorioDeSoftware.Core.Entities
{
    public class Produto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string MarcaFabricante { get; set; }
        public string Modelo { get; set; }
        public string CACalibracao { get; set; }
        public string CAVerificacoes { get; set; }
        public string CapaciadadeMedicao { get; set; }
        public enPeriodicidade PeriodicidadeCalibracao { get; set; }
        public enPeriodicidade PeriodicidadeVerificacaoIntermediaria { get; set; }
        public string ResolucaoDivisaoEscala { get; set; }
        public enTipoProduto TipoProduto { get; set; }
        public string Observacoes { get; set; }

        public void Validar()
        {
            if (Id == Guid.Empty)
                throw new ApplicationException("Informe um Id válido!");

            if (string.IsNullOrWhiteSpace(Nome))
                throw new ApplicationException("O nome do produto é obrigatório.");

            if (string.IsNullOrWhiteSpace(MarcaFabricante))
                throw new ApplicationException("A marca/fabricante é obrigatória.");

            if (string.IsNullOrWhiteSpace(Modelo))
                throw new ApplicationException("O modelo é obrigatório.");

            if (string.IsNullOrWhiteSpace(CACalibracao))
                throw new ApplicationException("O campo CA de calibração é obrigatório.");

            if (string.IsNullOrWhiteSpace(CAVerificacoes))
                throw new ApplicationException("O campo CA de verificações é obrigatório.");

            if (string.IsNullOrWhiteSpace(CapaciadadeMedicao))
                throw new ApplicationException("A capacidade de medição é obrigatória.");

            if (!Enum.IsDefined(typeof(enPeriodicidade), PeriodicidadeCalibracao))
                throw new ApplicationException("A periodicidade de calibração é inválida.");

            if (!Enum.IsDefined(typeof(enPeriodicidade), PeriodicidadeVerificacaoIntermediaria))
                throw new ApplicationException("A periodicidade de verificação intermediária é inválida.");

            if (string.IsNullOrWhiteSpace(ResolucaoDivisaoEscala))
                throw new ApplicationException("A resolução/divisão de escala é obrigatória.");

            if (!Enum.IsDefined(typeof(enTipoProduto), TipoProduto))
                throw new ApplicationException("O tipo de produto é inválido.");
        }
    }
}
