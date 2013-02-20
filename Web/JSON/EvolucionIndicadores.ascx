

<style type="text/css">
   div.jqplot-table-legend-swatch {
    width: 5px;
    height: 5px;
    border-width: 3px 4px;
    }
    div.jqplot-table-legend-swatch-outline {
        border: 1px solid #CCCCCC;
        padding: 1px;
    }
    table.jqplot-table-legend, table.jqplot-cursor-legend {
        background-color: rgba(255, 255, 255, 0.6);
        border: 1px solid #CCCCCC;
        font-size: 0.75em;
        position: absolute;
    }
</style>

<script type="text/javascript" src="/js/funciones.js"></script>
<script type="text/javascript" src="/js/jquery.dataTables.js"></script>

<!--[if lt IE 9]><script language="javascript" type="text/javascript" src="/_layouts/1033/PortalCliente/js/jqplot/excanvas.js"></script><![endif]-->

<script type="text/javascript" src="/js/jqplot/jquery.jqplot.js"></script>
<script type="text/javascript" src="/js/jqplot/jqplot.dateAxisRenderer.js"></script>
<script type="text/javascript" src="/js/jqplot/jqplot.canvasTextRenderer.js"></script>
<script type="text/javascript" src="/js/jqplot/jqplot.canvasAxisLabelRenderer.js"></script>
<script type="text/javascript" src="/js/jqplot/jqplot.canvasAxisTickRenderer.js"></script>
<script type="text/javascript" src="/js/jqplot/jqplot.categoryAxisRenderer.js"></script>
<script type="text/javascript" src="/js/jqplot/jqplot.barRenderer.js"></script>

