
$(function () {
    $("#btnGuardar").click(function () {
        console.log('btnGuardar');
        $("form").submit();
    });

    $("#btnEliminar").click(function () {
        console.log('btnEliminar', '@Url.Action("Eliminar", "Tarea")');
        var id = $(this).val();
        debugger
        $.ajax({
            url: 'https://localhost:44356/Tarea/Eliminar',
            type: 'POST',
            data: { Id: id },
            success: function (result) {
                if (result.success) {
                    alert(result.message);
                    // Redireccionar a la página de inicio u otra página
                    window.location.href = 'https://localhost:44356/Tarea/MostrarListadoTareas';
                } else {
                    alert(result.message);
                }
            },
            error: function () {
                // Error de conexión o servidor
                alert("Error al intentar eliminar la tarea.");
            }
        });
    });
});
