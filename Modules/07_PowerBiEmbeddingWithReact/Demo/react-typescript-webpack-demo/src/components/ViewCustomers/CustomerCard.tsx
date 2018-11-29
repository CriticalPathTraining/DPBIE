import * as React from 'react';
import { withRouter, RouteComponentProps, Link, match } from 'react-router-dom'


import { CustomerRouteParams } from './ViewCustomers';

import ICustomer from "./../../models/ICustomer";
import MockCustomersService from "./../../services/MockCustomersService";
import CustomerService from "./../../services/CustomerService";

import './CustomerCard.css';

interface CustomerCardProperties {
  customer: ICustomer;
}

type CustomerCardPropertiesWithRouter =
  CustomerCardProperties &
  RouteComponentProps<CustomerRouteParams>;


class CustomerCard extends React.Component<CustomerCardPropertiesWithRouter, any> {

  render() {
    return (
        <div className="customer-card card" onClick={() => { this.props.history.push(this.props.match!.url + "/" + this.props.customer.CustomerId) }} >
          <div className="card-header">
            {this.props.customer.FirstName + " " + this.props.customer.LastName}
          </div>
          <div className="card-body">
            <div className="card-text" >Work Phone: <strong>{this.props.customer.WorkPhone}</strong></div>
            <div className="card-text" >Home Phone: <strong>{this.props.customer.HomePhone}</strong></div>
          </div>
        </div>
    );
  }
}

export default withRouter(CustomerCard)