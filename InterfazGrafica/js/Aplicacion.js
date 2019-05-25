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

/**
 * Función utilizada para pasar a parametros objeto
 * @param {Objeto a serializar} objItem 
 */
function parametrosGet(strParam,objItem) {
    var strParametros = "?"
    for (const prop in objItem) {
        strParametros += strParam + prop + "=" + objItem[prop] + "&";
    }
    return strParametros.substring(0,strParametros.length -1);
}





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
    $scope.genero = "";
    $scope.email = "";
    $scope.fecha_nacimiento = null;
    $scope.Tipo_Usuario = 0;


    /**
     * Metodo utilizado para validar datos
     */
    $scope.funValidarUsuario = function () {
        var objDato = {
            Nombre_Usuario: $scope.Nombre_Usuario,
            Contrasena: $scope.Contrasena
        }
        $http.post("Api/Seguridad/authenticate" + parametrosGet("",objDato)).then(
            function (request) {
                var data = request.data;
                if (data.ResultadoProceso) {
                    $scope.Responsables = data.objetoData;
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
            identificacion : $scope.identificacion,
            tipo_identificacion : $scope.tipo_identificacion,
            Nombre_Usuario : $scope.Nombre_Usuario,
            Contrasena : $scope.Contrasena,
            Contrasena_Transaccion : $scope.Contrasena_Transaccion,
            Nombre : $scope.Nombre,
            Apellido : $scope.Apellido,
            genero : $scope.genero,
            email : $scope.email,
            fecha_nacimiento : $scope.fecha_nacimiento
        }
        $http.get("Api/Seguridad/registrar" + parametrosGet("", objDato)).then(
            function (request) {
                var data = request.data;
                if (data.ResultadoProceso) {
                    $scope.Responsables = data.objetoData;
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