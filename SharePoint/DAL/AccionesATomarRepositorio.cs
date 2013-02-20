
using System;
using Comunes.Log.GestionExcepciones;
using DTO;

namespace Datos
{
    public class AccionesATomarRepositorio : BaseRepositorioLista<AccionATomar>
    {
        public AccionesATomarRepositorio(string url)
            : base(url, "Acciones a Tomar")
        {
            _gestorDeError = new GestorExcepciones(this.GetType().Namespace, this.GetType().Name);
        }
    }
}
