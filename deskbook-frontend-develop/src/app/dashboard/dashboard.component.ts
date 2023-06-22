import { Component, OnInit } from '@angular/core';
import { AuthService } from '../core/services/auth/auth.service';

@Component({
  selector: 'desk-book-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  host: {
    class: 'flex-grow-1',
  },
})
export class DashboardComponent implements OnInit {
  constructor(private authService: AuthService) {}

  ngOnInit(): void {}
  // public logout() {
  //   this.authService.logout();
  // }
}
