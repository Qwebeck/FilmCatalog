import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  isAuthenticated: boolean = false;
  private username: string = '';
  constructor() { }

  login() {
    this.isAuthenticated = true;
    this.username = "John";
  }

  logout() {
    this.username = '';
    this.isAuthenticated = false;
  }

  currentUser(): string {
    return this.username;
  }
}
