
using System;
using Comunes.Log.GestionExcepciones;
using DTO;

namespace Datos
{
    public class ReunionesRepositorio: BaseRepositorioLista<Reunion>
    {
        public ReunionesRepositorio(string url)
            : base(url, "Reuniones")
        {
            _gestorDeError = new GestorExcepciones(this.GetType().Namespace, this.GetType().Name);
        }
    }
}
