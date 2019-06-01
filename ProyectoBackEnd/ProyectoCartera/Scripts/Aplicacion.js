var myApp = angular.module('myAplicacion', [])
    .filter('jsonDate', ['$filter', function ($filter) {
        return function (input, format) {
            return (input)
                ? $filter('date')(parseInt(input.substr(6)), format)
                : '';
        }
    }]);
var strDireccion = "http://localhost:54070/api/";
let headers = new Headers();
headers.append('Content-Type', 'application/json');


var configToken = {
    tokenName: "token",
    authHeader: false
}

/**
 * Función utilizada para pasar a parametros objeto
 * @param {Objeto a serializar} objItem 
 */
function parametrosGet(strParam, objItem) {
    var strParametros = "?"
    for (const prop in objItem) {
        strParametros += strParam + prop + "=" + objItem[prop] + "&";
    }
    return strParametros.substring(0, strParametros.length - 1);
}

/**
 * RECIBE Y RETORNA FECHA VALIDA
 * @param {any} xFecha
 */
function validarFecha(xFecha) {
    try {
        return xFecha.getDate() + "/" + (xFecha.getMonth() + 1) + "/" + xFecha.getFullYear();
    } catch {
        return "01/01/0001";
    }
}


myApp.factory('sessionInjector', ['$log', function (SessionService) {
    var sessionInjector = {
        request: function (config) {
            if (
                localStorage.getItem(configToken.tokenName) != null &&
                (
                    config.url.indexOf("Seguridad") == -1 ||
                    configToken.authHeader
                )
            ) {
                var xToken = localStorage.getItem(configToken.tokenName);
                xToken = "Bearer " + (xToken == null ? "" : xToken);
                config.headers['Authorization'] = xToken;
            }
            return config;
        }
    };
    return sessionInjector;
}]);


myApp.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.interceptors.push('sessionInjector');
}]);


/**
 * CONTROLADOR UTILIZADO PARA ADMINISTRAR DE USUARIOS
 */
myApp.controller('ctrSeguridad', ['$scope', '$http', function ($scope, $http) {

    //MODELO
    $scope.identificacion = 0;
    $scope.tipo_identificacion = "CC";
    $scope.Nombre_Usuario = "";
    $scope.Contrasena = "";
    $scope.Contrasena_Transaccion = "";
    $scope.Nombre = "";
    $scope.Apellido = "";
    $scope.genero = "M";
    $scope.email = "";
    $scope.fecha_nacimiento = null;
    $scope.Tipo_Usuario = 0;
    $scope.AceptaTerminos = false;

    /**
     * Se ejecuta al iniciar el controlador de angular
     * */
    $scope.fun_validaSesion = function () {
        if (localStorage.getItem(configToken.tokenName) != null) {
            configToken.authHeader = true;
            $http.post("/Api/Seguridad/echouser").then(
                function (request) {
                    var data = request.data;
                    if (data.ResultadoProceso) {
                        location.href = "../PanelDeControl";
                    }
                    configToken.authHeader = false;
                }
            ).error(function (data, status, headers, config) {

                console.debug("saved comment", $scope.comment);

            })

        }
    }

    /**
     * Metodo utilizado para validar datos
     */
    $scope.funValidarUsuario = function () {
        var objDato = {
            Nombre_Usuario: $scope.Nombre_Usuario,
            Contrasena: $scope.Contrasena
        }
        $http.post("/Api/Seguridad/authenticate" + parametrosGet("", objDato)).then(
            function (request) {
                var data = request.data;
                if (data.ResultadoProceso) {
                    localStorage.setItem(configToken.tokenName, data.objetoData);
                    location.href = "../PanelDeControl";
                } else {
                    mostrarMensaje(1, data.CadenaError);
                }
            },
            function (data) {
                mostrarMensaje(2, data.message);
            }
        )
    }


    /**
    * Metodo utilizado para validar datos
    */
    $scope.funGuardarUsuario = function () {
        var objDato = {
            identificacion: $scope.identificacion,
            tipo_identificacion: $scope.tipo_identificacion,
            Nombre_Usuario: $scope.Nombre_Usuario,
            Contrasena: $scope.Contrasena,
            Contrasena_Transaccion: $scope.Contrasena_Transaccion,
            Nombre: $scope.Nombre,
            Apellido: $scope.Apellido,
            genero: $scope.genero,
            email: $scope.email,
            fecha_nacimiento: validarFecha($scope.fecha_nacimiento),
            AceptaTerminos: $scope.AceptaTerminos
        }
        $http.get("/Api/Seguridad/registrar" + parametrosGet("", objDato)).then(
            function (request) {
                var data = request.data;
                if (data.ResultadoProceso) {
                    mostrarMensaje(3, data.CadenaError);
                } else {
                    mostrarMensaje(1, data.CadenaError);
                }
            },
            function (data, param, param2) {
                mostrarMensaje(2, data.message);
            }
        )
    }


}]);


