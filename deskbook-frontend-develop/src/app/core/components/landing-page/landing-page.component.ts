import { Component } from '@angular/core';
import { UserprofileService } from '../../../shared/userprofile/userprofile.service';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'desk-book-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrls: ['./landing-page.component.scss'],
})
export class LandingPageComponent {
  constructor(
    private userProfileService: UserprofileService,
    private router: Router,
    private modalService: NgbModal
  ) {}
  isUserUpdated(userIsUpdated: any) {
    this.userProfileService.userIsUpdated().subscribe((res: any) => {
      // console.log(res);
      if (res.data.updated == true) {
        this.router.navigate(['book-seat']);
      } else {
        this.openVerticallyCentered(userIsUpdated);
      }
    });
  }
  openVerticallyCentered(userIsUpdated: any) {
    this.modalService.open(userIsUpdated, {
      centered: true,
      windowClass: 'custom',
    });
  }
  userProfile() {
    this.router.navigate(['landing-page/user-profile']);
  }
}
