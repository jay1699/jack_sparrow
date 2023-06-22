import { Component, OnInit, ViewChild } from '@angular/core';
import { RegisteredUserService } from './registered-user.service';
import { FormControl } from '@angular/forms';
import { debounceTime, take } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbPopover } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'desk-book-registered-users',
  templateUrl: './registered-users.component.html',
  styleUrls: ['./registered-users.component.scss'],
  host: {
    class:
      'registered-users-component d-flex flex-column h-100 overflow-hidden',
  },
})
export class RegisteredUsersComponent implements OnInit {
  public getEmplopyees: any[] = [];
  public searchControl = new FormControl();
  public tableProperty: any;
  public currentPage: number = 1;
  public currentPageData: number = 10;
  public searchText: string = '';
  public userId: any;
  public viewEmployeeData: any;
  public toggleArray: any = [];
  public toggleArray1: any;
  public updateEmployee: any = {};
  public newToggleArray: any = [];
  public updatedIndex: any;
  public uniqueArray: any;
  public isFind = true;
  public employee: any;
  public scroll: string = 'scroll';
  public search: string = 'search';
  public initialData!: any[];

  // @ViewChild('optionPopContent') optionPopContent!: ElementRef;
  @ViewChild('optionPopContent', { static: true })
  optionPopContent!: NgbPopover;


  constructor(
    private registerdUserService: RegisteredUserService,
    private router: Router,
    private route: ActivatedRoute,
    private modalService: NgbModal
  ) {
    this.registerdUserService.updatedUserStatus.subscribe((res: any) => {
      this.toggleArray = res;
    });
  }

  ngOnInit(): void {
    this.tableProperty = {
      pageNo: this.currentPage,
      pageSize: this.currentPageData,
      search: '',
    };
    this.getEmployeesData(this.tableProperty, this.scroll); // call the function for get all employee data
  }

  updateEmployeeStatusSuccess() {
    this.updateEmployee = {
      employeeId: this.employee.employeeId,
      isActive: this.employee.status,
    };

    this.toggleArray.push(this.updateEmployee);
    this.toggleArray.reverse();
    this.uniqueArray = this.toggleArray.filter(
      (object: any, index: any, uniqueArray: any) =>
        index ===
        uniqueArray.findIndex(
          (item: any) => item.employeeId === object.employeeId
        )
    );
    this.toggleArray = this.uniqueArray;
    this.registerdUserService.updatedUserStatus.next(this.toggleArray);
  }

  toggleStatus(employee: any) {
    employee.status = !employee.status;
    this.employee = employee;
    this.registerdUserService.isButtonDisable.next(false);
    this.updateEmployeeStatusSuccess();
  }

  updateEmployeeStatusUnsuccess() {
    this.employee.status = !this.employee.status;
    // this.registerdUserService.isEmployeeStautsConfirm.next(true);
  }

  // infinite scroll
  onScroll() {
    this.getEmployeesData(this.tableProperty, this.scroll);
    this.tableProperty.pageNo++;
    // this.optionPopContent.close(); // to remove popover on scroll
  }
  get allEmployees(): any[] {
    return this.getEmplopyees; // Return all employees
  }

  // search employee by name from server side
  searchData() {
    this.searchControl.valueChanges
      .pipe(debounceTime(1000), take(1))
      .subscribe((searchText) => {
        this.searchText = searchText.trim();
        if (this.searchText.length < 2) {
          this.clearSearch();
        } else {
          this.tableProperty.search = this.searchText;
          this.tableProperty.pageNo = this.currentPage;
          this.tableProperty.pageSize = 9999;
          this.getEmployeesData(this.tableProperty, this.search);
        }
      });
  }

  // clear search after remove text
  clearSearch() {
    // Clear the search text and perform any necessary actions
    this.searchText = '';
    // Call the getEmployeesData function with empty search to retrieve all employees
    this.tableProperty.search = '';
    this.tableProperty.pageNo = this.currentPage;
    this.tableProperty.pageSize = 10;
    this.getEmployeesData(this.tableProperty, this.search);
    this.getEmplopyees = [...this.initialData]; // Restore the initial data when clearing the search
  }

  // get all the employee data from the server side
  getEmployeesData(tableProperty: any, param: string) {
    return this.registerdUserService
      .getEmployeeData(tableProperty)
      .subscribe((res: any) => {
        // message for search on wrong data field
        if (res.data.length === 0 && param === 'search') {
          this.isFind = false; // when data length is 0 then no record found message will show
        } else {
          this.isFind = true; // all data has been shown
        }
        this.toggleArray = [];
        if (this.searchText) {
          this.getEmplopyees = res.data;
          // this.getEmplopyees = this.getEmplopyees.concat(res.data);
        } else if (this.getEmplopyees.length >= 10) {
          this.getEmplopyees = this.getEmplopyees.concat(res.data);
        } else {
          this.getEmplopyees = res.data;
          this.initialData = res.data; // Store the initial data received from the server
        }
      });
  }
  // View user profile
  viewUser(userId: any) {
    this.route.params.subscribe((params) => {
      this.userId = params['id']; //infiniteScrollDistance Access the ID parameter from the URL
    });
    this.router.navigate(['registered-users/user-profile', userId]);
    this.getEmployeeDataById(userId);
    this.registerdUserService.profileTitle.next(true);
  }
  // get user id
  getEmployeeDataById(userId: any) {
    this.registerdUserService.getEmployeeById(userId).subscribe((res: any) => {
      this.viewEmployeeData = res.data;
      this.registerdUserService.viewEmployeeDataById.next(
        this.viewEmployeeData
      );
    });
  }
  // open popup message
  openVerticallyCentered(statusMessage: any) {
    this.modalService.open(statusMessage, {
      centered: true,
      windowClass: 'confirmation-popup',
      backdrop: 'static', // outside click is dissable
    });
  }

}


