import * as React from 'react';
import { withRouter, RouteComponentProps, Route, Switch, Link, match } from 'react-router-dom'

import ICustomer from '../../models/ICustomer';
import MockCustomersService from "./../../services/MockCustomersService";
import CustomerService from "./../../services/CustomerService";

import CustomersToolbar from './CustomersToolbar';
import CustomersTable from './CustomersTable';
import CustomerCard from './CustomerCard';

import CustomerToolbar from './CustomerToolbar';
import Customer from './Customer';

import './ViewCustomers.css';

export interface CustomerRouteParams {
  id: string;
}

interface ViewCustomersProperties {
  match?: match<CustomerRouteParams>;
}

type ViewCustomersPropertiesWithRouter =
  ViewCustomersProperties &
  RouteComponentProps<CustomerRouteParams>;

type CustomerViewType = 'table' | 'cards';

interface ViewCustomersState {
  viewType: CustomerViewType;
  customers: ICustomer[];
  loading: boolean;
}


export default class ViewCustomers extends React.Component<ViewCustomersPropertiesWithRouter, ViewCustomersState> {

  state: ViewCustomersState = {
    viewType: 'table',
    customers: [],
    loading: false
  }

  render() {
    return (
      <div>

        <Route exact path={this.props.match.path} render={()=>
          <CustomersToolbar viewCustomers={this} />
        } />

        <Route path={`${this.props.match.path}/:id`} render={({ match })=>
          <CustomerToolbar viewCustomers={this} />
        } />

        <div className="view-customers" >

          <Route exact path={this.props.match.path} render={
            () => (
              this.state.viewType === "table" ?
                <CustomersTable customers={this.state.customers} /> :
                this.state.customers.map((customer: ICustomer) => <CustomerCard {...this.props} customer={customer} />)
            )}
          />

          <Route path={`${this.props.match!.path}/:id`} render={
            () => <Customer />
          } />



        </div>
      </div>);
  }

  componentDidMount() {
    console.log("ViewCustomers.componentDidMount");
    this.setState({ loading: true });
    CustomerService.getCustomers().then((customers: ICustomer[]) => {
      this.setState({ customers: customers, loading: false });
    })
  }

  getCustomersByLastName(lastNameSearch: string) {
    this.setState({ loading: true });
    CustomerService.getCustomersByLastName(lastNameSearch).then((customers: ICustomer[]) => {
      this.setState({ customers: customers, loading: false });
    })

  }

}