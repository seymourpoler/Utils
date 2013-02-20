
using System;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.SharePoint.Utilities;

namespace DTO
{
    public class Utilidades
    {
        public static string ConseguirTexto(Enum valor)
        {
            Type tipo = valor.GetType();

            FieldInfo campos = tipo.GetField(valor.ToString());
            StringValueAttribute[] atributos = campos.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];
            
            return atributos.Length > 0 ? atributos[0].Value : string.Empty;
        }

        public static string ConseguirTexto(object objeto, string nombreDeLaPropiedad)
        {
            var resultado = string.Empty;
            var tipo = objeto.GetType();
            var propertiedades = tipo.GetProperties();
            var cont = 0;
            var encontrado = false;
            
            while (cont < propertiedades.Length && !encontrado)
            {
                if (propertiedades[cont].Name == nombreDeLaPropiedad)
                {
                    var atributos = propertiedades[cont].GetCustomAttributes(true);
                    if (atributos.Length > 0)
                    {
                        StringValueAttribute stringAttr = atributos[0] as StringValueAttribute;
                        if (null != stringAttr)
                        {
                            resultado = stringAttr.Value;
                            encontrado = true;
                        }
                    }
                }
                cont++;
            }
            return resultado;
        }

        public static object  DarValorALaPropiedad(object objeto, string nombreDePropiedad, object valor)
        {
            if (objeto == null)
            {
                throw new Exception("No se pueden tratar objetos nulos");
            }
            var tipo = objeto.GetType();
            if (tipo == null)
            {
                throw new Exception("No se ha encontrado el tipo del objeto");
            }
            PropertyInfo propiedad = tipo.GetProperty(nombreDePropiedad);
            if (propiedad == null) 
            {
                throw new Exception(string.Format("No se encuentra la propiedad {} en el objeto", nombreDePropiedad));
            }
            if (!propiedad.CanWrite)
            {
                throw new Exception("Propiedad que no se puede modificar su valor");
            }

            valor = ConseguirValorDelCampo(propiedad.PropertyType.Name , valor);

            propiedad.SetValue(objeto, valor, null);

            return objeto;
        }

        private static object ConseguirValorDelCampo(string nombreTipoPropiedad, object valor)
        {
            switch (nombreTipoPropiedad)
            {
                case "Int32":
                    return ConseguirValorDelCampoInt32(valor);
                case "Guid":
                    return ConseguirValorDelCampoGuid(valor);
                case "String":
                    return Convert.ToString(valor);
                case "DateTime":
                    return ConseguirValorDelCampoDateTime(valor);
                case "TipoEstadoEvento":
                    return ConseguirValorDelCampoTipoEstadoEvento(valor);
                case "TipoImpactoEnAccionATomar":
                    return ConseguirValorDelCampoTipoImpactoEnAccionATomar(valor);
                case "TipoNivelModeloRelacion":
                    return ConseguirValorDelCampoTipoNivelModeloRelacion(valor);
            }
            return valor;
        }

        public static Int32 ConseguirValorDelCampoInt32(object valor)
        {
           if(valor == null)
           {
               return 0;
           }
            return  Convert.ToInt32(valor);
        }

        public static Guid ConseguirValorDelCampoGuid(object valor)
        {
            if(valor == null)
            {
                return  new Guid();
            }
            return new Guid(Convert.ToString(valor));
        }

        public static DateTime ConseguirValorDelCampoDateTime(object valor)
        {
            return Convert.ToDateTime(valor); 
        }

        public static TipoEstadoEvento ConseguirValorDelCampoTipoEstadoEvento(object valor)
        {
            switch (Convert.ToString(valor))
            {
                case "En Curso":
                    return TipoEstadoEvento.EnCurso;
                case "Cerrada":
                    return TipoEstadoEvento.Cerrada;
                default:
                    throw new Exception("Valor no encontrado dentro del TipoEstadoEvento");
            }
        }

