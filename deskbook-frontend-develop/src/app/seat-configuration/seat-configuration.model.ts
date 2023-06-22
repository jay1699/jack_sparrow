// model for get all city
export class SeatConfigurationCity {
  cityId?: number;
  cityName?: string;
  constructor(cityId?: number, cityName?: string) {
    this.cityId = cityId;
    this.cityName = cityName;
  }
}

// Model for getAllFloor
export class SeatConfigurationFloor {
  florrId?: number;
  floorName?: string;
  constructor(florrId?: number, floorName?: string) {
    this.florrId = florrId;
    this.floorName = floorName;
  }
}

// model for getAllSeatConfiguration
export class SeatConfigurationGetAllSeat {
  bookedSeat?: SeatConfiguration[];
  availableSeat?: SeatConfiguration[];
  reservedseat?: SeatConfiguration[];
  unavailableSeat?: SeatConfiguration[];
  unassignedSeat?: SeatConfiguration[];
  seatConfigVisible?:SeatConfiguration[];
  seatId?:SeatConfiguration[];

}

export class SeatConfiguration {
  seatNumber?: number;
  columnName?: string;
  seatId?: number;
  constructor(seatNumber?: number, columnName?: string, seatId?: number) {
    this.seatNumber = seatNumber;
    this.columnName = columnName;
    this.seatId = seatId;
  }
}






