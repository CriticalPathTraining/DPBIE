import * as React from 'react';

import ICustomer from "./../../models/ICustomer";

import './CustomerCard.css';

interface CustomerCardProperties {
  customer: ICustomer;
}

export default class CustomerCard extends React.Component<CustomerCardProperties, any> {

  render() {
    return (
      <div className="card customer-card" >
        <div className="card-header">
          {this.props.customer.FirstName + " " + this.props.customer.LastName}
        </div>
        <div className="card-body">
          <div className="card-text" >
            Work Phone: <strong>{this.props.customer.WorkPhone}</strong>
          </div>
          <div className="card-text" >
            Home Phone: <strong>{this.props.customer.HomePhone}</strong>
          </div>
        </div>
      </div>
    );
  }
}