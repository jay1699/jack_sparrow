import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import {
  ProfileStandardClaims,
  User,
  UserManager,
  UserManagerSettings,
  WebStorageStateStore,
} from 'oidc-client';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable()
export class AuthService {
  /** UserManager ref */
  public userManager: UserManager;

  /** Whole User data */
  public currentUser!: User;

  public userLoaded: boolean;

  public currentUserData: BehaviorSubject<ProfileStandardClaims>;

  constructor(private _router: Router) {
    this.userLoaded = false;
    this.currentUserData = new BehaviorSubject<ProfileStandardClaims>({});
    const settings: UserManagerSettings = {
      authority: environment.authority,
      client_id: environment.client_id,
      scope: environment.scope,
      redirect_uri: `${environment.redirect_uri}auth-callback`,
      post_logout_redirect_uri: `${environment.redirect_uri}`,
      silent_redirect_uri: `${environment.redirect_uri}silent-callback.html`,
      response_type: environment.response_type,
      automaticSilentRenew: true,
      includeIdTokenInSilentRenew: true,
      silentRequestTimeout: 10000,
      loadUserInfo: true,
      userStore: new WebStorageStateStore({ store: window.localStorage }),
      acr_values: environment.acr_values,
    };

    this.userManager = new UserManager(settings);

    this.userManager.events.addUserLoaded((user: User) => {
      this.userManager.clearStaleState().then(() => { });
      this.currentUser = user;
      this.currentUserData.next(this.currentUser.profile);
    });
  }

  /** method used to get user details from user manager */
  public async getUser(): Promise<User> {
    const user: any = await this.userManager.getUser();
    this.setUserData(user);
    return user;
  }

  /** Set User Data */
  public setUserData(user: User): void {
    if (user) {
      this.currentUser = user;
      this.currentUserData.next(user.profile);
    }
  }

  /** Start calling Auth Callback to check authentication */
  public login(): Promise<void> {
    return this.userManager.signinRedirect();
  }

  public logout(): Promise<void> {
    return this.userManager.signoutRedirect();
  }

  /** Set user token if successfully authenticated in auth callback component */
  public completeAuthentication(): Promise<void> {
    return this.userManager
      .signinRedirectCallback()
      .then((user: User) => {
        this.currentUser = user;
        this._router.navigate(['/']);
      })
      .catch((err: any) => {
        err;
        setTimeout(() => {
          this._router.navigate(['/']);
        }, 3000);
      });
  }

  /** Get User Data */
  public getUserData(): Observable<ProfileStandardClaims> {
    return this.currentUserData.asObservable();
  }

  /** Get Authorization Header String
   * Contains token_type, access_token
   */
  public getAuthorizationHeaderValue(): string {
    if (this.currentUser) {
      return `${this.currentUser.token_type} ${this.currentUser.access_token}`;
    } else {
      return '';
    }
  }

  /** Clear User session */
  private clearUserSession(): void {
    // this.currentUser = null;
    this.userManager.removeUser();
    this.userLoaded = false;
  }
}
