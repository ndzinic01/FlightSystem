import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminLayout } from './admin-layout/admin-layout';
import { Dashboard } from './dashboard/dashboard';
import { Aircraft } from './aircraft/aircraft';
import { Airports} from './airports/airports';
import { Flights } from './flights/flights';
import { Reservations } from './reservations/reservations';
import { Users } from './users/users';
import { Notifications } from './notifications/notifications';
import { Reports } from './reports/reports';
import { Profile } from './profile/profile';
import { Search } from './search/search';

const routes: Routes = [
  {
    path: '',
    component: AdminLayout,
    children: [
      { path: 'dashboard', component: Dashboard },
      { path: 'aircraft', component: Aircraft },
      { path: 'airports', component: Airports },
      { path: 'flights', component: Flights },
      { path: 'reservations', component: Reservations },
      { path: 'users', component: Users },
      { path: 'notifications', component: Notifications },
      { path: 'reports', component: Reports },
      { path: 'profile', component: Profile },
      { path: 'search', component: Search },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {}
