﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorDeSeñales {
    class SeñalExponencial : Señal {
        public double Alpha { get; set; }

        public SeñalExponencial() {
            Alpha = 0.0;

            Muestras = new List<Muestra>();
            AmplitudMaxima = 0.0;
        }

        public SeñalExponencial(double alpha) {
            Alpha = alpha;

            Muestras = new List<Muestra>();
            AmplitudMaxima = 0.0;
        }

        override public double evaluar(double tiempo) {
            double resultado;

            resultado = Math.Exp(Alpha * tiempo);

            return resultado;
        }
    }
}
