app.controller('CtrlParticipanteExistente', ['$scope', 'SendBox', '$state', 'EVEServices', '$window', '$stateParams', '$location', function ($scope, SendBox, $state, EVEServices, $window, $stateParams, $location) {

    $scope.participante = new Object();
    $scope.participante.idParticipante = $stateParams.participante;

    $scope.generos = [
        { genero: 'M', descripcion: 'MASCULINO' },
        { genero: 'F', descripcion: 'FEMENINO' }
    ];


    EVEServices.ObtenerInfoParticipante($scope.participante).then(function (data) {
        
        var fechanac = new Date(data.fechaNacimiento);
        $scope.participante = data;
        $scope.participante.fechaNacimiento = fechanac;
    });


    $scope.guardarParticipante = function (participante) {

        EVEServices.ActualizarRegistroParticipante(participante).then(function (data) {
            $state.go('inscripcion', { participante: $scope.participante.idParticipante });
        });
    }


}]);