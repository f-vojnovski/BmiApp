import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MaterialModule } from './materials.module';
import { AppComponent } from './app.component';
import { BmiCalculatorComponent } from './components/bmi-calculator/bmi-calculator.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HeaderComponent } from './components/header/header.component';
import { AppRoutingModule } from './app-routing.module';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { HttpClientModule } from '@angular/common/http';
import { OnRegisterSuccessComponent } from './components/on-register-success/on-register-success.component';
import { OnLoginSuccessComponent } from './components/on-login-success/on-login-success.component';

@NgModule({
  declarations: [AppComponent, BmiCalculatorComponent, HeaderComponent, LoginComponent, RegisterComponent, OnRegisterSuccessComponent, OnLoginSuccessComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MaterialModule,
    FormsModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
