import { Component, OnInit } from '@angular/core';
import { BmiWriteRecordDto } from 'src/app/interfaces/bmi';
import { AuthService } from 'src/app/services/auth.service';
import { BmiService } from 'src/app/services/bmi.service';

@Component({
  selector: 'app-bmi-calculator',
  templateUrl: './bmi-calculator.component.html',
  styleUrls: ['./bmi-calculator.component.css'],
})
export class BmiCalculatorComponent implements OnInit {
  calculatedBmi?: number = undefined;
  userHeight?: number = undefined;
  userWeight?: number = undefined;
  isUserAuthenticated: boolean;
  resultSaved: boolean = false;
  errorMessage?: string;

  constructor(
    private authService: AuthService,
    private bmiService: BmiService
  ) {}

  ngOnInit(): void {
    this.authService.currentUser.subscribe((res) => {
      this.isUserAuthenticated = res != null;
    });
  }

  onCalculateBmi(): void {
    this.calculatedBmi = this.calculateBmi(this.userWeight!, this.userHeight!);
  }

  calculateBmi(weight: number, height: number): number {
    return weight / (((height / 100) * height) / 100);
    this.resultSaved = false;
  }

  onSaveBmi(): void {
    const bmiRecord: BmiWriteRecordDto = {
      email: this.authService.currentUserValue.email,
      weight: this.userWeight,
      height: this.userHeight,
      bmi: this.calculatedBmi
    };
    this.bmiService.saveBmiRecord(bmiRecord).subscribe({
      next: () => {
        this.resultSaved = true;
      },
      error: () => {
        this.errorMessage = "Something went wrong while trying to save result!"
      },
    });
  }
}
