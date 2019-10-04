(function () {

    var app = angular.module('customersApp', ['ngRoute']);

    app.config(function ($routeProvider) {
        $routeProvider
            .when('/', {
                controller: 'TripsController',
                templateUrl: 'trips.html'
            })
            .when('/days/:tripId', {
                controller: 'DaysController',
                templateUrl: 'days.html'
            })
            .when('/add/', {
                controller: 'TripsController',
                templateUrl: 'addTrip.html'
            })
            .otherwise({ redirectTo: '/' });
    });

}());