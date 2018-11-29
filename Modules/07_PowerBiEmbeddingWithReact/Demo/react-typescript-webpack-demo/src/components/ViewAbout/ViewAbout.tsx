import * as React from 'react';
import './ViewAbout.css';

interface ViewAboutProperties {
}


export default class ViewAbout extends React.Component<ViewAboutProperties, any> {

  render() {
    return (
      <div className="content-body" >
        <div className="row">
          <div className="jumbotron col">
            <h3>About this app</h3>
            <p>This React.js app was created by Ted Pattison of &nbsp;
              <a href="https://www.criticalpathtraining.com/" target="_blank" >Critical Path Training</a>
              &nbsp; for demonstration purposes. We you you enjoy it and learn from it.</p>
          </div>
        </div>
      </div>
    );
  }
}