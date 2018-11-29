import * as React from 'react';
import { Link, NavLink } from 'react-router-dom';
import IUserAction from "./../models/IUserAction"
import "./TopNav.css";

interface TopNavProperties {
  userActions: IUserAction[];
}

export default class TopNav extends React.Component<TopNavProperties, any> {

  render() {
    return (
      <div id="top-nav" className="navbar-collapse collapse" >
      <nav>
        <ul className="nav navbar-nav" >
            <li className="nav-item" ><NavLink exact to="/" className="navbar-link" activeClassName="active-nav-link" >Home</NavLink></li>
            <li className="nav-item" ><NavLink to="/customers" className="navbar-link" activeClassName="active-nav-link" >Customers</NavLink></li>
            <li className="nav-item" ><NavLink to="/about" className="navbar-link" activeClassName="active-nav-link">About</NavLink></li>
          </ul>          
         </nav>
      </div>
    );
  }

}
