export interface RegisterDto {
  email: string;
  password: string;
  roles: string[];
}

export interface LoginDto {
  email: string;
  password: string;
}

export interface AuthResponseDto {
  isAuthSuccessful: boolean;
  errorMessage: string;
  token: string;
}

export interface User {
  email: string;
  token?: string;
}
