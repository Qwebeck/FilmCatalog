import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
// import { MenuModule } from './menu.module';
import { User } from './user';

@Injectable({
  providedIn: 'root'
})
export class SignupManagerService {

  private url = "https://dev-221155.okta.com/api/v1/users?activate=true";
  private token = "00-DmEWPPkypwzLTK1bdHExPWe6oGSgtrDEEIZYPjQ";

  constructor(
    private http: HttpClient
  ) { }

  signup(user: User): Observable<User> {
    if ( !user.name )
      user.name = user.email;
    let data = {
      "profile": {
      "firstName": user.name,
      "lastName": user.surname,
      "email": user.email,
      "login": user.email,
    },
    "credentials": {
      "password" : { "value": user.password }
     }
    }
    return this.http.post<User>(this.url, data, {headers:
      {
        "Authorization": `SSWS ${this.token}`,
        "Content-Type": "application/json",
        "Accept": "application/json",
      }});
  }
}
