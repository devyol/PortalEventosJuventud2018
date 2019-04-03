app.controller('CtrlListado', ['$scope', 'SendBox', '$state', 'EVEServices', '$window', function ($scope, SendBox, $state, EVEServices, $window) {

    /***************************************
    VARIABLES
    ****************************************/
    $scope.participantes = new Array();
    $scope.valores = new Object();
    $scope.totales = new Object();

    $scope.valida = new Object();

    $scope.configListParticipantes = new Array();

    $scope.configListParticipantes = {
        itemsPerPage: 5,
        fillLastPage: false,
        maxPages: 10
    }

    /***************************************
    SE VALIDA SI ESTA ACTIVO EL EVENTO EN LA VARIABLE GLOBAL
    ****************************************/
    EVEServices.ValidaEvento("").then(function (data) {
        $scope.valida = data;        
        if (data.activo == true) {            
            /***************************************
            SE OBTIENE EL LISTADO DE PARTICIPANTES
            ****************************************/
            EVEServices.ObtenerParticipantes("").then(function (data) {
                $scope.participantes = data;
            });
            /***************************************
            SE OBTIENE EL LISTADO DE VALORES ESTADISTICOS
            ****************************************/
            EVEServices.valoresEstadistica("").then(function (data) {
                $scope.valores = data;
            });
            /***************************************
            SE OBTIENE EL LISTADO DE DE TOTALES ESTADISTICOS
            ****************************************/
            EVEServices.valoresTotales("").then(function (data) {
                $scope.totales = data;
            });
        }
    });

    $scope.SeleccionarParticipante = function (objParticipante) {
        //EVEServices.participante = objParticipante;
        $state.go('inscripcion', { participante: objParticipante.idParticipante });
    };





}]);