/**
 * CONTROLADOR UTILIZADO PARA ADMINISTRAR DE USUARIOS
 */
myApp.controller('ctrPanelDeControl', ['$scope', '$http', function ($scope, $http) {


    //MODELO
    $scope.Usuario = {};
    $scope.Nombre = "";
    $scope.Usuario.Cargado = false;

    /**
     * Se ejecuta al iniciar el controlador de angular
     * */
    $scope.fun_validaSesion = function () {
        configToken.authHeader = true;
        $http.post("/api/Seguridad/echouser").then(
            function (request) {
                var data = request.data;
                if (data.ResultadoProceso) {
                    $scope.Usuario = data.objetoData;
                    $scope.Usuario.Cargado = true;
                    try {
                        //$scope.$apply();
                    } catch (a) { }
                } else {
                    if (request.config.url.indexOf("PanelDeControl") != -1) {
                        location.href = "../Segurida/IniciarSesion";
                    }
                }
                configToken.authHeader = false;
            }, function (err) {          //second function "error"
                if (err.status == 401) {
                    localStorage.clear();
                    location.href = "../Segurida/IniciarSesion";
                }
            }
        )
    }

    /**
     * Metodo utilizado para cerrar sesión
     * */
    $scope.cerrarSession = function () {
        localStorage.clear();
        location.href = "../Segurida/IniciarSesion";
    }

}]);





/**
 * CONTROLADOR UTILIZADO PARA CREAR INCIDENCIAS
 */
myApp.controller('ctrIncidencias', ['$scope', '$http', function ($scope, $http) {


    //MODELO
    $scope.Asunto = "";
    $scope.Mensaje = "";

    /**
     * Guarda datos de la incidencia
     * */
    $scope.fun_Guardar = function () {
        var objRequest = {
            xAsunto: $scope.Asunto,
            xMensaje: $scope.Mensaje
        }
        $http.post("/api/APIPanelDeControl/guardarIncidencia" + parametrosGet("", objRequest)).then(
            function (request) {
                var data = request.data;
                if (data.ResultadoProceso) {
                    mostrarMensajeAccion(data.TipoMensaje, data.CadenaError, function () {
                        location.href = "../PanelDeControl";
                    }, null);
                } else {
                    mostrarMensaje(data.TipoMensaje, data.CadenaError);
                }
            }
        )
    }


}]);



/**
 * CONTROLADOR UTILIZADO PARA CARGAR DINERO
 */
