
using System;
using System.Collections.Generic;
using Microsoft.SharePoint;
using Comunes.Log.GestionExcepciones;
using DTO;

namespace Datos
{
    public class BaseRepositorioLista<T> where T : class, new()
    {
        protected SPList _spLista;
        protected GestorExcepciones _gestorDeError;
        protected SPListItemEntityMapper<T> _listItemFieldMapper;

        public BaseRepositorioLista(string url, string nombre)
        {
            try
            {
                _gestorDeError = new GestorExcepciones(this.GetType().Namespace, this.GetType().Name);
                _listItemFieldMapper = new SPListItemEntityMapper<T>();

                using (SPSite sitio = new SPSite(url))
                using (SPWeb web = sitio.OpenWeb())
                { _spLista = web.Lists[nombre]; }
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                       "Error al instanciar la lista",
                                                       "BaseRepositorio");
            }
        }

        protected SPListItemCollection ConseguirTodosLosSPListItems()
        {
            try
            {
                SPQuery consulta = new SPQuery();
                consulta.Query = "<Query><OrderBy><FieldRef Name='ID' /></OrderBy></Query>";
                SPListItemCollection resultado = ConseguirElementos(consulta);
                return resultado;
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    "Error al consultar los valores de configuración.",
                                                    "ConseguirTodosLosElementos");
            }
        }

        public IList<T> ConseguirTodosLosElementos()
        {
            try
            {
                SPListItemCollection elementos = ConseguirTodosLosSPListItems();
                return Mapear(elementos);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    "Error al consultar los valores de configuración.",
                                                    "ConseguirTodosLosElementos");
            }
        }

        protected SPListItemCollection ConseguirElementos(SPQuery consulta)
        {
            try
            {
                return _spLista.GetItems(consulta);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    string.Format("Error al conseguir los elementos de la consulta: {0}.", consulta.Query),
                                                    "ConseguirElementos");
            }
        }

        public T ConseguirElementoPorId(int id)
        {
            try
            {
                var datos = _spLista.GetItemById(id);
                return Mapear(datos);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    string.Format("Error al conseguir el elemento con id: {0}.", id),
                                                    "ConseguirElementoPorID");
            }
        }

        public IList<T> ConseguirElementosPorGuidSerie(Guid guidSerie)
        {
            try
            {
                var consulta = new SPQuery();
                consulta.Query = "<Where><Eq><FieldRef Name='GuidSerie' /><Value Type='Text'>" + guidSerie.ToString().ToUpper() + "</Value></Eq></Where>";
                SPListItemCollection elementos = ConseguirElementos(consulta);
                return Mapear(elementos);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    string.Format("Error al conseguir los elemento con guidserie: {0}.", guidSerie),
                                                    "ConseguirElementosPorGuidSerie");
            }
        }

        protected SPListItem ConseguirSPListItemPorId(int id)
        {
            try
            {
                return _spLista.GetItemById(id);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    string.Format("Error al conseguir el elemento con id: {0}.", id),
                                                    "ConseguirElementoPorID");
            }
        }

        public void EliminarElementoPorId(int id)
        {
            try
            {
                var elemento = _spLista.GetItemById(id);
                _spLista.ParentWeb.AllowUnsafeUpdates = true;
                elemento.Delete();
                _spLista.Update();
                _spLista.ParentWeb.AllowUnsafeUpdates = false;
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    string.Format("Error al eliminar el elemento con id: {0}.", id),
                                                    "ConseguirElementos");
            }
        }

        public void EliminarElementosPorGuidSerie(Guid guidSerie)
        {
            try
            {
                var datos = ConseguirElementosPorGuidSerie(guidSerie);
                int id = 0;
                foreach (var elemento in datos)
                {
                    id = Convert.ToInt32(Utilidades.ConseguirValorDeLaPropiedad(elemento, "ID"));
                    EliminarElementoPorId(id);
                }
                
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    string.Format("Error al eliminar el elemento con guid: {0}.", guidSerie),
                                                    "ConseguirElementos");
            }
        }

        public void EliminarTodosLosElementos()
        {
            try
            {
                SPListItemCollection ListItemColl = _spLista.Items;
                _spLista.ParentWeb.AllowUnsafeUpdates = true;
                foreach (SPListItem item in ListItemColl)
                {
                    _spLista.GetItemById(item.ID).Delete();
                }
                _spLista.Update();
                _spLista.ParentWeb.AllowUnsafeUpdates = false;
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    string.Format("Error al eliminar todos los elementos del tipo {0}.", typeof(T)),
                                                    "EliminarTodosLosElementos");
            }
        }

        protected virtual IList<T> Mapear(SPListItemCollection elementos)
        {
            try
            {
                return _listItemFieldMapper.Map(elementos);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    string.Format("Error al mapear los elementos de la lista, del tipo {0}", typeof(T)),
                                                    "Mapear");
            }
        }

        protected virtual T Mapear(SPListItem elemento)
        {
            try
            {
                return _listItemFieldMapper.Map(elemento);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    string.Format("Error al mapear el elemento de la lista, del tipo {0}", typeof(T)),
                                                    "Mapear");
            }
        }

        public virtual void GuardarElementos(IList<T> elementos)
        {
            try
            {
                foreach (var elemento in elementos)
                {
                    GuardarElemento(elemento);
                }
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    string.Format("Error al guardar los elementos en la lista, del tipo: {0}", typeof(T)),
                                                    "Guardar");
            }
        }

        public virtual int GuardarElemento(T elemento)
        {
            try
            {
                var nuevoItem = _spLista.Items.Add();
                InsertarDatos(nuevoItem, elemento, _listItemFieldMapper.Mappings);
                GuardarDocumentosAdjuntos(nuevoItem, elemento);
                return nuevoItem.ID;
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    string.Format("Error al guardar el elemento en la lista, del tipo: {0}", typeof(T)),
                                                    "GuardarElemento");
            }
        }

        public virtual void ActualizarElemento(T elemento)
        {
            try
            {
                int id = Convert.ToInt32(Utilidades.ConseguirValorDeLaPropiedad(elemento, "ID"));
                var item = ConseguirSPListItemPorId(id);
                InsertarDatos(item, elemento, _listItemFieldMapper.Mappings);
                EliminarDocumentosAdjuntos(item);
                GuardarDocumentosAdjuntos(item, elemento);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    string.Format("Error al actualizar el elemento en la lista, del tipo: {0}", typeof(T)),
                                                    "ActualizarElemento");
            }
        }

        private void InsertarDatos(SPListItem item,  T dato, IList<PropertyMapping> mappings)
        {
            try
            {
                foreach (var map in mappings)
                {
                    if ("ID" != map.SPInternalName)
                    {
                        
                        item[map.SPInternalName] = Utilidades.ConseguirValorDeLaPropiedad(dato, map.EntityPropertyName);
                        Actualizar(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    string.Format("Error al insertar los datos en la lista, del tipo: {0}", typeof(T)),
                                                    "InsertarDatos");
            }
        }

        private void Actualizar(SPListItem item) 
        {
            _spLista.ParentWeb.AllowUnsafeUpdates = true;
            item.Update();
            _spLista.ParentWeb.AllowUnsafeUpdates = false;
        }

        public virtual void ActualizarElValorDeUnCampoDeUnElemento(string nombreCampo, object valorCampo, int idElemento)
        {
            try
            {
                var elemento = ConseguirSPListItemPorId(idElemento);
                elemento[nombreCampo] = valorCampo;
                Actualizar(elemento);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    string.Format("Error al intentar sacar de la setie al elemento de tipo {0} y con id: {1}", typeof(T), idElemento),
                                                    "ActualizarElValorDeUnCampoDeUnElemento");
            }
        }

        private void GuardarDocumentosAdjuntos(SPListItem item, T elemento)
        {
            try
            {
                IList<FicheroAdjunto> valores = Utilidades.ConseguirValorDeLaPropiedad(elemento, "DocumentosAdjuntos") as IList<FicheroAdjunto>;
                
                _spLista.ParentWeb.AllowUnsafeUpdates = true;

                foreach (FicheroAdjunto fichero in valores)
                {
                    try
                    {
                        item.Attachments.Add(fichero.NombreFichero, fichero.Contenido);
                        item.Update();
                    }
                    catch { ;}
                }
                _spLista.ParentWeb.AllowUnsafeUpdates = false;
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    ex.ToString(),
                                                    "GuardarDocumentosAdjuntos");
            }
        }

        private void EliminarDocumentosAdjuntos(SPListItem elemento)
        {
            try
            {
                var adjuntos = elemento.Attachments;
                SPAttachmentCollection ColAdjuntos = elemento.Attachments;
                int nAttachments = ColAdjuntos.Count;

                string p = string.Empty;
                while (nAttachments != 0)
                {
                    p = elemento.Attachments[0].ToString();
                    elemento.Attachments.Delete(p);
                    elemento.Update();
                    nAttachments = elemento.Attachments.Count;
                }

            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    ex.ToString(),
                                                    "EliminarDocumentosAdjuntos");
            }
        }

    }
}
