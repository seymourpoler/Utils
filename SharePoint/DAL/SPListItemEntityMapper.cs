
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using Microsoft.SharePoint;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Comunes.Log.GestionExcepciones;
using DTO;

namespace Datos
{
    //Para más información consultar: http://msdn.microsoft.com/en-us/library/ff650583.aspx
    //Sobre Mapping: http://martinfowler.com/eaaCatalog/dataMapper.html
    //Sobre el patrón repositorio: http://martinfowler.com/eaaCatalog/repository.html
    public class SPListItemEntityMapper<TEntity> where TEntity : class, new()
    {
        private List<PropertyMapping> _mappings;
        public List<PropertyMapping> Mappings
        {
            get
            {
                return this._mappings;
            }
        }
        protected GestorExcepciones _gestorDeError;
        private SPList _spLista;

        public SPListItemEntityMapper()
        {
            _gestorDeError = new GestorExcepciones(this.GetType().Namespace, this.GetType().Name);
            this._mappings = new List<PropertyMapping>();
            CagarMapeos();
        }
        private void CagarMapeos()
        {
            try
            {
                var tipo = typeof(TEntity);
                TEntity local = new TEntity();

                var propiedades = Utilidades.ConseguirTodasLasPropiedades(tipo);
                var nombreInterno = string.Empty;
                var nombrePropiedad = string.Empty;

                foreach (var campo in propiedades)
                {
                    nombreInterno = Utilidades.ConseguirTexto(local, campo.Name);
                    nombrePropiedad = campo.Name;
                    if ((!string.IsNullOrEmpty(nombreInterno)) && (!string.IsNullOrEmpty(nombrePropiedad)))
                    { 
                        AniadirMapeo(nombreInterno, nombrePropiedad); 
                    }
                }
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       "Falla al maperar los campos de la lista y las entidades de negocio",
                                                       "CagarMapeos");
            }
        }
        public void AniadirMapeo(string internalName, string entityPropertyName)
        {
            try
            {
                PropertyMapping item = new PropertyMapping();
                item.EntityPropertyName = entityPropertyName;
                item.SPInternalName = internalName;
                _mappings.Add(item);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       string.Format("Falla al añadir un mapero de los campos de la lista y la entidad de negocio nombre del campo: {0}, propiedad de la entidad: {1}", internalName, entityPropertyName),
                                                       "AniadirMapeo");
            }
        }
        private TEntity CrearEntidad(SPListItem item)
        {
            try
            {
                TEntity local = new TEntity();
                foreach (var map in this._mappings)
                {
                    local = (TEntity)EstablecerValor(item, local, map) as TEntity;
                }

                var propiedad = local.GetType().GetProperty("DocumentosAdjuntos");
                var valores = ConseguirDocumentosAdjuntos(item);
                propiedad.SetValue(local, valores, null);
                return local;
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       string.Format("Fallo al crear la entidad {0} con los datos procedentes de la lilsta", typeof(TEntity)),
                                                       "CrearEntidad");
            }
        }
        private object EstablecerValor(SPListItem elemento, object entidad, PropertyMapping map)
        {
            try
            {
                var valor = elemento[map.SPInternalName];
                entidad = Utilidades.DarValorALaPropiedad(entidad, map.EntityPropertyName, valor);
                return entidad;
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       string.Format("Fallo al establer el valor en la propiedad {0} de la entidad {1} con los datos procedentes de la lista", map.EntityPropertyName, typeof(TEntity)),
                                                       "EstablecerValor");
            }
        }
        private IList<FicheroAdjunto> ConseguirDocumentosAdjuntos(SPListItem item)
        {
            try
            {
                IList<FicheroAdjunto> resultado = new List<FicheroAdjunto>();
                
                for (int cont = 0; cont < item.Attachments.Count; cont++)
                {
                    SPFile Spfich = item.ParentList.ParentWeb.GetFile(item.Attachments.UrlPrefix + item.Attachments[cont]);
                    byte[] binFile = Spfich.OpenBinary();
                    var fich = new FicheroAdjunto();
                    fich.Contenido = binFile;
                    fich.UrlFichero = item.Attachments.UrlPrefix + Spfich.Name;
                    fich.NombreFichero = Spfich.Name;
                    resultado.Add(fich);
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    ex.ToString(),
                                                    "ConseguirDocumentosAdjuntos");
            }
        }
        public IList<TEntity> Map(SPListItemCollection items)
        {
            try
            {
                var resultado = new List<TEntity>();

                foreach (SPListItem elemento in items)
                {              
                    resultado.Add(CrearEntidad(elemento));
                }

                return resultado;
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       string.Format("Fallo al maperar los datos de la lista en entidades de negocio : {0}", typeof(TEntity)),
                                                       "Map");
            }
        }

        public TEntity Map(SPListItem elemento)
        {
            try
            {
                return CrearEntidad(elemento);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       string.Format("Fallo al maperar los datos de la lista en entidad de negocio : {0}", typeof(TEntity)),
                                                       "Map");
            }
        }
    }

    public class PropertyMapping
    {
        public string EntityPropertyName { get; set; }
        public string SPInternalName { get; set; }
    }
}
