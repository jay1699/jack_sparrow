import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { SeatConfigurationService } from '../seat-configuration.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'desk-book-admin-footer',
  templateUrl: './admin-footer.component.html',
  styleUrls: ['./admin-footer.component.scss'],
})
export class AdminFooterComponent implements OnInit {
  isDisabled: boolean = true;
  addButtonVisible: boolean = true;
  obtainedCityAndFloorId: any;
  updatedSeats: any;
  newSeats: any;
  isClicked: boolean = true;

  @Output() addButtonVisibleEvent = new EventEmitter();
  constructor(
    private seatConfigurationService: SeatConfigurationService,
    private toasterService: ToastrService
  ) {
    this.seatConfigurationService.isManageDisable.subscribe((res) => {
      this.isDisabled = res;
    });
  }

  ngOnInit(): void {
    this.isDisabled = true;
  }
  toShowAddButton() {
    this.isClicked = false;
    this.addButtonVisibleEvent.emit(this.addButtonVisible);
    this.seatConfigurationService.showPopover.next(true);
    this.seatConfigurationService.dropDownDisable.next(true);
    this.seatConfigurationService.hidePopoverOnPlus.next(true);
  }

  addNewSeats() {
    this.isClicked = true;
    this.addButtonVisibleEvent.emit(!this.addButtonVisible);
    this.seatConfigurationService.dropDownDisable.next(false);
    this.seatConfigurationService.showPopover.next(false);
    this.seatConfigurationService.newSeats.subscribe((res) => {
      this.newSeats = res;
    });
    this.seatConfigurationService.hidePopoverOnPlus.next(false);
    this.seatConfigurationService
      .addNewSeats(this.newSeats)
      .subscribe((res: any) => {
        if (this.newSeats.length == 0) {
        } else {
          this.toasterService.success(res.data);
        }
      });
    this.seatConfigurationService.cityAndFloorId.subscribe((res) => {
      this.obtainedCityAndFloorId = res;
    });

    setTimeout(() => {
      this.seatConfigurationService
        .getSeats(
          this.obtainedCityAndFloorId.cityId,
          this.obtainedCityAndFloorId.floorId
        )
        .subscribe((res) => {
          this.updatedSeats = res;
          this.seatConfigurationService.seatData.next(this.updatedSeats.data);
        });
    }, 500);
  }
}
