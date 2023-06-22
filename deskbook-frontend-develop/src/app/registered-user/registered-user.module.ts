import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RegisteredUserRoutingModule } from './registered-user-routing.module';
import { RegisteredUsersComponent } from './registered-users/registered-users.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NgbPopoverModule } from '@ng-bootstrap/ng-bootstrap';
import { HttpClientModule } from '@angular/common/http';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { RestrictSpecialCharactersDirective } from './restrict-special-characters.directive';
import { RegisteredUserFooterComponent } from './registered-user-footer/registered-user-footer.component';
import { SharedModule } from '../shared/shared.module';
import { BrowserModule } from '@angular/platform-browser';

@NgModule({
  declarations: [
    RegisteredUsersComponent,
    RegisteredUserFooterComponent,
    RestrictSpecialCharactersDirective,
  ],
  imports: [
    CommonModule,
    RegisteredUserRoutingModule,
    NgbPopoverModule,
    HttpClientModule,
    ReactiveFormsModule,
    InfiniteScrollModule,
  ],
})
export class RegisteredUserModule {}