        public static TipoImpactoEnAccionATomar ConseguirValorDelCampoTipoImpactoEnAccionATomar(object valor)
        {
            switch (Convert.ToString(valor))
            {
                case "Si Alcance - Si Replanif":
                    return TipoImpactoEnAccionATomar.Si_Alcance_Si_Replanif;
                case "Si Alcance - No Replanif":
                    return TipoImpactoEnAccionATomar.Si_Alcance_No_Replanif;
                case "No Alcance - Si Replanif":
                    return TipoImpactoEnAccionATomar.No_Alcance_Si_Replanif;
                case "No Alcance - No Replanif":
                    return TipoImpactoEnAccionATomar.No_Alcance_No_Replanif;
                default:
                    throw new Exception("Valor no encontrado dentro del TipoEstadoEvento");
            }
        }

        public static TipoNivelModeloRelacion ConseguirValorDelCampoTipoNivelModeloRelacion(object valor)
        {
            switch (Convert.ToString(valor))
            {
                case "Contrato":
                    return TipoNivelModeloRelacion.Contrato;
                case "Proyecto":
                    return TipoNivelModeloRelacion.Proyecto;
                case "Servicio":
                    return TipoNivelModeloRelacion.Servicio;
                case "Nada":
                    return TipoNivelModeloRelacion.Nada;
                default:
                    throw new Exception("Valor no encontrado dentro del TipoNivelModeloRelacion");
            }
        }

        public static TipoNumeroDeLaSemana ConseguirValorDelCampoTipoNumeroDeLaSemana(object valor)
        {
            switch (Convert.ToString(valor))
            {
                case "Primera":
                    return TipoNumeroDeLaSemana.Primera;
                case "Segunda":
                    return TipoNumeroDeLaSemana.Segunda;
                case "Tercera":
                    return TipoNumeroDeLaSemana.Tercera;
                case "Cuarta":
                    return TipoNumeroDeLaSemana.Cuarta;
                default:
                    throw new Exception("Valor no encontrado dentro del TipoNumeroDeLaSemana");
            }
        }

        public static TipoDiaDeLaSemana ConseguirValorDelCampoTipoDiaDeLaSemana(object valor)
        {
            switch (Convert.ToString(valor))
            {
                case "Domingo":
                    return TipoDiaDeLaSemana.Domingo;
                case "Lunes":
                    return TipoDiaDeLaSemana.Lunes;
                case "Martes":
                    return TipoDiaDeLaSemana.Martes;
                case "Miercoles":
                case "Miércoles":
                    return TipoDiaDeLaSemana.Miercoles;
                case "Jueves":
                    return TipoDiaDeLaSemana.Jueves;
                case "Viernes":
                    return TipoDiaDeLaSemana.Viernes;
                case "Sabado":
                case "Sábado":
                    return TipoDiaDeLaSemana.Sabado;
                default:
                    throw new Exception("Valor no encontrado dentro del TipoDiaDeLaSemana");
            }
        }

        public static TipoPerioicidad ConseguirValorDelCampoTipoPerioicidad(object valor) 
        {
            switch (Convert.ToString(valor))
            {
                case "Anual":
                case "2":
                    return TipoPerioicidad.Anual;
                case "Semestral":
                case "5":
                    return TipoPerioicidad.Semestral;
                case "Trimestral":
                case "1":
                    return TipoPerioicidad.Trimestral;
                case "Mensual":
                case "3":
                    return TipoPerioicidad.Mensual;
                case "Semanal":
                case "6":
                    return TipoPerioicidad.Semanal;
                case "Diario":
                case "4":
                    return TipoPerioicidad.Diario;
                case "Nada":
                case "0":
                    return TipoPerioicidad.Nada;
                
                default:
                    throw new Exception("Valor no encontrado dentro del TipoPerioicidad");
            }
        }

        public static TipoMes ConseguirValorDelCampoTipoMes(object valor)
        {
            switch (Convert.ToString(valor))
            {
                case "Enero":
                    return TipoMes.Enero;
                case "Febrero":
                    return TipoMes.Febrero;
                case "Marzo":
                    return TipoMes.Marzo;
                case "Abril":
                    return TipoMes.Abril;
                case "Mayo":
                    return TipoMes.Mayo;
                case "Junio":
                    return TipoMes.Junio;
                case "Julio":
                    return TipoMes.Julio;
                case "Agosto":
                    return TipoMes.Agosto;
                case "Septiembre":
                    return TipoMes.Septiembre;
                case "Octubre":
                    return TipoMes.Octubre;
                case "Noviembre":
                    return TipoMes.Noviembre;
                case "Diciembre":
                    return TipoMes.Diciembre;

                default:
                    throw new Exception("Valor no encontrado dentro del TipoPerioicidad");
            }
        }

