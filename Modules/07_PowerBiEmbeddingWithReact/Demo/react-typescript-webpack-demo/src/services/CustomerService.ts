import ICustomer from "./../models/ICustomer";
import ICustomerDetail from "./../models/ICustomerDetail";

export default class CustomerService {

  static getCustomers = (): Promise< ICustomer[]> => {
    const restUrl = "http://subliminalsystems.com/api/Customers/?$select=CustomerId,LastName,FirstName,EmailAddress,WorkPhone,HomePhone,Company&$filter=(CustomerId+le+12)&$top=200";
     return  fetch(restUrl)
        .then(response => response.json())
        .then(response=>{
          console.log(response.value);
          return response.value;
        });
  }
  
  static getCustomersByLastName = (lastNameSearch: string): Promise< ICustomer[]> => {
    const restUrl = `http://subliminalsystems.com/api/Customers/?$select=CustomerId,LastName,FirstName,EmailAddress,WorkPhone,HomePhone,Company&$filter=startswith(tolower(LastName),tolower('${lastNameSearch}'))&$orderby=LastName,FirstName`;
     return  fetch(restUrl)
        .then(response => response.json())
        .then(response=>{
          console.log(response.value);
          return response.value;
        });
  }
  
  static getCustomer = (customerId: string): Promise<ICustomerDetail> => {
    const restUrl = "http://subliminalsystems.com/api/Customers(" + customerId + ")";
     return  fetch(restUrl)
        .then(response => response.json())
        .then(response=>{
          console.log(response);
          return response;
        });
  } 

}