<script type="text/javascript">
    function cargarTabla(datos) {
        var datosParaDataTable = ConvertObjectToArray(datos);
        $('#tablaDatos').dataTable({
            "bDestroy": true,
            "bProcessing": true,
            "bPaginate": true,
            "bSortable": true,
            "aaSorting": [[ 5, "desc" ]],
            "bSearchable": true,
            "bFilter": true,
            "bInfo": true,
            "bJQueryUI": false,
            "bAutoWidth": true,
            "sDom": 'Rlfrtip',
            "sPaginationType": "full_numbers",
            "aaData": datosParaDataTable,
            "oLanguage": { "sProcessing": '<asp:Literal ID="oLanguage_sProcessing" runat="server" Text='<%$ Resources:JQueryDataTable, oLanguage_sProcessing%>' />',
                "sLengthMenu": '<asp:Literal ID="oLanguage_sLengthMenu" runat="server" Text='<%$ Resources:JQueryDataTable, oLanguage_sLengthMenu%>' />',
                "sZeroRecords": '<asp:Literal ID="oLanguage_sZeroRecords" runat="server" Text='<%$ Resources:JQueryDataTable, oLanguage_sZeroRecords%>' />',
                "sEmptyTable": '<asp:Literal ID="oLanguage_sEmptyTable" runat="server" Text='<%$ Resources:JQueryDataTable, oLanguage_sEmptyTable%>' />',
                "sInfo": '<asp:Literal ID="oLanguage_sInfo" runat="server" Text='<%$ Resources:JQueryDataTable, oLanguage_sInfo%>' />',
                "sInfoEmpty": '<asp:Literal ID="oLanguage_sInfoEmpty" runat="server" Text='<%$ Resources:JQueryDataTable, oLanguage_sInfoEmpty%>' />',
                "sInfoFiltered": '<asp:Literal ID="oLanguage_sInfoFiltered" runat="server" Text='<%$ Resources:JQueryDataTable, oLanguage_sInfoFiltered%>' />',
                "sInfoPostFix": '<asp:Literal ID="oLanguage_sInfoPostFix" runat="server" Text='<%$ Resources:JQueryDataTable, oLanguage_sInfoPostFix%>' />',
                "sSearch": '<asp:Literal ID="oLanguage_sSearch" runat="server" Text='<%$ Resources:JQueryDataTable, oLanguage_sSearch%>' />',
                "sUrl": '<asp:Literal ID="oLanguage_sUrl" runat="server" Text='<%$ Resources:JQueryDataTable, oLanguage_sUrl%>' />',
                "sInfoThousands": '<asp:Literal ID="oLanguage_sInfoThousands" runat="server" Text='<%$ Resources:JQueryDataTable, oLanguage_sInfoThousands%>' />',
                "sLoadingRecords": '<asp:Literal ID="oLanguage_sLoadingRecords" runat="server" Text='<%$ Resources:JQueryDataTable, oLanguage_sLoadingRecords%>' />',
                "oPaginate": {
                    "sFirst":  '<asp:Literal ID="oLanguage_oPaginate_sFirst" runat="server" Text='<%$ Resources:JQueryDataTable, oLanguage_oPaginate_sFirst%>' />',
                    "sPrevious": '<asp:Literal ID="oLanguage_oPaginate_sPrevious" runat="server" Text='<%$ Resources:JQueryDataTable, oLanguage_oPaginate_sPrevious%>' />',
                    "sNext": '<asp:Literal ID="oLanguage_oPaginate_sNext" runat="server" Text='<%$ Resources:JQueryDataTable, oLanguage_oPaginate_sNext%>' />',
                    "sLast": '<asp:Literal ID="oLanguage_oPaginate_sLast" runat="server" Text='<%$ Resources:JQueryDataTable, oLanguage_oPaginate_sLast%>' />'
                },
                "fnInfoCallback": null,
                "oAria": {
                    "sSortAscending": '<asp:Literal ID="oAria_sSortAscending" runat="server" Text='<%$ Resources:JQueryDataTable, oAria_sSortAscending%>' />',
                    "sSortDescending": '<asp:Literal ID="oAria_sSortDescending" runat="server" Text='<%$ Resources:JQueryDataTable, oAria_sSortDescending%>' />'
                }
            },
            "aoColumns": [{ "sName": "Fecha", 
                            "sTitle": '<asp:Literal ID="lblCabeceraFecha" runat="server" Text='<%$ Resources:EvolucionIndicadores, lblCabeceraFecha%>' />', 
                            "bSortable": true, 
                            "bVisible": true,
                            "fnRender": function (evolucionMensual) {
                                  return evolucionMensual.aData[5] + '/' + evolucionMensual.aData[6];
                              } 
                        },
                            { "sName": "Umbral", 
                              "sTitle": '<asp:Literal ID="lblCabeceraUmbral" runat="server" Text='<%$ Resources:EvolucionIndicadores, lblCabeceraUmbral%>' />', 
                              "bSortable": true, 
                              "bVisible": true,
                              "fnRender": function (evolucionMensual) {
                                  return evolucionMensual.aData[4];
                              } 
                        },
                        {   "sName": "ValorObjetivo", 
                            "sTitle": '<asp:Literal ID="lblCabeceraValorObjetivo" runat="server" Text='<%$ Resources:EvolucionIndicadores, lblCabeceraValorObjetivo%>' />', 
                            "bSortable": true, 
                            "bVisible": true,
                            "fnRender": function (evolucionMensual) {
                                  return evolucionMensual.aData[7];
                              }  
                        },
                        {   "sName": "ValorObtenido", 
                            "sTitle": '<asp:Literal ID="lblCabeceraValorObtenido" runat="server" Text='<%$ Resources:EvolucionIndicadores, lblCabeceraValorObtenido%>' />', 
                            "bSortable": true, 
                            "bVisible": true,
                            "fnRender": function (evolucionMensual) {
                                  return evolucionMensual.aData[8];
                              } 
                        },
                        {   "sName": "Desviacion", 
                            "sTitle": '<asp:Literal ID="lblCabeceraDesviacion" runat="server" Text='<%$ Resources:EvolucionIndicadores, lblCabeceraDesviacion%>' />', 
                            "bSortable": true, 
                            "bVisible": false,
                        },
                        {   "sName": "TimeStamp", 
                            "sTitle": 'TimeStamp', 
                            "bSortable": true, 
                            "bVisible": false,
                             "fnRender": function (evolucionMensual) {
                                  return evolucionMensual.aData[9];
                              } 
                        }
                        ]
        });
    }

    function pintarGrafica(nombreControl, valores, meses, tendencias){
        $.jqplot(nombreControl, [valores, tendencias], {
                                title: '',
                                seriesDefaults: {
                                    rendererOptions:{
                                       barPadding: 0,
                                       barWidth: 25,
                                       barMargin: 20,
                                       barDirection: 'vertical',
                                   },
                                   showLabel : true,
                                   pointLabels: {show: true, formatString: '%d'},
                                },
                                axesDefaults: {
                                    tickRenderer: $.jqplot.CanvasAxisTickRenderer,
                                    tickOptions: {
                                        angle: -30,
                                        fontSize: '9pt',
                                        formatString: '%,2f'
                                    }
                                },
                                axes: {
                                     xaxis: {
                                        renderer: $.jqplot.CategoryAxisRenderer,
                                        ticks: meses,                       
                                    },
                                    yaxis: {
                                        'min' : 0,
                                        'max' : 100,
                                        'pad' : 10,
                                        'numberTicks' : 20,
                                        tickOptions: {
                                                formatString: '%d'
                                            }
                                    }
                                },
                                grid: {
                                    borderColor: "#FFF",
                                    background: "#FFF",
                                    drawGridlines: true,
                                    shadow: true
                                }, 
                                series: [
                                    {
                                        label: 'Valor Obtenido', 
                                        renderer:$.jqplot.BarRenderer,
                                    },
                                    {
                                        label: 'Valor Objetivo', 
                                    },
                                ],
                                legend: {
                                    show: true,
                                    location: 'ne',
                                    placement: 'inside',
                                    xoffset: 5,
                                    yoffset: 5
                                }
                            }); 
    }

    function esDatoParaPintarValido(dato){
        if(!EsNumeroValido(dato.ValorObtenido)){
            alert('<asp:Literal ID="lblErrorFormatoValorObtenido" runat="server" Text='<%$ Resources:EvolucionIndicadores, lblErrorFormatoValorObtenido%>' />');
            return false;
        }
        if(!EsNumeroValido(dato.ValorObjetivo)){
            alert('<asp:Literal ID="lblErrorFormatoValorObjetivo" runat="server" Text='<%$ Resources:EvolucionIndicadores, lblErrorFormatoValorObjetivo%>' />');
            return false;
        }
        return true;
    }

    function conseguirAnioMesDelDato(dato){
        var resultado = '';
        resultado =  dato.Mes + dato.Anio;
        return resultado.toString();
    }
    //para los decimales trata hay que tratarlos en formato anglosajón con '.'
    function conseguirValorObtenidoDelDato(dato){
        var numero = '';
        numero = dato.ValorObtenido.replace(',','.');
        return parseFloat(numero);
    }
    function conseguirValorObjetivoDelDato(dato){
        var numero = '';
        numero = dato.ValorObjetivo.replace(',','.');
        return parseFloat(numero);
    }

    function cargarGrafica(nombreControl, datos) {
        var meses = [];
        var valores = [];
        var tendencias = [];
        for (cont = 0; cont < datos.length; cont++) {
            if(!esDatoParaPintarValido(datos[cont])){return;}
            meses.push(conseguirAnioMesDelDato(datos[cont]));
            valores.push(conseguirValorObtenidoDelDato(datos[cont]));
            tendencias.push(conseguirValorObjetivoDelDato(datos[cont]));
        }
        if((valores.length > 0) && (meses.length > 0) && (tendencias.length > 0)){
            pintarGrafica(nombreControl, valores, meses, tendencias);
        }
    }

    function EsNumeroValido(numero){
        if((numero == '') || (numero == null)){
            return true;
        }
        else{
            return (isInteger(numero)) || (isFloat(numero));
        }
    }

    function CargarSeguimientoIndicadoresDelMismoAnioEnLaPagina(guidIndicador, nombrePagina, anio, mes){
        var nocache = new Date().getTime();
        var param = {
            "accion": "CargarEvolucionesMensualesDelMismoAnio",
            "guidIndicador": guidIndicador,
            "nocache": nocache,
            "anio": anio,
            "mes": mes
        };

        $.getJSON(nombrePagina, param, function (datos) {
            cargarGrafica('SeguimientoIndicadoresDelMismoAnio', datos);
        });
    }

    function CargarSeguimientoIndicadoresDelAnioAnteriorEnLaPagina(guidIndicador, nombrePagina, anio, mes){
        var nocache = new Date().getTime();
        var param = {
            "accion": "CargarEvolucionesMensualesDelAnioAnterior",
            "guidIndicador": guidIndicador,
            "nocache": nocache,
            "anio": anio,
            "mes": mes
        };

        $.getJSON(nombrePagina, param, function (datos) {
            cargarGrafica('SeguimientoIndicadoresDelAnioAnterior', datos);
        });
    }

    function CargarIndicadoresActualesEnLaPagina(guidIndicador, nombrePagina){
        var nocache = new Date().getTime();
        var param = {
            "accion": "CargarEvolucionesMensuales",
            "guidIndicador": guidIndicador,
            "nocache": nocache,
        };

        $.getJSON(nombrePagina, param, function (datos) {
            cargarTabla(datos);
            cargarGrafica('IndicadoresActuales', datos);
        });
    }

    function cargarDatosEnLaPagina(guidIndicador, nombrePagina, mes, anio){
        CargarIndicadoresActualesEnLaPagina(guidIndicador, nombrePagina);
        CargarSeguimientoIndicadoresDelMismoAnioEnLaPagina(guidIndicador,  nombrePagina, anio, mes);
        CargarSeguimientoIndicadoresDelAnioAnteriorEnLaPagina(guidIndicador,  nombrePagina, anio, mes);
    }

    function sonDatosValidos(guidIndicador, nombrePagina, mes, anio){ 
        if((guidIndicador == null) || (guidIndicador == '')){
            alert('<asp:Literal ID="lblErrorGuidIndicador" runat="server" Text='<%$ Resources:EvolucionIndicadores, lblErrorGuidIndicador%>' />');
            return false;
        }
        if((nombrePagina == null) || (nombrePagina == ''))
        {
            alert('<asp:Literal ID="lblErrorNombrePagina" runat="server" Text='<%$ Resources:EvolucionIndicadores, lblErrorNombrePagina%>' />');
            return false;
        }
        if((mes == null) || (mes == '')){
            alert('<asp:Literal ID="lblErrorMes" runat="server" Text='<%$ Resources:EvolucionIndicadores, lblErrorMes%>' />');
            return false;
        }
        if((anio == null) || (anio == '') || (!isInteger(anio))){
            alert('<asp:Literal ID="lblErrorAnio" runat="server" Text='<%$ Resources:EvolucionIndicadores, lblErrorAnio%>' />');
            return false;
        }
        return true;
    }

    function cargarPagina(){
        var guidIndicador = conseguirValorDelQueryString('guidIndicador');
        var nombrePagina = conseguirNombreDeLaPagina();
        var mes = conseguirValorDelQueryString('mes');
        var anio = conseguirValorDelQueryString('anio');

        if (sonDatosValidos(guidIndicador, nombrePagina, mes, anio)) {
            cargarDatosEnLaPagina(guidIndicador, nombrePagina, mes, anio);
        }
    }

    $(document).ready(function () {
        cargarPagina();
    });