        public static TipoFrencuenciaAnual ConseguirValorDelCampoTipoFrencuenciaAnual(object valor)
        {
            switch (Convert.ToString(valor))
            {
                case "ElDiaXdelMesX":
                    return TipoFrencuenciaAnual.ElDiaXdelMesX;
                case "LaSemanaXdelMesX":
                    return TipoFrencuenciaAnual.LaSemanaXdelMesX;

                default:
                    throw new Exception("Valor no encontrado dentro del TipoFrencuenciaAnual");
            }
        }

        public static TipoFrecuenciaMensual ConseguirValorDelCampoTipoFrecuenciaMensual(object valor)
        {
            switch (Convert.ToString(valor))
            {
                case "ElDiaXcadaXmeses":
                    return TipoFrecuenciaMensual.ElDiaXcadaXmeses;
                case "LaXsemanaCadaXmeses":
                    return TipoFrecuenciaMensual.LaXsemanaCadaXmeses;

                default:
                    throw new Exception("Valor no encontrado dentro del TipoFrecuenciaMensual");
            }
        }

        public static TipoFrencuenciaDiaria ConseguirValorDelCampoTipoFrencuenciaDiaria(object valor)
        {
            switch (Convert.ToString(valor))
            {
                case "CadaXdias":
                    return TipoFrencuenciaDiaria.CadaXdias;
                case "TodosLosDias":
                    return TipoFrencuenciaDiaria.TodosLosDias;

                default:
                    throw new Exception("Valor no encontrado dentro del TipoFrencuenciaDiaria");
            }
        }

        public static TipoAlgoritmo ConseguirValorDelCampoTipoAlgoritmo(object valor)
        {
            switch (Convert.ToInt32(valor))
            {
                case 0: 
                    return TipoAlgoritmo.AlgoritmoCero;
                case 1:
                    return TipoAlgoritmo.AlgoritmoUno;
                case 2:
                    return TipoAlgoritmo.AlgoritmoDos;
                case 3:
                    return TipoAlgoritmo.AlgoritmoTres;
                case 4:
                    return TipoAlgoritmo.AlgoritmoCuatro;
                case 5:
                    return TipoAlgoritmo.AlgoritmoCinco;
                case 6:
                    return TipoAlgoritmo.AlgoritmoSeis;
                case 7:
                    return TipoAlgoritmo.AlgoritmoSiete;
                case 8:
                    return TipoAlgoritmo.AlgoritmoOcho;

                default:
                    throw new Exception("Valor no encontrado dentro del TipoAlgoritmo");
            }
        }

        public static TipoEntidad ConseguirValorDelCampoTipoEntidad(object valor)
        {
            switch (Convert.ToInt32(valor))
            {
                case 1:
                    return TipoEntidad.AccionATomar;
                case 2:
                    return TipoEntidad.Entregable;
                case 3:
                    return TipoEntidad.Reunion;

                default:
                    throw new Exception("Valor no encontrado dentro del TipoEntidad");
            }
        }

        public static object ConseguirValorDeLaPropiedad(object objeto, string nombreDePropiedad)
        {
            if (objeto == null)
            {
                throw new Exception("No se pueden tratar objetos nulos");
            }
            var tipo = objeto.GetType();
            if (tipo == null)
            {
                throw new Exception("No se ha encontrado el tipo del objeto");
            }
            PropertyInfo propiedad = tipo.GetProperty(nombreDePropiedad);
            if (propiedad == null)
            {
                throw new Exception(string.Format("No se encuentra la propiedad {} en el objeto", nombreDePropiedad));
            }
            if (!propiedad.CanRead)
            {
                throw new Exception("Propiedad que no se puede leer su valor");
            }
            var resultado = propiedad.GetValue(objeto, null);
            resultado = ConseguirValorDelTipo(resultado, propiedad.PropertyType.Name);
            objeto = resultado;

            return objeto;
        }

        public static DateTime ConseguirDateTimeFromISO601DateTimeString(string valor)
        {
            return SPUtility.CreateDateTimeFromISO8601DateTimeString(valor);
        }

