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

export type LoginFormData = {
  username: string;
  password: string;
};
