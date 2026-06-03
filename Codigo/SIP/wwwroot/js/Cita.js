// Nuevo
//$('#btnNuevaCita').on('click', function () {
//    limpiarModal();
//    $('#tituloModalCita').html('<i class="bi bi-calendar2-plus"></i> Nueva cita');
//    $('#modalCita').modal('show');
//});

// Editar (carga desde data-*)
$('#tblCitas').on('click', '.btnEditar', function () {
    const $b = $(this);
    limpiarModal();
    $('#tituloModalCita').html('<i class="bi bi-pencil-square"></i> Editar cita');

    $('#CitaId').val($b.data('id'));
    $('#Fecha').val($b.data('fecha'));
    $('#Hora').val($b.data('hora'));
    $('#Notas').val($b.data('notas') ?? '');
    $('#EstatusCitaId').val($b.data('estatusid'));

    // En dummy: llenamos texto (luego lo harás con combos reales)
    const $tr = $b.closest('tr');
    $('#Paciente').val($tr.find('td:eq(2)').text().trim());
    $('#Terapeuta').val($tr.find('td:eq(3)').text().trim());

    $('#modalCita').modal('show');
});

// Guardar (AJAX → tú lo conectas al back)
$('#btnGuardarCita').on('click', function () {
    //const _data = $(this);
    const _CitaId = $('#CitaId').val();
    const _PacienteId = $('#PacienteId').val();
    const _Fecha = $('#Fecha').val();
    const _Email = $('#Email').val();
    const _Teleono = $('#Telefono').val();
    const _Sexo = $('#Sexo').val();
    const _FechaNacimiento = $('#FechaNacimiento').val();
    const _PacienteId = $('#PacienteId').val();

    Swal.fire({
        icon: 'success',
        title: 'Listo',
        text: 'Aquí se enviaría por AJAX al controlador.',
        confirmButtonColor: '#6b4b3e'
    });
    $('#modalCita').modal('hide');
});

// Confirmar / Cancelar (AJAX → tú lo conectas)
$('#tblCitas').on('click', '.btnConfirmar', function () {
    const id = $(this).data('id');
    Swal.fire({
        title: '¿Confirmar cita?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Sí, confirmar',
        cancelButtonText: 'Cancelar',
        confirmButtonColor: '#6b4b3e',
        cancelButtonColor: '#8f7a6a'
    }).then(r => {
        if (r.isConfirmed) {
            // aquí tu ajax: /Citas/CambiarEstado
            Swal.fire({ icon: 'success', title: 'Confirmada', timer: 1200, showConfirmButton: false });
        }
    });
});

$('#tblCitas').on('click', '.btnCancelar', function () {
    const id = $(this).data('id');
    Swal.fire({
        title: '¿Cancelar cita?',
        text: 'La cita quedará cancelada.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, cancelar',
        cancelButtonText: 'Regresar',
        confirmButtonColor: '#b04a3a',
        cancelButtonColor: '#8f7a6a'
    }).then(r => {
        if (r.isConfirmed) {
            // aquí tu ajax: /Citas/CambiarEstado
            Swal.fire({ icon: 'success', title: 'Cancelada', timer: 1200, showConfirmButton: false });
        }
    });
});

function limpiarModal() {
    $('#formCita')[0].reset();
    $('#CitaId').val('');
}