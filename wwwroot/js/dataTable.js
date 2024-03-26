$(document).ready(function () {
    $('#tipoCunetas').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/TiposCuentas/ListaCuentas",
            "type": "POST",
            "data": function (d) {
                d.start = d.start || 0;
                d.length = d.length || 10;
            }
        },
        "columns": [
            { "data": "id" },
            { "data": "nombre" },
            { "data": "usuarioId" },
            { "data": "orden" },
            {
                "data": null,
                "render": function (data, type, row) {
                    return "<button class='btnEliminar btn-outline-info' data-id='" + row.id + "'>Eliminar</button> <button class='btnEditar btn-outline-warning' data-id='" + row.id + "'>Actualizar</button>"
                        ;
                },
                "orderable": false
            }
        ],
    });

    $('#tipoCunetas tbody').on('click', '.btnEliminar', function () {
        var id = $(this).data('id');
        eliminarTipoCuenta(id);
    });

    function eliminarTipoCuenta(id) {
        $.ajax({
            type: "POST",
            url: '/TiposCuentas/BorrarTipoCuenta',
            data: { id: id },
            success: function () {
                location.reload();
            },
            error: function () {
                alert('Error al eliminar el tipo de cuenta.');
            }
        });
    }
    $(document).ready(function () {
        $('#tipoCunetas tbody').on('click', '.btnEditar', function () {
            var id = $(this).data('id');
            window.location.href = urlEditar + '/' + id;
        });
    });


    function editarTipoCuenta(id) {
        $.ajax({
            type: "POST",
            url: '/TiposCuentas/Editar',
            data: { id: id },
            success: function () {
                location.reload();
            },
            error: function () {
                alert('Error al editar el tipo de cuenta.');
            }
        });
    }
});