</script>
<style>
    #chart{ 
        margin: auto;
        width: 75%;
    }
</style>

<div class="">
    <h2 class="titpagina">
    <asp:Literal ID="lblCabeceraNombreDelIndicador" Text="" runat="server"></asp:Literal>
    </h2>
    <br />
</div>

<div class="sep">
    <h3 class="titu2 padTop16">
        <img src="/images/ico_tabl_indicadores.png" width="43" height="43" alt="Evolución semanal del indicador" />
        <asp:Literal ID="lblCabeceraTablaEvolucionMensualDelIndicador" runat="server" Text='<%$ Resources:EvolucionIndicadores, lblCabeceraTablaEvolucionMensualDelIndicador%>' />
    </h3>
    <table  id="tablaDatos" class="tablaDatos1" width="100%" border="0"></table>   
</div>

<div class="sep">
    <h3 class="titu2 padTop16">
        <img src="/images/ico_tabl_indicadores.png" width="43" height="43" alt="Evolución mensual del indicador" />
        <asp:Literal ID="lblCabeceraGraficaEvolucionMensualDelIndicador" runat="server" Text='<%$ Resources:EvolucionIndicadores, lblCabeceraGraficaEvolucionMensualDelIndicador%>' />
    </h3>
    <div id="IndicadoresActuales"></div>
</div>

<div class="sep">
    <h3 class="titu2 padTop16">
        <img src="/images/ico_tabl_indicadores.png" width="43" height="43" alt="Evolución mensual anterior del indicador" />
        <asp:Literal ID="lblCabeceraGraficaEvolucionMensualPasadaDelIndicador" runat="server" Text='<%$ Resources:EvolucionIndicadores, lblCabeceraGraficaEvolucionMensualPasadaDelIndicador%>' />
    </h3>
    <div id="SeguimientoIndicadoresDelMismoAnio" style="width:48%;float:left;"></div>
    <div id="SeguimientoIndicadoresDelAnioAnterior" style="width:48%;float:right;"></div>
</div>

<asp:Label ID="lblInfo"  ForeColor="Red"  Visible="true" Text=""  runat="server"></asp:Label>
