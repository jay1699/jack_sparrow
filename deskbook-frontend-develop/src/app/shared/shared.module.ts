import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserprofileComponent } from './userprofile/userprofile.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgbPopoverModule } from '@ng-bootstrap/ng-bootstrap';
import { HttpClientModule } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RestrictSpecialCharactersDirective } from '../registered-user/restrict-special-characters.directive';

@NgModule({
  declarations: [UserprofileComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    NgSelectModule,
    NgbPopoverModule,
    HttpClientModule,
    ToastrModule.forRoot(),
    RouterModule,
    BrowserAnimationsModule,
  ],
  exports: [
    RouterModule,
    ReactiveFormsModule,
    FormsModule,
    ToastrModule,
    BrowserAnimationsModule,
    UserprofileComponent,
  ],
})
export class SharedModule {}
