import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'desk-book-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss'],
  host: {
    class: 'sidebar-component',
  },
})
export class SidebarComponent implements OnInit {
  constructor(private modalService: NgbModal) {}

  ngOnInit(): void {}

  //   seatConfigDisabled(value: any) {
  //     if (value) {
  //       this.isDisabled =false;
  //     } else {
  //       this.isDisabled;
  //     }
  // }
}
