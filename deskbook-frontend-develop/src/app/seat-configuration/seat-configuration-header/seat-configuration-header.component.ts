import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { SeatConfigurationService } from '../seat-configuration.service';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'desk-book-seat-configuration-header',
  templateUrl: './seat-configuration-header.component.html',
  styleUrls: ['./seat-configuration-header.component.scss'],
})
export class SeatConfigurationHeaderComponent implements OnInit {
  isDisabled: boolean = true;
  selectCity: any;
  selectFloor: any;
  seatConfigVisible: boolean = false;
  dropDownDisable!: boolean;
  @Output() openSeatConfigEvent = new EventEmitter();
  @Output() selectedFloorValueEvent = new EventEmitter();

  public cities: any;
  public floors: any;
  public seats: any;
  public cityId?: any;
  public floorId?: any;
  selectedFloorValue: any;
  public city = new FormControl();
  public floor = new FormControl();

  constructor(private seatConfigurationService: SeatConfigurationService) {
    this.seatConfigurationService.dropDownDisable.subscribe(
      (res) => (this.dropDownDisable = res)
    );
  }

  ngOnInit(): void {
    this.dropDownDisable = false;
    this.seatConfigurationService.getCities().subscribe((res: any) => {
      this.cities = res.data;
    });
  }

  getFloorByCity(cityId: number) {
    this.seatConfigurationService.getFloors(cityId).subscribe((res: any) => {
      this.floors = res.data;
    });
  }

  getSeatsData(cityId: number, floorId: number) {
    this.seatConfigurationService
      .getSeats(cityId, floorId)
      .subscribe((res: any) => {
        this.seatConfigurationService.seatData.next(res.data);
      });
  }

  onCitySelect(value: any) {
    if (value) {
      this.floor.reset();
      this.seatConfigurationService.isManageDisable.next(true);
      this.selectedFloorValueEvent.emit({
        cityId: this.cityId,
      });
      this.cityId = value.cityId;
      this.isDisabled = false;
    } else {
      this.isDisabled;
    }
    // if (this.floorId) {
    //   this.getSeatsData(this.cityId, this.floorId);
    // }
    this.getFloorByCity(value.cityId);
  }

  onChangeFloor(value: any) {
    if (value) {
      this.floorId = value.floorId;
      this.selectedFloorValue = value;
      this.seatConfigVisible = true;
      this.getSeatsData(this.cityId, value.floorId);
      this.selectedFloorValueEvent.emit({
        ...this.selectedFloorValue,
        cityId: this.cityId,
      });
      setTimeout(() => {
        this.seatConfigurationService.cityAndFloorId.next({
          ...this.selectedFloorValue,
          cityId: this.cityId,
        });
      }, 500);
    } else {
      this.seatConfigVisible;
    }
    this.seatConfigurationService.isManageDisable.next(false);
    this.seatConfigurationService.hidePopoverOnPlus.next(false);
    this.openSeatConfigEvent.emit(this.seatConfigVisible);
  }
}
