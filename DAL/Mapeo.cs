
using System;
using System.Data;
using System.Collections.Generic;
using DTO;
using Comunes.Log.GestionExcepciones;

namespace Datos
{
    public class Mapeo
    {
        private GestorExcepciones _gestorDeError;

        public Mapeo()
        {
            _gestorDeError = new GestorExcepciones(this.GetType().Namespace, this.GetType().Name);
        }

        public static IList<ConfiguracionDelElementoDeVerificacionAutomatico> ToConfiguracionDelElementoDeVerificacionAutomatico(DataTable tablaModulo)
        {
            try
            {
                IList<ConfiguracionDelElementoDeVerificacionAutomatico> resultado;
                resultado = new List<ConfiguracionDelElementoDeVerificacionAutomatico>();

                ConfiguracionDelElementoDeVerificacionAutomatico  configuracion;

                foreach (DataRow fila in tablaModulo.Rows)
                {
                    configuracion = new ConfiguracionDelElementoDeVerificacionAutomatico();
                    if (DBNull.Value.Equals(fila["EntidadDestino"])) { throw new Exception("El campo EntidadDestino, no puede ser nulo."); }
                    configuracion.EntidadDeDestino = Utilidades.ConseguirValorDelCampoTipoEntidad(fila["EntidadDestino"]);

                    if (!DBNull.Value.Equals(fila["NombreEnDestino"]))
                    { configuracion.NombreEnDestino = Convert.ToString(fila["NombreEnDestino"]); }

                    if (!DBNull.Value.Equals(fila["AlgoritmoAplica"]))
                    { configuracion.AlgoritmoAplicado = Utilidades.ConseguirValorDelCampoTipoAlgoritmo(fila["AlgoritmoAplica"]); }

                    if (!DBNull.Value.Equals(fila["NumMaxRepeticiones"]))
                    { configuracion.NumeroMaximoDeRepeticiones = Convert.ToInt32(fila["NumMaxRepeticiones"]); }

                    if (!DBNull.Value.Equals(fila["PeriodicidadDefecto"]))
                    { configuracion.PerioicidadPorDefecto = Utilidades.ConseguirValorDelCampoTipoPerioicidad(fila["PeriodicidadDefecto"]); }

                    if (!DBNull.Value.Equals(fila["TipoEV"]))
                    { configuracion.TipoElementoDeVerificacion = Convert.ToString(fila["TipoEV"]).Trim(); }

                    if (!DBNull.Value.Equals(fila["Confirmado"]))
                    { configuracion.Confirmado = Convert.ToBoolean(fila["Confirmado"]); }

                    resultado.Add(configuracion);
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw new GestorExcepciones("Ieci.Optima.SitioSeguimiento.GestionDeElementosPeriodicos.Datos",
                                            "Mapeo").TratarExcepcion(ex,
                                                   "Algún valor de la tabla no es correcto",
                                                   OptimaException.CapaOrigenExcepcion.AccesoDatos,
                                                   "ToConfiguracionDelElementoDeVerificacionAutomatico");
            }
        }

        public static IList<RegistroDeEntidadDeVerificacionProcesado> ToRegistroDeEntidadDeVerificacionProcesado(DataTable tablaModulo)
        {
            try
            {
                IList<RegistroDeEntidadDeVerificacionProcesado> resultado;
                resultado = new List<RegistroDeEntidadDeVerificacionProcesado>();

                RegistroDeEntidadDeVerificacionProcesado registro;

                foreach (DataRow fila in tablaModulo.Rows)
                {
                    registro = new RegistroDeEntidadDeVerificacionProcesado();

                    if (DBNull.Value.Equals(fila["ID"])) { throw new Exception("El campo ID, no puede ser nulo."); }
                    registro.ID = Convert.ToInt32(fila["ID"]);

                    if (DBNull.Value.Equals(fila["Pep"])) { throw new Exception("El campo Pep, no puede ser nulo."); }
                    registro.Pep = Convert.ToString(fila["Pep"]).Trim();

                    if (!DBNull.Value.Equals(fila["TipoEV"]))
                    { registro.TipoDeElementoDeVerificacion = Convert.ToString(fila["TipoEV"]).Trim(); }

                    if (!DBNull.Value.Equals(fila["FechaAlta"]))
                    { registro.FechaAlta = Convert.ToDateTime(fila["FechaAlta"]); }

                    resultado.Add(registro);
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw new GestorExcepciones("Ieci.Optima.SitioSeguimiento.GestionDeElementosPeriodicos.Datos",
                                            "Mapeo").TratarExcepcion(ex,
                                                   "Algún valor de la tabla no es correcto",
                                                   OptimaException.CapaOrigenExcepcion.AccesoDatos,
                                                   "ToRegistroDeEntidadDeVerificacionProcesado");
            }
        }
    }
}
