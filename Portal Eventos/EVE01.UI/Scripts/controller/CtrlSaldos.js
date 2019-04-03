app.controller('CtrlSaldos', ['$scope', 'SendBox', '$state', 'EVEServices', '$window', '$stateParams', '$location', function ($scope, SendBox, $state, EVEServices, $window, $stateParams, $location) {

    
    $scope.saldosdiarios = new Object();
    $scope.saldototal = new Object();

        EVEServices.saldosDiariosEvento("").then(function (data) {
            $scope.saldosdiarios = data;
        });

        EVEServices.saldosTotalEvento("").then(function (data) {
            $scope.saldototal = data;
        });


}]);