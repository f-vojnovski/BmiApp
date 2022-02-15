import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { BmiReadRecordDto, BmiWriteRecordDto } from '../interfaces/bmi';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class BmiService {
  apiUrl: string = environment.apiUrl;

  constructor(private http: HttpClient, private authService: AuthService) {}

  public saveBmiRecord = (body: BmiWriteRecordDto) => {
    return this.http.post<any>(
      this.createCompleteRoute('api/bmi', this.apiUrl),
      body
    );
  };

  public readBmiRecords = () => {
    return this.http.get<any>(
      this.createCompleteRoute(
        `api/bmi/email/${this.authService.currentUserValue.email}`,
        this.apiUrl
      )
    );
  };

  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}${route}`;
  };
}
