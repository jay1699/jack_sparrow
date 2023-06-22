import { Component, OnInit } from '@angular/core';
import { ProfileStandardClaims } from 'oidc-client';
import { AuthService } from '../../services/auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'desk-book-master',
  templateUrl: './master.component.html',
  styleUrls: ['./master.component.scss'],
})
export class MasterComponent implements OnInit {
  public isSuperAdmin: boolean = false;
  constructor(private authService: AuthService, private _router: Router) { }
  ngOnInit(): void {
    this.authService.getUserData().subscribe((res: any) => {
      if (res.role === 'SUPER_ADMIN') {
        this.isSuperAdmin = true;
        this._router.navigate(['/dashboard']);
      } else {
        this.isSuperAdmin = false;
      }
    });
  }
}
