import * as React from 'react';
import './ViewHome.css';

export default class ViewHome extends React.Component<any, any> {
  render() {
    return (
      <div id="view-home" className="content-body" >
        <div className="row">
          <div className="jumbotron col">
            <h3>My Home Page</h3>
            <p>This is my React.js lab app</p>
          </div>
        </div>
        <div className="row">
          <div className="col">
            <h4>React.js is awesome</h4>
            <div>You're going to love it.</div>
          </div>
          <div className="col">
            <h4>React.js is wholesome</h4>
            <div>You can build apps that are huge.</div>
          </div>
        </div>
      </div>
    );
  }
}