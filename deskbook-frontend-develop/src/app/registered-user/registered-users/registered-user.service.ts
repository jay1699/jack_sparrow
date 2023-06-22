import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class RegisteredUserService {
  updatedStatus = new BehaviorSubject<any>({});
  constructor(private _httpClient: HttpClient) { }
  viewEmployeeDataById = new BehaviorSubject<any>({});
  updatedUserStatus = new BehaviorSubject<any>([]);
  isButtonDisable = new BehaviorSubject<boolean>(true);
  isEmployeeStautsConfirm = new BehaviorSubject<boolean>(false);
  profileTitle = new BehaviorSubject<boolean>(false);
  public url = environment.authority_register_uri;

  // get all employee data
  getEmployeeData(tableProperty: any) {
    return this._httpClient.get(
      `${this.url}users?search=${tableProperty.search}&pageNo=${tableProperty.pageNo}&pageSize=${tableProperty.pageSize}`
    );
  }

  // get employee by id
  getEmployeeById(userId: any) {
    return this._httpClient.get(`${this.url}users/${userId}`);
  }

  // chnage employee status
  updateUserStatus(data: any) {
    return this._httpClient.put(`${this.url}users`, data);
  }
}
