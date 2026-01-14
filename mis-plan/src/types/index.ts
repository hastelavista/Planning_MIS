// FILE: src/types/index.ts
// ============================================
export interface User {
  id: string;
  username: string;
  email?: string;
  name: string;
}

export interface AuthState {
  token: string;
  user: User;
}

export interface MenuItem {
  label: string;
  icon: React.ReactNode;
  route?: string;
  children?: MenuItem[];
}

export interface LoginCredentials {
  username: string;
  password: string;
}
