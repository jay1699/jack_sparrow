import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'desk-book-admin-header',
  templateUrl: './admin-header.component.html',
  styleUrls: ['./admin-header.component.scss'],
})
export class AdminHeaderComponent implements OnInit {
  currentRoute: any;
  constructor(private router: Router, private authService: AuthService) { }

  ngOnInit(): void { }
  public logout() {
    this.authService.logout();
  }
  userProfile() {
    this.currentRoute = this.router.url;
    this.router.navigate([`${this.currentRoute}/user-profile`]);
  }
}
