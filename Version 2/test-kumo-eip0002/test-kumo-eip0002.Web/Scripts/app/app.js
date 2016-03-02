'use strict';

var KumoApp = angular.module('KumoApp', ['kendo.directives', 'ngRoute', 'ngAnimate'])
    .config(function ($routeProvider)
    {
        $routeProvider
            .when('/home',
            {
                templateUrl: '/home/home',
                controller: 'homeController'
            })
            .when('/customer',
            {
                templateUrl: '/customer',
                controller: 'customerController'
            })
            .when('/customer/edit/:id',
            {
                templateUrl: '/customer/edit',
                controller: 'customerEditController'
            })
            .when('/help',
            {
                templateUrl: '/help'
            })
            .otherwise(
            {
                redirectTo: '/home'
            });
    });