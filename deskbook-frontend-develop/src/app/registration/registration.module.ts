import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RegistrationRoutingModule } from './registration-routing.module';

import { UserRegistrationComponent } from './user-registration/user-registration.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [UserRegistrationComponent],

  imports: [CommonModule, RegistrationRoutingModule, ReactiveFormsModule],
})
export class RegistrationModule { }




