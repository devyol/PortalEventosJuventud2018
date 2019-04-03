app.controller('CtrlNotaCredito', ['$scope', 'SendBox', '$state', 'EVEServices', '$window', '$stateParams', '$location', function ($scope, SendBox, $state, EVEServices, $window, $stateParams, $location) {

    $scope.participante = new Object();
    $scope.participante.idParticipante = $stateParams.participante;

    $scope.regresar = function () {

        $state.go('inscripcion', { participante: $scope.participante.idParticipante });

    }

}]);