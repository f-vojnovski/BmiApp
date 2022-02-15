import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AuthResponseDto, LoginDto, RegisterDto } from '../interfaces/users';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  apiUrl: string = environment.apiUrl;
  private authChangeSub = new Subject<boolean>();
  public authChanged = this.authChangeSub.asObservable();

  constructor(private http: HttpClient) {}

  public loginUser = (body: LoginDto) => {
    return this.http.post<AuthResponseDto>(
      this.createCompleteRoute("api/auth/login", this.apiUrl),
      body
    );
  };

  public registerUser = (body: RegisterDto) => {
    return this.http.post<AuthResponseDto>(
      this.createCompleteRoute("api/auth/register", this.apiUrl),
      body
    );
  };

  public sendAuthStateChangeNotification = (isAuthenticated: boolean) => {
    this.authChangeSub.next(isAuthenticated);
  };

  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}${route}`;
  };
}
