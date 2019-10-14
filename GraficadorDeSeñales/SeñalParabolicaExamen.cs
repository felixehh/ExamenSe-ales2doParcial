using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorDeSeñales {
    class SeñalParabolicaExamen {
        public List<Muestra> Muestras { get; set; }

        public SeñalParabolicaExamen() {
            Muestras = new List<Muestra>();
        }

        public double evaluar(double tiempo) {
            double resultado;

            resultado = tiempo * tiempo;

            return resultado;
        }
    }
}
