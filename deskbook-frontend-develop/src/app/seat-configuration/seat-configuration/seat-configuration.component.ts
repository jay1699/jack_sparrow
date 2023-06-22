import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { SeatConfigurationService } from '../seat-configuration.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'desk-book-seat-configuration',
  templateUrl: './seat-configuration.component.html',
  styleUrls: ['./seat-configuration.component.scss'],
  host: {
    class: 'book-seat-component',
  },
})
export class SeatConfigurationComponent implements OnInit {
  //use interface instead of any
  public seatConfigVisible: boolean = false;
  public addButtonVisible: boolean = false;
  public adminFooterVisible: boolean = false;
  public filteredData: any = {};
  public SeatCollection: any = [];
  public seats?: any;
  public numberOfRows: any;
  public indexArray?: any;
  public seatId: any;
  public seatName: any;
  public tableHeaders = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J'];
  public newSeatsArray: any = [];
  public isUnassignBtnDisable = false;
  public isUnavailableBtnDisable = false;
  public obtainedCityAndFloorId: any;
  public rowIdArray: any = [];
  public SeatTempArray: any = [];
  public columnLengthArray: any = [];
  public columnData: any = {};
  public selectedFloorValue: any;
  public removeSeats!: boolean;
  public seatTypes = [
    { name: 'Reserved', color: 'reserved-color' },{ name: 'Available', color: 'available-color' },{ name: 'Booked', color: 'booked-color' },
    { name: 'Unavailable', color: 'unavailable-color' }, { name: 'Unassigned', color: 'unassigned-color' },
  ];
  @ViewChild('TableContainer') TableContainer!: ElementRef;
  showPopover!: boolean;
  hidePopoverOnPlus: boolean = true;
  constructor(
    private seatConfigurationService: SeatConfigurationService,
    private modalService: NgbModal,
    private toasterService: ToastrService
  ) {
    this.seats = [];
    this.seatConfigurationService.seatData.subscribe((res) => {
      this.newSeatsArray = [];
      this.seatConfigurationService.newSeats.next(this.newSeatsArray);
      this.seatConfigurationService.hidePopoverOnPlus.subscribe((res) => {
        this.hidePopoverOnPlus = res;
      });
      //Remove existing row elements
      if (this.rowIdArray.length !== 0) {
        this.rowIdArray.forEach((element: any) => {
          const divElement = document.getElementById(element.seatId);
          if (divElement) {
            divElement.remove();
          }
        });
      }
      this.seatConfigurationService.showPopover.subscribe((res: any) => {
        this.showPopover = res;
      });
      this.rowIdArray = [];
      this.seats = res;
      this.SeatCollection = [];
      this.SeatTempArray = [];
      this.columnLengthArray = [];
      const arrayNames = [
        'availableforBookingSeat',
        'bookedSeat',
        'reservedSeat',
        'unavailableSeat',
        'unallocatedSeat',
      ];
      //Create the filtered data array
      arrayNames.forEach((arrayName: any) => {
        const filteredArray = this.seats[arrayName]?.map((item: any) => (
          {
          ...item,
          seatType: arrayName,
        }
        ));
      
        if (filteredArray) {
          this.SeatCollection.push(...filteredArray);
        }
      });
    //  Sort the seats by seatId
      this.seats = this.SeatCollection.sort(
        (a: { seatId: number }, b: { seatId: number }) => a.seatId - b.seatId
      );
      //Filter and process the seats for each table header
      this.numberOfRows = 15;
      this.tableHeaders.forEach((element) => {
        this.SeatCollection = this.seats.filter((item: any) => item.columnName == element);
        this.columnLengthArray.push({
          columnName: element,
          columnLength: this.SeatCollection.length,
        });
        //Add empty seats to match the maximum number of rows
        this.SeatCollection.map((item: any) => {
          if (this.SeatCollection.length < this.numberOfRows) {
            this.SeatCollection.push({ columnName: element });
          }
        });
        this.SeatCollection.map((element: any) => this.SeatTempArray.push(element));
      });
      this.indexArray = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];
      this.seats = this.SeatTempArray;
    });
  }
  
  ngOnInit(): void {
    this.seatConfigurationService.getCities();
    // for addbuttonVisible
    this.seatConfigurationService.addButtonVisible.subscribe((res) => {
      this.addButtonVisible = res;
    });
  }
  // to open seat-configuration after selecting floor and city
  openSeatConfig(value: any) {
    this.seatConfigVisible = true;
  }
  // to disply the selected floor on the ui
  receivedFloor(value: any) {
    this.selectedFloorValue = value;
  }

  // to make add button visible
  addButton(value: any) {
    this.addButtonVisible = value;
  }
   //for getting seats
  getSeatId(event: any) {
    this.seatId = event.target.id;
    this.seatName = event.target.name;
    this.isUnassignBtnDisable = this.seatName === 'unallocatedSeat';
    this.isUnavailableBtnDisable = this.seatName === 'unavailableSeat';
  }
   //Update the status of seats
  updateSeatStatus(data: any) {
    this.seatConfigurationService.updateSeatStatus(data).subscribe(
      (res: any) => {
        const message = res.data ? res.data : res.error;
        this.toasterService.success(message);
      },
      (error: any) => {
        this.toasterService.error(error);
      }
    );
  }
  getTotalRows(data: any, table: any, content: any) {
    this.hidePopoverOnPlus = false;
    this.numberOfRows = table.querySelectorAll('tr').length;
    this.columnLengthArray.forEach((element: any) => {
      if (element.columnName == data.columnName) {
        this.columnData = element;
      }
    });
    switch (this.columnData.columnLength < 15) {
      case true:
        if (this.columnData.columnLength + 1 >= this.numberOfRows) {
          table.appendChild(document.createElement('tr'));
        }
        let newRow = table.rows[this.columnData.columnLength + 1];
        table.setAttribute('id', 'table' + data.columnId);
        newRow.setAttribute('id', 'myRow' + data.columnId + this.numberOfRows);
        this.rowIdArray.push({
          seatId: 'myRow' + data.columnId + this.numberOfRows,
          seatNumber: this.columnData.columnLength + 1,
          tableId: 'table' + data.columnId,
        });
        this.newSeatsArray.push({
          columnId: data.columnId,
          seatNumber: this.columnData.columnLength + 1,
        });
        this.columnData.columnLength++;
        let myRow: any = document.createElement('tr');
        myRow.classList.add('employee-seat-table-thead');
        let td: any = document.createElement('td');
        myRow.setAttribute('id', 'myRow' + data.columnId + this.numberOfRows);
        const button = document.createElement('button');
        button.classList.add('border-0', 'receivedSeat', 'unallocatedSeat');
        td.appendChild(button);
        myRow.appendChild(td);
        newRow.replaceWith(myRow);
        if (!this.indexArray.includes(this.columnData.columnLength)) {
          this.indexArray.push(this.columnData.columnLength);
        }
        break;
      case false:
        this.modalService.open(content, {
          centered: true,
          windowClass: 'seat-config-modal-size',
        });
        break;
    }
  }
  //for unassigned seats
  unassignSeat(event: any) {
    this.updateSeatStatus([
      {
        unAssigned: true,
        isAvailable: true,
        seatId: Number(this.seatId),
      },
    ]);
    this.seatConfigurationService.cityAndFloorId.subscribe((res) => {
      this.obtainedCityAndFloorId = res;
    });
    setTimeout(() => {
      this.updatedSeats();
    }, 500);
  }
  private updatedSeats() {
    this.seatConfigurationService.getSeats(
      this.obtainedCityAndFloorId.cityId,
      this.obtainedCityAndFloorId.floorId
    ).subscribe((res: any) => {
      this.seats = res.data;
      this.seatConfigurationService.seatData.next(this.seats);
    });
  }
 //for unavailable seats
 unavailableSeat(event: any) {
  this.updateSeatStatus([
    {
      unAssigned: false,
      isAvailable: false,
      seatId: Number(this.seatId),
    },
  ]);
  this.seatConfigurationService.cityAndFloorId.subscribe((res) => {
    this.obtainedCityAndFloorId = res;
  });
  setTimeout(() => {
    this.updatedSeats();
  }, 500);
}
}
