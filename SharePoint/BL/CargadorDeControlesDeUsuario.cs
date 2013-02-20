using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Log.GestionExcepciones;

namespace Presentacion.CargadorDeControlesDeUsuario
{
    [ToolboxItemAttribute(false)]
    public class CargadorDeControlesDeUsuario : WebPart
    {
        private Control _childControl = null;
        private string _userControlVirtualPath = string.Empty;
        private string _errMessage = string.Empty;
        private GestorExcepciones gestorDeError = null;

        public CargadorDeControlesDeUsuario()
        {
            this.ExportMode = WebPartExportMode.All;
            gestorDeError = new GestorExcepciones("Presentacion", "CargadorDeControlesDeUsuario");
        }

        [Personalizable(),
         Category("Miscellaneous"),
         DefaultValue("~/_CONTROLTEMPLATES/Presentacion/xxx.ascx"),
         WebBrowsable(true),
         WebDisplayName("Ruta del Control de Usuario"),
         WebDescription("Ruta donde se encuentra físicamente el Control de Usuario, por ejemplo: ~/_CONTROLTEMPLATES/CarpetaDeControlesDeUsuario/FicheroControlDeUsuario.ascx")]
        public string UserControlVirtualPath
        {
            get { return _userControlVirtualPath; }
            set { _userControlVirtualPath = value; }
        }

        protected override void RenderChildren(HtmlTextWriter output)
        {
            try
            {
                this.EnsureChildControls();
                if (this._childControl != null)
                    this._childControl.RenderControl(output);
            }
            catch (Exception ex)
            {
                _errMessage = string.Format("Exception Message (RenderWebPart) = {0}<br />", ex.Message);
                gestorDeError.TratarExcepcion(ex, _errMessage,  "RenderChildren"); 
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            this.Controls.Clear();

            if (_userControlVirtualPath != string.Empty)
            {
                if (_childControl != null) { return; }

                _childControl = Page.LoadControl(_userControlVirtualPath);
                if (_childControl != null){ Controls.AddAt(0, _childControl); }
            }
        }
    }
}
