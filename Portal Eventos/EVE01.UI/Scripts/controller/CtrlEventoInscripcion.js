app.controller('CtrlEventoInscripcion', ['$scope', 'SendBox', '$state', 'EVEServices', '$window', '$stateParams', '$location', function ($scope, SendBox, $state, EVEServices, $window, $stateParams, $location) {

    $scope.valida = new Object();
    $scope.participante = new Object();
    $scope.participante.idParticipante = $stateParams.participante;
    $scope.opcionesParticipante = new Object();
    $scope.saldo = new Object();
    $scope.informacionBuses = new Object();
    $scope.busesDisponibles = new Object();
    $scope.inscripcionBus = new Object();
    $scope.inscripcionSilla = new Object();
    $scope.inscripcionHospedaje = new Object();
    $scope.informacionBusAsignado = new Object();
    $scope.test = true;
    $scope.movimiento = new Object();
    $scope.movimientos = new Object();
    $scope.movimientosCargos = new Object();
    $scope.movimientosAnulados = new Object();
    $scope.cantidad;

    $scope.tipopago = {
        opcion: 'efectivo'        
    };

    $scope.pago = {
        opcion: 'abono'
    };

    $scope.valtext = $scope.pago.opcion == 'total' ? true : false;
    
    
    /***************************************
    SE VALIDA SI ESTA ACTIVO EL EVENTO EN LA VARIABLE GLOBAL
    ****************************************/
    EVEServices.ValidaEvento("").then(function (data) {
        $scope.valida = data;
    });
    
    $scope.regresarListado = function () {
        $state.go('listadoParticipantes');
    };

    EVEServices.ObtenerInfoParticipante($scope.participante).then(function (data) {
        $scope.participante = data;        
        if (data.validaInscripcionbool) {//VALIDA SI ESTA INSCRITO PARA HACER LA PETICION DE INFORMACION
            //PETICION INFORMACION DE OPCIONES
            $scope.listarOpciones($scope.participante);
            //PETICION INFORMACIOIN DE SALDOS
            $scope.saldos($scope.participante);
            //PETICION INFORMACIOIN DE BUS ASIGNADO
            $scope.getinfoBusesAsignados();
            //PETICION INFORMACIOIN DE SILLA ASIGNADA
            $scope.getinfoSillaAsignada();
            //PETICION INFORMACIOIN DE MOVIMIENTOS
            $scope.getMovimientos();
            //PETICION INFORMACIOIN DE MOVIMIENTOS CARGOS
            $scope.getMovimientosCargos();
            //PETICION INFORMACIOIN DE MOVIMIENTOS CARGOS
            $scope.getMovimientosAnulados();            
        }
    });    

    /***************************************
    METODO PARA INSCRIBIR A UN PARTICIPANTE
    ****************************************/
    $scope.Inscribir = function (objParticipante) {

        var status = confirm("Desea Inscribir al Participante :" + objParticipante.NombreCompleto);

        if (status == true) {
            EVEServices.inscribirParticipante(objParticipante).then(function (data) {
                $scope.listarOpciones(objParticipante);
                $state.transitionTo($state.current, $stateParams, { reload: true, inherit: false, notify: true });
            });
        } else {

        }
    }

    /***************************************
    METODO PARA ANULAR LA INSCRIPCION DE UN PARTICIPANTE
    ****************************************/
    $scope.Anular = function (objParticipante) {

        var status = confirm("Desea Anular la Inscripcion del Participante :" + objParticipante.NombreCompleto);

        if (status == true) {
            EVEServices.anularInscripcion(objParticipante).then(function (data) {
                $state.transitionTo($state.current, $stateParams, { reload: true, inherit: false, notify: true });
            });
        }

    }

    $scope.listarOpciones = function (objParticipante) {
        EVEServices.opcionesInscripcion(objParticipante).then(function (data) {
            $scope.opcionesParticipante = data;
            
        });
    }

    $scope.saldos = function (objParticipante) {
        EVEServices.saldoParticipanteEvento(objParticipante).then(function (data) {
            $scope.saldo = data;            
        });
    }

    $scope.accionCheck = function (val, obj) {
        obj.estadoopcion = val;
        
        EVEServices.cambiarEstado(obj).then(function (data) {
            //$state.transitionTo($state.current, $stateParams, { reload: true, inherit: false, notify: true });
            $scope.listarOpciones($scope.participante);
            $scope.saldos($scope.participante);
            $scope.getinfoBusesAsignados();
        });
    }

    $scope.asignacionBus = function (objectoBus) {

        $scope.inscripcionBus.idParticipante = $scope.participante.idParticipante;
        $scope.inscripcionBus.idBus = objectoBus.idBus;
        $scope.inscripcionBus.noBus = objectoBus.noBus;

        EVEServices.asignaBuses($scope.inscripcionBus).then(function (data) {
            $scope.getInfoBuses();
            $scope.getinfoBusesDisponibles();
            $scope.getinfoBusesAsignados();
        });        
    }

    $scope.mostralModalAsignacionBuses = function () {

        $scope.getInfoBuses();
        $scope.getinfoBusesDisponibles();        
        $('#ModalAsignarBus').modal('show');
    }
    
    $scope.getInfoBuses = function () {
        EVEServices.infoBuses("").then(function (data) {
            $scope.informacionBuses = data;
        });
    }
    
    $scope.getinfoBusesDisponibles = function () {
        EVEServices.infoBusesDisponibles($scope.participante).then(function (data) {
            $scope.busesDisponibles = data;
        });
    }

    $scope.getinfoBusesAsignados = function () {        
        EVEServices.infoBusAsignado($scope.participante).then(function (data) {
            if (data.noBus == null) {                
                $scope.informacionBusAsignado = "SIN BUS";
            } else {                
                $scope.informacionBusAsignado = String(data.noBus);                
            }
            
        });
    }

    $scope.clickradio = function (val) {        

        if (val == 'total') {
            $scope.valtext = true;
            $scope.cantidad = "";
        } else {            
            $scope.valtext = false;            
        }

    }

    $scope.pagar = function (tipopago, cantidad, participante, modalidad, saldoPendiente) {

        var cant = modalidad == 'abono' ? cantidad : saldoPendiente;        

        var mensaje = confirm("Esta seguro de realizar el pago por: " + cant);
        
        if (mensaje == true) {

            var tipo = tipopago == 'efectivo' ? 1 : 2;

            $scope.movimiento.idParticipante = $scope.participante.idParticipante;
            $scope.movimiento.idTipoPago = tipo;
            $scope.movimiento.cantidad = cant;            
            $scope.movimiento.nombreCompleto = participante.nombre + " " + participante.apellido;

            EVEServices.registrarPago($scope.movimiento).then(function (data) {
                $scope.cantidad = "";
                //PETICION INFORMACIOIN DE SALDOS
                $scope.saldos($scope.participante);
                //PETICION INFORMACIOIN DE SILLA ASIGNADA
                $scope.getinfoSillaAsignada();
                //PETICION INFORMACIOIN DE MOVIMIENTOS
                $scope.getMovimientos();
            });
        }

    }
    
    $scope.getinfoSillaAsignada = function () {

        $scope.inscripcionSilla.idParticipante = $scope.participante.idParticipante;

        EVEServices.infoSilla($scope.inscripcionSilla).then(function (data) {
            $scope.inscripcionSilla = data;
        });
    }

    $scope.getMovimientos = function () {

        $scope.movimiento.idParticipante = $scope.participante.idParticipante;

        EVEServices.listaMovimientos($scope.movimiento).then(function (data) {
            $scope.movimientos = data;
        });
    }

    $scope.getMovimientosCargos = function () {

        $scope.movimiento.idParticipante = $scope.participante.idParticipante;

        EVEServices.listaMovimientosCargos($scope.movimiento).then(function (data) {
            $scope.movimientosCargos = data;
        });
    }

    $scope.getMovimientosAnulados = function () {

        $scope.movimiento.idParticipante = $scope.participante.idParticipante;

        EVEServices.listaMovimientosAnulados($scope.movimiento).then(function (data) {
            $scope.movimientosAnulados = data;
        });
    }

    $scope.irRecibo = function (objMOv) {
        $window.open("http://localhost:1663/Inscripciones/recibo?idmov=" + objMOv.idMovimiento);
        //$window.open("http://192.168.1.2/Inscripciones/recibo?idmov=" + objMOv.idMovimiento);
    }

    $scope.actualizarDatosEvento = function () {        

        $state.go('actualizarParticipante', { participante: $scope.participante.idParticipante });

    }

    $scope.mostralModalAnotacionHospedaje = function () {

        $scope.getinfoHospedaje();
        $('#ModalAnotacionHospedaje').modal('show');
    }

    $scope.getinfoHospedaje = function () {

        $scope.objetoHospedaje = new Object();
        $scope.objetoHospedaje.idParticipante = $scope.participante.idParticipante;

        EVEServices.infoHospedaje($scope.objetoHospedaje).then(function (data) {
            $scope.inscripcionHospedaje = data;
        });
    }

    $scope.registroHospedaje = function (obj) {

        EVEServices.registroHospedaje(obj).then(function (data) {
            $scope.getinfoHospedaje();
        });
    }

    $scope.irNotaCredito = function () {

        $state.go('notaCredito', { participante: $scope.participante.idParticipante });

    }

}]);