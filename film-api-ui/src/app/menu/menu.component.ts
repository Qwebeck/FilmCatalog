import { Component, OnInit } from '@angular/core';
import { OktaAuthService } from '@okta/okta-angular';
import { Location } from '@angular/common';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {

  isAuthenticated: boolean;
  userClaims;
  userName: string;

  constructor(
    private auth: OktaAuthService,
    private locaction: Location
  ) { 
    this.auth.$authenticationState.subscribe(
      (isAuthenticated) => this.isAuthenticated = isAuthenticated
    )
  }

  async ngOnInit() {
    this.isAuthenticated = await this.auth.isAuthenticated(); 
    if ( this.isAuthenticated ) {
      this.userClaims = await this.auth.getUser();
      this.userName = this.userClaims.name;
    }
  }

  login(): void {
    this.auth.login();
    // this.auth.login();
    // this.isAuthenticated = this.auth.isAuthenticated;
  }

  logout(): void {
    this.auth.logout
    // this.auth.logout();
    // this.isAuthenticated = this.auth.isAuthenticated;
  }
  goBack(): void {
    this.locaction.back();
  }
}
