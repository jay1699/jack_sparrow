import {
  Component,
  ElementRef,
  OnInit,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { SafeResourceUrl, DomSanitizer } from '@angular/platform-browser';
import { UserprofileService } from './userprofile.service';
import {
  City,
  UserProfile,
  UserProfileDays,
  UserProfileSeat,
  UserProfileSeatDetails,
  seatDetailsObj,
  seatObj,
} from './userprofile.model';
import { ActivatedRoute, Router } from '@angular/router';
import { RegisteredUserService } from 'src/app/registered-user/registered-users/registered-user.service';
import { Location } from '@angular/common';
@Component({
  selector: 'desk-book-userprofile',
  templateUrl: './userprofile.component.html',
  styleUrls: ['./userprofile.component.scss'],
  host: {
    class: 'userprofile-component',
  },
})
export class UserprofileComponent implements OnInit {
  public submitted: boolean = false;
  public selectModeOfWork: boolean = false;
  public maxDaysValidationMessage!: string;
  public minDaysValidationMessage!: string;
  public workFromHome: boolean = false;
  public city: any;
  public floor: any;
  public column: any;
  public seat: any;
  public designation: any;
  public modeOfWork: any;
  public hybridDays: any;
  public columnId: any = 1;
  public employeeData!: any;
  public isEditable: boolean = true;
  public openEdittedProfile: boolean = true;
  public updatedEmployeeData: any;
  public selectedDays: any;
  public getData: boolean = true;
  public updatedData: boolean = false;
  public updatedEmployeeImage: any;
  public defaultImageUrl: any = './assets/images/upload-img.png';
  public userProfileForm!: FormGroup;
  public userId: any;
  public isDisable: boolean = false;
  public currentRoute: any;
  profileTitle: any;

  constructor(
    private _formBuilder: FormBuilder,
    private _modalService: NgbModal,
    private toastrService: ToastrService,
    private _userprofileService: UserprofileService,
    private sanitizer: DomSanitizer,
    private route: ActivatedRoute,
    private router: Router,
    private registerdUserService: RegisteredUserService,
    private location: Location
  ) {
    this.userProfileForm = this._formBuilder.group({
      profilePictureFileString: [null],
      firstName: [
        null,
        [
          Validators.required,
          Validators.pattern(/^[A-Za-z]+('[a-z]+)?$/),
          Validators.maxLength(100),
          Validators.minLength(2),
        ],
      ],
      lastName: [
        null,
        [
          Validators.required,
          Validators.pattern(/^[A-Za-z]+('[a-z]+)?$/),
          Validators.maxLength(100),
          Validators.minLength(2),
        ],
      ],
      phoneNumber: [
        null,
        [
          Validators.required,
          Validators.pattern(/^[0-9]*$/),
          Validators.minLength(10),
          Validators.maxLength(10),
        ],
      ],
      designation: [null, Validators.required],
      modeOfWork: [null, Validators.required],
      city: [null, Validators.required],
      floor: [null, Validators.required],
      column: [null, Validators.required],
      seat: [null, Validators.required],
      workingDays: this._formBuilder.array([]),
    });
  }

  imageSRC!: SafeResourceUrl;

  ngOnInit() {
    this.route.params.subscribe((params) => {
      this.userId = params['id']; // Access the ID parameter from the URL
    });
    this.currentRoute = this.router.url;
    if (this.userId) {
      this.registerdUserService.viewEmployeeDataById.subscribe((res) => {
        this.employeeData = res;
        console.log(res);

        if (res.profilePictureFileString) {
          this.imageSRC = this.sanitizer.bypassSecurityTrustResourceUrl(
            'data:image/png;base64,' + res.profilePictureFileString
          );
          this.defaultImageUrl = this.imageSRC;
          this.userProfileForm
            .get('profilePictureFileString')
            ?.patchValue(
              'data:image/png;base64,' + res.profilePictureFileString
            );
        } else {
          this.defaultImageUrl = './assets/images/upload-img.png';
        }
        this.isDisable = true;
      });
    } else {
      this.getEmployeeData();
    }
    this.getCityValues();
    this.getDesignationValues();
    this.getModeOfWorkValues();
    this.getWorkingDaysValue();
    this.registerdUserService.profileTitle.subscribe((res: any) => {
      this.profileTitle = res;
    });
  }

  // Getting a registered employee data
  getEmployeeData() {
    this._userprofileService.getEmployee().subscribe((res: any) => {
      this.employeeData = res.data;
      console.log(res.data);
      if (this.employeeData.active == false) {
        this.isDisable = true;
      } else {
        this.isDisable = false;
      }
      //converting base64 to image
      if (res.data.profilePictureFileString) {
        this.imageSRC = this.sanitizer.bypassSecurityTrustResourceUrl(
          'data:image/png;base64,' + res.data.profilePictureFileString
        );
        this.defaultImageUrl = this.imageSRC;
        this.userProfileForm
          .get('profilePictureFileString')
          ?.patchValue(
            'data:image/png;base64,' + res.data.profilePictureFileString
          );
      }
      if (res.data.profilePictureFileString == null) {
        this.userProfileForm.get('profilePictureFileString')?.patchValue(null);
      }

      if (
        this.employeeData.modeOfWork?.id == 2 ||
        this.employeeData.modeOfWork?.id == 3
      ) {
        this.userProfileForm.removeControl('workingDays');
      }
      this.selectModeOfWork = false;
      if (this.employeeData.modeOfWork?.id == 1) {
        this.selectModeOfWork = true;
        this.selectedDays = this.employeeData.days;
        let hybridArr = this.userProfileForm.get('workingDays') as FormArray;

        //Get days checked

        let mergedArray = this.employeeData.days.concat(this.hybridDays);

        let updatedArray = mergedArray.reduce((result: any, obj: any) => {
          let foundIndex = result.findIndex(
            (item: { id: any; name: any }) =>
              item?.id === obj?.id && item.name === obj.name
          );
          if (foundIndex === -1) {
            result.push({ ...obj, selected: false });
          } else {
            result[foundIndex].selected = true;
          }
          return result;
        }, []);

        updatedArray.sort(
          (a: { id: number }, b: { id: number }) => a.id - b.id
        );

        updatedArray.forEach((element: { selected: boolean; id: any }) => {
          if (element.selected == true) {
            hybridArr.push(new FormControl(element.id));
          }
        });
        this.hybridDays = updatedArray;
      }

      this.userProfileForm.patchValue({
        firstName: this.employeeData.firstName,
        lastName: this.employeeData.lastName,
        phoneNumber: this.employeeData.phoneNumber,
        designation: this.employeeData.designation?.id,
        modeOfWork: this.employeeData.modeOfWork?.id,
        city: this.employeeData.city?.id,
        floor: this.employeeData.floor?.id,
        column: this.employeeData.column?.id,
        seat: this.employeeData.seat?.id,
      });
    });
  }

  //Getting the dropdown value of city
  getCityValues() {
    this._userprofileService.getCities().subscribe((res: any) => {
      this.city = res.data;
    });
    // this._userprofileService.selectCity.next(this.city);
  }

  //Getting the dropdown value of floor according to the value selected in city
  getFloorByCityId(cityId: number) {
    if (cityId) {
      this._userprofileService
        .getFloor(cityId)
        .subscribe((res: UserProfileSeatDetails) => {
          this.floor = res.data;
        });
    }
  }

  //Getting the dropdown value of column according to the value selected in floor
  getColumnByFloorId(floorId: number) {
    if (floorId) {
      this._userprofileService
        .getColumns(floorId)
        .subscribe((res: UserProfileSeatDetails) => {
          this.column = res.data;
        });
    }
  }

  //Getting the dropdown value of seat number according to the value selected in column
  getSeatNoByColumnId(columnId: number) {
    if (columnId) {
      this._userprofileService
        .getSeatNo(columnId)
        .subscribe((res: UserProfileSeat) => {
          this.seat = res?.data?.map((obj: seatObj) => {
            if (obj.booked === true || obj.available == false) {
              return {
                ...obj,
                disabled: true,
              };
            } else {
              return {
                ...obj,
                disabled: false,
              };
            }
          });
        });
    }
  }

  //Getting the dropdown value of designation
  getDesignationValues() {
    this._userprofileService
      .getDesignation()
      .subscribe((res: UserProfileSeatDetails) => {
        this.designation = res.data;
      });
  }
  //Getting the dropdown value of mode of work
  getModeOfWorkValues() {
    this._userprofileService
      .getModeOfWork()
      .subscribe((res: UserProfileSeatDetails) => {
        this.modeOfWork = res.data;
      });
  }
  //Getting the days to be selected during hybrid mode of work
  getWorkingDaysValue() {
    this._userprofileService
      .getWorkingDays()
      .subscribe((res: UserProfileDays) => {
        this.hybridDays = res.data;
      });
  }
  //updating the employee data
  updateEmployeeData(employeeData: UserProfile) {
    this._userprofileService
      .updateEmployee(employeeData)
      .subscribe((res: any) => {
        this.updatedEmployeeData = res.data;
        this.updatedEmployeeImage =
          'data:image/png;base64,' + res?.data?.profilePictureFileString;
        this.getEmployeeData();
      });
  }
  //Getting the values of selected checkbox
  getSelectedDaysValues(e: any) {
    let hybridArr = this.userProfileForm.get('workingDays') as FormArray;
    if (e.target.checked) {
      hybridArr.push(new FormControl(Number(e.target.id)));
    } else {
      hybridArr.controls.forEach((control, i) => {
        if (control.value == e.target.id) {
          hybridArr.removeAt(i);
        }
      });
    }
    //validation for maximum days selection
    if (hybridArr.length > 4) {
      e.target.checked = false;
      hybridArr.controls.forEach((control, i) => {
        if (control.value == e.target.id) {
          hybridArr.removeAt(i);
        }
      });

      this.maxDaysValidationMessage = 'You can select Maximum 4 days';
    } else if (hybridArr.length == 0) {
      this.minDaysValidationMessage = ' Please select Days';
    } else {
      this.maxDaysValidationMessage = '';
      this.minDaysValidationMessage = '';
    }
  }

  //Updating user-profile details
  updateUserData() {
    this.submitted = true;
    if (this.userProfileForm.controls['modeOfWork'].value == 1) {
      let hybridArr = this.userProfileForm.get('workingDays') as FormArray;
      if (hybridArr.length == 0) {
        this.minDaysValidationMessage = ' Please select Days';
        this.userProfileForm
          .get('workingDays')
          ?.setErrors({ selectDays: true });
      } else {
        this.userProfileForm.get('workingDays')?.updateValueAndValidity();
      }
    }

    if (
      this.userProfileForm.controls['modeOfWork'].value == 1 ||
      this.userProfileForm.controls['modeOfWork'].value == 3
    ) {
      this.userProfileForm.controls['floor'].enable();
      this.userProfileForm.controls['column'].enable();
      this.userProfileForm.controls['seat'].enable();
    }

    if (this.userProfileForm.valid) {
      this.isEditable = true;
      this.updateEmployeeData(this.userProfileForm.value);
      this.userProfileForm.reset();
      this.toastrService.success('Profile successfully updated');
    }
    this.maxDaysValidationMessage = '';
  }
  //getting the value of mode of work on onchange event
  onChangeModeOfWork(value: seatDetailsObj) {
    this.modeOfWorkHybrid(value);
    // console.log(value);

    if (value.id == 2 || value.id == 3) {
      this.userProfileForm.removeControl('workingDays');
      this.maxDaysValidationMessage = '';
      this.minDaysValidationMessage = '';
    } else {
      this.userProfileForm.setControl('workingDays', new FormArray([]));
    }

    if (value.id == 2) {
      this.disableControls();
    } else {
      this.enableControls();
    }
    this.hybridDays.map((item: any) => (item.selected = false));
    this.resetControls();

    this.userProfileForm.controls['floor'].disable();
    this.userProfileForm.controls['column'].disable();
    this.userProfileForm.controls['seat'].disable();
  }

  //Enabling floor when city value is selected
  onChangeCity(value: any) {
    if (value) {
      this.userProfileForm.controls['floor'].enable();
      this.userProfileForm.controls['column'].disable();
      this.userProfileForm.controls['seat'].disable();
    } else {
      this.userProfileForm.controls['floor'].disable();
    }
    this.getFloorByCityId(value.id);
    this.userProfileForm.controls['floor'].reset();
    this.userProfileForm.controls['column'].reset();
    this.userProfileForm.controls['seat'].reset();
  }
  //Enabling column when floor value is selected
  onChangeFloor(value: any) {
    if (value) {
      this.userProfileForm.controls['column'].enable();
      this.userProfileForm.controls['seat'].disable();
    } else {
      this.userProfileForm.controls['column'].disable();
    }

    this.getColumnByFloorId(value.id);
    this.userProfileForm.controls['column'].reset();
    this.userProfileForm.controls['seat'].reset();
  }
  //Enabling seat no. when column value is selected
  onChangeColumn(value: any) {
    if (value) {
      this.userProfileForm.controls['seat'].enable();
      this.getSeatNoByColumnId(value.id);
    } else {
      this.userProfileForm.controls['seat'].disable();
    }
    this.userProfileForm.controls['seat'].reset();
  }
  //Uploading base64 converted image with validations
  uploadImage(event: any, invalidImageFormat: any, invalidImageSize: any) {
    let imageFile = event.target.files[0];
    if (
      imageFile.type !== 'image/png' &&
      imageFile.type !== 'image/jpg' &&
      imageFile.type !== 'image/jpeg'
    ) {
      if (this.employeeData.profilePictureFileString == null) {
        this.defaultImageUrl = './assets/images/upload-img.png';
      } else {
        this.defaultImageUrl =
          'data:image/png;base64,' + this.employeeData.profilePictureFileString;
      }

      this.openModalPopUp(invalidImageFormat);
    } else if (imageFile.size > 2000000) {
      if (this.employeeData.profilePictureFileString == null) {
        this.defaultImageUrl = './assets/images/upload-img.png';
      } else {
        this.defaultImageUrl =
          'data:image/png;base64,' + this.employeeData.profilePictureFileString;
      }
      this.openModalPopUp(invalidImageSize);
    } else {
      if (imageFile) {
        let reader = new FileReader();
        reader.readAsDataURL(imageFile);
        reader.onload = (event: any) => {
          this.defaultImageUrl = event.target.result;
          this.toastrService.success('Profile picture is updated successfully');
          this.userProfileForm
            .get('profilePictureFileString')
            ?.patchValue(this.defaultImageUrl);
        };
      }
    }
  }
  //Displaying modal pop-up for image validations and confirmation of closing form
  formCloseConfirmation(popUpMessage: any) {
    this.registerdUserService.profileTitle.next(false);
    if (this.userProfileForm.dirty == true) {
      this.openPopUpOnCross(popUpMessage);
    } else {
      this.location.back();
      setTimeout(() => {
        this.isEditable = true;
      }, 2000);
    }
  }

  openPopUpOnCross(popUpMessage: any) {
    this._modalService.open(popUpMessage, {
      centered: true,
      windowClass: 'confirmation-popup',
      backdrop: 'static',
    });
  }

  openModalPopUp(popUpMessage: any) {
    this._modalService.open(popUpMessage, {
      centered: true,
      windowClass: 'confirmation-popup',
      backdrop: 'static',
    });
  }

  //Resetting the form while closing it by clicking ok button
  closeForm() {
    this.userProfileForm.reset();
    this.isEditable = true;
  }

  //Displaying days when mode of work hybrid is selected
  modeOfWorkHybrid(value: seatDetailsObj) {
    if (value.id == 1) {
      this.selectModeOfWork = true;
    } else {
      this.selectModeOfWork = false;
    }
  }
  //Opening form by clicking on edit button
  openProfileForm() {
    this.isEditable = false;
    this.userProfileForm.removeControl('workingDays');
    this.userProfileForm.setControl('workingDays', new FormArray([]));
    this.getEmployeeData();

    setTimeout(() => {
      this.getFloorByCityId(this.userProfileForm.controls['city'].value);
    }, 1000);

    setTimeout(() => {
      this.getColumnByFloorId(this.userProfileForm.controls['floor'].value);
    }, 1500);

    setTimeout(() => {
      this.getSeatNoByColumnId(this.userProfileForm.controls['column'].value);
    }, 2000);

    this.userProfileForm.controls['floor'].disable();
    this.userProfileForm.controls['column'].disable();
    this.userProfileForm.controls['seat'].disable();

    if (this.employeeData.modeOfWork?.id == 2) {
      this.disableControls();
    }
  }

  //disable city,floor,column,seat
  disableControls() {
    this.userProfileForm.controls['city'].disable();
    this.userProfileForm.controls['floor'].disable();
    this.userProfileForm.controls['column'].disable();
    this.userProfileForm.controls['seat'].disable();
  }
  //enable city,floor,column,seat
  enableControls() {
    this.userProfileForm.controls['city'].enable();
    this.userProfileForm.controls['floor'].enable();
    this.userProfileForm.controls['column'].enable();
    this.userProfileForm.controls['seat'].enable();
  }

  //reset city,floor,column,seat
  resetControls() {
    this.userProfileForm.controls['city'].reset();
    this.userProfileForm.controls['floor'].reset();
    this.userProfileForm.controls['column'].reset();
    this.userProfileForm.controls['seat'].reset();
  }
}
