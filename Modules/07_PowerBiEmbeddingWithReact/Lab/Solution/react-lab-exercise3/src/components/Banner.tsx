import * as React from 'react';

import './Banner.css';

interface BannerProperties {
  appTitle: string;
}

export default class Banner extends React.Component<BannerProperties, any> {
  render() {
    return (
      <div id="banner" className="row navbar navbar-expand-sm navbar-dark bg-dark" role="navigation" >
        <div id="app-icon"  ></div>
        <div id="app-title">{this.props.appTitle}</div>
        {this.props.children}
      </div>
    );
  }
}
