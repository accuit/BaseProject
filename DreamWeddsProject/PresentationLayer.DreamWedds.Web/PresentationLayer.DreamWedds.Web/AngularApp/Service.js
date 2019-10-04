(function () {
    var tripsService = function ($http, $q) {
        var tripsLoaded = false;
        var trips = null;

        this.getTrips = function () {
            if (!tripsLoaded) {
                return $http.get('http://localhost:58561/DreamWedds.svc/GetWeddingDetailByID/?id=1').then(function (response) {
                    trips = response.data.SingleResult;
                    console.log(trips);
                    tripsLoaded = true;
                    return trips;
                });
            }
            else {
                return $q.when(trips);
            }

        };

        this.addTrip = function (date, fahrzeit, kilometer) {
            var topID = trips.length + 1001;
            trips.push({
                id: topID,
                tripstart: date,
                fahrzeit: fahrzeit,
                kilometer: kilometer,
                DAYS: []
            });
        };

        this.deleteTrip = function (id) {
            for (var i = trips.length - 1; i >= 0; i--) {
                if (trips[i].id === id) {
                    trips.splice(i, 1);
                    break;
                }
            }
        }

        this.getTrip = function (tripId) {
            for (var i = 0, len = trips.length; i < len; i++) {
                if (trips[i].id === parseInt(tripId)) {
                    console.log(tripId);
                    return trips[i];
                    break;
                }
            }
            return {};
        }

        this.deleteDay = function (tripId, dayId) {
            for (var i = trips.length - 1; i >= 0; i--) {
                if (trips[i].id === tripId) {
                    for (var k = trips[i].DAYS.length - 1; k >= 0; k--) {
                        if (trips[i].DAYS[k].id === dayId) {
                            trips[i].DAYS.splice(k, 1);
                            break;
                        }
                    }
                }
            } // 1st for-loop

        } // Ende edelete Order

    };

    angular.module('customersApp').service('tripsService', tripsService);
}());