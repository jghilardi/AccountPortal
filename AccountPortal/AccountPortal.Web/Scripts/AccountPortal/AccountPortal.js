var app = angular.module("accountPortal", []);

app.controller("accountPortalController", ["$scope", "$cacheFactory" ,"$http", function ($scope, $cacheFactory, $http) {

        var cache = $cacheFactory('username');
        //var activeUser = new object;
        //expect($cacheFactory.get('username')).toBe(cache);
        //expect($cacheFactory.get('usernameNotFound')).not.toBeDefined();

        var validateNameRegex = function (name) {
            var regex = /^[a-zA-Z ,.'-]+$/;
            if (name.match(regex) !== null) {
                return true;
            }
            return false;
        };

        var validateUsername = function () {
            if ($scope.username.length < 46 && validateNameRegex($scope.username)) {
                return true;
            } else {
                return false;
            }
        };
        var validatePassword = function () {
            if ($scope.password.length > 4) {
                return validatePassword();
            }
            return false;
        };

        var passValidation = function () {
            var response = false;
            if (validateUsername()){
                response = true;
            } else {
                    toastr.error("Name must be within length parameters and contain valid characters only.");                          
                }
            return response;
        }

        var PopulateAccount = function () {
            activeUser.username = response.username;
            activeUser.password = response.password;
            activeUser.transactions = response.transactions;
            activeUser.accountBalance = response.accountBalance;
        }

        $scope.gridOptions = {
            enableColumnResizing: true,
            enableFiltering: true,

            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            },

            columns: [
                { name: "Submitted Date", field: "SubmittedDate", width: '*' },
                { name: "Amount", field: "Amount", width: '*' },
                { name: "Transaction Type", field: "TransactionType", width: '*', enableFiltering: false, cellFilter: 'date' },
            ]
        }

        $scope.addAccount = function () {
            if (passValidation()) {
                var account = {
                    username: $scope.username,
                    password: $scope.password
                }
                var url = "/AccountPortal/AddAccount/";
                var response = $http.post(url, account)
                cache.put(account.username, account);
                activeUser.username = response.username;
                activeUser.password = response.password;
                activeUser.accountBalance = 0;
                return activeUser;
            }
        };

        $scope.getAccount = function () {
            if (passValidation()) {
                var account = {
                    username: $scope.username,
                    password: $scope.password
                }
                var url = "/AccountPortal/GetAccount/";
                var response = $http.post(url, account)
                cache.get(account.username, account);
                activeUser.username = response.username;
                activeUser.password = response.password;
                activeUser.accountBalance = response.accountBalance;
                activeUser.transactions = response.transactions;
            }
        };
    }
]);
