// ============================================
// FILE: src/contexts/AuthContext.tsx
// ============================================
'use client';

import React, { createContext, useContext, useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { authAPI, userAPI } from '@/lib/api';
import type { AuthState, LoginCredentials } from '@/types';

interface AuthContextType {
  authState: AuthState | null;
  login: (credentials: LoginCredentials) => Promise<void>;
  logout: () => void;
  isLoading: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return context;
};

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [authState, setAuthState] = useState<AuthState | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const router = useRouter();

  useEffect(() => {
    // Check for stored token on mount
    const token = localStorage.getItem('token');
    const userStr = localStorage.getItem('user');
    
    if (token && userStr) {
      try {
        const user = JSON.parse(userStr);
        setAuthState({ token, user });
      } catch (error) {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
      }
    }
    setIsLoading(false);
  }, []);

  const login = async (credentials: LoginCredentials) => {
    try {
      const authresponse = await authAPI.login(credentials);
      const { token, user } = authresponse;
      
       // Store token, so API calls are authenticated
      localStorage.setItem('token', token);
      
      //fetch and store Userdetails
      const userDetails = await userAPI.getByUsername(user);
      localStorage.setItem('user', JSON.stringify(userDetails));

      
      setAuthState({ token, user:userDetails});
      router.push('/dashboard');
    } catch (error) {
      localStorage.removeItem('token');
    localStorage.removeItem('user');
      console.error('Login failed:', error);
      throw error;
    }
  };

  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    setAuthState(null);
    router.push('/login');
  };

  return (
    <AuthContext.Provider value={{ authState, login, logout, isLoading }}>
      {children}
    </AuthContext.Provider>
  );
  
};
