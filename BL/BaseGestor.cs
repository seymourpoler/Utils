
using System;
using System.Collections.Generic;
using Comunes.Log.GestionExcepciones;
using DTO;
using Datos;

namespace Logica
{
    public abstract  class BaseGestor<T> where T : class, new()
    {
        protected GestorExcepciones _gestorDeError;
        protected BaseRepositorioLista<T> _repositorio { get; set; }
        protected InformacionDeRepeticionesRepositorio _repositorioDeRepeticiones { get; set; }
        protected GestorDeMachineConfig _gestorConfiguracion;

        public BaseGestor()
        {
            _gestorDeError = new GestorExcepciones("Datos", "BaseGestor");
            _gestorConfiguracion = new GestorDeMachineConfig();
        }

        public T ConseguirElementoPorId(int id)
        {
            try
            {
                return _repositorio.ConseguirElementoPorId(id);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       string.Format("Error intentar recuperar el elemento de tipo {0}, por el id: {1}", typeof(T), id),
                                                       "ConseguirElementoPorId");
            }
        }

        public IList<T> ConseguirElementosPorGuidSerie(Guid guidSerie)
        {
            try
            {
                return _repositorio.ConseguirElementosPorGuidSerie(guidSerie);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       string.Format("Error al intentar conseguir los elementos de tipo con guidserie: {0}", typeof(T), guidSerie),
                                                       "ConseguirElementosPorGuidSerie");
            }
        }

        public virtual void GuardarElementos(IList<T> datos)
        {
            try
            {
                foreach (var elemento in datos)
                {
                    GuardarElemento(elemento);
                }
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       string.Format("Error al guardar los elementos de tipo: {0}", typeof(T)),
                                                       "GuardarElementos");
            }
        }

        public IList<T> GuardarElementos(T elemento, InformacionDeRepeticion infoRepeticion)
        {
            try
            {
                var eventos = GenerarElementos(elemento, infoRepeticion);
                var configuracion = _gestorConfiguracion.ConseguirValoresDeConfiguracion();
                eventos = ValidarElementosGenerados(eventos, configuracion);
                GuardarElementos(eventos);
                infoRepeticion.ID = _repositorioDeRepeticiones.GuardarElemento(infoRepeticion);
                return eventos;
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       string.Format("Error al guardar los elementos repetidos del tipo {0}", typeof(T)),
                                                       "GuardarElementos");
            }
        }

        public IList<T> ValidarElementosGenerados(IList<T> elementos, Configuracion  configuracion)
        {
            try
            {
                var resultado = new List<T>();
                var numeroMaximoDeElementos = configuracion.NumeroMaximoDeElementosRepetidos;
                var contador = 0;
                while((contador < elementos.Count) && (contador < numeroMaximoDeElementos))
                {
                    resultado.Add(elementos[contador]);
                    contador++;
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       ex.ToString(),
                                                       "ValidarElementosGenerados");
            }
        }

        public IList<T> GenerarElementos(T elemento, InformacionDeRepeticion infoRepeticion)
        {
            try
            {
                var gestorDeEventos = FabricaDeGestoresDeEventos<T>.CrearGestorDeEventos(infoRepeticion.Perioicidad);
                return gestorDeEventos.GenerarEventos(elemento, infoRepeticion);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       "Error al generar los elementos",
                                                       "GenerarElementos");
            }
        }

        public virtual int GuardarElemento(T elemento)
        {
            try
            {
                return _repositorio.GuardarElemento(elemento);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       string.Format("Error al guardar el elemento de tipo: {0}", typeof(T)),
                                                       OptimaException.CapaOrigenExcepcion.Logica,
                                                       "GuardarElemento");
            }
        }

        public virtual void ActualizarElemento(T elemento)
        {
            try
            {
                _repositorio.ActualizarElemento(elemento);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       string.Format("Error al actualizar el elemento de tipo: {0}", typeof(T)),
                                                       "ActualizarElemento");
            }
        }

        public virtual IList<T> ActualizarSerieDeElementos(T elemento, InformacionDeRepeticion infoRepeticion)
        {
            try
            {
                EliminarElementosDeLaSerie(elemento, infoRepeticion);
                EliminarInformacionDeRepeticion(infoRepeticion);
                return GuardarElementos(elemento, infoRepeticion);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       string.Format("Error al intentar actualizar la serie de elmentos de tipo: {0}", typeof(T)),
                                                       "ActualizarSerieDeElementos");
            }
        }

        private void EliminarElementosDeLaSerie(T elemento, InformacionDeRepeticion infoRepeticion)
        { 
            try
            {
                Guid guidSerie = (Guid)Utilidades.ConseguirValorDeLaPropiedad(elemento, "GuidSerie");
                var datos = _repositorio.ConseguirElementosPorGuidSerie(guidSerie);
                foreach (var dato in datos)
                {
                    if(EsElementoValidoParaEliminar(dato, infoRepeticion.FechaInicio))
                    {
                        EliminarElemento(dato);
                    }
                }
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       ex.ToString(),
                                                       "EliminarElementosDeLaSerie");
            }
        }

        private void EliminarInformacionDeRepeticion(InformacionDeRepeticion infoRepeticion)
        {
            try
            {
                _repositorioDeRepeticiones.EliminarElementoPorId(infoRepeticion.ID);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       "Error al intentar eliminar la información de repeticion",
                                                       "EliminarInformacionDeRepeticion");
            }
        }

        private bool EsElementoValidoParaEliminar(T elemento, DateTime fechaInicio)
        { 
            try
            {
                var resultado = false;
                var fecha = Convert.ToString(Utilidades.ConseguirValorDeLaPropiedad(elemento, "FechaInicio"));
                var fechaInicioElemento = Utilidades.ConseguirDateTimeFromISO601DateTimeString(fecha);
                if (fechaInicioElemento >= fechaInicio) 
                { 
                    resultado = true; 
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       ex.ToString(),
                                                       "EsElementoValido");
            }
        }

        private void EliminarElemento(T elemento)
        {
            try
            {
                var id = (int)Utilidades.ConseguirValorDeLaPropiedad(elemento, "ID");
                EliminarElementoPorId(id);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       ex.ToString(),
                                                       "EliminarElemento");
            }
        }

        public virtual void EliminarElementoPorId(int id)
        {
            try
            {
                _repositorio.EliminarElementoPorId(id);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       string.Format("Error al actualizar el elemento de tipo: {0}", typeof(T)),
                                                       "ActualizarElemento");
            }
        }

        public virtual void EliminarElementos(IList<T> elementos)
        {
            try
            {
                int idElemento;
                foreach(var elemento in elementos)
                {
                    idElemento = (int)Utilidades.ConseguirValorDeLaPropiedad(elemento, "ID"); 
                    EliminarElementoPorId(idElemento);
                }
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    "Fallo al eliminar los elementos",
                                                    "EliminarElementos");
            }
        }

        public virtual void EliminarTodosLosElementosDeLaSerie(Guid guidSerie)
        {
            try
            {
                IList<T> elementos = ConseguirElementosPorGuidSerie(guidSerie);
                EliminarElementos(elementos);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    string.Format("Error al eliminar todos los elementos del tipo {0} y de la serie {1}", typeof(T), guidSerie),
                                                    "EliminarTodosLosElementosDeLaSerie");
            }
        }

        public virtual void EliminarTodosLosElementos()
        { 
            try
            {
                _repositorio.EliminarTodosLosElementos();
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    string.Format("Error al eliminar todos los elementos del tipo {0}.", typeof(T)),
                                                    "EliminarTodosLosElementos");
            }

        }

        public void ActualizarElValorDeUnCampoDeUnElemento(string nombreCampo, object valorCampo, int id)
        {
            try
            {
                _repositorio.ActualizarElValorDeUnCampoDeUnElemento(nombreCampo, valorCampo, id);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       string.Format("Fallo al intentar actualizar un valor de un campo  de un elemento con id: {0}", id),
                                                       "ActualizarElValorDeUnCampoDeUnElemento");
            }
        }

        public void SacarUnElementoDeLaSerie(int IdElemento)
        {
            try
            {
                _repositorio.ActualizarElValorDeUnCampoDeUnElemento("GuidSerie", null, IdElemento);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       string.Format("Error al intentar sacar un elemento de tipo {0} con id {1} de la serie",typeof(T) ,IdElemento),
                                                       "SacarLaReunionDeLaSerie");
            }
        }
    }
}
