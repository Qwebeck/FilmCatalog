import { Component, OnInit } from '@angular/core';
import { OktaAuthService } from '@okta/okta-angular';
import { Location } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { SignupFormComponent } from '../signup-form/signup-form.component';
import { SignupManagerService } from '../signup-manager.service';

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
    private locaction: Location,
    private dialog: MatDialog,
    private signupManager: SignupManagerService
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
  }

  logout(): void {
    this.auth.logout();
  }

  goBack(): void {
    this.locaction.back();
  }

  openSignupModal(): void {
    const dialogRef = this.dialog.open(SignupFormComponent, {
      width: '400px',
    })

    dialogRef.afterClosed().subscribe(result => {
      this.signupManager.signup(result)
          .subscribe( result => console.log('here'));
    });
  }
}
