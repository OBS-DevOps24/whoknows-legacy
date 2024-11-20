import React, { createContext, useState, useContext, useEffect } from "react";
import {
  loginUser,
  logoutUser,
  checkAuthStatusApi,
  checkPasswordStatusApi,
} from "../services/apiFacade";
import { LoginFormData } from "../interfaces/types";

interface AuthContextType {
  isLoggedIn: boolean;
  login: (loginData: LoginFormData) => Promise<void>;
  logout: () => Promise<void>;
  checkAuthStatus: () => Promise<void>;
  isPasswordExpired: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [isPasswordExpired, setIsPasswordExpired] = useState(false);

  const checkAuthStatus = async () => {
    try {
      const status = await checkAuthStatusApi();
      setIsLoggedIn(status);
    } catch (error) {
      console.error("Error checking auth status:", error);
      setIsLoggedIn(false);
    }
  };

  useEffect(() => {
    checkAuthStatus();
    checkIsPasswordExpired();
  }, []);

  const login = async (loginData: LoginFormData) => {
    try {
      await loginUser(loginData);
      setIsLoggedIn(true);
      await checkAuthStatus();
    } catch (error) {
      console.error("Login failed:", error);
      setIsLoggedIn(false);
      throw error;
    }
  };

  const logout = async () => {
    try {
      await logoutUser();
      setIsLoggedIn(false);
    } catch (error) {
      console.error("Logout failed:", error);
    }
  };

  const checkIsPasswordExpired = async () => {
    try {
      const status = await checkPasswordStatusApi();
      setIsPasswordExpired(status);
    } catch (error) {
      console.error("Error checking password status:", error);
      setIsPasswordExpired(false);
    }
  };

  return (
    <AuthContext.Provider
      value={{ isLoggedIn, login, logout, checkAuthStatus, isPasswordExpired }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
