import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-auth-callback',
  templateUrl: './auth-callback.component.html'
})
export class AuthCallbackComponent implements OnInit {

  constructor(private _authService: AuthService) { }

  ngOnInit(): void {
    
    this._authService.completeAuthentication();
  }

}