        public static DateTime ConseguirUltimoDiaDelMes(DateTime fecha)
        {
            fecha = new DateTime(fecha.Year, fecha.Month, 1);
            return fecha.AddMonths(1).AddDays(-1);
        }

        public static object ConseguirValorDelTipo(object valor, string nombreTipo)
        {

            switch (nombreTipo)
            {
                case "DateTime":
                    return SPUtility.CreateISO8601DateTimeFromSystemDateTime(Convert.ToDateTime(valor));
                case "TipoEstadoEvento":
                    return ConseguirTexto((TipoEstadoEvento)valor);
                case "TipoImpactoEnAccionATomar":
                    return ConseguirTexto((TipoImpactoEnAccionATomar)valor);
            }

            return valor;
        }

        public static List<PropertyInfo> ConseguirTodasLasPropiedades(Type type)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            var list = new List<PropertyInfo>();

            if (type != null)
            {
                list.AddRange(type.GetProperties(flags));
                list.AddRange(ConseguirTodasLasPropiedades(type.BaseType));
            }

            return list;
        }

        public static List<FieldInfo> ConseguirTodasLosCampos(Type type)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            var list = new List<FieldInfo>();

            if (type != null)
            {
                list.AddRange(type.GetFields(flags));
                list.AddRange(ConseguirTodasLosCampos(type.BaseType));
            }

            return list;
        }

        //Es necesario que el objeto a clonar sea serializable 
        //Hay que poner [Serializable()] en la declaración de la clase
        //Hay soluciones más óptimas en tiempo de ejecución
        public static T Clonar<T>(T original)
        {
            if (original == null) { throw new Exception("Objeto a copiar no puede ser nulo"); }

            var tipo = original.GetType();
            var resultado = Activator.CreateInstance(tipo);
            foreach (PropertyInfo propiedad in ConseguirTodasLasPropiedades(tipo))
            {
                propiedad.SetValue(resultado, propiedad.GetValue(original, null), null);
            }
            return (T)resultado;
        }

        public static T Clonar<T>(T elemento, DateTime fechaInicio, DateTime fechaFin)
        {
            var elementoClon = Utilidades.Clonar<T>(elemento);
            elementoClon = (T)Utilidades.DarValorALaPropiedad(elementoClon, "FechaInicio", fechaInicio.ToShortDateString());
            elementoClon = (T)Utilidades.DarValorALaPropiedad(elementoClon, "FechaFinPrevista", fechaFin.ToShortDateString());
            return elementoClon;
        }

        public static T Clonar<T>(T elemento, DateTime fechaInicio, int numeroDeDias)
        {
            return Clonar<T>(elemento, fechaInicio, fechaInicio.AddDays(numeroDeDias));
        }

        //Para maperar los días de la semana del System al enumerado propio
        public static TipoDiaDeLaSemana ConseguirDiaDeLaSemana(DayOfWeek diaDeLaSemana)
        {
            switch (diaDeLaSemana)
            {
                case DayOfWeek.Sunday:
                    return TipoDiaDeLaSemana.Domingo;
                case DayOfWeek.Monday:
                    return TipoDiaDeLaSemana.Lunes;
                case DayOfWeek.Tuesday:
                    return TipoDiaDeLaSemana.Martes;
                case DayOfWeek.Wednesday:
                    return TipoDiaDeLaSemana.Miercoles;
                case DayOfWeek.Thursday:
                    return TipoDiaDeLaSemana.Jueves;
                case DayOfWeek.Friday:
                    return TipoDiaDeLaSemana.Viernes;
                case DayOfWeek.Saturday:
                    return TipoDiaDeLaSemana.Sabado;
                default: throw new Exception("Tipo de DayOfWeek no contemplado");
            }
        }
		
		public static string ConstruirUrl(string url, string nuevaPagina, string parametros)
        {
            string resultado = string.Empty;

            string pagina = System.IO.Path.GetFileName( url);
            resultado = url.Replace(pagina, nuevaPagina) + parametros;
            return resultado;
        }
		
        public static string ConseguirEtiqueta(string idEtiqueta,string ficheroDeRecursos)
        {
            int LCID = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;
            string resultado = string.Empty;

            resultado = SPUtility.GetLocalizedString("$Resources: " + idEtiqueta, "PortalCliente\\" + ficheroDeRecursos, (uint)LCID);

            return resultado;
        }
    }
}