myApp.controller('ctrCargarDinero', ['$scope', '$http', function ($scope, $http) {


    //MODELO
    $scope.moneda_movimiento = "USD";
    $scope.tipo_movimiento = "PDTM";
    $scope.monto = "0";
    $scope.Dato1 = '';
    $scope.Dato2 = '';
    $scope.Dato3 = '';
    $scope.Dato4 = '';

    $scope.configCtr = {};

    $scope.configCtr['PDTM'] = {};
    $scope.configCtr['TRBC'] = {};
    $scope.configCtr['CCFR'] = {};

    $scope.configCtr['PDTM']['Dato1'] = true;
    $scope.configCtr['PDTM']['Dato2'] = true;
    $scope.configCtr['PDTM']['Dato3'] = true;
    $scope.configCtr['PDTM']['Dato4'] = false;

    $scope.configCtr['PDTM']['strDato1'] = 'Número cuenta';
    $scope.configCtr['PDTM']['strDato2'] = 'Número documento';
    $scope.configCtr['PDTM']['strDato3'] = 'Nombre propietario de la cuenta';

    $scope.configCtr['TRBC']['Dato1'] = true;
    $scope.configCtr['TRBC']['Dato2'] = true;
    $scope.configCtr['TRBC']['Dato3'] = false;
    $scope.configCtr['TRBC']['Dato4'] = false;

    $scope.configCtr['TRBC']['strDato1'] = 'Número cuenta';
    $scope.configCtr['TRBC']['strDato2'] = 'Número documento';

    $scope.configCtr['CCFR']['Dato1'] = true;
    $scope.configCtr['CCFR']['Dato2'] = false;
    $scope.configCtr['CCFR']['Dato3'] = false;
    $scope.configCtr['CCFR']['Dato4'] = false;

    $scope.configCtr['CCFR']['strDato1'] = 'Número cupon';

    /**
     * Metodo utilizado para cargar tipo de movimiento PDTM
     * @param {any} ctrControl
     */
    $scope.fun_SeleccionarMetodoRecarga_PDTM = function () {
        $scope.tipo_movimiento = "PDTM";
    }

    /**
     * Metodo utilizado para cargar tipo de movimiento TRBC
     * @param {any} ctrControl
     */
    $scope.fun_SeleccionarMetodoRecarga_TRBC = function () {
        $scope.tipo_movimiento = "TRBC";
    }

    /**
     * Metodo utilizado para cargar tipo de movimiento CCFR
     * @param {any} ctrControl
     */
    $scope.fun_SeleccionarMetodoRecarga_CCFR = function () {
        $scope.tipo_movimiento = "CCFR";
    }

    /**
     * Carmbia el tipo de moneda a USD
     * */
    $scope.fun_TipoMoneda_USD = function () {
        $scope.moneda_movimiento = "USD";
    }

    /**
    * Carmbia el tipo de moneda a BTC
    * */
    $scope.fun_TipoMoneda_BTC = function () {
        $scope.moneda_movimiento = "BTC";
    }

    /**
     * Valida datos para iniciar transacción
     * */
    function validarDatos() {
        var lstCadena = [];
        if ($scope.moneda_movimiento != 'USD' && $scope.moneda_movimiento != 'BTC') {
            lstCadena.push("Debe seleccionar un tipo de moneda valida.");
        }
        if ($scope.tipo_movimiento != 'PDTM' && $scope.tipo_movimiento != 'TRBC' && $scope.tipo_movimiento != 'CCFR') {
            lstCadena.push("Debe seleccionar un tipo de movimiento valido.");
        }
        if (isNaN(parseFloat($scope.monto))) {
            lstCadena.push("Debe ingresar un monto valido.");
        } else {
            if (parseFloat($scope.monto) == 0) {
                lstCadena.push("Debe ingresar un monto valido.");
            }
        }
        if ($scope.configCtr[$scope.tipo_movimiento]['Dato1']) {
            if ($scope.Dato1 == '') {
                lstCadena.push("Debe ingresar el campo '" + $scope.configCtr[$scope.tipo_movimiento]['strDato1'] + "'.");
            }
        }
        if ($scope.configCtr[$scope.tipo_movimiento]['Dato2']) {
            if ($scope.Dato2 == '') {
                lstCadena.push("Debe ingresar el campo '" + $scope.configCtr[$scope.tipo_movimiento]['strDato2'] + "'.");
            }
        }
        if ($scope.configCtr[$scope.tipo_movimiento]['Dato3']) {
            if ($scope.Dato3 == '') {
                lstCadena.push("Debe ingresar el campo '" + $scope.configCtr[$scope.tipo_movimiento]['strDato3'] + "'.");
            }
        }
        if ($scope.configCtr[$scope.tipo_movimiento]['Dato4']) {
            if ($scope.Dato4 == '') {
                lstCadena.push("Debe ingresar el campo '" + $scope.configCtr[$scope.tipo_movimiento]['strDato4'] + "'.");
            }
        }
        return lstCadena;
    }

    /**
     * Guarda datos de la incidencia
     * */
    $scope.fun_Guardar = function () {

        var lstMensajes = validarDatos();
        if (lstMensajes.length == 0) {

            var objRequest = {
                xmoneda_movimiento: $scope.moneda_movimiento,
                xtipo_movimiento: $scope.tipo_movimiento,
                xmonto: $scope.monto,
                xDato1: $scope.Dato1,
                xDato2: $scope.Dato2,
                xDato3: $scope.Dato3,
                xDato4: $scope.Dato4
            }
            $http.post("/api/APIPanelDeControl/cargarDinero" + parametrosGet("", objRequest)).then(
                function (request) {
                    var data = request.data;
                    if (data.ResultadoProceso) {
                        mostrarMensajeAccion(data.TipoMensaje, data.CadenaError, function () {
                            location.href = "../PanelDeControl";
                        }, null);
                    } else {
                        mostrarMensaje(data.TipoMensaje, data.CadenaError);
                    }
                }
            )

        } else {
            mostrarMensaje(1, lstMensajes[0]);
        }
    }


}]);













