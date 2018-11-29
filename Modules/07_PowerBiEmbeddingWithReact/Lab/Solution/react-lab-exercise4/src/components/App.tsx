import * as React from 'react';
import { Link, Route, Switch } from 'react-router-dom';

import Banner from "./Banner";
import TopNav from "./Topnav";

import ViewHome from './ViewHome/ViewHome';
import ViewCustomers from './ViewCustomers/ViewCustomers';
import ViewAbout from './ViewAbout/ViewAbout';

import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap';

import './App.css';

export default class App extends React.Component<any, any> {

  render() {

    return (
      <div id="page-container" className="container">
        <Banner appTitle="React Lab App" >
          <TopNav />
        </Banner>
        <Switch>
          <Route path="/" exact component={ViewHome} />
          <Route path="/customers" component={ViewCustomers} />
          <Route path="/about" component={ViewAbout} />
        </Switch>
      </div>
    );

  }

}
