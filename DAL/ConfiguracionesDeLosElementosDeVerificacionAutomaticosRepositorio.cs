
using System;
using System.Data;
using System.Collections.Generic;
using Comunes.Log.GestionExcepciones;
using DTO;

namespace Datos
{
    public class ConfiguracionesDeLosElementosDeVerificacionAutomaticosRepositorio : BaseRepositorioTabla
    {
        public ConfiguracionesDeLosElementosDeVerificacionAutomaticosRepositorio()
            : base()
        {
            _gestorDeError = new GestorExcepciones(this.GetType().Namespace, this.GetType().Name);
        }

        public IList<ConfiguracionDelElementoDeVerificacionAutomatico> ConseguirTodosLosElementos()
        {
            try
            {
                IList<ConfiguracionDelElementoDeVerificacionAutomatico> resultado;
                resultado = new List<ConfiguracionDelElementoDeVerificacionAutomatico>();

                DataTable tabla = new dsConfiguracionDelElementoDeVerificacionAutomatico.ConfiguracionDelElementoDeVerificacionAutomaticoDataTable();

                string sentenciaSQL = @"SELECT [ConfiguracionEVAutomatico].[ID]
                                           ,[ConfiguracionEVAutomatico].[TipoEV]
                                           ,[ConfiguracionEVAutomatico].[EntidadDestino]
                                           ,[ConfiguracionEVAutomatico].[NombreEnDestino]
                                           ,[ConfiguracionEVAutomatico].[AlgoritmoAplica]
                                           ,[ConfiguracionEVAutomatico].[NumMaxRepeticiones]
                                           ,[ConfiguracionEVAutomatico].[PeriodicidadDefecto]
                                        FROM [ConfiguracionEVAutomatico]";

                Dictionary<string, object> parametros = new Dictionary<string, object>();

                CargarTabla(ref tabla, sentenciaSQL, parametros);
                resultado = Mapeo.ToConfiguracionDelElementoDeVerificacionAutomatico(tabla);

                return resultado;
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       "Error al intentar obtener los datos de la tabla ConfiguracionEVAutomatico",
                                                       "ConseguirTodosLosElementos");
            }
        }

        public ConfiguracionDelElementoDeVerificacionAutomatico ConseguirElementoPorTipoEV(string tipoEV)
        {
            try
            {
                DataTable tabla = new dsConfiguracionDelElementoDeVerificacionAutomatico.ConfiguracionDelElementoDeVerificacionAutomaticoDataTable();

                string sentenciaSQL = @"SELECT [ConfiguracionEVAutomatico].[ID]
                                           ,[ConfiguracionEVAutomatico].[TipoEV]
                                           ,[ConfiguracionEVAutomatico].[NombreEnDestino]
                                           ,[ConfiguracionEVAutomatico].[EntidadDestino]
                                           ,[ConfiguracionEVAutomatico].[AlgoritmoAplica]
                                           ,[ConfiguracionEVAutomatico].[NumMaxRepeticiones]
                                           ,[ConfiguracionEVAutomatico].[PeriodicidadDefecto]
                                        FROM [ConfiguracionEVAutomatico]
                                        WHERE [ConfiguracionEVAutomatico].[TipoEV] = @tipoEV";

                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("@tipoEV", tipoEV);

                CargarTabla(ref tabla, sentenciaSQL, parametros);
                var resultado = Mapeo.ToConfiguracionDelElementoDeVerificacionAutomatico(tabla);

                if (resultado.Count == 0)
                { throw new Exception(string.Format("No se encuentra el registro con TipoEV: {0}", tipoEV)); }
                else { return resultado[0]; }
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       string.Format("Error al intentar obtener los datos por tipoEV: {0}", tipoEV),
                                                       "ConseguirElementoPorTipoEV");
            }
        }
    }
}
