import * as React from 'react';
import './ViewHome.css';

interface ViewHomeProperties {
}


export default class ViewHome extends React.Component<ViewHomeProperties, any> {

  render() {
    return (
      <div className="content-body" >
        <div className="row">
          <div className="jumbotron col">
            <h3>My Home Page</h3>
            <p>This is my React.js demo app</p>
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