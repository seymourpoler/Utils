
using System;
using System.Web.UI;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Entidades;
using Representacion;
using Comunes.Log.GestionExcepciones;

namespace Web.ControlTemplates
{
    public partial class EvolucionIndicadores : UserControl, IEvolucionIndicadorVista
    {
        private GestorExcepciones _gestorDeError;
        private EvolucionIndicadorPresentador _presentador = null;
        protected EvolucionIndicadorPresentador Presentador
        {
            get
            {
                if (_presentador == null)
                {
                    _presentador = new EvolucionIndicadorPresentador(this);
                }
                return _presentador;
            }
        }
        public string Anio { get; set; }
        public string Mes { get; set; }
        public string Url { get { return SPContext.Current.Web.Url; } }
        public string EtiquetaInformacion { set { lblInfo.Text = value; } }
        public string NombreIndicador { set { lblCabeceraNombreDelIndicador.Text = value; } }
        public ListGeneric<EvolucionMensual> DatosSeguimientoMensualDeIndicadores
        {
            set
            {
                Response.ContentType = "application/json";
                Response.Write(value.ToJSON());
                Response.End();
            }
        }

        public EvolucionIndicadores()
        {
            _gestorDeError = new GestorExcepciones("Web.ControlTemplates", "EvolucionIndicadores");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string accion = Request["accion"];
                Guid guidIndicador = new Guid(Request["guidIndicador"]);
                string mes = Request["mes"];
                string anio = Request["anio"];

                if (!Page.IsPostBack)
                {
                    CargarNombreIndicador(guidIndicador);
                }

                
                switch (accion)
                {
                    case "CargarEvolucionesMensuales":
                            CargarEvolucionesMensuales(guidIndicador);
                        break;
                    case "CargarEvolucionesMensualesDelMismoAnio":
                            CargarEvolucionesMensualesDelMismoAnio(guidIndicador, anio, mes);
                        break;
                    case "CargarEvolucionesMensualesDelAnioAnterior":
                            CargarEvolucionesMensualesDelAnioAnterior(guidIndicador, anio, mes);
                        break;
                }
            }
            catch (System.Threading.ThreadAbortException th) { }
            catch (Exception ex)
            {
                Presentador.PintarError();
                _gestorDeError.TratarExcepcion(ex,
                                                   "Fallo en la carga de la página",
                                                   "Page_Load");
            }
        }

        private void CargarEvolucionesMensuales(Guid guidIndicador)
        {
            try
            {

                Presentador.CargarEvolucionesMensualesPorGuidIndicador(new EvolucionMensual() { GuidIndicador = guidIndicador });
            }
            catch (System.Threading.ThreadAbortException e) { }
            catch (Exception ex)
            {
                _gestorDeError.TratarExcepcion(ex,
                                                   ex.ToString(),
                                                   "CargarEvolucionesMensuales");
            }
        }

        private void CargarEvolucionesMensualesDelMismoAnio(Guid guidIndicador, string anio, string mes)
        {
            try
            {
                Presentador.CargarEvolucionesMensualesPorGuidIndicadoryAnioyMes(new EvolucionMensual()
                {
                    GuidIndicador = guidIndicador,
                    Anio = anio,
                    Mes = mes
                });
            }
            catch (System.Threading.ThreadAbortException e) { }
            catch (Exception ex)
            {
                _gestorDeError.TratarExcepcion(ex,
                                                   ex.ToString(),
                                                   "CargarEvolucionesMensualesDelMismoAnio");
            }
        }

        private void CargarEvolucionesMensualesDelAnioAnterior(Guid guidIndicador, string anio, string mes)
        {
            try
            {
                Presentador.CargarEvolucionesMensualesAnterioresPorGuidIndicadoryAnioAnterioryMes(new EvolucionMensual()
                {
                    GuidIndicador = guidIndicador,
                    Anio = anio,
                    Mes = mes
                });
            }
            catch (System.Threading.ThreadAbortException e) { }
            catch (Exception ex)
            {
                _gestorDeError.TratarExcepcion(ex,
                                                   ex.ToString(),
                                                   "CargarEvolucionesMensualesAnterioresPorGuidIndicadoryAnioAnterioryMes");
            }
        }

        private void CargarNombreIndicador(Guid guidIndicador)
        { 
            try
            {
                Presentador.CargarNombreIndicador(guidIndicador);
            }
            catch (Exception ex)
            {
                _gestorDeError.TratarExcepcion(ex,
                                                   ex.ToString(),
                                                   "CargarNombreIndicador");
            }
        }
    }
}
