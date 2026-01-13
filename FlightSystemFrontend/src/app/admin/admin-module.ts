import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing-module';
import { Dashboard } from './dashboard/dashboard';
import { AdminLayout } from './admin-layout/admin-layout';
import { Aircraft } from './aircraft/aircraft';
import { Airports } from './airports/airports';
import { Flights } from './flights/flights';
import { Reservations } from './reservations/reservations';
import { Users } from './users/users';
import { Notifications } from './notifications/notifications';
import { Reports } from './reports/reports';
import { Search } from './search/search';
import { Profile } from './profile/profile';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {HttpClientModule} from '@angular/common/http';
import { Countries } from './countries/countries';
import { Destination } from './destination/destination';
import { AddDestinationDialog } from './destination/add-destination-dialog/add-destination-dialog';
import { AddFlightDialog } from './flights/add-flight-dialog/add-flight-dialog';



@NgModule({
  declarations: [
    Dashboard,
    AdminLayout,
    Aircraft,
    Airports,
    Flights,
    Reservations,
    Users,
    Notifications,
    Reports,
    Search,
    Profile,
    Countries,
    Destination,
    AddDestinationDialog,
    AddFlightDialog,
  ],
    imports: [
        CommonModule,
        AdminRoutingModule,
        HttpClientModule,
        FormsModule,
        ReactiveFormsModule
    ]
})
export class AdminModule { }
