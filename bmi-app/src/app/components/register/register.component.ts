import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { RegisterDto } from 'src/app/interfaces/users';
import { AuthService } from 'src/app/services/auth.service';
import { OnRegisterSuccessComponent } from '../on-register-success/on-register-success.component';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  navigationUrl: string = 'login';
  errorMessage?: string;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required],
    });
  }

  get f() {
    return this.registerForm.controls;
  }

  checkIfPasswordsMatch(register): boolean {
    if (register.password != register.confirmPassword) {
      this.errorMessage = "Passwords must match!";
      return false;
    }
    return true;
  }

  registerUser = (registerFormValue) => {
    if (this.registerForm.invalid) {
      return;
    }

    const register = { ...registerFormValue };

    if (!this.checkIfPasswordsMatch(register)) {
      return;
    }

    const registerDto: RegisterDto = {
      email: register.email,
      password: register.password,
      roles: ['User'],
    };

    this.authService.registerUser(registerDto).subscribe({
      next: () => {
        this.dialog.open(OnRegisterSuccessComponent);
        this.router.navigate([this.navigationUrl]);
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage = err.message;
      },
    });
  };
}
