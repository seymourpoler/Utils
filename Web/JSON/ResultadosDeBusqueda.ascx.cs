using System;
using System.Web.UI;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Entidades;
using Representacion;
using GestionExcepciones;

namespace Web.ControlTemplates
{
    public partial class ResultadosDeBusqueda : UserControl, IResultadosDeBusquedaVista
    {
        private GestorExcepciones _gestorDeError;
        private ResultadosDeBusquedaPresentador _presentador = null;
        protected ResultadosDeBusquedaPresentador Presentador
        {
            get
            {
                if (_presentador == null)
                {
                    _presentador = new ResultadosDeBusquedaPresentador(this);

                }
                return _presentador;
            }
        }
        public string Url { get { return SPContext.Current.Web.Url; } }
        public SPPrincipal UsuarioLogueado { get { return SPContext.Current.Web.CurrentUser; } }
        public string EtiquetaInformacion { set { lblInfo.Text = value; } }

        public ListGeneric<ElementoSistema> DatosElementosDelSistema
        {
            set
            {
                Response.ContentType = "application/json";
                Response.Write(value.ToJsonWithOptimization());
                Response.End();
            }
        }

        public ListGeneric<ModuloParaDesplegable> NombreModulosActivos
        {
            set
            {
                Response.ContentType = "application/json";
                Response.Write(value.ToJSON());
                Response.End();
            }
        }

        public ResultadosDeBusqueda()
        {
            _gestorDeError = new GestorExcepciones("Web.ControlTemplates", "ResultadosDeBusqueda");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string accion = Request["accion"];
                switch (accion)
                {
                    case "Buscar":
                        Buscar();
                        break;
                    case "CargarAmbitos":
                        CargarAmbitos();
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

        private void Buscar()
        {
            try
            {
                string ambito = Request["ambito"];
                string loQueBuscar = Request["loQueBuscar"];

                Presentador.CargarResultadosDeLaBusqueda(new Busqueda() { Texto = loQueBuscar, Ambito = ambito });
            }
            catch (System.Threading.ThreadAbortException th) { }
            catch (Exception ex)
            {
                _gestorDeError.TratarExcepcion(ex,
                                                   ex.ToString(),
                                                   "Buscar");
            }
        }

        private void CargarAmbitos()
        { 
            try
            {
                Presentador.CargarModulosActivos();
            }
            catch (System.Threading.ThreadAbortException th) { }
            catch (Exception ex)
            {
                _gestorDeError.TratarExcepcion(ex,
                                                   ex.ToString(),
                                                   "CargarAmbitos");
            }
        }
    }
}
