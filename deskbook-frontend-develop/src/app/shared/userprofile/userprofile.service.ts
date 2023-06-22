import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { UserProfile } from './userprofile.model';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserprofileService {
  // getAccessToken: any = localStorage.getItem(
  //   'oidc.user:https://1authority-interns.1rivet.com:5004/:DeskBook'
  // );
  // accessToken: any = JSON.parse(this.getAccessToken);
  // selectCity = new BehaviorSubject<any>([]);

  constructor(private _httpClient: HttpClient) {}

  //  headers: any = new HttpHeaders().set('Authorization','Bearer' + this.accessToken.access_token  )

  // header = new HttpHeaders({
  //   // 'Content-Type' : 'application/json',
  //   Authorization: `Bearer ${this.accessToken.access_token}`,
  // });
  // requestOptions = { headers: this.header };

  public url: string = environment.userprofile_uri;
  // public adminUrl = environment.authority_register_uri;

  getEmployee() {
    return this._httpClient.get(`${this.url}user-profile/`);
  }

  // getEmployee() {
  //   return this._httpClient.get(`${this.url}user-profile/`);
  // }

  getCities() {
    return this._httpClient.get(`${this.url}cities`);
  }

  getFloor(cityId: any) {
    return this._httpClient.get(`${this.url}floors/${cityId}`);
  }

  getColumns(floorId: any) {
    return this._httpClient.get(`${this.url}columns/${floorId}`);
  }
  getSeatNo(columnId: any) {
    return this._httpClient.get(`${this.url}seats/${columnId}`);
  }

  getDesignation() {
    return this._httpClient.get(`${this.url}designations`);
  }

  getModeOfWork() {
    return this._httpClient.get(`${this.url}mode-of-works`);
  }

  getWorkingDays() {
    return this._httpClient.get(`${this.url}working-days`);
  }

  updateEmployee(employeeData: any) {
    return this._httpClient.put(`${this.url}user-profile/`, employeeData);
  }

  userIsUpdated() {
    return this._httpClient.get(`${this.url}user-profile/isUpdated`);
  }
}
