// NUEVO
$('#btnNuevoPaciente').on('click', function () {
    $('#tituloModalPaciente').text('Nuevo paciente');
    $('#formPaciente')[0].reset();
    $('#PacienteId').val('');
  
});

// EDITAR
$('#tblPacientes').on('click', '.btnEditar', function () {
    const b = $(this);
    $('#tituloModalPaciente').text('Editar paciente');

    $('#PacienteId').val(b.data('id'));
    $('#Nombre').val(b.data('nombre'));
    $('#ApellidoP').val(b.data('apellidop'));
    $('#ApellidoM').val(b.data('apellidom'));
    $('#Email').val(b.data('email'));
    $('#Telefono').val(b.data('telefono'));
    $('#FechaNacimiento').val(b.data('fechanacimiento'));
    $('#Sexo').val(b.data('sexo'));
    //$('#Activo').prop('checked', b.data('activo') === true || b.data('activo') === "True");

    var modal = new bootstrap.Modal(document.getElementById('modalPaciente'));
    modal.show();
});

// ELIMINAR
$('#tblPacientes').on('click', '.btnEliminar', function () {
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
            eliminarPaciente($(this).data('id'));
        }
    });

    //var modal = new bootstrap.Modal(document.getElementById('modalEliminar'));
    //modal.show();
});

function eliminarPaciente(PacienteId) {

    $.ajax({
        url: '/Paciente/eliminarPaciente',
        type: 'POST',
        data: { PacienteId: PacienteId },
        success: function (response) {
            if (response.success) {

                Swal.fire({
                    icon: 'success',
                    title: '',
                    text: 'Paciente eliminado exitosamente',
                    timer: 2000,
                    showConfirmButton: false,
                    willClose: () => location.reload()
                });

            }
        }
    });

}

// GUARDAR (aquí tú conectas tu back)
$('#btnGuardarPaciente').on('click', function () {

    const _Nombre = $('#Nombre').val();
    const _ApellidoP = $('#ApellidoP').val();
    const _ApellidoM = $('#ApellidoM').val();
    const _Email = $('#Email').val();
    const _Teleono = $('#Telefono').val();
    const _Sexo = $('#Sexo').val();
    const _FechaNacimiento = $('#FechaNacimiento').val();
    const _PacienteId = $('#PacienteId').val();

    //validaciones de campos

    if (_Nombre == "") {
        Swal.fire("", "Introduzca el nombre", "error");
        return;
    }
    if (_ApellidoP == "") {
        Swal.fire("", "Introduzca el apellido paterno", "error");
        return;
    }
    
    if (_Sexo == "") {
        Swal.fire("", "Seleccione el sexo", "error");
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

    if (_FechaNacimiento == "") {
        Swal.fire("", "Seleccion la fecha de nacimiento", "error");
        return;
    }
   

    var paciente = {
        PacienteId: _PacienteId,
        Nombre: _Nombre,
        ApellidoP: _ApellidoP,
        ApellidoM: _ApellidoM,
        Sexo: _Sexo,
        Email: _Email,
        Telefono: _Teleono,
        FechaNacimiento: _FechaNacimiento
    };

    $.ajax({
        url: '/Paciente/guardarPaciente',
        type: 'POST',
        data: paciente,
        success: function (response) {
            if (response.success) {

                const modalEl = document.getElementById('modalPaciente');
                const modal = bootstrap.Modal.getInstance(modalEl);
                modal.hide();

                Swal.fire({
                    icon: 'success',
                    title: '',
                    text: 'Paciente guardado exitosamente',
                    timer: 2000,
                    showConfirmButton: false,
                    willClose: () => location.reload()
                });
            }
        }
    });
});

//$('#tblPacientes').on('click', '.btnAgendarCita', function () {
//    const id = $(this).data('id');
//    const nombre = $(this).data('nombre');

//    //$('#CitaPacienteId').val(id);
//    //$('#CitaPacienteNombre').val(nombre);

//    $('#lblPaciente').text(nombre);
//    $('#CitaPacienteId').val(id);

//    // Limpia campos básicos
//    $('#Fecha').val('');
//    $('#Hora').val('');
//    $('#TerapeutaId').val('');
//    $('#Notas').val('');

//    // abre modal
//    const modal = new bootstrap.Modal(document.getElementById('modalCita'));
//    modal.show();
//});

