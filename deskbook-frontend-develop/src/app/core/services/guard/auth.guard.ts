import { Injectable } from '@angular/core';
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/auth.service';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private _authService: AuthService) {}

  /** Route Gaurd for Authentication */
  public canActivate(): Promise<boolean> {
    return this.getUser();
  }

  public async getUser(): Promise<boolean> {
    let loggedIn: boolean = false;
    const user: any = await this._authService.getUser();
    if (user) {
      loggedIn = true;
    } else {
      loggedIn = false;
      this._authService.login();
    }
    return loggedIn;
  }
}
