import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
@Injectable({
  providedIn: 'root',
})
export class RequestHistoryService {
  constructor(private httpClient: HttpClient) {}
  public adminurl = environment.authority_register_uri;
  public userUrl = environment.userprofile_uri;
  requestedUser = new BehaviorSubject<any>([]);
  noDataFound = new BehaviorSubject<any>(false);

  //Admin request-history

  //Get All requested user.
  getAllRequestedEmployee(tableProperty: any) {
    return this.httpClient.get(
      `${this.adminurl}userSeatRequests?pageNo=${tableProperty.pageNo}&pageSize=${tableProperty.pageSize}&search=${tableProperty.search}&sort=${tableProperty.sort}`
    );
  }

  //On the button of take action employee's Detail get.
  getEmployeeById(seatRequestId: any) {
    return this.httpClient.get(
      `${this.adminurl}userSeatRequests/${seatRequestId}/employee`
    );
  }

  //Accept or Reject the Seat request.
  updateSeatApproval(seatRequestId: any, data: any) {
    return this.httpClient.put(
      `${this.adminurl}userSeatRequests/${seatRequestId}/employee`,
      data
    );
  }

  //User request-history

  //Get All Employee requests
  getAllEmployeeRequest() {
    return this.httpClient.get(`${this.userUrl}request-history/`);
  }

  //get filtered employee
  getFilteredEmployee(statusId: any) {
    return this.httpClient.get(`${this.userUrl}request-history/${statusId}`);
  }

  //get Search employee
  getSearchEmployee(employeeName: any) {
    return this.httpClient.get(
      `${this.userUrl}request-history/search/${employeeName}`
    );
  }

  //Accept or Reject the Seat request
  employeeSeatApproval(employeeData: any) {
    return this.httpClient.put(
      `${this.userUrl}request-seat/accepted-reject`,
      employeeData
    );
  }
}
