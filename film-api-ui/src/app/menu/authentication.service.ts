import { Injectable } from '@angular/core';
import { OktaAuthService, UserClaims } from '@okta/okta-angular'

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private _currentUser: UserClaims;
  private _accessToken: string;

  get accessToken() {
    return 'Bearer ' + this._accessToken;
  }

  set accessToken(value: string) {
    this._accessToken = value;
  }
  
  get currentUser() {
    return this._currentUser;
  }

  set currentUser(value: UserClaims) {
    this._currentUser = value;
  }

  login() {
    this.oktaAuth.login();
  }

  logout() {
    this.oktaAuth.logout();
    this._accessToken = "";
    this._currentUser = null;
  }

  constructor(
    private oktaAuth: OktaAuthService
  ) { }
}
