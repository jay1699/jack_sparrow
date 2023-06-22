import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BookSeatService } from '../book-seat.service';
import { FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'desk-book-book-seat',
  templateUrl: './book-seat.component.html',
  styleUrls: ['./book-seat.component.scss'],
  host: {
    class: 'd-flex flex-column overflow-hidden h-100',
  },
})
export class BookSeatComponent implements OnInit {
  employees: any;
  selectedFloorValue: any;
  bookseatVisible: boolean = false;
  floorname: any;
  indexArray: number[];
  numberOfRows: any;
  SeatCollection: any;
  columnLengthArray: any;
  SeatTempArray: any = [];
  getDate: any;
  emplyeeData: any;
  seatId: any;
  reasonControl = new FormControl();
  public tableHeaders: any;
  public seats?: any;
  showBookSeat = false;
  showPopover = false;
  reasonEntered: boolean = false;
  disableSubmitBtn = true;
  public seatTypes = [
    { name: 'Reserved', color: 'reserved-color' },
    { name: 'Available', color: 'available-color' },
    { name: 'Booked', color: 'booked-color' },
    { name: 'Unavailable', color: 'unavailable-color' },
    { name: 'Unassigned', color: 'unassigned-color' },
  ];

  constructor(
    private modalService: NgbModal,
    private _bookseatService: BookSeatService,
    private toaster: ToastrService
  ) {
    this.indexArray = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];
    this.seats = [];

    this._bookseatService.seat.subscribe((res) => {
      this.seats = res;

      this.tableHeaders = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J'];

      this.numberOfRows = 15;
      this.columnLengthArray = [];

      this.SeatTempArray = [];
      this.tableHeaders.forEach((element: any) => {
        this.SeatCollection = this.seats.filter(
          (item: any) => item.column.name == element
        );
        this.columnLengthArray.push({
          columnName: element,
          columnLength: this.SeatCollection.length,
        });
        //Add empty seats to match the maximum number of rows
        this.SeatCollection.map((item: any) => {
          if (this.SeatCollection.length < this.numberOfRows) {
            this.SeatCollection.push({
              seat: {
                seatNumber: this.SeatCollection.length + 1,
                id: 'tempSeat',
              },
              column: { name: element },
            });
          }
        });
        this.SeatCollection.map((element: any) =>
          this.SeatTempArray.push(element)
        );
      });

      this.seats = this.SeatTempArray;
    });
    this._bookseatService.showBookSeat.subscribe(
      (res) => (this.showBookSeat = res)
    );
    this._bookseatService.showPopover.subscribe(
      (res) => (this.showPopover = res)
    );
  }

  // get all Seats
  ngOnInit(): void {
    this._bookseatService.floorName.subscribe(
      (res: any) => (this.floorname = res)
    );
    this.showBookSeat = false;
  }

  //get seat by Id
  getSeatId(data: any) {
    this.reasonControl.reset();
    this._bookseatService.date.subscribe((res) => (this.getDate = res));
    this.seatId = data.seat.id;
    if (
      data.status == 'Reserved' ||
      data.status == 'Available' ||
      data.status == 'Booked'
    ) {
      this._bookseatService
        .getSeatDataById({ date: this.getDate, seatId: data.seat.id })
        .subscribe((res: any) => {
          this.emplyeeData = res.data;
        });
    }
  }
  popover: any;
  // close the popover
  closePopover() {
    this._bookseatService.date.subscribe((res) => (this.getDate = res));
    this._bookseatService
      .requestForSeat({
        reason: this.reasonControl.value,
        requestDateTime: this.getDate,
        seatId: this.seatId,
      })
      .subscribe((res: any) => {
        this.toaster.success(res.data);
      });
    this.popover.close();
  }

  stopEventPropagation(event: MouseEvent) {
    event.stopPropagation();
  }

  getPopoverContent(item: any): string {
    return item.content;
  }

  showErrorMessage() {
    if (this.reasonControl.value.length == 0) {
      this.reasonEntered = true;
      this.disableSubmitBtn = true;
    } else {
      this.reasonEntered = false;
      this.disableSubmitBtn = false;
    }
  }
}
