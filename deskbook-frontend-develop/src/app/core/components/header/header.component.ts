import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { UserprofileService } from '../../../shared/userprofile/userprofile.service';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { RequestHistoryService } from 'src/app/request-history/request-history.service';

@Component({
  selector: 'desk-book-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnInit {
  /** user profile data */
  @Input() userData: any;
  employeeDetails: any;
  employeeRequest: any;
  userIsActive: any;
  @ViewChild('userIsActive', { static: true }) content!: undefined;
  // isFind:boolean=false;
  constructor(
    private authService: AuthService,
    private router: Router,
    private userProfileService: UserprofileService,
    private modalService: NgbModal,
    private _requestHistory: RequestHistoryService
  ) {}
  ngOnInit() {
    this.userProfileService.userIsUpdated().subscribe((res: any) => {
      if (res.data.active == false) {
        this.openVerticallyCentered(this.content);
      }
    });
  }

  public logout() {
    this.authService.logout();
  }

  isUserUpdated(userIsUpdated: any, userIsActive: any) {
    this.userProfileService.userIsUpdated().subscribe((res: any) => {
      if (res.data.active == true) {
        if (res.data.updated == true) {
          this.router.navigate(['book-seat']);
        } else {
          this.router.navigate(['landing-page']);
          this.openVerticallyCentered(userIsUpdated);
        }
      } else {
        this.openVerticallyCentered(userIsActive);
      }
    });
  }
  isUserUpdatedRequest(userIsUpdated: any, userIsActive: any) {
    this.userProfileService.userIsUpdated().subscribe((res: any) => {
      if (res.data.active == true) {
        if (res.data.updated == true) {
          this.getEmployeeRequests();
          this.router.navigate(['request-history']);
        } else {
          this.router.navigate(['landing-page']);
          this.openVerticallyCentered(userIsUpdated);
        }
      } else {
        this.openVerticallyCentered(userIsActive);
      }
    });
  }
  isUserUpdatedBooking(userIsUpdated: any, userIsActive: any) {
    this.userProfileService.userIsUpdated().subscribe((res: any) => {
      if (res.data.active == true) {
        if (res.data.updated == true) {
          this.router.navigate(['booking-history']);
        } else {
          this.router.navigate(['landing-page']);
          this.openVerticallyCentered(userIsUpdated);
        }
      } else {
        this.openVerticallyCentered(userIsActive);
      }
    });
  }
  openVerticallyCentered(userIsUpdated: any) {
    this.modalService.open(userIsUpdated, {
      backdrop: 'static',
      centered: true,
      windowClass: 'custom',
    });
  }

  userProfile() {
    this.router.navigate(['landing-page/user-profile']);
  }

  getEmployeeRequests() {
    this._requestHistory.getAllEmployeeRequest().subscribe((res: any) => {
      // console.log(res);
      this.employeeRequest = res.data;
      console.log(this.employeeRequest.length);

      this._requestHistory.requestedUser.next(this.employeeRequest);
      if (this.employeeRequest.length === 0) {
        this._requestHistory.noDataFound.next(false);
      } else {
        this._requestHistory.noDataFound.next(true);
      }
    });
  }
}
