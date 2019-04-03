app.controller('CtrlAsignacionEvento', ['$scope', 'SendBox','$state', 'EVEServices', '$window', function ($scope, SendBox, $state, EVEServices, $window) {

    $scope.eventos = new Object();
    $scope.ObtenerEventosAll = new Array(); //Si se va a utilizar en un angular-table debe de ser un array

    $scope.configListEventos = new Array();

    $scope.configListEventos = {
        itemsPerPage: 7,
        fillLastPage: false,
        maxPages: 10
    }


    /******************************************************************
    METODO QUE RETORNA EL LISTADO DE EVENTOS ACTIVOS PARA INICIAR EN LA VARIABLE GLOBAL
    ******************************************************************/
    EVEServices.ObtenerEventos("").then(function (data) {
        $scope.eventos = data;
    });

    EVEServices.ObtenerEventosAll("").then(function (data) {
        $scope.ObtenerEventosAll = data;
    });


    /******************************************************************
    METODO QUE ASIGNA EN LA VARIABLE GLOBAL EL ID DEL EVENTO
    ******************************************************************/
    $scope.AsignarEvento = function (ObjetoEvento) {
        if (typeof ObjetoEvento == 'undefined') {
            alert('Seleccione un Evento para iniciar las Inscripciones');
        } else if (ObjetoEvento.idEvento != null) {
            EVEServices.AsignarEvento(ObjetoEvento).then(function (data) {
                //$state.go('listadoParticipantes');
            });
        }
    };


}]);


