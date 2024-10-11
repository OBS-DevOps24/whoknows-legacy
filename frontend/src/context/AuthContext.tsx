import React, { createContext, useState, useContext, useEffect } from 'react';
import { loginUser, logoutUser, checkAuthStatusApi } from '../services/apiFacade';
import { LoginFormData } from '../interfaces/types';

interface AuthContextType {
  isLoggedIn: boolean;
  login: (loginData: LoginFormData) => Promise<void>;
  logout: () => Promise<void>;
  checkAuthStatus: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  const checkAuthStatus = async () => {
    try {
      const status = await checkAuthStatusApi();
      setIsLoggedIn(status);
    } catch (error) {
      console.error('Error checking auth status:', error);
      setIsLoggedIn(false);
    }
  };

  useEffect(() => {
    checkAuthStatus();
  }, []);

  const login = async (loginData: LoginFormData) => {
    try {
      await loginUser(loginData);
      setIsLoggedIn(true);
    } catch (error) {
      console.error('Login failed:', error);
      setIsLoggedIn(false);
      throw error;
    }
  };

  const logout = async () => {
    try {
      await logoutUser();
      setIsLoggedIn(false);
    } catch (error) {
      console.error('Logout failed:', error);
    }
  };

  return (
    <AuthContext.Provider value={{ isLoggedIn, login, logout, checkAuthStatus }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};