/**
 * CONTROLADOR UTILIZADO PARA CONSULTAR CUENTAS DE LOS SALDOS
 */
myApp.controller('ctrConsultarSaldo', ['$scope', '$http', function ($scope, $http) {


    //MODELO
    $scope.moneda_movimiento = "USD";
    $scope.SaldoCuenta = 0;
    $scope.tipo_saldo = '';
    $scope.SaldoConvertido = 0;
    $scope.Movimientos = [];

    /**
     * Carmbia el tipo de moneda a USD
     * */
    $scope.fun_TipoMoneda_USD = function () {
        $scope.moneda_movimiento = "USD";
        $scope.fun_Consultar();
    }

    /**
    * Carmbia el tipo de moneda a BTC
    * */
    $scope.fun_TipoMoneda_BTC = function () {
        $scope.moneda_movimiento = "BTC";
        $scope.fun_Consultar();
    }

    /**
     * Guarda datos de la incidencia
     * */
    $scope.fun_Consultar = function () {


        var objRequest = {
            xmoneda_movimiento: $scope.moneda_movimiento,
        }
        $http.post("/api/APIPanelDeControl/ConsultarCuentas" + parametrosGet("", objRequest)).then(
            function (request) {
                var data = request.data;
                if (data.ResultadoProceso) {
                    $scope.SaldoCuenta = data.objetoData.balance;
                    $scope.tipo_saldo = data.objetoData.tipo_saldo;
                    $scope.SaldoConvertido = data.objetoData.SaldoConvertido;
                    $scope.NumeroCuenta = data.objetoData.NumeroCuenta;
                    $scope.Movimientos = data.objetoData.MovimientosCuenta;
                } else {
                    mostrarMensaje(data.TipoMensaje, data.CadenaError);
                }
            }
        )

    }


}]);



/**
 * CONTROLADOR UTILIZADO PARA CONSULTAR CUENTAS DE LOS SALDOS
 */
myApp.controller('ctrConsultarMovimientos', ['$scope', '$http', function ($scope, $http) {


    //MODELO
    $scope.moneda_movimiento = "USD";
    $scope.fecha_Inicial = new Date();
    $scope.fecha_Final = new Date();
    $scope.Movimientos = [];

    /**
     * Carmbia el tipo de moneda a USD
     * */
    $scope.fun_TipoMoneda_USD = function () {
        $scope.moneda_movimiento = "USD";
        $scope.fun_Consultar();
    }

    /**
    * Carmbia el tipo de moneda a BTC
    * */
    $scope.fun_TipoMoneda_BTC = function () {
        $scope.moneda_movimiento = "BTC";
        $scope.fun_Consultar();
    }

    /**
     * Crea una cadena con formato dia - Mes - Año
     * @param {any} xFecha
     */
    function fechaACadena(xFecha) {
        if (xFecha != null) {
            return xFecha.getDate() + '-' + (xFecha.getMonth() + 1) + '-' + xFecha.getFullYear()
        } else {
            return "";
        }
    }

    /**
     * Guarda datos de la incidencia
     * */
    $scope.fun_Consultar = function () {


        var objRequest = {
            xmoneda_movimiento: $scope.moneda_movimiento,
            xFechaInicial: fechaACadena($scope.fecha_Inicial),
            xFechaFinal: fechaACadena($scope.fecha_Final)
        }
        $http.post("/api/APIPanelDeControl/ConsultarMovimientos" + parametrosGet("", objRequest)).then(
            function (request) {
                var data = request.data;
                if (data.ResultadoProceso) {
                    $scope.Movimientos = data.objetoData;
                } else {
                    mostrarMensaje(data.TipoMensaje, data.CadenaError);
                }
            }
        )

    }


}]);





