export class UserProfile {
  profilePictureFileString?: string;
  firstName?: string;
  lastName?: string;
  phoneNumber?: string;
  designation?: number;
  modeOfWork?: number;
  city?: number;
  floor?: number;
  column?: number;
  seat?: number;
  workingDays?: number[];
  constructor(
    profilePictureFileString?: string,
    firstName?: string,
    lastName?: string,
    phoneNumber?: string,
    designation?: number,
    modeOfWork?: number,
    city?: number,
    floor?: number,
    column?: number,
    seat?: number,
    workingDays?: number[]
  ) {
    this.profilePictureFileString = profilePictureFileString;
    this.firstName = firstName;
    this.lastName = lastName;
    this.phoneNumber = phoneNumber;
    this.designation = designation;
    this.modeOfWork = modeOfWork;
    this.city = city;
    this.floor = floor;
    this.column = column;
    this.seat = seat;
    this.workingDays = workingDays;
  }
}

//for response of put

// export class UpdateUserProfile{
//    data?:;
//     profilePictureFileString?: string;
//     emailId?:string;
//     firstName?: string;
//     lastName?: string;
//     phoneNumber?: string;
//     designation?: {
//       id:number;
//       name:string
//     }
//     modeOfWork?:{
//       id:number;
//       name:string
//     };
//     city?:{
//       id:number;
//       name:string
//     };
//     floor?:{
//       id:number;
//       name:string
//     };
//     column?:{
//       id:number;
//       name:string
//     };
//     seat?:{
//       id:number;
//       seatNumber:string;
//       booked: boolean
//     };
//     days?:
//       {
//         id: number,
//         day: string
//       }[]
//   ;
//   error?:string
// constructor( profilePictureFileString?: string, emailId?:string, firstName?: string, lastName?: string, phoneNumber?: string, designation?: {
//   id:number;
//   name:string
// },   modeOfWork?:{
//   id:number;
//   name:string
// }, city?:{
//   id:number;
//   name:string
// },floor?:{
//   id:number;
//   name:string
// }, column?:{
//   id:number;
//   name:string
// },seat?:{
//   id:number;
//   seatNumber:string;
//   booked: boolean
// },days?:
// {
//   id: number,
//   day: string
// }[] ,error?:string){
//   this.profilePictureFileString = profilePictureFileString;
//   this.firstName = firstName;
//   this.lastName = lastName;
//   this.phoneNumber=phoneNumber;
//   this.designation=designation;
//   this.modeOfWork = modeOfWork;
//   this.city = city;
//   this.floor = floor;
//   this.column = column;
//   this.seat = seat;
//   this.days = days
//   this.error = error
// }
// }

// for response of city,floor,column,designation and mode of work
export class UserProfileSeatDetails {
  data?: { id: number; name: string }[];
  error?: string;

  constructor(data?: { id: number; name: string }[], error?: string) {
    this.data = data;
    this.error = error;
  }
}

//for response of seat
export class UserProfileSeat {
  data?: seatObj[];
  error?: string;

  constructor(data?: seatObj[], error?: string) {
    this.data = data;
    this.error = error;
  }
}
//object of seat response
export class seatObj {
  id?: number;
  seatNumber?: string;
  booked?: boolean;
  available?: boolean;
  constructor(
    id: number,
    seatNumber: string,
    booked: boolean,
    available: boolean
  ) {
    (this.id = id),
      (this.seatNumber = seatNumber),
      (this.booked = booked),
      (this.available = available);
  }
}

//for response of working days
export class UserProfileDays {
  data?: { id: number; day: string }[];
  error?: string;
  constructor(data?: { id: number; day: string }[], error?: string) {
    this.data = data;
    this.error = error;
  }
}

//mode of work,floor,column and city

export class seatDetailsObj {
  id?: number;
  name?: string;
  constructor(id: number, name: string) {
    this.id = id;
    this.name = name;
  }
}

export class City {
  id?: number;
  name?: string;
}

//for response of put

// export class UpdateUserProfile{
//    data?:;
//     profilePictureFileString?: string;
//     emailId?:string;
//     firstName?: string;
//     lastName?: string;
//     phoneNumber?: string;
//     designation?: {
//       id:number;
//       name:string
//     }
//     modeOfWork?:{
//       id:number;
//       name:string
//     };
//     city?:{
//       id:number;
//       name:string
//     };
//     floor?:{
//       id:number;
//       name:string
//     };
//     column?:{
//       id:number;
//       name:string
//     };
//     seat?:{
//       id:number;
//       seatNumber:string;
//       booked: boolean
//     };
//     days?:
//       {
//         id: number,
//         day: string
//       }[]
//   ;
//   error?:string
// constructor( profilePictureFileString?: string, emailId?:string, firstName?: string, lastName?: string, phoneNumber?: string, designation?: {
//   id:number;
//   name:string
// },   modeOfWork?:{
//   id:number;
//   name:string
// }, city?:{
//   id:number;
//   name:string
// },floor?:{
//   id:number;
//   name:string
// }, column?:{
//   id:number;
//   name:string
// },seat?:{
//   id:number;
//   seatNumber:string;
//   booked: boolean
// },days?:
// {
//   id: number,
//   day: string
// }[] ,error?:string){
//   this.profilePictureFileString = profilePictureFileString;
//   this.firstName = firstName;
//   this.lastName = lastName;
//   this.phoneNumber=phoneNumber;
//   this.designation=designation;
//   this.modeOfWork = modeOfWork;
//   this.city = city;
//   this.floor = floor;
//   this.column = column;
//   this.seat = seat;
//   this.days = days
//   this.error = error
// }
// }
