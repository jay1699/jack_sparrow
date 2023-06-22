import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { UserRegistrationService } from '../services/user-registration.service';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/core/services/auth/auth.service';
import { LoaderService } from 'src/app/core/components/loader/loader.service';

@Component({
  selector: 'desk-book-user-registration',
  templateUrl: './user-registration.component.html',
  styleUrls: ['./user-registration.component.scss'],
})
export class UserRegistrationComponent {
  public signupform!: FormGroup;
  public passwordType = 'password';
  public confirmpasswordType = 'password';
  public showpassword = false;
  public showConfirmpassword = false;

  public repeatpass: string = 'none';
  constructor(
    private userRegistrationService: UserRegistrationService,
    private toastrService: ToastrService,
    private authService: AuthService,
    private loaderservice: LoaderService
  ) {
    //create form-group for taking input
    this.signupform = new FormGroup({
      firstname: new FormControl('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(100),
        Validators.pattern(/^[^-\s][a-zA-Z ,'-]+$/),
        Validators.pattern(/^\S*$/),
      ]),
      lastname: new FormControl('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(100),
        Validators.pattern(/^[^-\s][a-zA-Z ,'-]+$/),
        Validators.pattern(/^\S*$/),
      ]),
      email: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(80),
        Validators.pattern(
          /^[a-zA-Z0-9]([.]?[a-zA-Z0-9]+)*@[a-zA-Z0-9]+[a-zA-Z0-9]+(\.[a-zA-Z]+|(\.[a-zA-Z]+\.[a-zA-Z])+)+$/
        ),
      ]),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(8),
        Validators.maxLength(25),
        Validators.pattern(
          /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!_&#])[A-Za-z\d@$!_&#]{8,25}$/
        ),
      ]),

      confirmpassword: new FormControl('', Validators.required),
    });
  }

  //for displaying all error message on click of submit button
  public validate(): void {
    if (this.signupform.invalid) {
      for (const control of Object.keys(this.signupform.controls)) {
        this.signupform.controls[control].markAsTouched();
      }
      return;
    }
  }

  //For password hide/show
  togglePassword() {
    this.passwordType = this.passwordType === 'password' ? 'text' : 'password';
    this.showpassword = true;
  }
  toggleshowPassword() {
    this.passwordType = this.passwordType === 'password' ? 'text' : 'password';
    this.showpassword = false;
  }

  //for confirmpassword hide/show
  toggleConfirmPassword() {
    this.confirmpasswordType =
      this.confirmpasswordType === 'password' ? 'text' : 'password';
    this.showConfirmpassword = true;
  }
  toggleConfirmshowPassword() {
    this.confirmpasswordType =
      this.confirmpasswordType === 'password' ? 'text' : 'password';
    this.showConfirmpassword = false;
  }
  public cpwd: any;
  confirmPasswordValue(value: any) {
    this.cpwd = value;
    if (this.cpwd !== '') {
      this.passwordMatching();
    }
  }
  public pwd: any;
  passwordValue(value: any) {
    this.pwd = value;
  }

  //Matching both field input value
  passwordMatching() {
    if (this.cpwd != this.pwd) {
      this.signupform.controls['confirmpassword'].setErrors({
        passwordmatch: true,
      });
      this.repeatpass = 'inline';
    } else {
      this.repeatpass = 'none';
    }
  }

  backToLogin() {
    this.authService.login();
  }

  //post call
  registerEmployee() {
    if (!this.signupform.valid) {
      return;
    } else {
      // this.loaderservice.requestStarted();
      this.userRegistrationService
        .addUserRegistration(this.signupform.value)
        .subscribe((res: any) => {
          if (res) {
            this.toastrService.success(res.data);
            this.authService.login();
          }
        });
    }
  }
}
