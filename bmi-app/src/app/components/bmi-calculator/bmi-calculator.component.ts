import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-bmi-calculator',
  templateUrl: './bmi-calculator.component.html',
  styleUrls: ['./bmi-calculator.component.css'],
})
export class BmiCalculatorComponent implements OnInit {
  calculatedBmi?: number = undefined;
  userHeight?: number = undefined;
  userWeight?: number = undefined;

  constructor() {}

  ngOnInit(): void {}

  onCalculateBmi(): void {
    this.calculatedBmi = this.calculateBmi(this.userWeight!, this.userHeight!);
  }

  calculateBmi(weight: number, height: number): number {
    return weight / (((height / 100) * height) / 100);
  }
}
