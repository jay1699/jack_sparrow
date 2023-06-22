import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RequestHistoryComponent } from './request-history/request-history.component';
import { UserprofileComponent } from '../shared/userprofile/userprofile.component';

const routes: Routes = [
  {
    path:'',component:RequestHistoryComponent,
    children: [
      {
        path: 'user-profile',
        component: UserprofileComponent,
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RequestHistoryRoutingModule { }
