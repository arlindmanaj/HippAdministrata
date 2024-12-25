

export interface RegisterRequest {
  name: string;
  password: string;
  email?: string;
  role: string;
}

export interface RegisterDriverRequest {
  name: string;
  password: string;
  email: string;
  licensePlate: string;
  carModel: string;
}

export interface RegisterEmployeeRequest {
  name: string;
  password: string;
}

export interface RegisterManagerRequest {
  name: string;
  password: string;
}

export interface RegisterSalesPersonRequest {
  name: string;
  password: string;

}
