import { Component, OnInit } from '@angular/core';
import { RequestHistoryService } from '../request-history.service';
import { AuthService } from 'src/app/core/services/auth/auth.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { FormControl } from '@angular/forms';
import { debounceTime } from 'rxjs';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'desk-book-request-history',
  templateUrl: './request-history.component.html',
  styleUrls: ['./request-history.component.scss'],
  host: {
    class: 'd-flex flex-column h-100 overflow-hidden',
  },
})
export class RequestHistoryComponent implements OnInit {
  requestHistory: any;
  requestedUser: any;
  employee: any;
  userrole: any;
  superAdmin: boolean = false;
  public tableProperty: any;
  public currentPage: number = 1;
  public currentPageData: number = 300;
  public searchText: string = '';
  public seatRequestId: any;

  public searchControl = new FormControl();
  dropDownsearch: string = '';
  isFind: boolean = false;
  takeActionEmployee: any;

  constructor(
    private _authService: AuthService,
    private _requestService: RequestHistoryService,
    private modalService: NgbModal,
    private toasterService: ToastrService,
    private router: Router
  ) {
    this.tableProperty = {
      pageNo: this.currentPage,
      pageSize: this.currentPageData,
      search: '',
      sort: 'select',
    };
  }

  ngOnInit() {
    this.getAllEmpoyee();
    this._requestService.requestedUser.subscribe((res) => {
      this.requestedUser = res;
    });
    this._requestService.noDataFound.subscribe((res) => {
      this.isFind = res;
    });
  }

  public getAllEmpoyee() {
    this._authService.getUserData().subscribe((res: any) => {
      if (res.role === 'SUPER_ADMIN') {
        this.superAdmin = true;
        this.userrole = res.role;
        this._requestService
          .getAllRequestedEmployee(this.tableProperty)
          .subscribe((res: any) => {
            this.requestedUser = res.data;

            if (res.data.length === 0) {
              this.isFind = false; // when data length is 0 then no record found message will show
            } else {
              this.isFind = true; // all data has been shown
            }
          });
      }
    });
  }

  public filterArr = [
    { id: 0, name: 'All' },
    { id: 1, name: 'Pending' },
    { id: 2, name: 'Accepted' },
    { id: 3, name: 'Rejected' },
  ];

  openVerticallyCentered(requestPopup: any, user: any) {
    this.modalService.open(requestPopup, { centered: true });
    if (this.userrole === 'SUPER_ADMIN') {
      this._requestService
        .getEmployeeById(user.seatRequestId)
        .subscribe((res: any) => {
          this.employee = res.data;
          this.seatRequestId = user.seatRequestId;
        });
    } else {
      this.employee = user;
    }
  }

  closeVerticallyCentered() {
    this.router.navigate(['request-history']);
  }

  //Take-action, request accepted
  requestAccepted() {
    if (this.userrole === 'SUPER_ADMIN') {
      this._requestService
        .updateSeatApproval(this.seatRequestId, {
          requestStatus: 2,
          employeeId: this.employee.employeeId,
        })
        .subscribe((res) => {
          this._requestService
            .getAllRequestedEmployee(this.tableProperty)
            .subscribe((res: any) => {
              this.requestedUser = res.data;
              this.toasterService.success('Request Accepted');
            });
        });
    } else {
      let date = this.employee.requestFor.split('-').join('/');
      const formattedDate = new DatePipe('en-US').transform(date, 'MM/dd/yyyy');
      this.takeActionEmployee = {
        employeeId: this.employee.employeeId,
        bookingDate: formattedDate,
        seatId: this.employee.seatId,
        requestStatus: 2,
        emailId: this.employee.emailId,
        floor: this.employee.floorNo,
      };
      this._requestService
        .employeeSeatApproval(this.takeActionEmployee)
        .subscribe((res: any) => {
          this.getEmployeeRequests();
        });
    }
  }
  //Take-action, request rejected
  requestRejected() {
    if (this.userrole === 'SUPER_ADMIN') {
      this._requestService
        .updateSeatApproval(this.seatRequestId, {
          requestStatus: 3,
          employeeId: this.employee.employeeId,
        })
        .subscribe((res) => {
          this._requestService
            .getAllRequestedEmployee(this.tableProperty)
            .subscribe((res: any) => {
              this.requestedUser = res.data;
              this.toasterService.error('Request Rejected');
            });
        });
    } else {
      let date = this.employee.requestFor.split('-').join('/');
      const formattedDate = new DatePipe('en-US').transform(date, 'MM/dd/yyyy');
      this.takeActionEmployee = {
        employeeId: this.employee.employeeId,
        bookingDate: formattedDate,
        seatId: this.employee.seatId,
        requestStatus: 3,
        emailId: this.employee.emailId,
        floor: this.employee.floorNo,
      };
      this._requestService
        .employeeSeatApproval(this.takeActionEmployee)
        .subscribe((res: any) => {
          this.getEmployeeRequests();
        });
    }
  }

  searchData(value: any) {
    if (this.userrole === 'SUPER_ADMIN') {
      this.searchUser();
    } else {
      this.searchEmployee(value);
    }
  }

  //search admin-side data
  searchUser() {
    this.getAllEmpoyee();
    this.searchControl.valueChanges
      .pipe(debounceTime(300))
      .subscribe((searchText) => {
        this.searchText = searchText.trim();

        if (this.searchText.length >= 3) {
          this.tableProperty = {
            pageNo: this.currentPage,
            pageSize: this.currentPageData,
            sort: this.dropDownsearch,
            search: this.searchText,
          };
          this.getAllEmpoyee();
        } else {
          this.tableProperty = {
            pageNo: this.currentPage,
            pageSize: this.currentPageData,
            sort: this.dropDownsearch,
            search: '',
          };
          this.getAllEmpoyee();
        }
      });
  }

  //search user-side data
  searchEmployee(value: any) {
    if (value.length >= 2) {
      this._requestService.getSearchEmployee(value).subscribe((res: any) => {
        this.requestedUser = res.data;
      });
    } else {
      this.getEmployeeRequests();
    }
  }

  getEmployeeRequests() {
    this._requestService.getAllEmployeeRequest().subscribe((res: any) => {
      this.requestedUser = res.data;
    });
  }

  filterData(data: any) {
    if (this.userrole === 'SUPER_ADMIN') {
      this.dropDown(data);
    } else {
      this.filterRequests(data);
    }
  }

  //filter admin-side requests
  dropDown(data: any) {
    this.tableProperty = {
      pageNo: this.currentPage,
      pageSize: this.currentPageData,
      search: this.searchText,
      sort: data.name,
    };
    this.dropDownsearch = data.name;

    this._requestService
      .getAllRequestedEmployee(this.tableProperty)
      .subscribe((res: any) => {
        this.requestedUser = res.data;
      });
  }
  //filter user-side requests
  filterRequests(value: any) {
    if (value.id === 0) {
      this._requestService.getAllEmployeeRequest().subscribe((res: any) => {
        this.requestedUser = res.data;
      });
    } else {
      this._requestService
        .getFilteredEmployee(value.id)
        .subscribe((res: any) => {
          this.requestedUser = res.data;
        });
    }
  }

  requestAcceptPopup(requestPopup: any) {
    this.modalService.open(requestPopup, { centered: true });
  }
  requestRejectPopup(requestPopup: any) {
    this.modalService.open(requestPopup, { centered: true });
  }
}
