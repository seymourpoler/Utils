var DataTableCalendarioFestivos = (function() {
    function DataTableCalendarioFestivos(idTabla) {
        this.idTabla = idTabla;

        this.BOTON_EDITAR = '<input type="image" class="edit txt-center display-block" src="/_layouts/3082/JS/DataTable/images/edit.png" title="editar" />';
        this.BOTON_ELIMINAR = '<input type="image" class="delete txt-center display-block" src="/_layouts/3082/JS/DataTable/images/delete.png" title="borrar" />';
        this.BOTON_CANCELAR = '<input type="image" class="cancel txt-center display-block" src="/_layouts/3082/JS/DataTable/images/back_enabled.png" title="cancelar" />';
        this.BOTON_GUARDAR = '<input type="image" class="save txt-center display-block" src="/_layouts/3082/JS/DataTable/images/save.png" title="guardar" />';
		
        this.tabla = $("#" + this.idTabla).dataTable({
            "oLanguage": { "sUrl": "/_layouts/3082/JS/DataTable/dataTables.spanish.txt" },
            "bFilter": false,
            "bInfo": false,
            "bJQueryUI": false,
            "bPaginate": false,
            "bDestroy": true,
            "bRetrieve": true,
            "bSort": false,
            "aoColumns": [{ "sTitle": "Guid", "bVisible": true,  "sClass": "guid" },
						  { "sTitle": "Fecha",  "sClass": "fecha"  },
						  { "sTitle": "Descripción",  "sClass": "descripcion"  },
						  { "sTitle": "Editar" },
						  { "sTitle": "Eliminar"}]
        });

        $(".tb-datos").on('focus', '.fecha', function() {
            $(this).datepicker();
        });
        var self = this;
        $('.tb-datos').on('click', 'input.edit', function(e) {
            e.preventDefault();
            $('.btn-anadir').attr('disabled', 'disabled');
            var fila = $(this).parents('tr')[0];
            self.editarFila(fila);
            $('.btn-anadir').removeAttr('disabled');
        });

        $('.tb-datos').on('click', 'input.cancel', function(e) {
            e.preventDefault();
            $('.btn-anadir').attr('disabled', 'disabled');
            var fila = $(this).parents('tr')[0];
            self.restaurarFila(fila);
            $('.btn-anadir').removeAttr('disabled');
        });

        $('.tb-datos').on('click', 'input.delete', function(e) {
            e.preventDefault();
            $('.btn-anadir').attr('disabled', 'disabled');
            var fila = $(this).parents('tr')[0];
			self.eliminarFila(fila);
        });

        $('.tb-datos').on('click', 'input.save', function(e) {
            e.preventDefault();
            var fila = $(this).parents('tr')[0];
            self.guardarFila(fila);
        });
    }

    var esFilaValida = function(fila) {
        var celdas = $('input', fila);
        for (var i = 0; i < celdas.length; i++) {
            if ($(celdas[i]).hasClass('required') && $(celdas[i]).val().trim() == "") {
                $(celdas[i]).addClass('error-form');
                return false;
            }
        }

        return true;
    }
	
	var esFilaVacia = function(fila){
		var celdas = $('input', fila);
        for (var i = 0; i < celdas.length; i++) {
            if ($(celdas[i]).val().trim() != "") {
                return false;
            }
        }

        return true;
	}

    DataTableCalendarioFestivos.prototype.pintarDatos = function(json) {
        for (var contador = 0; contador < json.length; contador++) {
            this.aniadirFila(json[contador]);
        }
    }
	
	DataTableCalendarioFestivos.prototype.limpiarTabla = function() {
		this.tabla.fnClearTable();
	}

    DataTableCalendarioFestivos.prototype.editarFila = function(fila) {
		var datosFila = this.tabla.fnGetData(fila);
        var celdas = $('>td', fila);
        celdas[0].innerHTML = '<input type="text" class="width-98 txt-center guid" value="' + datosFila[0] + '">';
        celdas[1].innerHTML = '<input type="text" class="fecha-ini fecha width-98 txt-center required fecha" value="' + datosFila[1] + '">';
        celdas[2].innerHTML = '<input type="text" class="width-98 txt-center required descripcion" value="' + datosFila[2] + '">';
        celdas[3].innerHTML = this.BOTON_GUARDAR;
        celdas[4].innerHTML = this.BOTON_CANCELAR;
    }

    DataTableCalendarioFestivos.prototype.guardarFila = function(fila) {
        if (!esFilaValida(fila)) { return; }

        var celdas = $('input', fila);

        var guid = celdas[0].value;
        var fecha = celdas[1].value;
        var descripcion = celdas[2].value;

        this.tabla.fnUpdate(guid, fila, 0, true);
        this.tabla.fnUpdate(fecha, fila, 1, true);
        this.tabla.fnUpdate(descripcion, fila, 2, true);
        this.tabla.fnUpdate(this.BOTON_EDITAR, fila, 3, true);
        this.tabla.fnUpdate(this.BOTON_ELIMINAR, fila, 4, true);
        this.tabla.fnDraw();
    }

    DataTableCalendarioFestivos.prototype.restaurarFila = function(fila) {
		if(esFilaVacia(fila)){
			this.eliminarFila(fila);
			return false;
		}
		var datos = this.tabla.fnGetData(fila);
        var celdas = $('>td', fila);
        for (var i = 0; i < celdas.length; i++) {
            this.tabla.fnUpdate(datos[i], fila, i, false);
        }
        this.tabla.fnDraw();
    }

    DataTableCalendarioFestivos.prototype.aniadirFila = function(datos) {
        return this.tabla.fnAddData([datos.Guid,
                                    toLocaleDateString(datos.Fecha),
                                    datos.Descripcion,
                                    this.BOTON_EDITAR,
                                    this.BOTON_ELIMINAR]);
    }

    DataTableCalendarioFestivos.prototype.crearNuevaFila = function() {
        var datosDeLaNuevaFila = { Guid: '', Fecha: '', Descripcion: '' };
        var nuevaFila = this.aniadirFila(datosDeLaNuevaFila);
		var filaHtml = this.tabla.fnGetNodes(nuevaFila[0]);
        this.editarFila(filaHtml);
    }

    DataTableCalendarioFestivos.prototype.eliminarFila = function(fila) {
        var respuesta = confirm('¿está seguro de eliminar la fila?');
        if (respuesta) {
            this.tabla.fnDeleteRow(fila);
        }
    }

    DataTableCalendarioFestivos.prototype.toJSON = function() {
        var resultado = [];
		var filas = $("#" + this.idTabla).find("tbody tr");
		$.each(filas, function(indiceFila) {
			var datos = {};		
			datos.Guid = $(this).find(".guid").text();
			datos.Fecha = $(this).find(".fecha").text();
			datos.Descripcion = $(this).find(".descripcion").text();
			resultado.push(datos);
		});
        return JSON.stringify(resultado);
    }

    return DataTableCalendarioFestivos;
})();
