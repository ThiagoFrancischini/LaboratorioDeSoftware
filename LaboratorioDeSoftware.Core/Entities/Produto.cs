using static LaboratorioDeSoftware.Core.Entities.Enums.Enums;

namespace LaboratorioDeSoftware.Core.Entities
{
    public class Produto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string MarcaFabricante { get; set; }
        public string Modelo { get; set; }
        public enTipoProduto TipoProduto { get; set; }
        public string Observacoes { get; set; }
        public ICollection<Equipamento> Equipamentos { get; set; } = new List<Equipamento>();
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

            if (!Enum.IsDefined(typeof(enTipoProduto), TipoProduto))
                throw new ApplicationException("O tipo de produto é inválido.");
        }
    }
}
