using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Comunes.Helpers;
using Comunes.Log;

namespace ArquitecturaAplicaciones.PatronTableModule.AccesoDatos
{
    /// <summary>
    /// Define las funcionalidades básicas de trato con la base de datos para las clases de acceso a datos.
    /// Utiliza una cadena de conexión definida en el fichero de configuración de la aplicación con el nombre "CadenaConexionListasVerificacion"
    /// </summary>
    public abstract class BaseAccesoDatos
    {

        protected abstract string NombreCadenaConexion
        {
            get;
        }
        /// <summary>
        /// Nombre del proveedor de acceso a datos
        /// </summary>
        protected string nombreProveedor = "";
        /// <summary>
        /// Cadena de conexión a la base de datos
        /// </summary>
        protected string cadenaConexion = "";

        /// <summary>
        /// Conexión a la base de datos
        /// </summary>
        protected DbConnection conexion;

        /// <summary>
        /// Crea la conexión a base de datos asociada con el objeto, quedando en estado cerrado.
        /// </summary>
        public BaseAccesoDatos()
        {
            nombreProveedor = ConfigurationManager.ConnectionStrings[NombreCadenaConexion].ProviderName;
            cadenaConexion = ConfigurationManager.ConnectionStrings[NombreCadenaConexion].ConnectionString;

            this.conexion = CrearConexion();
        }

        private DbConnection CrearConexion()
        {
            DbProviderFactory factoria = DbProviderFactories.GetFactory(nombreProveedor);
            DbConnection con = factoria.CreateConnection();
            con.ConnectionString = cadenaConexion;
            return con;
        }


        /// <summary>
        /// Carga una de las tablas del DataSet asociado al objeto de acceso a datos utilizando una sentencia SQL que no es el nombre de un procedimiento alamacenado
        /// </summary>
        /// <param name="tabla">La tabla que se quiere llenar con datos</param>
        /// <param name="sentenciaSelect">Una sentencia SQL parametrizada para obtener la tabla</param>
        /// <param name="parametros">Una con los valores de los parámetros de la sentencia SQL; 
        /// en el key, el nombre del parámetro, y en el value, el valor, que será de un tipo primitivo de .Net y no un tipo de base de datos.
        /// Si la sentencia no requiere parámetros, se puede pasar NULL</param>
        protected void CargarTabla(ref DataTable tabla, string sentenciaSelect, Dictionary<string, object> parametros)
        {
            CargarTabla(ref tabla, sentenciaSelect, parametros, false);
        }

        /// <summary>
        /// Carga una de las tablas del DataSet asociado al objeto de acceso a datos. Puede usar tanto el nombre de un procedimiento almacenado
        /// como una sentencia SQL.
        /// </summary>
        /// <param name="tabla">La tabla que se quiere llenar con datos</param>
        /// <param name="sentenciaSelect">Una sentencia SQL parametrizada para obtener la tabla</param>
        /// <param name="parametros">Una con los valores de los parámetros de la sentencia SQL; 
        /// en el key, el nombre del parámetro, y en el value, el valor, que será de un tipo primitivo de .Net y no un tipo de base de datos.
        /// Si la sentencia no requiere parámetros, se puede pasar NULL</param>
        public void CargarTabla(ref DataTable tabla, string sentenciaSelect, Dictionary<string, object> parametros, bool esProcedimientoAlmacenado)
        {
            try
            {
                DbProviderFactory factoria = DbProviderFactories.GetFactory(nombreProveedor);
                DbDataAdapter adaptador = factoria.CreateDataAdapter();

                DbCommand comando = conexion.CreateCommand();
                comando.CommandText = sentenciaSelect;

                if (esProcedimientoAlmacenado)
                    comando.CommandType = CommandType.StoredProcedure;
                else
                    comando.CommandType = CommandType.Text;

                DbHelper helper = new DbHelper(NombreCadenaConexion);
                if (parametros != null)
                {
                    foreach (string nombreParametro in parametros.Keys)
                        helper.AddInParameter(comando, nombreParametro, parametros[nombreParametro]);
                }

                adaptador.SelectCommand = comando;

                adaptador.Fill(tabla);
            }
            catch (Exception ex)
            {
                throw new Exception("Error ejecutando sentencia SQL: " + sentenciaSelect, ex);
            }
        }

