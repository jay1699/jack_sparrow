import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth/auth.service';
import { BookingHistoryService } from '../booking-history.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormControl } from '@angular/forms';
import { debounceTime } from 'rxjs';

@Component({
  selector: 'desk-book-booking-history',
  templateUrl: './booking-history.component.html',
  styleUrls: ['./booking-history.component.scss'],
  host: {
    class: 'd-flex h-100',
  },
})
export class BookingHistoryComponent implements OnInit {
  public dropDown = new FormControl();
  public searchControl = new FormControl();
  public user: any;
  public isSuperAdmin: boolean = false;
  public bookingHistoryData: any = [];
  public status: any;
  public tableProperty: any;
  public currentPage: number = 1;
  public currentPageData: number = 999;
  public searchText: any = '';
  public array = [];
  public showNoRecord: boolean = false;
  public isDisabled: boolean = false;
  public getRequestId: any;
  public responseData: any;

  constructor(
    private bookingHistoryService: BookingHistoryService,
    private authService: AuthService,
    private modalService: NgbModal
  ) {}

  ngOnInit(): void {
    //Initial table property
    this.tableProperty = {
      pageNo: this.currentPage,
      pageSize: this.currentPageData,
      search: '',
      sort: 0,
    };

    //check if user is Admin or Employee
    this.authService.getUserData().subscribe((res: any) => {
      this.user = res.role;

      //For Admin
      if (res.role === 'SUPER_ADMIN') {
        this.status = [
          { id: 'all', name: 'All Booking' },
          { id: 'Upcoming', name: 'Upcoming' },
          { id: 'Past Booking', name: 'Past Booking' },
        ];
        this.getBookingHistory();
        this.isSuperAdmin;
      } else {
        //for Employee
        this.isSuperAdmin = true;
        this.tableProperty.sort = '';
        this.status = [
          { id: '', name: 'All-booking' },
          { id: 1, name: 'Pending' },
          { id: 2, name: 'Accepted' },
          { id: 3, name: 'Rejected' },
          { id: 4, name: 'cancelled' },
        ];
        this.getBookingHistory();
      }
    });
  }

  //get all booking-history data
  getBookingHistory() {
    //get data for Admin
    if (this.user === 'SUPER_ADMIN') {
      this.bookingHistoryService
        .getBookingHistoryDataAdmin(this.tableProperty)
        .subscribe((res: any) => {
          this.bookingHistoryData = res.data;
          this.showNoRecord = false;
          //show "Data not found" if no data found on search
          if (this.bookingHistoryData.length === 0) {
            this.showNoRecord = true;
          }
          //  else if (this.responseData.length >= 10) {
          //   this.bookingHistoryData = this.bookingHistoryData.concat(
          //     this.responseData
          //   );
          // }
        });
    } else {
      //get data for Employee

      this.bookingHistoryService
        .getBookingHistoryEmployeeData(this.tableProperty)
        .subscribe((res: any) => {
          this.bookingHistoryData = res.data;
          //show "Data not found" if no data found on search
          if (this.bookingHistoryData.length === 0) {
            this.showNoRecord = true;
          }
        });
    }
  }

  //popup on cross button click
  openVerticallyCentered(statusMessage: any, requestId: any) {
    this.modalService.open(statusMessage, {
      centered: true,
      windowClass: 'confirmation-popup',
      backdrop: 'static', // outside click is dissable
    });
    // to get the user by id
    this.getRequestId = requestId;
  }

  // to cancel the booking request
  cancelUserRequest() {
    this.bookingHistoryService
      .cancelRequest(this.getRequestId)
      .subscribe((res: any) => res);
  }

  //drop-down change event
  dropDownChange() {
    this.showNoRecord = false;
    this.tableProperty = {
      pageNo: this.currentPage,
      pageSize: this.currentPageData,
      search: this.searchText,
      sort: this.dropDown.value,
    };
    this.getBookingHistory();
  }

  //search for employee
  searchData() {
    this.searchControl.valueChanges
      .pipe(debounceTime(1000))
      .subscribe((searchText) => {
        this.searchText = searchText.trim();
        if (this.searchText.length >= 3) {
          this.tableProperty = {
            pageNo: this.currentPage,
            pageSize: this.currentPageData,
            search: this.searchControl.value,
            sort: this.dropDown.value,
          };
        } else {
          this.tableProperty = {
            pageNo: this.currentPage,
            pageSize: this.currentPageData,
            search: '',
            sort: this.dropDown.value,
          };
        }
        this.getBookingHistory();
      });
  }

  // onScroll() {
  //   this.tableProperty.pageNo++;
  //   console.log(this.tableProperty.pageNo);

  //   this.getBookingHistory();
  // }
}
