app.controller('CtrlParticipanteNuevo', ['$scope', 'SendBox', '$state', 'EVEServices', '$window', '$stateParams', '$location', function ($scope, SendBox, $state, EVEServices, $window, $stateParams, $location) {

    $scope.participante = new Object();
    
    $scope.generos = [
        { genero: 'M', descripcion: 'MASCULINO' },
        { genero: 'F', descripcion: 'FEMENINO' }
    ];

    /***************************************
    SE VALIDA SI ESTA ACTIVO EL EVENTO EN LA VARIABLE GLOBAL
    ****************************************/
    EVEServices.ValidaEvento("").then(function (data) {
        $scope.valida = data;
    });

    $scope.guardarParticipante = function (participante) {
        
        EVEServices.nuevoRegistroParticipante(participante).then(function (data) {
            $state.go('inscripcion', { participante: data.idParticipante });
        });
    }

}]);