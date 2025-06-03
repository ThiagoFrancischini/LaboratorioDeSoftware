using System;
using System.Collections.Generic;
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
            NaoSeAplica = 0,
            Analogico = 1,
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
    }
}
