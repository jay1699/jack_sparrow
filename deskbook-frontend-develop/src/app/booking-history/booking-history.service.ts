import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class BookingHistoryService {
  constructor(private httpClient: HttpClient) {}
  public admin_url = environment.authority_register_uri;
  public employee_url = environment.userprofile_uri;

  //Get booking history data for Admin
  getBookingHistoryDataAdmin(tableProperty: any) {
    return this.httpClient.get(
      `${this.admin_url}userSeatBookings/?pageNo=${tableProperty.pageNo}&pageSize=${tableProperty.pageSize}&search=${tableProperty.search}&sort=${tableProperty.sort}`
    );
  }

  //Get booking history data for Employee
  getBookingHistoryEmployeeData(tableProperty: any) {
    return this.httpClient.get(
      `${this.employee_url}booking-history/${tableProperty.sort}`
    );
  }

  //to cancel the booking by the user
  cancelRequest(requestId: any) {
    return this.httpClient.put(
      `${this.employee_url}cancel-request/${requestId}`,
      {}
    );
  }
}
