import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
@Injectable({
  providedIn: 'root',
})
export class SeatConfigurationService {
  // start: add button visible on click to manage
  addButtonVisible = new Subject<boolean>();
  //get call on adding new seat i.e on save button
  cityAndFloorId = new BehaviorSubject<any>({});
  seatData = new BehaviorSubject<any>([]);
  newSeats = new BehaviorSubject<any>([]);
  dropDownDisable = new BehaviorSubject<boolean>(false);
  isManageDisable = new BehaviorSubject<boolean>(true);
  removeSeats = new BehaviorSubject<boolean>(true);
  showPopover = new BehaviorSubject<boolean>(false);
  hidePopoverOnPlus = new BehaviorSubject<boolean>(false);

  constructor(private httpClient: HttpClient) { }
  public url: string = environment.authority_register_uri;

  getCities() {
    return this.httpClient.get(`${this.url}Cities`);
  }

  getFloors(cityId: number) {
    return this.httpClient.get(`${this.url}Cities/${cityId}/floors`);
  }

  getSeats(cityId?: number, floorId?: number) {
    return this.httpClient.get(
      `${this.url}Cities/${cityId}/floors/${floorId}/seats`
    );
  }

  updateSeatStatus(body: any) {
    return this.httpClient.put(`${this.url}Seats`, body);
  }

  addNewSeats(body: any) {
    return this.httpClient.post(`${this.url}Seats`, body);
  }

}
