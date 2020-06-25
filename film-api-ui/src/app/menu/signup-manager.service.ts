import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
// import { MenuModule } from './menu.module';
import { User } from './user';

@Injectable({
  providedIn: 'root'
})
export class SignupManagerService {

  private url = "https://localhost:5001/api/users";

  constructor(
    private http: HttpClient
  ) { }

  signup(user: User): Observable<User> {
    let data = {
      "firstName": user.name,
      "lastName": user.surname,
      "email": user.email,
      "login": user.email,
      "password" : user.password 
    }
    return this.http.post<User>(this.url, data);
  }
}
