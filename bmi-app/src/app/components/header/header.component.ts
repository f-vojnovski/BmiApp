import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  isUserAuthenticated: boolean;
  onLogoutRedirectUrl: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.authService.currentUser.subscribe((res) => {
      this.isUserAuthenticated = res != null;
    });
  }

  onLogout(): void {
    this.authService.logout();
    this.router.navigate([this.onLogoutRedirectUrl]);
  }
}
