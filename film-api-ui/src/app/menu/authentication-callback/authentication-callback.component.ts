import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../authentication.service';
import { OktaAuthService } from '@okta/okta-angular';
import { Router } from '@angular/router';

@Component({
  selector: 'app-authentication-callback',
  templateUrl: './authentication-callback.component.html',
  styleUrls: ['./authentication-callback.component.scss']
})
export class AuthenticationCallbackComponent implements OnInit {

  error: string;

  constructor(
    private okta: OktaAuthService,
    private auth: AuthenticationService,
    private router: Router ) {}

  async ngOnInit() {
    try{
    const fromUri = this.okta.getFromUri();
    let redirect = /https?:\/\/[^/]*(.*)/.exec(fromUri)[1];
    await this.okta.handleAuthentication();
    this.auth.currentUser = await this.okta.getUser();
    this.auth.accessToken = await this.okta.getAccessToken();
    this.router.navigate([redirect]); 
    } catch ( err ) {
        this.error = err.ToString();
    }
  }
}
