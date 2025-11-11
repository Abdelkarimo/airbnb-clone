export interface User {
  id: string;
  email: string;
  fullName: string;
  role: UserRole;
  profileImg?: string;
  emailConfirmed: boolean;
}

export enum UserRole {
  Guest = 'Guest',
  Host = 'Host',
  Admin = 'Admin'
}

export interface AuthResponse {
  token: string;
  expiration: string;
  user: User;
}

export interface LoginCredentials {
  email: string;
  password: string;
}

export interface RegisterData {
  fullName: string;
  email: string;
  password: string;
  role: UserRole;
}

export interface ChangePasswordData {
  currentPassword: string;
  newPassword: string;
}