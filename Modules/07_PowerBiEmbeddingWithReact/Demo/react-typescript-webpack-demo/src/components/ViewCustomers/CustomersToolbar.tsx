import * as React from 'react';
import './CustomersToolbar.css';
import ViewCustomers from './ViewCustomers'
import ICustomer from '../../models/ICustomer';


interface CustomersToolbarProperties {
  viewCustomers: ViewCustomers
}

export default class CustomersToolbar extends React.Component<CustomersToolbarProperties, any> {

  private letters = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];

  render() {
    let inTableView: boolean = this.props.viewCustomers.state.viewType == "table";

    return (
      <div className="row btn-toolbar customers-toolbar" role="toolbar" >   
        <nav className="container-fluid navbar navbar-expand-xl">


          <div className="view-menu btn-group btn-group-sm" role="group" >
            <button type="button"
              className={"btn btn-sm btn-secondary" + (inTableView ? " active" : "")}
              onClick={() => { this.props.viewCustomers.setState({ 'viewType': 'table' }) }} >Table View</button>
            <button type="button"
              className={"btn btn-sm btn-secondary" + (!inTableView ? " active" : "")}
              onClick={() => { this.props.viewCustomers.setState({ 'viewType': 'cards' }) }} >Cards View</button>
          </div>

          <div className="customers-menu btn-group btn-group-sm collapse navbar-collapse" role="group" >
            {this.letters.map((letter: string) =>
              <button type="button" key={letter} className="btn btn-sm btn-secondary"
                onClick={() => {
                  (document.getElementById('searchbox') as HTMLInputElement).value = letter;
                  this.props.viewCustomers.getCustomersByLastName(letter);
                }}>{letter}</button>
            )}
          </div>

          <div className="search-menu input-group input-group-sm ml-auto">
            <div className="input-group-prepend">
              <span className="input-group-text" id="basic-addon1">Search</span>
            </div>
            <input id="searchbox" type="text" className="form-control form-control-sm" placeholder=""
              onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
                if(event.target.value != ""){
                  this.props.viewCustomers.getCustomersByLastName(event.target.value);
                }
                else {
                  this.props.viewCustomers.setState({ customers:[] });
                }
              }} />
          </div>

        </nav>
      </div>
    );
  }
}