/**
 * CONTROLADOR UTILIZADO PARA CARGAR DINERO
 */
myApp.controller('ctrRetirarDinero', ['$scope', '$http', function ($scope, $http) {


    //MODELO
    $scope.moneda_movimiento = "USD";
    $scope.tipo_movimiento = "PDTM";
    $scope.monto = "0";
    $scope.Dato1 = '';
    $scope.Dato2 = '';
    $scope.Dato3 = '';
    $scope.Dato4 = '';

    $scope.configCtr = {};

    $scope.configCtr['PDTM'] = {};
    $scope.configCtr['TRBC'] = {};
    $scope.configCtr['CCFR'] = {};

    $scope.configCtr['PDTM']['Dato1'] = true;
    $scope.configCtr['PDTM']['Dato2'] = true;
    $scope.configCtr['PDTM']['Dato3'] = true;
    $scope.configCtr['PDTM']['Dato4'] = false;

    $scope.configCtr['PDTM']['strDato1'] = 'Número cuenta';
    $scope.configCtr['PDTM']['strDato2'] = 'Número documento';
    $scope.configCtr['PDTM']['strDato3'] = 'Nombre propietario de la cuenta';

    $scope.configCtr['TRBC']['Dato1'] = true;
    $scope.configCtr['TRBC']['Dato2'] = true;
    $scope.configCtr['TRBC']['Dato3'] = false;
    $scope.configCtr['TRBC']['Dato4'] = false;

    $scope.configCtr['TRBC']['strDato1'] = 'Número cuenta';
    $scope.configCtr['TRBC']['strDato2'] = 'Número documento';

    $scope.configCtr['CCFR']['Dato1'] = true;
    $scope.configCtr['CCFR']['Dato2'] = false;
    $scope.configCtr['CCFR']['Dato3'] = false;
    $scope.configCtr['CCFR']['Dato4'] = false;

    $scope.configCtr['CCFR']['strDato1'] = 'Número cupon';

    /**
     * Metodo utilizado para cargar tipo de movimiento PDTM
     * @param {any} ctrControl
     */
    $scope.fun_SeleccionarMetodoRecarga_PDTM = function () {
        $scope.tipo_movimiento = "PDTM";
    }

    /**
     * Metodo utilizado para cargar tipo de movimiento TRBC
     * @param {any} ctrControl
     */
    $scope.fun_SeleccionarMetodoRecarga_TRBC = function () {
        $scope.tipo_movimiento = "TRBC";
    }

    /**
     * Metodo utilizado para cargar tipo de movimiento CCFR
     * @param {any} ctrControl
     */
    $scope.fun_SeleccionarMetodoRecarga_CCFR = function () {
        $scope.tipo_movimiento = "CCFR";
    }

    /**
     * Carmbia el tipo de moneda a USD
     * */
    $scope.fun_TipoMoneda_USD = function () {
        $scope.moneda_movimiento = "USD";
    }

    /**
    * Carmbia el tipo de moneda a BTC
    * */
    $scope.fun_TipoMoneda_BTC = function () {
        $scope.moneda_movimiento = "BTC";
    }

    /**
     * Valida datos para iniciar transacción
     * */
    function validarDatos() {
        var lstCadena = [];
        if ($scope.moneda_movimiento != 'USD' && $scope.moneda_movimiento != 'BTC') {
            lstCadena.push("Debe seleccionar un tipo de moneda valida.");
        }
        if ($scope.tipo_movimiento != 'PDTM' && $scope.tipo_movimiento != 'TRBC' && $scope.tipo_movimiento != 'CCFR') {
            lstCadena.push("Debe seleccionar un tipo de movimiento valido.");
        }
        if (isNaN(parseFloat($scope.monto))) {
            lstCadena.push("Debe ingresar un monto valido.");
        } else {
            if (parseFloat($scope.monto) <= 0) {
                lstCadena.push("Debe ingresar un monto valido.");
            }
        }
        if ($scope.configCtr[$scope.tipo_movimiento]['Dato1']) {
            if ($scope.Dato1 == '') {
                lstCadena.push("Debe ingresar el campo '" + $scope.configCtr[$scope.tipo_movimiento]['strDato1'] + "'.");
            }
        }
        if ($scope.configCtr[$scope.tipo_movimiento]['Dato2']) {
            if ($scope.Dato2 == '') {
                lstCadena.push("Debe ingresar el campo '" + $scope.configCtr[$scope.tipo_movimiento]['strDato2'] + "'.");
            }
        }
        if ($scope.configCtr[$scope.tipo_movimiento]['Dato3']) {
            if ($scope.Dato3 == '') {
                lstCadena.push("Debe ingresar el campo '" + $scope.configCtr[$scope.tipo_movimiento]['strDato3'] + "'.");
            }
        }
        if ($scope.configCtr[$scope.tipo_movimiento]['Dato4']) {
            if ($scope.Dato4 == '') {
                lstCadena.push("Debe ingresar el campo '" + $scope.configCtr[$scope.tipo_movimiento]['strDato4'] + "'.");
            }
        }
        return lstCadena;
    }

    /**
     * Guarda datos de la incidencia
     * */
    $scope.fun_Guardar = function () {

        var lstMensajes = validarDatos();
        if (lstMensajes.length == 0) {

            var objRequest = {
                xmoneda_movimiento: $scope.moneda_movimiento,
                xtipo_movimiento: $scope.tipo_movimiento,
                xmonto: $scope.monto * -1,
                xDato1: $scope.Dato1,
                xDato2: $scope.Dato2,
                xDato3: $scope.Dato3,
                xDato4: $scope.Dato4
            }
            $http.post("/api/APIPanelDeControl/cargarDinero" + parametrosGet("", objRequest)).then(
                function (request) {
                    var data = request.data;
                    if (data.ResultadoProceso) {
                        mostrarMensajeAccion(data.TipoMensaje, data.CadenaError, function () {
                            location.href = "../PanelDeControl";
                        }, null);
                    } else {
                        mostrarMensaje(data.TipoMensaje, data.CadenaError);
                    }
                }
            )

        } else {
            mostrarMensaje(1, lstMensajes[0]);
        }
    }


}]);





