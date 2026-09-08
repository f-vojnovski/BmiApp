import { Component, OnInit } from '@angular/core';
import { BmiReadRecordDto } from 'src/app/interfaces/bmi';
import { BmiService } from 'src/app/services/bmi.service';

@Component({
  selector: 'app-view-bmi-records',
  templateUrl: './view-bmi-records.component.html',
  styleUrls: ['./view-bmi-records.component.css'],
})
export class ViewBmiRecordsComponent implements OnInit {
  records: BmiReadRecordDto[];
  displayedColumns: string[] = ['weight', 'height', 'bmi'];
  dataSource = null;

  constructor(private bmiService: BmiService) {}

  ngOnInit(): void {
    this.bmiService.readBmiRecords().subscribe({
      next: (res) => {
        this.records = res;
        this.dataSource = this.records;
      },
      error: () => {
      }
    });
  }
}
