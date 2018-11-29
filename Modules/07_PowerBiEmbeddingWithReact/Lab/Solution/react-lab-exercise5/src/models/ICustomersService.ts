import ICustomer from "./ICustomer"
import ICustomerDetail from "./ICustomerDetail";

export default interface ICustomersService {
  getCustomers(): Promise<ICustomer[]>;
  getCustomersByLastName(lastNameSearch: string): Promise<ICustomer[]>;
  getCustomer(customerId: string): Promise<ICustomerDetail>;
}