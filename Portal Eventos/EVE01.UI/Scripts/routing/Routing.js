app.config(function ($stateProvider, $urlRouterProvider) {

    $stateProvider
    .state('Home', {
        url: '/',
        templateUrl: 'home/index2'
    })
    .state('asignacionevento', {
        url: '/AsignacionEvento',
        templateUrl: 'evento/AsignacionEvento',
        controller: 'CtrlAsignacionEvento'
    })
    .state('listarEventos', {
        url: '/Eventos',
        templateUrl: 'evento/ListarEventosMantenimiento',
        controller: 'CtrlAsignacionEvento'
    })
    .state('listadoParticipantes', {
        url: '/Listado',
        templateUrl: 'Listado/Listado',
        controller: 'CtrlListado'
    })
    .state('inscripcion', {
        url: '/Inscripciones/:participante',
        templateUrl: 'inscripciones/inscripciones',
        controller: 'CtrlEventoInscripcion'
    })
    .state('nuevoParticipante', {
        url: '/nuevo',
        templateUrl: 'Participante/registroParticipante',
        controller: 'CtrlParticipanteNuevo'
    })
    .state('actualizarParticipante', {
        url: '/actualizar/:participante',
        templateUrl: 'Participante/actualizarParticipanteEvento',
        controller: 'CtrlParticipanteExistente'
    })
    .state('saldoEvento', {
        url: '/saldo',
        templateUrl: 'Movimientos/Saldos',
        controller: 'CtrlSaldos'
    })
    .state('notaCredito', {
        url: '/nota/:participante',
        templateUrl: 'Movimientos/notacredito',
        controller: 'CtrlNotaCredito'
    });
});