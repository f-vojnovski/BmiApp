import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Validators, FormGroup, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AuthResponseDto, LoginDto } from 'src/app/interfaces/users';
import { AuthService } from 'src/app/services/auth.service';
import { OnLoginSuccessComponent } from '../on-login-success/on-login-success.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  navigationUrl: string = '';
  errorMessage?: string;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private dialog: MatDialog,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  get f() {
    return this.loginForm.controls;
  }

  loginUser = (loginFormValue) => {
    if (this.loginForm.invalid) {
      return;
    }

    const login = { ...loginFormValue };

    const loginDto: LoginDto = {
      email: login.email,
      password: login.password,
    };

    this.authService.loginUser(loginDto).subscribe({
      next: () => {
        this.dialog.open(OnLoginSuccessComponent);
        this.router.navigate([this.navigationUrl]);
      },
      error: () => {
        this.errorMessage = 'Invalid credentials!';
      },
    });
  };
}
