import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class UserRegistrationService {
  constructor(private http: HttpClient) { }
  public url = environment.authority_register_uri;
  addUserRegistration(data: any) {
    return this.http.post(`${this.url}users/register`, data);
  }
}
