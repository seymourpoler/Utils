
using System;
using System.Collections.Generic;
using Comunes.Log.GestionExcepciones;
using ArquitecturaAplicaciones.PatronTableModule.AccesoDatos;
using DTO;

namespace Datos
{
    public class BaseRepositorioTabla : BaseAccesoDatos
    {
        protected GestorExcepciones _gestorDeError;

        protected override string NombreCadenaConexion
        {
            get { return "CadenaConexionSitioSeguimiento"; }
        }

        public BaseRepositorioTabla()
        {
            _gestorDeError = new GestorExcepciones(this.GetType().Namespace, this.GetType().Name);
        }
    }
}
