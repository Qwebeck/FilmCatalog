import { Component, OnInit } from '@angular/core';
// import { OktaAuthService } from '@okta/okta-angular';
import { AuthenticationService } from '../authentication.service';
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
  // userClaims;
  userName: string;

  constructor(
    private auth: AuthenticationService,
    private locaction: Location,
    private dialog: MatDialog,
    private signupManager: SignupManagerService
  ) { }

  ngOnInit() {
    if ( this.auth.currentUser ) {
      this.isAuthenticated = true;
      this.userName = this.auth.currentUser.name;
    } else {
      this.isAuthenticated = false;
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
