
using System;
using System.Data;
using System.Data.Common;
using Comunes.Helpers;
using System.Collections.Generic;
using DTO;
using Comunes.Log.GestionExcepciones;
using System.Collections;

namespace Datos
{
    public class RegistrosDeEntidadesDeVerificacionProcesadosRepositorio: BaseRepositorioTabla
    {
        public RegistrosDeEntidadesDeVerificacionProcesadosRepositorio()
            : base()
        {
            _gestorDeError = new GestorExcepciones(this.GetType().Namespace, this.GetType().Name);
        }

        public int InsertarUnElemento(RegistroDeEntidadDeVerificacionProcesado registro)
        {
            try
            {
                string sql = string.Empty;

                sql = @"INSERT INTO [RegistroEVAutoProcesado]
                                   ([Pep]
                                   ,[TipoEV]
                                   ,[FechaAlta])
                             VALUES
                                   (@Pep
                                   ,@TipoEV
                                   ,@FechaAlta)";

                DbHelper helper = new DbHelper(NombreCadenaConexion);
                DbCommand comando = helper.GetSqlStringCommond(sql);

                helper.AddInParameter(comando, "@Pep", registro.Pep);
                helper.AddInParameter(comando, "@TipoEV", registro.TipoDeElementoDeVerificacion);
                helper.AddInParameter(comando, "@FechaAlta", registro.FechaAlta);

                return helper.ExecuteNonQuery(comando);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       ex.ToString(),
                                                       "InsertarUnRegistroDeEntidadDeVerificacionProcesado");
            }
        }

        public IList<RegistroDeEntidadDeVerificacionProcesado> ConseguirTodosLosElementos()
        {
            try
            {
                IList<RegistroDeEntidadDeVerificacionProcesado> resultado = new List<RegistroDeEntidadDeVerificacionProcesado>();
                DataTable tabla = new dsRegistroDeEntidadDeVerificacionProcesado.RegistroDeEntidadDeVerificacionProcesadoDataTable();

                string sentenciaSQL = @"SELECT [RegistroEVAutoProcesado].[ID]
                                              ,[RegistroEVAutoProcesado].[Pep]
                                              ,[RegistroEVAutoProcesado].[TipoEV]
                                              ,[RegistroEVAutoProcesado].[FechaAlta]
                                          FROM [RegistroEVAutoProcesado]";

                Dictionary<string, object> parametros = new Dictionary<string, object>();

                CargarTabla(ref tabla, sentenciaSQL, parametros);
                resultado = Mapeo.ToRegistroDeEntidadDeVerificacionProcesado(tabla);

                return resultado;
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       "Fallo al intentar conseguir todos los registro de entidad de verificación procesados",
                                                       "ConseguirTodosLosElementos");
            }
        }

        public RegistroDeEntidadDeVerificacionProcesado ConseguirElementoPorId(int id)
        {
            try
            {
                DataTable tabla = new dsRegistroDeEntidadDeVerificacionProcesado.RegistroDeEntidadDeVerificacionProcesadoDataTable();

                string sentenciaSQL = @"SELECT [RegistroEVAutoProcesado].[ID]
                                              ,[RegistroEVAutoProcesado].[Pep]
                                              ,[RegistroEVAutoProcesado].[TipoEV]
                                              ,[RegistroEVAutoProcesado].[FechaAlta]
                                          FROM [RegistroEVAutoProcesado]
							                WHERE (@Id IS NULL OR [RegistroEVAutoProcesado].[ID] = @Id)";

                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("@Id", id);

                CargarTabla(ref tabla, sentenciaSQL, parametros);
                var resultado = Mapeo.ToRegistroDeEntidadDeVerificacionProcesado(tabla);

                if (resultado.Count == 0)
                { throw new Exception(string.Format("No se encuentra un área con el id: {0}", id)); }
                else { return resultado[0]; }
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       string.Format("Fallo al intentar conseguir el regitro de entidad de verificación procesado con id: {0}", id),
                                                       "ConseguirElementoPorId");
            }
        }

        public IList<RegistroDeEntidadDeVerificacionProcesado> ConseguirElementoPorPepyTipoDeElementoDeVerificacion(string pep, string tipoDeElementoDeVerificacion)
        {
            try
            {
                DataTable tabla = new dsRegistroDeEntidadDeVerificacionProcesado.RegistroDeEntidadDeVerificacionProcesadoDataTable();

                string sentenciaSQL = @"SELECT [RegistroEVAutoProcesado].[ID]
                                              ,[RegistroEVAutoProcesado].[Pep]
                                              ,[RegistroEVAutoProcesado].[TipoEV]
                                              ,[RegistroEVAutoProcesado].[FechaAlta]
                                          FROM [RegistroEVAutoProcesado]
							                WHERE (@Pep IS NULL OR [RegistroEVAutoProcesado].[Pep] = @Pep)
                                            AND (@TipoEV IS NULL OR [RegistroEVAutoProcesado].[TipoEV] = @TipoEV)";

                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("@Pep", pep);
                parametros.Add("@TipoEV", tipoDeElementoDeVerificacion);

                CargarTabla(ref tabla, sentenciaSQL, parametros);
                return Mapeo.ToRegistroDeEntidadDeVerificacionProcesado(tabla);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       string.Format("Fallo al intentar conseguir el regitro de entidad de verificación procesado por pepe  {0} y tipo de entidad de verificacion", pep, tipoDeElementoDeVerificacion),
                                                       "ConseguirElementoPorPepyTipoDeElementoDeVerificacion");
            }
        }



        public void EliminarElementoPorID(int id)
        {
            try
            {
                string sentenciaSQL = @"DELETE [RegistroEVAutoProcesado]
							             WHERE ([RegistroEVAutoProcesado].[ID] = @ID)";

                DbHelper helper = new DbHelper(NombreCadenaConexion);
                DbCommand comando = helper.GetSqlStringCommond(sentenciaSQL);
                helper.AddInParameter(comando, "@ID", id);

                helper.ExecuteDataSet(comando);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       string.Format("Fallo al intentar eliminar un regitro de entidad de verificación procesado con Id: {0}", id),
                                                       "EliminarElementoPorID");
            }
        }

        public void EliminarTodosLosElementos()
        {
            try
            {
                string sentenciaSQL = @"DELETE [RegistroEVAutoProcesado]";

                DbHelper helper = new DbHelper(NombreCadenaConexion);
                DbCommand comando = helper.GetSqlStringCommond(sentenciaSQL);

                helper.ExecuteDataSet(comando);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       "Fallo al intentar eliminar todos los regitros de entidades de verificación procesados ",
                                                       "EliminarTodosLosElementos");
            }
        }
    }
}
