import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisteredUsersComponent } from './registered-users/registered-users.component';
import { BookingHistoryComponent } from '../booking-history/booking-history/booking-history.component';
import { UserprofileComponent } from '../shared/userprofile/userprofile.component';

const routes: Routes = [
  {
    path: '',
    component: RegisteredUsersComponent,
    children: [
      {
        path: 'user-profile',
        component: UserprofileComponent,
      },
      {
        path: 'user-profile/:id',
        component: UserprofileComponent,
      },
    ],
  },
  {
    path: 'booking-history',
    component: BookingHistoryComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class RegisteredUserRoutingModule {}
