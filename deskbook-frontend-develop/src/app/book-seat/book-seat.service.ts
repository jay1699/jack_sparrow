import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class BookSeatService {
  cityAndFloorId: any;
  seatData: any;
  floorName = new BehaviorSubject<any>('');
  seat = new BehaviorSubject<any>([]);
  date = new BehaviorSubject<any>('');
  showBookSeat = new BehaviorSubject<boolean>(false);
  showPopover = new BehaviorSubject<boolean>(false);
  constructor(private _httpClient: HttpClient) {}

  public url: string = environment.userprofile_uri;

  getCities() {
    return this._httpClient.get(`${this.url}cities`);
  }

  getFloors(cityId: number) {
    return this._httpClient.get(`${this.url}floors/${cityId}`);
  }

  getSeats(data: any) {
    return this._httpClient.get(
      `${this.url}seat-view?date=${data.date}&cityId=${data.cityId}&floorId=${data.floorId}`
    );
  }

  getSeatDataById(data: any) {
    return this._httpClient.get(
      `${this.url}seat-view/get-seat-details?date=${data.date}&seatId=${data.seatId}`
    );
  }

  requestForSeat(body: any) {
    return this._httpClient.post(`${this.url}request-seat/`, body);
  }
}
