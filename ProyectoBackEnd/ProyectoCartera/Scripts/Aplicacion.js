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
            )

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
        $http.post("/Api/Seguridad/echouser").then(
            function (request) {
                var data = request.data;
                if (data.ResultadoProceso) {
                    $scope.Usuario = data.objetoData;
                    $scope.Usuario.Cargado = true;
                    $scope.$apply();
                } else {
                    if (request.config.url.indexOf("PanelDeControl") != -1) {
                        location.href = "../Segurida/IniciarSesion";
                    }
                }
                configToken.authHeader = false;
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