import { HttpClient, HttpHeaders } from '@angular/common/http';
import { core } from '@angular/compiler';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { CoreModel } from '../core.model';

@Injectable({
  providedIn: 'root',
})
export class UserprofileService {
  id: any = 'c8d45a72-fa4b-4f14-a6d7-e2f77d9d8ac5';
  // getAccessToken: any = localStorage.getItem(
  //   'oidc.user:https://1authority-interns.1rivet.com:5004/:DeskBook'
  // );
  // accessToken: any = JSON.parse(this.getAccessToken);

  constructor(private _httpClient: HttpClient) {}

  //  headers: any = new HttpHeaders().set('Authorization','Bearer' + this.accessToken.access_token  )

  // header = new HttpHeaders({
  //   // 'Content-Type' : 'application/json',
  //   Authorization: `Bearer ${this.accessToken.access_token}`,
  // });
  // requestOptions = { headers: this.header };

  url = environment.userprofile_uri;
  getEmployeeById() {
    return this._httpClient.get(`${this.url}user-profile/${this.id}`);
  }
  getCities() {
    return this._httpClient.get(`${this.url}cities`);
  }
  getFloor(cityId: CoreModel) {
    return this._httpClient.get(`${this.url}floors/${cityId}`);
  }
  getColumns(floorId: CoreModel) {
    return this._httpClient.get(`${this.url}columns/${floorId}`);
  }
  getSeatNo(columnId: CoreModel) {
    return this._httpClient.get(`${this.url}seats/${columnId}`);
  }
  getDesignation() {
    return this._httpClient.get(`${this.url}designations`);
  }
  updateEmployee(employeeData: CoreModel) {
    return this._httpClient.put(
      `${this.url}user-profile/${this.id}`,
      employeeData
    );
  }
}
