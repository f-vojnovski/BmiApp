import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import {
  AuthResponseDto,
  LoginDto,
  RegisterDto,
  User,
} from '../interfaces/users';
import { BehaviorSubject, map, Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  apiUrl: string = environment.apiUrl;
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;

  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<User>(
      JSON.parse(localStorage.getItem('currentUser'))
    );
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  public loginUser = (body: LoginDto) => {
    return this.http
      .post<any>(this.createCompleteRoute('api/auth/login', this.apiUrl), body)
      .pipe(
        map((user) => {
          user.email = body.email;
          localStorage.setItem('currentUser', JSON.stringify(user));
          this.currentUserSubject.next(user);
          return user;
        })
      );
  };

  public logout() {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  public registerUser = (body: RegisterDto) => {
    return this.http.post<AuthResponseDto>(
      this.createCompleteRoute('api/auth/register', this.apiUrl),
      body
    );
  };

  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}${route}`;
  };
}
