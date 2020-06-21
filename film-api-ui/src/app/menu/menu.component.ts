import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../authentication.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {

  isAuthenticated: boolean = false;

  constructor(
    private auth: AuthenticationService
  ) { 
    this.isAuthenticated = auth.isAuthenticated; 
  }

  ngOnInit(): void {

  }

  login(): void {
    this.auth.login();
    this.isAuthenticated = this.auth.isAuthenticated;
  }

  logout(): void {
    this.auth.logout();
    this.isAuthenticated = this.auth.isAuthenticated;
  }
}