        /// <summary>
        /// Actualiza en la base de datos los cambios de un DataTable, utilizando sentencias SQL que no son procedimientos almacenados.
        /// </summary>
        /// <param name="tabla">La tabla que va a ser actualizada</param>
        /// <param name="sentenciaInsert">Una sentencia SQL parametrizada para los nuevos registros, donde los nombres de los parámetros tienen por nombre el símbolo @ seguido del nombre de la columna relacionada del DataTable</param>
        /// <param name="sentenciaUpdate">Una sentencia SQL parametrizada para los registros modificados, donde los nombres de los parámetros tienen por nombre el símbolo @ seguido del nombre de la columna relacionada del DataTable</param>
        /// <param name="sentenciaDelete">Una sentencia SQL parametrizada para los registros borrados, donde los nombres de los parámetros tienen por nombre el símbolo @ seguido del nombre de la columna relacionada del DataTable</param>
        protected void GuardarTabla(DataTable tabla, string sentenciaInsert, string sentenciaUpdate, string sentenciaDelete)
        {
            GuardarTabla(tabla, sentenciaInsert, false, sentenciaUpdate, false, sentenciaDelete, false);
        }


        /// <summary>
        /// Actualiza en la base de datos los cambios de un DataTable, utilizando sentencias SQL que no son procedimientos almacenados.
        /// </summary>
        /// <param name="tabla">La tabla que va a ser actualizada</param>
        /// <param name="sentenciaInsert">Una sentencia SQL parametrizada para los nuevos registros, donde los nombres de los parámetros tienen por nombre el símbolo @ seguido del nombre de la columna relacionada del DataTable</param>
        /// <param name="esInsertProcedimiento">Indica si la sentencia SQL de insert es el nombre de un procedimiento almacenado</param>
        /// <param name="sentenciaUpdate">Una sentencia SQL parametrizada para los registros modificados, donde los nombres de los parámetros tienen por nombre el símbolo @ seguido del nombre de la columna relacionada del DataTable</param>
        /// <param name="esUpdateProcedimiento">Indica si la sentencia SQL de Update es el nombre de un procedimiento almacenado</param>
        /// <param name="sentenciaDelete">Una sentencia SQL parametrizada para los registros borrados, donde los nombres de los parámetros tienen por nombre el símbolo @ seguido del nombre de la columna relacionada del DataTable</param>
        /// <param name="esDeleteProcedimiento">Indica si la sentencia SQL de delete es el nombre de un procedimiento almacenado</param>
        protected void GuardarTabla(DataTable tabla, string sentenciaInsert, bool esInsertProcedimiento, string sentenciaUpdate, bool esUpdateProcedimiento, string sentenciaDelete, bool esDeleteProcedimiento)
        {
            try
            {
                if (tabla.GetChanges() == null || tabla.GetChanges().Rows.Count == 0)
                    return;

                DbProviderFactory factoria = DbProviderFactories.GetFactory(nombreProveedor);
                DbDataAdapter adaptador = factoria.CreateDataAdapter();

                if (sentenciaInsert != null)
                {
                    DbCommand comandoInsert = ConstruyeComando(conexion, tabla, sentenciaInsert, esInsertProcedimiento);
                    adaptador.InsertCommand = comandoInsert;
                }
                if (sentenciaUpdate != null)
                {
                    DbCommand comandoUpdate = ConstruyeComando(conexion, tabla, sentenciaUpdate, esUpdateProcedimiento);
                    adaptador.UpdateCommand = comandoUpdate;
                }
                if (sentenciaDelete != null)
                {
                    DbCommand comandoDelete = ConstruyeComando(conexion, tabla, sentenciaDelete, esDeleteProcedimiento);
                    adaptador.DeleteCommand = comandoDelete;
                }

                adaptador.Update(tabla);

            }
            catch (Exception ex)
            {
                throw new Exception("Error actualizando tabla", ex);
            }
        }

        private DbCommand ConstruyeComando(DbConnection conexion, DataTable tabla, string sentenciaSQL, bool esProcedimientoAlmacenado)
        {
            try
            {
                DbCommand comando = conexion.CreateCommand();
                comando.CommandText = sentenciaSQL;

                if (esProcedimientoAlmacenado)
                    comando.CommandType = CommandType.StoredProcedure;
                else
                    comando.CommandType = CommandType.Text;

                DbHelper helper = new DbHelper(NombreCadenaConexion);

                foreach (DataColumn columna in tabla.Columns)
                {
                    string nombreCampo = columna.ColumnName;
                    string nombreParametro = "@" + columna.ColumnName;
                    if (sentenciaSQL.IndexOf(nombreParametro) == -1)
                        continue;

                    DbParameter parametro = comando.CreateParameter();
                    parametro.Direction = ParameterDirection.Input;
                    parametro.SourceColumn = nombreCampo;
                    parametro.ParameterName = nombreParametro;

                    comando.Parameters.Add(parametro);
                }

                return comando;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creando comando", ex);
            }
        }


    }
}
