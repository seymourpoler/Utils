

<script type="text/javascript" src="/_layouts/1033/PortalCliente/js/funciones.js"></script>
<script type="text/javascript" src="/_layouts/1033/PortalCliente/js/gestorHTMLControles.js"></script>
<script type="text/javascript" src="/_layouts/1033/PortalCliente/js/jquery.dataTables.js"></script>

<script type="text/javascript">
function cargarDatosEnTabla(datos) {
        $('#tablaDatos').dataTable({
            "bDestroy": true,
            "bProcessing": true,
            "bPaginate": true,
            "bSortable": true,
            "bSearchable": true,
            "bFilter": true,
            "bJQueryUI": true,
            "bAutoWidth": false,
            "bInfo": true,
            "sDom": 'Rlfrtip',
            "sPaginationType": "full_numbers",
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
            "aaData": datos,
            "aoColumns": [{ "sName": "Fecha", 
                            "sTitle": "<asp:Literal ID="lblCabeceraFecha" runat="server" Text='<%$ Resources:ResultadosDeBusqueda, lblCabeceraFecha%>' />", 
                            "bSortable": true, 
                            "bVisible": true,
                            "sWidth" : "15%",
                            "fnRender": function (elementoDelSistema) {
                                  return elementoDelSistema.aData[0];
                              }
                          },
                          { "sName": "Texto", 
                            "sTitle": "<asp:Literal ID="lblCabeceraTexto" runat="server" Text='<%$ Resources:ResultadosDeBusqueda, lblCabeceraTexto%>' />", 
                            "bSortable": true, 
                            "bVisible": true,
                            "sWidth" : "85%",
                             "fnRender": function (elementoDelSistema) {
                                  return "<a href='" + elementoDelSistema.aData[2] + "'>" + elementoDelSistema.aData[1] + "</a>";
                              }
                          }
                          ]
        });
}
    
function conseguirValorDelAmbito(idCombo){
	var gestorCombo = new GestorCombo(idCombo);
	return gestorCombo.conseguirValorSeleccionado();
}

function conseguirLoQueHayQueBuscarDeLaCajaDeTexto(idTxtBox){
	var cajaDeTexto = document.getElementById(idTxtBox);
	return cajaDeTexto.value;
}

function buscar(){
	var ambito = conseguirValorDelAmbito('filtro');
	var loQueBuscar = conseguirLoQueHayQueBuscarDeLaCajaDeTexto('termino');
    if((loQueBuscar == ''))
    {
        alert("<asp:Literal ID="lblCajaDeTextoObligatoria" runat="server" Text='<%$ Resources:ResultadosDeBusqueda, lblCajaDeTextoObligatoria%>' />");
        return;
    }
	var nombrePagina = conseguirNombreDeLaPagina();
    var nocache = new Date().getTime();
    var param = {
        "accion": "Buscar",
        "nocache": nocache,
        "ambito": ambito,
        "loQueBuscar": loQueBuscar
    };

    $.getJSON(nombrePagina, param, function (returnData) {
        cargarDatosEnTabla(returnData);
    });
}

function cargarModulosEnElDesplegable(datos) {
    var gestorCombo = new GestorCombo('filtro');
    for (var contador = 0; contador < datos.length; contador++) {
        gestorCombo.aniadirOption(new ComboOption(datos[contador].NombreParaMostar, datos[contador].NombreInterno));
    }
}

function cargarPagina(datos){
    var nombrePagina = conseguirNombreDeLaPagina();
    var nocache = new Date().getTime();
    var param = {
        "accion": "CargarAmbitos",
        "nocache": nocache
    };

    $.getJSON(nombrePagina, param, function (datos) {
        cargarModulosEnElDesplegable(datos);    
    });
}

$(document).ready(function (){
        cargarPagina();
    });
</script>

<div class="sep">
    <h3 class="titu2">
	    <IMG alt="Búsqueda avanzada" src="/_layouts/1033/PortalCliente/images/ico_busqueda_avan.png" width=43 height=43 />
	    Búsqueda avanzada
    </h3>

    <div class="form nopad">
	    <LABEL class=" pie_first-child" for=termino _pieId="_75"></LABEL>
	    <!-- <input id="termino" name="termino" value='Page.Request.Form["txtBuscar"] ' type="text" /> -->
        <input id="termino" name="termino" value='' type="text" />
	    <LABEL class="padLeft16 " for=filtro _pieId="_80">en:</LABEL> 
	    <SELECT id="filtro" name="filtro"> 
            <OPTION value="" selected>Todo el site</OPTION> 
        </SELECT> 
	
	    <input id="buscar" class="btn" name="buscar" value="Buscar" onclick="window.buscar();" />
    </div>
</div>

<div class="sep">
    <H3 class=titu2
        <IMG alt="Resultado de la búsqueda" src="/_layouts/1033/PortalCliente/images/ico_busqueda.png" width=43 height=43>
        Resultados de la búsqueda
    </H3>
    <table id="tablaDatos" class="tablaDatos1 verde" border="0" width="100%"></table>
</div>

<asp:Label ID="lblInfo"  ForeColor="Red"  Visible="true" Text=""  runat="server"></asp:Label>
