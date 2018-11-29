import * as React from 'react';
import { render } from 'react-dom';
import App from './components/App';

var topLevelAppComponent = <App />;
var target = document.getElementById('react-target');

render(topLevelAppComponent, target);