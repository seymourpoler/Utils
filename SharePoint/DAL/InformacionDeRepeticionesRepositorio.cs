
using System;
using Microsoft.SharePoint;
using System.Collections.Generic;
using Comunes.Log.GestionExcepciones;
using DTO;

namespace Datos
{
    public class InformacionDeRepeticionesRepositorio : BaseRepositorioLista<InformacionDeRepeticion>
    {
        public InformacionDeRepeticionesRepositorio(string url)
            : base(url, "InformacionDeRepeticiones")
        {
            _gestorDeError = new GestorExcepciones(this.GetType().Namespace, this.GetType().Name);
        }

        public override int GuardarElemento(InformacionDeRepeticion elemento)
        {
            try
            {
                var nuevoItem = _spLista.Items.Add();
                nuevoItem = RellenarSPListItemConDatosDeLaRepeticion(nuevoItem, elemento);
                InsertarElemento(nuevoItem);
                return nuevoItem.ID;
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    "Error al guardar el elemento en la lista",
                                                    "GuardarElemento");
            }
        }

        public override void ActualizarElemento(InformacionDeRepeticion elemento)
        {
            try
            {
                var item = ConseguirSPListItemPorId(elemento.ID);
                item = RellenarSPListItemConDatosDeLaRepeticion(item, elemento);
                InsertarElemento(item);
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                   "Error al actualizar el elemento en la lista",
                                                    "ActualizarElemento");
            }
            
        }

        private void InsertarElemento(SPListItem item)
        {
            try
            {
                _spLista.ParentWeb.AllowUnsafeUpdates = true;
                item.Update();
                _spLista.ParentWeb.AllowUnsafeUpdates = false;

            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                   ex.ToString(),
                                                    "InsertarElemento");
            }
        }

        private SPListItem RellenarSPListItemConDatosDeLaRepeticion(SPListItem item, InformacionDeRepeticion informacion)
        {
            try
            {
                item["Perioicidad"] = informacion.Perioicidad;

                //Anual
                item["Mes"] = informacion.Anual.Mes;
                item["NumeroDeLaSemanaAnual"] = informacion.Anual.NumeroDeLaSemana;
                item["DiaDeLaSemanaAnual"] = informacion.Anual.DiaDeLaSemana;
                item["NumeroDeDiaDelMes"] = informacion.Anual.NumeroDeDiaDelMes;
                item["FrecuenciaAnual"] = informacion.Anual.FrecuenciaAnual;

                //Mensual               
                item["NumeroDeDia"] = informacion.Mensual.NumeroDeDia;
                item["FrecuenciaDeMes"] = informacion.Mensual.FrecuenciaDeMes;
                item["NumeroDeLaSemanaMensual"] = informacion.Mensual.NumeroDeLaSemana;
                item["DiaDeLaSemanaMensual"] = informacion.Mensual.DiaDeLaSemana;
                item["FrecuenciaMensual"] = informacion.Mensual.FrencuenciaMensual;

                //Semanal
                item["NumeroDeSemanas"] = informacion.Semanal.NumeroDeSemanas;
                item["DiaDeLaSemanaSemanal"] = ConseguirDiasDeLaSemana(informacion.Semanal.DiasDeLaSemana);

                //Diaria
                item["NumeroDeDias"] = informacion.Diaria.NumeroDeDias;
                item["FrecuenciaDiaria"] = informacion.Diaria.FrecuenciaDiaria;

                //El resto de propiedades
                item["NumeroDeDiasDeDuracionParaCadaRepeticion"] = informacion.NumeroDeDiasDeDuracionParaCadaRepeticion;
                item["NumeroDeOcurrencias"] = informacion.NumeroDeOcurrencias;
                item["FechaInicio"] = Utilidades.ConseguirValorDelTipo(informacion.FechaInicio, "DateTime");
                item["FechaFin"] = Utilidades.ConseguirValorDelTipo(informacion.FechaFin, "DateTime");
                item["GuidSerie"] = informacion.GuidSerie;

                return item;
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    ex.ToString(),
                                                    "RellenarSPListItemConDatosDeLaRepeticion");
            }
        }


        private SPFieldMultiChoiceValue ConseguirDiasDeLaSemana(IList<TipoDiaDeLaSemana> diasDeLaSemana)
        {   
            try
            {
                var resultado = new SPFieldMultiChoiceValue();
                foreach (var dia in diasDeLaSemana)
                {
                    resultado.Add(Utilidades.ConseguirTexto(dia));
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    ex.ToString(),
                                                    "ConseguirDiasDeLaSemana");
            }
        }

        private IList<TipoDiaDeLaSemana> ConseguirDiasDeLaSemana(SPFieldMultiChoiceValue valores)
        {
            try
            {
                var resultado = new List<TipoDiaDeLaSemana>();
                TipoDiaDeLaSemana valor;
                for(int contador = 0; contador<valores.Count; contador++)
                {
                    valor = Utilidades.ConseguirValorDelCampoTipoDiaDeLaSemana(valores[contador]);
                    resultado.Add(valor);
                }

                return resultado;
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    ex.ToString(),
                                                    "ConseguirDiasDeLaSemana");
            }
        }

        protected override IList<InformacionDeRepeticion> Mapear(SPListItemCollection elementos)
        {
            try
            {
                IList<InformacionDeRepeticion> resultados = new List<InformacionDeRepeticion>();
                InformacionDeRepeticion dato;
                foreach (SPListItem elemento in elementos)
                {
                    dato = Mapear(elemento);
                    resultados.Add(dato);
                }
                return resultados;
                
            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    "Error al mapear los elementos de la lista",
                                                    "Mapear");
            }
        }

        protected override InformacionDeRepeticion Mapear(SPListItem elemento)
        {
            try
            {
                var resultado = new InformacionDeRepeticion();

                if (elemento["Perioicidad"] != null)
                {
                    resultado.Perioicidad = Utilidades.ConseguirValorDelCampoTipoPerioicidad(elemento["Perioicidad"]);
                }

                resultado.Anual = MapearElementoAnual(elemento);
                resultado.Mensual = MapearElementoMensual(elemento);
                resultado.Semanal = MapearElementoSemanal(elemento);
                resultado.Diaria = MapearElementoDiario(elemento);
                
                if (elemento["FechaInicio"] != null)
                { 
                    resultado.FechaInicio = Utilidades.ConseguirValorDelCampoDateTime(elemento["FechaInicio"]); 
                }
                if (elemento["FechaFin"] != null)
                {
                    resultado.FechaFin = Utilidades.ConseguirValorDelCampoDateTime(elemento["FechaFin"]);
                }               
                if (elemento["GuidSerie"] != null)
                {
                    resultado.GuidSerie = Utilidades.ConseguirValorDelCampoGuid(elemento["GuidSerie"]);
                }
                if (elemento["ID"] != null)
                {
                    resultado.ID = Utilidades.ConseguirValorDelCampoInt32(elemento["ID"]);
                }
                if (elemento["NumeroDeDiasDeDuracionParaCadaRepeticion"] != null)
                {
                    resultado.NumeroDeDiasDeDuracionParaCadaRepeticion = Utilidades.ConseguirValorDelCampoInt32(elemento["NumeroDeDiasDeDuracionParaCadaRepeticion"]);
                }
                if (elemento["NumeroDeOcurrencias"] != null)
                {
                    resultado.NumeroDeOcurrencias = Utilidades.ConseguirValorDelCampoInt32(elemento["NumeroDeOcurrencias"]);
                }

                return resultado;

            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    "Error al mapear el elemento de la lista",
                                                    "Mapear");
            }
        }

        private Anual MapearElementoAnual(SPListItem elemento)
        {
            try
            {
                var resultado = new Anual();

                if(elemento["Mes"] != null)
                {
                    resultado.Mes = Utilidades.ConseguirValorDelCampoTipoMes(elemento["Mes"]);
                }

                if (elemento["NumeroDeLaSemanaAnual"] != null)
                {
                    resultado.NumeroDeLaSemana =  Utilidades.ConseguirValorDelCampoTipoNumeroDeLaSemana(elemento["NumeroDeLaSemanaAnual"]);
                }
                if(elemento["DiaDeLaSemanaAnual"] != null)
                {
                    resultado.DiaDeLaSemana = Utilidades.ConseguirValorDelCampoTipoDiaDeLaSemana(elemento["DiaDeLaSemanaAnual"]);
                }

                if(elemento["NumeroDeDiaDelMes"] != null)
                {
                    resultado.NumeroDeDiaDelMes = Utilidades.ConseguirValorDelCampoInt32(elemento["NumeroDeDiaDelMes"]);
                }

                if (elemento["FrecuenciaAnual"] != null)
                {
                    resultado.FrecuenciaAnual = Utilidades.ConseguirValorDelCampoTipoFrencuenciaAnual(elemento["FrecuenciaAnual"]);
                }
                

                return resultado;

            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    "Error al mapear el elemento de la lista",
                                                    "MapearElementoAnual");
            }
        }

        private Mensual MapearElementoMensual(SPListItem elemento)
        {
            try
            {
                var resultado = new Mensual();
                if(elemento["NumeroDeDia"] != null)
                {
                    resultado.NumeroDeDia = Utilidades.ConseguirValorDelCampoInt32(elemento["NumeroDeDia"]);
                }
                if(elemento["FrecuenciaDeMes"] != null)
                {
                    resultado.FrecuenciaDeMes = Utilidades.ConseguirValorDelCampoInt32(elemento["FrecuenciaDeMes"]);
                }
                if (elemento["FrecuenciaMensual"] != null)
                {
                    resultado.FrencuenciaMensual = Utilidades.ConseguirValorDelCampoTipoFrecuenciaMensual(elemento["FrecuenciaMensual"]);
                }

                if (elemento["NumeroDeLaSemanaMensual"] != null)
                {
                    resultado.NumeroDeLaSemana = Utilidades.ConseguirValorDelCampoTipoNumeroDeLaSemana(elemento["NumeroDeLaSemanaMensual"]);
                }

                if (elemento["DiaDeLaSemanaMensual"] != null)
                {
                    resultado.DiaDeLaSemana = Utilidades.ConseguirValorDelCampoTipoDiaDeLaSemana(elemento["DiaDeLaSemanaMensual"]);
                }
                return resultado;

            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    "Error al mapear el elemento de la lista",
                                                    "MapearElementoMensual");
            } 
        }

        private Semanal MapearElementoSemanal(SPListItem elemento)
        {
            try
            {
                var resultado = new Semanal();
                if (elemento["NumeroDeSemanas"] != null)
                {
                    resultado.NumeroDeSemanas = Utilidades.ConseguirValorDelCampoInt32(elemento["NumeroDeSemanas"]);
                }
                if (elemento["DiaDeLaSemanaSemanal"] != null)
                {
                    var valores = new SPFieldMultiChoiceValue(elemento["DiaDeLaSemanaSemanal"].ToString());
                    resultado.DiasDeLaSemana = ConseguirDiasDeLaSemana(valores);
                }
                return resultado;

            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    "Error al mapear el elemento de la lista",
                                                    "MapearElementoSemanal");
            }
        }

        private Diaria MapearElementoDiario(SPListItem elemento)
        {
            try
            {
                var resultado = new Diaria();

                if (elemento["NumeroDeDias"] != null)
                {
                    resultado.NumeroDeDias = Utilidades.ConseguirValorDelCampoInt32(elemento["NumeroDeDias"]);
                }

                if (elemento["FrecuenciaDiaria"] != null)
                {
                    resultado.FrecuenciaDiaria = Utilidades.ConseguirValorDelCampoTipoFrencuenciaDiaria(elemento["FrecuenciaDiaria"]);
                }
                
                return resultado;

            }
            catch (Exception ex)
            {
                throw _gestorDeError.TratarExcepcion(ex,
                                                    "Error al mapear el elemento de la lista",
                                                    "MapearElementoDiario");
            }
        }
    }
}
