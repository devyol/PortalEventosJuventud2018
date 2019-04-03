app.service('EVEServices', function ($http, $q, SendBox) {

    var EVEServices = new Object();

    //-----------------------------------------------------------------------
    //  OBTIENE EL DATO DE VALIDACION DEL EVENTO ACTIVO
    //-----------------------------------------------------------------------

    this.ValidaEvento = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/Eventos/ValidaEventoActivo')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.val = data.data;
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.val);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.val);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    //  OBTIENE EL DATO DE VALIDACION DEL EVENTO ACTIVO
    //-----------------------------------------------------------------------

    this.AsignarEvento = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/Eventos/AsignarEventoGlobal')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.ev = data.data;
                    alert(data.mensaje);
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.ev);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.ev);
        };
        return deferred.promise;
    };
    

    //-----------------------------------------------------------------------
    //  OBTIENE EL LISTADO DE EVENTOS ACTIVOS
    //-----------------------------------------------------------------------

    this.ObtenerEventos = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/Eventos/ListadoEventosActivos')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.eventos = data.data;
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.eventos);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.eventos);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    //  OBTIENE EL LISTADO DE EVENTOS ACTIVOS E INACTIVOS
    //-----------------------------------------------------------------------

    this.ObtenerEventosAll = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/Eventos/ListadoEventos')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.eve = data.data;
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.eve);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.eve);
        };
        return deferred.promise;
    };


    //-----------------------------------------------------------------------
    //  OBTIENE EL LISTADO DE PARTICIPANTES ACTIVOS
    //-----------------------------------------------------------------------

    this.ObtenerParticipantes = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/Participante/ObtenerListadoParticipantes')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.part = data.data;
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.part);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.part);
        };
        return deferred.promise;
    };


    //-----------------------------------------------------------------------
    //  OBTIENE INFORMACION DE UN PARTICIPANTE
    //-----------------------------------------------------------------------

    this.ObtenerInfoParticipante = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/Participante/ObtenerParticipante')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.info = data.data;
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.info);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.info);
        };
        return deferred.promise;
    };


    //-----------------------------------------------------------------------
    //  INSCRIBE A UN PARTICIPANTE
    //-----------------------------------------------------------------------

    this.inscribirParticipante = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/Inscripcion/Inscribir')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.ins = data.data;
                    alert(data.mensaje);
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.ins);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.ins);
        };
        return deferred.promise;
    };


    //-----------------------------------------------------------------------
    //  ANULA LA INSCRIPCION DE UN PARTICIPANTE
    //-----------------------------------------------------------------------

    this.anularInscripcion = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/Inscripcion/Anular')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.ins = data.data;
                    alert(data.mensaje);
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.ins);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.ins);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // OBTENER LAS OPCIONES DE INSCRIPCION DE UN PARTICIPANTE
    //-----------------------------------------------------------------------

    this.opcionesInscripcion = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/InscripcionOpcion/listarOpcionesInscripcion')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.opciones = data.data;
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.opciones);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.opciones);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // OBTENER EL SALDO DEL PARTICIPANTE
    //-----------------------------------------------------------------------

    this.saldoParticipanteEvento = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/ParticipanteSaldos/mostrarSaldoParticipante')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.saldo = data.data;
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.saldo);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.saldo);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // CAMBIAR EL ESTADO DE LAS OPCIONES
    //-----------------------------------------------------------------------

    this.cambiarEstado = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/InscripcionOpcion/cambiarEstadoOpcion')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.modificar = data.data;
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.modificar);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.modificar);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // LISTA INFORMACION DE LOS BUSES DEL EVENTO
    //-----------------------------------------------------------------------

    this.infoBuses = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/EventoBuses/informacionBuses')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.info = data.data;
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.info);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.info);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // LISTA INFORMACION DE LOS BUSES DISPONIBLES PARA EL PARTICIPANTE
    //-----------------------------------------------------------------------

    this.infoBusesDisponibles = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/InscripcionBus/busesDisponiblesInscrito')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.bus = data.data;
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.bus);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.bus);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // ASIGNA BUSES A LOS PARTICIPANTES
    //-----------------------------------------------------------------------

    this.asignaBuses = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/InscripcionBus/asignacionBus')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.asigna = data.data;
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.asigna);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.asigna);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // INFORMACION DE BUS ASIGNADO
    //-----------------------------------------------------------------------

    this.infoBusAsignado = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/InscripcionBus/infoBusAsignado')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.bus = data.data;
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.bus);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.bus);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // REGISTRA MOVIMIENTOS DE PAGO
    //-----------------------------------------------------------------------

    this.registrarPago = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/Movimientos/registraMovimiento')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.pago = data.data;
                    alert(data.mensaje);
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.pago);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.pago);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // INFORMACION DE SILLA ASIGNADA
    //-----------------------------------------------------------------------

    this.infoSilla = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/InscripcionSilla/infoSillaAsignada')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.infsilla = data.data;                    
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.infsilla);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.infsilla);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // LISTA MOVIMIENTOS DE PARTICIPANTE
    //-----------------------------------------------------------------------

    this.listaMovimientos = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/Movimientos/listaMovimientos')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.mov = data.data;
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.mov);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.mov);
        };
        return deferred.promise;
    };


    //-----------------------------------------------------------------------
    // REGISTRAR NUEVO PARTICIPANTE
    //-----------------------------------------------------------------------

    this.nuevoRegistroParticipante = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/Participante/NuevoParticipante')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.nuevo = data.data;
                    alert(data.mensaje);
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.nuevo);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.nuevo);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // ACTUALIZAR DATOS DE PARTICIPANTE
    //-----------------------------------------------------------------------

    this.ActualizarRegistroParticipante = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/Participante/ActualizarParticipante')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.upd = data.data;
                    alert(data.mensaje);
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.upd);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.upd);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // INFORMACION DE HOSPEDAJE
    //-----------------------------------------------------------------------

    this.infoHospedaje = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/InscripcionHospedaje/infoHospedaje')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.hos = data.data;                    
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.hos);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.hos);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // REGISTRAR HOSPEDAJE
    //-----------------------------------------------------------------------

    this.registroHospedaje = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/InscripcionHospedaje/anotacionHospedaje')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.hos = data.data;
                    alert(data.mensaje);
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.hos);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.hos);
        };
        return deferred.promise;
    };


    //-----------------------------------------------------------------------
    // LISTAR VALORES ESTADISTICOS
    //-----------------------------------------------------------------------

    this.valoresEstadistica = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/Estadistica/listaValores')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.val = data.data;
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.val);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.val);
        };
        return deferred.promise;
    };


    //-----------------------------------------------------------------------
    // LISTAR TOTALES ESTADISTICOS
    //-----------------------------------------------------------------------

    this.valoresTotales = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/Estadistica/listaTotales')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.tot = data.data;
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.tot);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.tot);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // LISTA MOVIMIENTOS CARGOS DE PARTICIPANTE
    //-----------------------------------------------------------------------

    this.listaMovimientosCargos = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/Movimientos/listaMovimientosCargos')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.mov = data.data;
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.mov);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.mov);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // LISTA MOVIMIENTOS ANULADOS DE PARTICIPANTE
    //-----------------------------------------------------------------------

    this.listaMovimientosAnulados = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/Movimientos/listaMovimientosAnulados')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.mov = data.data;
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.mov);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.mov);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // LISTA SALDOS DE MOVIMIENTOS DIARIOS DE UN EVENTO
    //-----------------------------------------------------------------------

    this.saldosDiariosEvento = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/Movimientos/saldosDiariosEvento')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.sal = data.data;
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.sal);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.sal);
        };
        return deferred.promise;
    };


    //-----------------------------------------------------------------------
    // LISTA SALDO TOTAL DE MOVIMIENTOS DE UN EVENTO
    //-----------------------------------------------------------------------

    this.saldosTotalEvento = function (paramInfo) {
        var deferred = $q.defer();

        try {
            SendBox.post(paramInfo, 'api/Movimientos/saldoTotalEvento')
            .then(function (data) {
                if (data.codigo == 0) {
                    EVEServices.sal = data.data;
                } else {
                    alert(data.mensaje);
                }
                deferred.resolve(EVEServices.sal);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(EVEServices.sal);
        };
        return deferred.promise;
    };

});