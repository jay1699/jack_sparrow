import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MasterComponent } from './core/components/master/master.component';
import { LandingPageComponent } from './core/components/landing-page/landing-page.component';
import { PageNotFoundComponent } from './core/components/page-not-found/page-not-found.component';
import { AuthGuard } from './core/services/guard/auth.guard';
import { AuthCallbackComponent } from './core/components/auth-callback/auth-callback.component';
import { UserprofileComponent } from './shared/userprofile/userprofile.component';

const routes: Routes = [
  {
    path: 'auth-callback',
    component: AuthCallbackComponent,
  },
  {
    path: '',
    component: MasterComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'landing-page',
      },
      {
        path: 'landing-page',
        component: LandingPageComponent,
        canActivate: [AuthGuard],
        children: [
          {
            path: 'user-profile',
            component: UserprofileComponent,
          },
        ],
      },
      {
        path: 'dashboard',
        loadChildren: () =>
          import('./dashboard/dashboard.module').then((m) => m.DashboardModule),
      },
      {
        path: 'book-seat',
        loadChildren: () =>
          import('./book-seat/book-seat.module').then((m) => m.BookSeatModule),
      },
      {
        path: 'seat-configuration',
        loadChildren: () =>
          import('./seat-configuration/seat-configuration.module').then(
            (m) => m.SeatConfigurationModule
          ),
      },
      {
        path: 'booking-history',
        loadChildren: () =>
          import('./booking-history/booking-history.module').then(
            (m) => m.BookingHistoryModule
          ),
      },
      {
        path: 'request-history',
        loadChildren: () =>
          import('./request-history/request-history.module').then(
            (m) => m.RequestHistoryModule
          ),
      },
      {
        path: 'registered-users',
        loadChildren: () =>
          import('./registered-user/registered-user.module').then(
            (m) => m.RegisteredUserModule
          ),
      },
    ],
  },
 
  {
    path: '',
    loadChildren: () =>
      import('./registration/registration.module').then(
        (m) => m.RegistrationModule
      ),
  },

  {
    path: '**',
    component: PageNotFoundComponent,
  },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
