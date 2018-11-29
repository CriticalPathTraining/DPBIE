import * as React from 'react';
import './CustomerToolbar.css';
import { withRouter, RouteComponentProps } from 'react-router-dom'

import { CustomerRouteParams } from './ViewCustomers';

import ViewCustomers from './ViewCustomers'

import ICustomer from '../../models/ICustomer';

interface CustomerToolbarProperties {
  viewCustomers: ViewCustomers;
}

type CustomerToolbarPropertiesWithRouter =
  CustomerToolbarProperties &
  RouteComponentProps<CustomerRouteParams>;

class CustomerToolbar extends React.Component<CustomerToolbarPropertiesWithRouter, any> {

  render() {
    return (
      <div className="customer-toolbar row" >    
        <ul className="customer-menu" >
          <li><a href="javascript:void(0)" onClick={() => { this.props.history.push("/customers") }} >Back to Customers List</a></li>
        </ul>
      </div>
    );
  }

}

export default withRouter<CustomerToolbarPropertiesWithRouter>(CustomerToolbar)