import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SeatConfigurationComponent } from './seat-configuration/seat-configuration.component';
import { UserprofileComponent } from '../shared/userprofile/userprofile.component';

const routes: Routes = [
  {
    path: '',
    component: SeatConfigurationComponent,
    children: [
      {
        path: 'user-profile',
        component: UserprofileComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SeatConfigurationRoutingModule {}
