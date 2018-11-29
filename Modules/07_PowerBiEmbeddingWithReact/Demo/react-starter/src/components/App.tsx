import * as React from 'react';

import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap';

import './App.css';

export default class App extends React.Component<any, any> {

  render() {

    return (
      <div id="page-container" className="container">
        <div className="row navbar navbar-expand-sm navbar-dark bg-dark" role="navigation" >
          <h1 style={{ 'color': 'white' }} >React Starter App</h1>
        </div>
        <div className="jumbotron">
          <div>This is a sample starter app for with with React and TypeScript.</div>
        </div>
      </div>
    );

  }

}
