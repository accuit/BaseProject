(function () {

    var TripsController = function ($scope, tripsService) {
        $scope.sortBy = 'name';
        $scope.reverse = false;
        $scope.trips = [];

        function init() {
            tripsService.getTrips().then(function (trips) {
                $scope.trips = trips;
            })
        }

        init();

        $scope.addTrip = function () {
            var date = $scope.addTrip.date;
            var fahrzeit = $scope.addTrip.fahrzeit;
            var kilometer = $scope.addTrip.kilometer;
            tripsService.addTrip(date, fahrzeit, kilometer);
            $scope.addTrip.date = '';
            $scope.addTrip.fahrzeit = '';
            $scope.addTrip.kilometer = '';
        };

        $scope.deleteTrip = function (id) {
            tripsService.deleteTrip(id);
        };

    };

    TripsController.$inject = ['$scope', 'tripsService'];

    angular.module('customersApp')
      .controller('TripsController', TripsController);

}());