using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraficadorDeSeñales {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void BtnGraficar_Click(object sender, RoutedEventArgs e) {
            double tiempoInicial = double.Parse(txtTiempoInicial.Text);
            double tiempoFinal = double.Parse(txtTiempoFinal.Text);
            double frecuenciaMuestreo = double.Parse(txtFrecuenciaMuestreo.Text);

            Señal señal;
            Señal segundaSeñal = null;
            Señal señalResultante;

            switch(cbTipoSeñal.SelectedIndex) {
                case 0: // Parabólica
                    señal = new SeñalParabolica();
                    break;
                case 1: // Senoidal
                    SeñalSenoidalUC señalSenoidalUC = (SeñalSenoidalUC)panelConfiguracion.Children[0];
                    double amplitud = double.Parse(señalSenoidalUC.txtAmplitud.Text);
                    double fase = double.Parse(señalSenoidalUC.txtFase.Text);
                    double frecuencia = double.Parse(señalSenoidalUC.txtFrecuencia.Text);

                    señal = new SeñalSenoidal(amplitud, fase, frecuencia);
                    break;
                case 2:
                    SeñalExponencialUC señalExponencialUC = (SeñalExponencialUC)panelConfiguracion.Children[0];
                    double alpha = double.Parse(señalExponencialUC.txtAlpha.Text);

                    señal = new SeñalExponencial(alpha);
                    break;
                case 3:
                    AudioUC audioUC = (AudioUC)panelConfiguracion.Children[0];
                    string rutaArchivo = audioUC.txtRutaArchivo.Text;

                    señal = new SeñalAudio(rutaArchivo);
                    txtTiempoInicial.Text = señal.TiempoInicial.ToString();
                    txtTiempoFinal.Text = señal.TiempoFinal.ToString();
                    txtFrecuenciaMuestreo.Text = señal.FrecuenciaMuestreo.ToString();
                    break;
                default:
                    señal = null;
                    break;
            }

            if (cbTipoSeñal.SelectedIndex != 3 && señal != null) {
                señal.TiempoInicial = tiempoInicial;
                señal.TiempoFinal = tiempoFinal;
                señal.FrecuenciaMuestreo = frecuenciaMuestreo;

                señal.construirSeñal();
            }

            // Construir segunda señal si es necesario
            if (cbOperacion.SelectedIndex == 2 || cbOperacion.SelectedIndex == 4) {
                switch (cbTipoSeñal_2.SelectedIndex) {
                    case 0: // Parabólica
                        segundaSeñal = new SeñalParabolica();
                        break;
                    case 1: // Senoidal
                        SeñalSenoidalUC señalSenoidalUC = (SeñalSenoidalUC)panelConfiguracion_2.Children[0];
                        double amplitud = double.Parse(señalSenoidalUC.txtAmplitud.Text);
                        double fase = double.Parse(señalSenoidalUC.txtFase.Text);
                        double frecuencia = double.Parse(señalSenoidalUC.txtFrecuencia.Text);

                        segundaSeñal = new SeñalSenoidal(amplitud, fase, frecuencia);
                        break;
                    case 2:
                        SeñalExponencialUC señalExponencialUC = (SeñalExponencialUC)panelConfiguracion_2.Children[0];
                        double alpha = double.Parse(señalExponencialUC.txtAlpha.Text);

                        segundaSeñal = new SeñalExponencial(alpha);
                        break;
                    case 3:
                        AudioUC audioUC = (AudioUC)panelConfiguracion_2.Children[0];
                        string rutaArchivo = audioUC.txtRutaArchivo.Text;

                        segundaSeñal = new SeñalAudio(rutaArchivo);
                        txtTiempoInicial.Text = segundaSeñal.TiempoInicial.ToString();
                        txtTiempoFinal.Text = segundaSeñal.TiempoFinal.ToString();
                        txtFrecuenciaMuestreo.Text = segundaSeñal.FrecuenciaMuestreo.ToString();
                        break;
                    default:
                        segundaSeñal = null;
                        break;
                }

                if (segundaSeñal != null) {
                    segundaSeñal.TiempoInicial = tiempoInicial;
                    segundaSeñal.TiempoFinal = tiempoFinal;
                    segundaSeñal.FrecuenciaMuestreo = frecuenciaMuestreo;

                    segundaSeñal.construirSeñal();
                }
            }

            switch(cbOperacion.SelectedIndex) {
                case 0: // Escala de amplitud
                    OperacionEscalaAmplitudUC operacionEscalaUC = (OperacionEscalaAmplitudUC)panelConfiguracionOperacion.Children[0];
                    double factorEscala = double.Parse(operacionEscalaUC.txtFactorEscala.Text);

                    señalResultante = Señal.escalarAmplitud(señal, factorEscala);
                    break;
                case 1: // Desplazamiento de amplitud
                    OperacionDesplazarAmplitudUC operacionDesplazarUC = (OperacionDesplazarAmplitudUC)panelConfiguracionOperacion.Children[0];
                    double cantidadDesplazaminto = double.Parse(operacionDesplazarUC.txtCantidadDesplazamiento.Text);

                    señalResultante = Señal.desplazarAmplitud(señal, cantidadDesplazaminto);
                    break;
                case 2: // Multiplicación de señales
                    señalResultante = Señal.multiplicarSeñales(señal, segundaSeñal);
                    break;
                case 3: // Escala exponencial
                    OperacionEscalaExponencialUC operacionEscalaExponencialUC = (OperacionEscalaExponencialUC)panelConfiguracionOperacion.Children[0];
                    double exponenteEscala = double.Parse(operacionEscalaExponencialUC.txtExponente.Text);

                    señalResultante = Señal.escalaExponencial(señal, exponenteEscala);
                    break;

                case 4: // Limite Superior
                    señalResultante = Señal.LimiteSuperior(señal, segundaSeñal);
                    break;
                default:
                    señalResultante = null;
                    break;
            }

            double amplitudMaxima = señal.AmplitudMaxima;
            
            plnGrafica.Points.Clear();
            plnGraficaResultante.Points.Clear();
            plnGrafica_2.Points.Clear();

            graficar(señal, tiempoInicial, tiempoFinal, amplitudMaxima,
                    plnGrafica, plnEjeX, plnEjeY, lblLimiteSuperior, lblLimiteInferior);

            if (señalResultante != null) {
                double amplitudMaximaResultado = señalResultante.AmplitudMaxima;

                if (amplitudMaxima > amplitudMaximaResultado) {
                    amplitudMaximaResultado = amplitudMaxima;
                } else {
                    amplitudMaxima = amplitudMaximaResultado;
                    graficar(señal, tiempoInicial, tiempoFinal, amplitudMaxima,
                            plnGrafica, plnEjeX, plnEjeY, lblLimiteSuperior, lblLimiteInferior);
                }

                if (segundaSeñal != null) {
                    double amplitudMaximaSegunda = segundaSeñal.AmplitudMaxima;
                    if (amplitudMaxima > amplitudMaximaSegunda) {
                        amplitudMaximaSegunda = amplitudMaxima;
                    } else {
                        amplitudMaxima = amplitudMaximaSegunda;
                    }

                    graficar(segundaSeñal, tiempoInicial, tiempoFinal, amplitudMaxima, plnGrafica_2,
                            plnEjeX, plnEjeY, lblLimiteSuperior, lblLimiteInferior);
                }

                graficar(señalResultante, tiempoInicial, tiempoFinal, amplitudMaximaResultado,
                        plnGraficaResultante, plnEjeXResultante, plnEjeYResultante,
                        lblLimiteSuperiorResultante, lblLimiteInferiorResultante);
            }
        }

        private void graficar(Señal señal, double tiempoInicial,
                    double tiempoFinal, double amplitudMaxima, Polyline polyline,
                    Polyline polylineX, Polyline polylineY,
                    TextBlock lblLimiteSuperior, TextBlock lblLimiteInferior) {
            polyline.Points.Clear();

            foreach (Muestra muestra in señal.Muestras) {
                double x = muestra.X;
                double y = muestra.Y;
                polyline.Points.Add(adaptarCoordenadas(x, y, tiempoInicial, amplitudMaxima));
            }

            lblLimiteSuperior.Text = amplitudMaxima.ToString("F");
            lblLimiteInferior.Text = "-" + amplitudMaxima.ToString("F");

            polylineX.Points.Clear();
            polylineX.Points.Add(adaptarCoordenadas(tiempoInicial, 0.0, tiempoInicial, amplitudMaxima));
            polylineX.Points.Add(adaptarCoordenadas(tiempoFinal, 0.0, tiempoInicial, amplitudMaxima));

            polylineY.Points.Clear();
            polylineY.Points.Add(adaptarCoordenadas(0.0, amplitudMaxima, tiempoInicial, amplitudMaxima));
            polylineY.Points.Add(adaptarCoordenadas(0.0, (amplitudMaxima * -1.0), tiempoInicial, amplitudMaxima));
        }

        public Point adaptarCoordenadas(double x, double y, double tiempoInicial, double amplitudMaxima) {
            double anchuraGrafica = scrGrafica.Width;
            double alturaGrafica = scrGrafica.Height;

            double xEscalada = (x - tiempoInicial) * anchuraGrafica;
            double yEscalada = -y * (((alturaGrafica / 2.0) - 25.0) / amplitudMaxima) + (alturaGrafica / 2.0);

            return new Point(xEscalada, yEscalada);
        }

        private void CbTipoSeñal_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            panelConfiguracion.Children.Clear();

            switch(cbTipoSeñal.SelectedIndex) {
                case 0: // Parabólica
                    break;
                case 1: // Senoidal
                    panelConfiguracion.Children.Add(new SeñalSenoidalUC());
                    break;
                case 2: // Exponencial
                    panelConfiguracion.Children.Add(new SeñalExponencialUC());
                    break;
                case 3: // Audio
                    panelConfiguracion.Children.Add(new AudioUC());
                    break;
                default:
                    break;
            }
        }

        private void CbOperacion_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            panelConfiguracionOperacion.Children.Clear();
            switch (cbOperacion.SelectedIndex) {
                case 0: // Escala de amplitud
                    panelConfiguracionOperacion.Children.Add(new OperacionEscalaAmplitudUC());
                    mostrarSegundaSeñal(false);
                    break;
                case 1: // Desplazamiento de amplitud
                    panelConfiguracionOperacion.Children.Add(new OperacionDesplazarAmplitudUC());
                    mostrarSegundaSeñal(false);
                    break;
                case 2: // Multiplicación de señales
                    mostrarSegundaSeñal(true);
                    break;
                case 3: // Escala exponencial
                    panelConfiguracionOperacion.Children.Add(new OperacionEscalaExponencialUC());
                    mostrarSegundaSeñal(false);
                    break;
                case 4: // Limite Superior
                    mostrarSegundaSeñal(true);
                    break;
                default:
                    mostrarSegundaSeñal(false);
                    break;
            }
        }

        private void CbTipoSeñal_2_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            panelConfiguracion_2.Children.Clear();

            switch (cbTipoSeñal_2.SelectedIndex) {
                case 0: // Parabólica
                    break;
                case 1: // Senoidal
                    panelConfiguracion_2.Children.Add(new SeñalSenoidalUC());
                    break;
                case 2: // Exponencial
                    panelConfiguracion_2.Children.Add(new SeñalExponencialUC());
                    break;
                case 3: // Audio
                    panelConfiguracion_2.Children.Add(new AudioUC());
                    break;
                default:
                    break;
            }
        }

        private void mostrarSegundaSeñal(bool mostrar) {
            if (mostrar) {
                lblTipoSeñal_2.Visibility = Visibility.Visible;
                cbTipoSeñal_2.Visibility = Visibility.Visible;
                panelConfiguracion_2.Visibility = Visibility.Visible;
            } else {
                lblTipoSeñal_2.Visibility = Visibility.Hidden;
                cbTipoSeñal_2.Visibility = Visibility.Hidden;
                panelConfiguracion_2.Visibility = Visibility.Hidden;
            }
        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}