/**
 * CONTROLADOR UTILIZADO PARA CARGAR DINERO
 */
myApp.controller('ctrEnviarDinero', ['$scope', '$http', function ($scope, $http) {


    //MODELO
    $scope.moneda_movimiento = "USD";
    $scope.tipo_movimiento = "RETI";
    $scope.monto = "0";
    $scope.Dato1 = '';
    $scope.Dato2 = '';
    $scope.Dato3 = '';
    $scope.Dato4 = '';

    $scope.configCtr = {};

    $scope.configCtr['PDTM'] = {};
    $scope.configCtr['TRBC'] = {};
    $scope.configCtr['CCFR'] = {};
    $scope.configCtr['RETI'] = {};

    $scope.configCtr['PDTM']['Dato1'] = true;
    $scope.configCtr['PDTM']['Dato2'] = true;
    $scope.configCtr['PDTM']['Dato3'] = true;
    $scope.configCtr['PDTM']['Dato4'] = false;

    $scope.configCtr['PDTM']['strDato1'] = 'Número cuenta';
    $scope.configCtr['PDTM']['strDato2'] = 'Número documento';
    $scope.configCtr['PDTM']['strDato3'] = 'Nombre propietario de la cuenta';

    $scope.configCtr['TRBC']['Dato1'] = true;
    $scope.configCtr['TRBC']['Dato2'] = true;
    $scope.configCtr['TRBC']['Dato3'] = false;
    $scope.configCtr['TRBC']['Dato4'] = false;

    $scope.configCtr['TRBC']['strDato1'] = 'Número cuenta';
    $scope.configCtr['TRBC']['strDato2'] = 'Número documento';

    $scope.configCtr['CCFR']['Dato1'] = true;
    $scope.configCtr['CCFR']['Dato2'] = false;
    $scope.configCtr['CCFR']['Dato3'] = false;
    $scope.configCtr['CCFR']['Dato4'] = false;

    $scope.configCtr['CCFR']['strDato1'] = 'Número cupon';


    $scope.configCtr['RETI']['Dato1'] = true;
    $scope.configCtr['RETI']['Dato2'] = false;
    $scope.configCtr['RETI']['Dato3'] = false;
    $scope.configCtr['RETI']['Dato4'] = false;

    $scope.configCtr['RETI']['strDato1'] = 'Diligenciar destinatario';

    /**
     * Metodo utilizado para cargar tipo de movimiento PDTM
     * @param {any} ctrControl
     */
    $scope.fun_SeleccionarMetodoRecarga_PDTM = function () {
        $scope.tipo_movimiento = "PDTM";
    }

    /**
     * Metodo utilizado para cargar tipo de movimiento TRBC
     * @param {any} ctrControl
     */
    $scope.fun_SeleccionarMetodoRecarga_TRBC = function () {
        $scope.tipo_movimiento = "TRBC";
    }

    /**
     * Metodo utilizado para cargar tipo de movimiento CCFR
     * @param {any} ctrControl
     */
    $scope.fun_SeleccionarMetodoRecarga_CCFR = function () {
        $scope.tipo_movimiento = "CCFR";
    }

    /**
     * Carmbia el tipo de moneda a USD
     * */
    $scope.fun_TipoMoneda_USD = function () {
        $scope.moneda_movimiento = "USD";
    }

    /**
    * Carmbia el tipo de moneda a BTC
    * */
    $scope.fun_TipoMoneda_BTC = function () {
        $scope.moneda_movimiento = "BTC";
    }

    /**
     * Valida datos para iniciar transacción
     * */
    function validarDatos() {
        var lstCadena = [];
        if ($scope.moneda_movimiento != 'USD' && $scope.moneda_movimiento != 'BTC') {
            lstCadena.push("Debe seleccionar un tipo de moneda valida.");
        }
        if (isNaN(parseFloat($scope.monto))) {
            lstCadena.push("Debe ingresar un monto valido.");
        } else {
            if (parseFloat($scope.monto) <= 0) {
                lstCadena.push("Debe ingresar un monto valido.");
            }
        }
        return lstCadena;
    }

    /**
     * Guarda datos de la incidencia
     * */
    $scope.fun_Guardar = function () {

        var lstMensajes = validarDatos();
        if (lstMensajes.length == 0) {

            var objRequest = {
                xmoneda_movimiento: $scope.moneda_movimiento,
                xtipo_movimiento: $scope.tipo_movimiento,
                xmonto: $scope.monto * -1,
                xDato1: $scope.Dato1,
                xDato2: $scope.Dato2,
                xDato3: $scope.Dato3,
                xDato4: $scope.Dato4
            }
            $http.post("/api/APIPanelDeControl/cargarDinero" + parametrosGet("", objRequest)).then(
                function (request) {
                    var data = request.data;
                    if (data.ResultadoProceso) {
                        mostrarMensajeAccion(data.TipoMensaje, data.CadenaError, function () {
                            location.href = "../PanelDeControl";
                        }, null);
                    } else {
                        mostrarMensaje(data.TipoMensaje, data.CadenaError);
                    }
                }
            )

        } else {
            mostrarMensaje(1, lstMensajes[0]);
        }
    }


}]);