
using System;
using System.Collections.Generic;
using Comunes.Log.GestionExcepciones;
using DTO;
using Datos;

namespace Logica
{
    public class GestorDeReuniones: BaseGestor<Reunion>
    {
        public GestorDeReuniones(string url)
        {
            _repositorio = new ReunionesRepositorio(url);
            _repositorioDeRepeticiones = new InformacionDeRepeticionesRepositorio(url);
            _gestorDeError = new GestorExcepciones("Datos", "GestorDeReuniones");
        }
    }
}
