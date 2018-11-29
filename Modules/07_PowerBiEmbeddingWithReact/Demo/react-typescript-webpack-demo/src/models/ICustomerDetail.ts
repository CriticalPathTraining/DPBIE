import ICustomer from './ICustomer';

export default interface ICustomerDetail extends ICustomer {
    Address: string;
    City: string;
    State: string;
    Zipcode: string;
    Gender: string;
    BirthDate: string;
}