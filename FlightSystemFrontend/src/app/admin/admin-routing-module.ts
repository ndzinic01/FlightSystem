import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Dashboard } from './dashboard/dashboard';
import {AuthGuard} from '../auth/auth.guard';
import {AdminLayout} from './admin-layout/admin-layout';
import {Aircraft} from './aircraft/aircraft';

const routes: Routes = [
  {
    path: '',
    component: AdminLayout,
    canActivate: [AuthGuard],
    children: [
      { path: 'dashboard', component: Dashboard },
      { path: 'aircraft', component: Aircraft },

      { path: '**', redirectTo: 'dashboard' }
    ]
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {}
