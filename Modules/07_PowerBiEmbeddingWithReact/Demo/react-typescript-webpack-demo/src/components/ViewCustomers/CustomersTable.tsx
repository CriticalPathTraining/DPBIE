import * as React from 'react';

import { withRouter, RouteComponentProps, Link, match } from 'react-router-dom'


import ICustomer from "./../../models/ICustomer";

import './CustomersTable.css';
import CustomerRouteParams from './ViewCustomers'

interface CustomersTableProperties {
  customers: ICustomer[];
}

type CustomersTablePropertiesWithRouter =
  CustomersTableProperties &
  RouteComponentProps<CustomerRouteParams>;


  class CustomersTable extends React.Component<CustomersTablePropertiesWithRouter, any> {

  render() {
    console.log(this.props);
    return (
      <div className="row" >
      { this.props.customers.length > 0 ? (
        <table id="customer-table" className="col customers-table table table-striped table-bordered table-hover table-sm">
          <thead className="thead-dark">
            <tr>
              <th>ID</th>
              <th>First Name</th>
              <th>Last Name</th>
              <th>Company</th>
              <th>Email</th>
              <th>Work Phone</th>
              <th>Home Phone</th>
            </tr>
          </thead>
          <tbody>
            {this.props.customers.map((customer: ICustomer) =>
              <tr key={customer.CustomerId} onClick={() => { this.props.history.push(this.props.match!.url + "/" + customer.CustomerId) }} >
                <td>{customer.CustomerId}</td>
                <td>{customer.FirstName}</td>
                <td>{customer.LastName}</td>
                <td>{customer.Company}</td>                
                <td>{customer.EmailAddress}</td>
                <td>{customer.WorkPhone}</td>
                <td>{customer.HomePhone}</td>
              </tr>
            )}
          </tbody>
        </table>
        ) : (
          <div className="col content-body">
          <div className="alert alert-info" role="alert"><strong>No customers returned.</strong> Please refine your search query.</div>
          </div>
        )
        }
      </div>
    );
  }
}

export default withRouter<CustomersTablePropertiesWithRouter>(CustomersTable)