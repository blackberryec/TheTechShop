/// <reference path="F:\C# Data\TheTechShop\TheTechShop.Web\Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('thetechshop.application_roles', ['thetechshop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouteProvider'];

    function config($stateProvider, $urlRouteProvider) {

        $stateProvider.state('application_roles', {
            url: "/application_roles",
            templateUrl: "/app/components/application_roles/applicationRoleListView.html",
            parent: 'base',
            controller: "applicationRoleListController"
        })
         .state('add_application_role', {
             url: "/add_application_role",
             parent: 'base',
             templateUrl: "/app/components/application_roles/applicationRoleAddView.html",
             controller: "applicationRoleAddController"
         })
         .state('edit_application_role', {
             url: "/edit_application_role/:id",
             templateUrl: "/app/components/application_roles/applicationRoleEditView.html",
             controller: "applicationRoleEditController",
             parent: 'base',
         });
    }
})();