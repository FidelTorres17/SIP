// NUEVO
$('#btnNuevoUsuario').on('click', function () {
    $('#tituloModalUsuario').text('Nuevo usuario');
    $('#formUsuario')[0].reset();
    $('#Txt_UsuarioId').val('');
   /* $('#Activo').prop('checked', true);*/
});

// EDITAR
$('#tblUsuarios').on('click', '.btnEditar', function () {
    const b = $(this);
    $('#tituloModalUsuario').text('Editar usuario');

    $('#Txt_UsuarioId').val(b.data('id'));
    $('#RolId').val(b.data('rol'));
    $('#Txt_Nombre').val(b.data('nombre'));
    $('#Txt_ApellidoP').val(b.data('apellidop'));
    $('#Txt_ApellidoM').val(b.data('apellidom'));
    $('#Txt_Email').val(b.data('email'));
    $('#Txt_Telefono').val(b.data('telefono'));
    $('#Txt_UserName').val(b.data('username'));
    $('#Txt_Password').val(b.data('password'));
    //$('#Activo').prop('checked', b.data('activo') === true || b.data('activo') === "True");

    var modal = new bootstrap.Modal(document.getElementById('modalUsuario'));
    modal.show();
});

// ELIMINAR
$('#tblUsuarios').on('click', '.btnEliminar', function () {
    //$('#EliminarUsuarioId').val($(this).data('id'));
    //$('#EliminarNombre').text($(this).data('nombre'));

    Swal.fire({
        title: `¿Eliminar ${$(this).data('nombre') }?`,
        text: 'Se eliminará el registro',
        icon: 'warning',
        showCancelButton: true,
        //confirmButtonColor: '#d33',
        //cancelButtonColor: '#3085d6',
        confirmButtonColor: "#7A4A2E",
        cancelButtonColor: "#C9B8A6",
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            eliminarUsuario($(this).data('id'));
        }
    });

    //var modal = new bootstrap.Modal(document.getElementById('modalEliminar'));
    //modal.show();
});

function eliminarUsuario(UsuarioId) {

    $.ajax({
        url: '/Usuario/eliminarUsuario',
        type: 'POST',
        data: { UsuarioId: UsuarioId },
        success: function (response) {
            if (response.success) {

                Swal.fire({
                    icon: 'success',
                    title: '',
                    text: 'Usuario eliminado exitosamente',
                    timer: 2000,
                    showConfirmButton: false,
                    willClose: () => location.reload()
                });

            }
        }
    });

}

// GUARDAR (aquí tú conectas tu back)
$('#btnGuardarUsuario').on('click', function () {

    const _Nombre = $('#Txt_Nombre').val();
    const _ApellidoP = $('#Txt_ApellidoP').val();
    const _ApellidoM = $('#Txt_ApellidoM').val();
    const _RolId = $('#RolId').val();
    const _Email = $('#Txt_Email').val();
    const _Teleono = $('#Txt_Telefono').val();
    const _UserName = $('#Txt_UserName').val();
    const _Password = $('#Txt_Password').val();
    const _UsuarioId = $('#Txt_UsuarioId').val();

    //validaciones de campos

    if (_Nombre == "") {
        Swal.fire("", "Introduzca el nombre", "error");
        return;
    }
    if (_ApellidoP == "") {
        Swal.fire("", "Introduzca el apellido paterno", "error");
        return;
    }
    
    if (_RolId == "0") {
        Swal.fire("", "Seleccione un rol", "error");
        return;
    }
    if (_Email == "") {
        Swal.fire("", "Introduzca el email", "error");
        return;
    }
    if (_Teleono == "") {
        Swal.fire("", "Introduzca el telefono", "error");
        return;
    }
    // Expresión regular para validar correo
    const regexCorreo = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

    if (!regexCorreo.test(_Email)) {
        Swal.fire('', 'Formato de correo inválido. Ejemplo: nombre@dominio.com', 'error');
        return;
    }

    if (_UserName == "") {
        Swal.fire("", "Introduzca el usuario", "error");
        return;
    }
    if (!_UsuarioId) {
        if (_Password == "") {
            Swal.fire("", "Introduzca la contraseña", "error");
            return;
        }
    }

    var usuario = {
        UsuarioId: $('#Txt_UsuarioId').val(),
        Nombre: $('#Txt_Nombre').val(),
        ApellidoP: $('#Txt_ApellidoP').val(),
        ApellidoM: $('#Txt_ApellidoM').val(),
        RolId: $('#RolId').val(),
        Email: $('#Txt_Email').val(),
        Telefono: $('#Txt_Telefono').val(),
        UserName: $('#Txt_UserName').val(),
        Password: $('#Txt_Password').val()
    };

    $.ajax({
        url: '/Usuario/guardarUsuario',
        type: 'POST',
        data: usuario,
        success: function (response) {
            if (response.success) {

                const modalEl = document.getElementById('modalUsuario');
                const modal = bootstrap.Modal.getInstance(modalEl);
                modal.hide();

                Swal.fire({
                    icon: 'success',
                    title: '',
                    text: 'Usuario guardado exitosamente',
                    timer: 2000,
                    showConfirmButton: false,
                    willClose: () => location.reload()
                });
            }
        }
    });
});

