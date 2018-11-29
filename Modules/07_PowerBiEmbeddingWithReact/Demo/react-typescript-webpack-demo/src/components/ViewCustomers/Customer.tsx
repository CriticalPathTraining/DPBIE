
import { CustomerRouteParams } from './ViewCustomers';


import * as React from 'react';
import { withRouter, RouteComponentProps, Link } from 'react-router-dom'

import ICustomer from "./../../models/ICustomer";
import ICustomerDetail from "./../../models/ICustomerDetail";


import MockCustomersService from "./../../services/MockCustomersService";
import CustomerService from "./../../services/CustomerService";

import './Customer.css';

interface CustomerProperties {
}

type CustomerPropertiesWithRouter =
  CustomerProperties &
  RouteComponentProps<CustomerRouteParams>;

interface CustomerState {
  customer: ICustomerDetail;
}

class Customer extends React.Component<CustomerPropertiesWithRouter, CustomerState> {

  render() {
    return (
      <div className="row customer">
        {(this.state) && (this.state.customer) ? (
          <form className="row container-fluid">
            <fieldset className="col form-group">
              <legend>Customer Info</legend>
              <label htmlFor="customerId">Customer ID:</label>
              <input type="input" className="form-control form-control-sm" readOnly id="customerId" value={this.state.customer.CustomerId} />
              <label htmlFor="firstName">First Name:</label>
              <input type="input" className="form-control form-control-sm" readOnly id="firstName" value={this.state.customer.FirstName} />
              <label htmlFor="lastName">Last Name:</label>
              <input type="input" className="form-control form-control-sm" readOnly id="lastName" value={this.state.customer.LastName} />
              <label htmlFor="Gender">Gender:</label>
              <input type="input" className="form-control form-control-sm" id="Gender" readOnly value={this.state.customer.Gender} />
              <label htmlFor="Birthdate">Birth Date:</label>
              <input type="input" className="form-control form-control-sm" id="Birthdate" readOnly value={(new Date(this.state.customer.BirthDate)).toLocaleDateString()} />
            </fieldset>
            <fieldset className="col form-group">
              <legend>Contact Info</legend>
              
              <label htmlFor="Company">Company:</label>
              <input type="input" className="form-control form-control-sm" id="Company" readOnly value={this.state.customer.Company} />
              <label htmlFor="EmailAddress">Email:</label>
              <input type="input" className="form-control form-control-sm" id="EmailAddress" readOnly value={this.state.customer.EmailAddress} />

              <label htmlFor="WorkPhone">Work Phone:</label>
              <input type="input" className="form-control form-control-sm" id="WorkPhone" readOnly value={this.state.customer.WorkPhone} />

              <label htmlFor="HomePhone">Home Phone:</label>
              <input type="input" className="form-control form-control-sm" id="HomePhone" readOnly value={this.state.customer.HomePhone} />       
              <label htmlFor="Address">Mailing Address:</label>
              <textarea className="form-control form-control-sm" id="Address" readOnly value={this.state.customer.Address + "\r\n" + this.state.customer.City + ", " + this.state.customer.State + "  " + this.state.customer.Zipcode} ></textarea>
            </fieldset>
          </form>
        ) : <h3>Loading</h3>}
      </div>
    );
  }

  componentDidMount() {
    console.log("App.componentDidMount");
    CustomerService.getCustomer(this.props.match.params.id).then((customer: ICustomerDetail) => {
      this.setState({ customer: customer });
    })
  }

}

export default withRouter<CustomerPropertiesWithRouter>(Customer)