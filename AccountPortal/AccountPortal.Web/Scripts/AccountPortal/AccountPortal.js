var app = angular.module("gpccAccountLookup", ["angular-spinkit", "ui.grid", "ui.grid.resizeColumns", "ui.grid.moveColumns", "ui.grid.saveState", "ngCookies"]);


app.controller("lookUpController", ["$scope", "$cookieStore", "$http", function ($scope, $cookieStore, $http) {


        var inputs = { badName: false }

        var validateNameRegex = function (name) {
            var regex = /^[a-zA-Z ,.'-]+$/;
            if (name.match(regex) != null) {
                return true;
            }
            return false;
        };

        //#region validation

        //name
        var validateFirstName = function () {
            inputCounter += 1;
            if ($scope.firstName.length < 46 && validateNameRegex($scope.firstName)) {
                return true;
            } else {
                inputs.badName = true;
                return false;
            }
        };
        var validateOrPassFirstName = function () {
            if ($scope.firstName) {
                return !!validateFirstName();
            }
            return true;
        };






        var passValidation = function () {
            var response = false;
            if (validateOrPassFirstName()){
                response = true;
            } else {

                    toastr.error("Name must be within length parameters and contain valid characters only.");                          
                }
            return response;
        }

        //var checkDobForNull = function () {
        //    if ($scope.dateOfBirth) {
        //        return $scope.dateOfBirth.toDateString();
        //    } else {
        //        return new Date().toDateString();
        //    }
        //}
        //#endregion

        //#region grid options
        $scope.gridOptions = {
            enableColumnResizing: true,
            enableFiltering: true,

            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            },

            columns: [
                { name: "Application Id", field: "ApplicationId", width: '*' },
                { name: "Prequalification Id", field: "PrequalificationId", width: '*' },
                { name: "Submitted On", field: "SubmissionDateTime", width: '*', enableFiltering: false, cellFilter: 'date' },
            ]
        }
        //#endregion

        $scope.getAccount = function () {

            if (passValidation()) {
                $scope.showSpinner = true;
                var lookup = {
                    //username: $scope.startDate.toDateString(),
                    //transactions: $scope.endDate.toDateString(),
                    //firstName: $scope.firstName,
                    //middleInitial: $scope.middleInitial
                }
                var url = "/GetAccount/";
                $http.post(url, lookup)
                    .success(function (data) {
                        $scope.gridOptions.data = data.account;
                        if (data.account == null) {
                            toastr.info("No accounts found.");
                        }
                        $scope.showSpinner = false;
                    })
                    .error(function (data) {
                        toastr.error("Failed to get account.", { timeout: 2000 });
                        $scope.showSpinner = false;
                    });
            }
        };

        //$scope.$watch("status", function () {
        //    $scope.firstName = "";
        //    $scope.lastName = "";
        //    $scope.middleInitial = "";
        //    $scope.applicationId = "";
        //});
    }
]);
