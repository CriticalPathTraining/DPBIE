import * as React from 'react';
import { render } from 'react-dom';
import App from './components/App';
import { BrowserRouter, HashRouter } from 'react-router-dom';

var topLevelAppComponent =
    <HashRouter>
        <App />
    </HashRouter>;

var target = document.getElementById('react-target');

render(topLevelAppComponent, target);