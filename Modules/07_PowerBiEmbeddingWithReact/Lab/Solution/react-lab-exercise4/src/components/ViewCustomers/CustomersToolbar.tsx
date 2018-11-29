import * as React from 'react';

import ViewCustomers from './ViewCustomers'
import ICustomer from '../../models/ICustomer';
import ICustomersService from '../../models/ICustomersService';

import './CustomersToolbar.css';

interface CustomersToolbarProperties {
  ViewCustomers: ViewCustomers
}

export default class CustomersToolbar extends React.Component<CustomersToolbarProperties, any> {

  private letters = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
    "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];

  render() {
    let inTableView: boolean = this.props.ViewCustomers.state.viewType == "table";

    return (
      <div className="row btn-toolbar customers-toolbar" role="toolbar" >
        <nav className="container-fluid navbar navbar-expand-xl">

          <div className="view-menu btn-group btn-group-sm" role="group" >
            <button type="button"
              className={"btn btn-sm btn-secondary" + (inTableView ? " active" : "")}
              onClick={() => { this.props.ViewCustomers.setState({ 'viewType': 'table' }) }} >Table View</button>
            <button type="button"
              className={"btn btn-sm btn-secondary" + (!inTableView ? " active" : "")}
              onClick={() => { this.props.ViewCustomers.setState({ 'viewType': 'cards' }) }} >Cards View</button>
          </div>

          <div className="filter-menu btn-group btn-group-sm " role="group" >
            {this.letters.map((letter: string) =>
              <button type="button" key={letter} className="btn btn-sm btn-secondary"
                onClick={() => {
                  (document.getElementById('searchbox') as HTMLInputElement).value = letter;
                  let customerService: ICustomersService = this.props.ViewCustomers.state.customerService;
                  customerService.getCustomersByLastName(letter).then((customers: ICustomer[]) => {
                    this.props.ViewCustomers.setState({ customers: customers, loading: false });
                  });
                }} >
                {letter}
              </button>
            )}
          </div>

          <div className="search-menu input-group input-group-sm ml-auto">
            <div className="input-group-prepend">
              <span className="input-group-text" id="basic-addon1">Search</span>
            </div>
            <input id="searchbox" type="text" className="form-control form-control-sm" placeholder=""
              onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
                let customerService: ICustomersService = this.props.ViewCustomers.state.customerService;
                let searchString: string = event.target.value;
                if (searchString != "") {
                  customerService.getCustomersByLastName(searchString).then((customers: ICustomer[]) => {
                    this.props.ViewCustomers.setState({ customers: customers, loading: false });
                  });
                }
                else {
                  this.props.ViewCustomers.setState({ customers: [], loading: false });
                }
              }} />
          </div>

        </nav>
      </div>
    );
  }

}
