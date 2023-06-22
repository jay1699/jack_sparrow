import { Component, OnInit } from '@angular/core';
import { RegisteredUserService } from '../registered-users/registered-user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'desk-book-registered-user-footer',
  templateUrl: './registered-user-footer.component.html',
  styleUrls: ['./registered-user-footer.component.scss'],
})
export class RegisteredUserFooterComponent implements OnInit {
  public getUpdatedStatus: any;
  public isButtonDisabled: boolean = true;

  constructor(
    private registeredUserService: RegisteredUserService,
    private toasterService: ToastrService
  ) {
    this.registeredUserService.updatedUserStatus.subscribe((res: any) => {
      this.getUpdatedStatus = res;
      this.toggleStautsMethod();
      this.statusChangeConfirmation();
    });
  }

  ngOnInit(): void {
    this.isButtonDisabled = true;
  }

  // update employee status
  updateStatus() {
    this.registeredUserService
      .updateUserStatus(this.getUpdatedStatus)
      .subscribe((res: any) => {
        this.toasterService.success(res.data);
      });
    this.registeredUserService.updatedUserStatus.next([]);
    this.isButtonDisabled = true;
  }
  // get isButtonDisabled initial true using behaviour subject from the registerd users
  toggleStautsMethod() {
    this.registeredUserService.isButtonDisable.subscribe((res: any) => {
      this.isButtonDisabled = res;
    });
  }

  statusChangeConfirmation() {
    this.registeredUserService.isEmployeeStautsConfirm.subscribe((res: any) => {
      this.isButtonDisabled = res;
    });
  }
}
