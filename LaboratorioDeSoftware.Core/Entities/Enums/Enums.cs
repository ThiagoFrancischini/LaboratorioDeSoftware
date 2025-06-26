using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratorioDeSoftware.Core.Entities.Enums
{
    public class Enums
    {
        public enum enTipoUsuario
        {
            Administrador = 0,
            Responsavel = 1,
            Operador = 2
        }

        public enum enTipoProduto
        {
            [Description("Não se aplica")]
            NaoSeAplica = 0,
            [Description("Analógico")]
            Analogico = 1,
            [Description("Digital")]
            Digital = 2,
        }

        public enum enPeriodicidade
        {
            Diario = 0,
            Semanal = 1,
            Bisemanal = 2,
            Quinzenal = 3,
            Mensal = 4,
            Bimestral = 5,
            Trimestral = 6,
            Quadrimestral = 7,
            Semestral = 8,
            Anual = 9,
            Bianual = 10,
            Trianual = 11,
        }

        public enum enStatusCalibracoes
        {
            [Description("Reprovado")]
            Reprovado = 0,
            [Description("Aprovado")]
            Aprovado = 1,
        }

        public enum enStatusManutencoes
        {
            [Description("Reprovado")]
            Reprovado = 0,
            [Description("Aprovado")]
            Aprovado = 1,
        }

        public enum  enTipoEvento
        {
            [Description("Calibração")]
            Calibracao = 0,
            [Description("Manutenção")]
            Manutencao = 1
        }
    }
}
