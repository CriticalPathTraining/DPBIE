import * as React from 'react';
import ICustomer from '../../models/ICustomer';
import ICustomerService from '../../models/ICustomersService';
import MockCustomersService from "./../../services/MockCustomersService";
import CustomersService from "./../../services/CustomersService";
import CustomersToolbar from './CustomersToolbar'
import CustomersTable from './CustomersTable';
import CustomerCard from './CustomerCard';
import './ViewCustomers.css';

type CustomerViewType = 'table' | 'cards';

interface ViewCustomersState {
  viewType: CustomerViewType;
  customerService: ICustomerService;
  customers: ICustomer[];
  loading: boolean;
}

export default class ViewCustomers extends React.Component<any, ViewCustomersState> {

  state: ViewCustomersState = {
    viewType: 'table',
    customerService: new CustomersService(),
    customers: [],
    loading: false
  }

  render() {
    return (
      <div>
        <CustomersToolbar ViewCustomers={this} />
        <div className="view-customers" >
          {this.state.viewType === "table" ?
            <CustomersTable customers={this.state.customers} /> :
            (this.state.customers.map((customer: ICustomer) => <CustomerCard customer={customer} />))
          }
        </div>

      </div>
    );
  }

  componentDidMount() {
    this.setState({ loading: true });
    this.state.customerService.getCustomersByLastName("A").then((customers: ICustomer[]) => {
      this.setState({ customers: customers, loading: false });
    })
  }

}
