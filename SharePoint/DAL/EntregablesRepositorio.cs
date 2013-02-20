
using System;
using Comunes.Log.GestionExcepciones;
using DTO;

namespace Datos
{
    public class EntregablesRepositorio: BaseRepositorioLista<Entregable>
    {
        public EntregablesRepositorio(string url)
            : base(url, "Resultados")
        {
            _gestorDeError = new GestorExcepciones(this.GetType().Namespace, this.GetType().Name);
        }
    }
}
