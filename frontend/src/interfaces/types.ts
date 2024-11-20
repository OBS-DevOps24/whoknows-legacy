// Example type of register data

export type RegisterFormData = {
  username: string;
  email: string;
  password: string;
  password2: string;
};

export type PageType = {
  title: string;
  url: string;
  language: "en" | "da";
  last_updated: string;
  content: string;
};

export type WeatherType = {
  latitude: number;
  longitude: number;
  time: string;
  country: string;
  city: string;
  temperatureUnit: string;
  temperatureValue: number;
  windSpeedUnit: string;
  windSpeedValue: number;
};

export type LoginFormData = {
  username: string;
  password: string;
};

export type ChangePasswordFormData = {
  oldPassword: string;
  newPassword: string;
};
